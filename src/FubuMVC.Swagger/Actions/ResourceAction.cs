using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Urls;
using FubuMVC.Swagger.Specification;

namespace FubuMVC.Swagger.Actions
{
    public class ResourceRequest
    {
        [RouteInput]
        public string GroupKey { get; set; }
    }

    public class ResourceAction
    {
        private readonly ConnegActionFinder _connegActionActionFinder;
        private readonly ActionCallMapper _actionCallMapper;
        private readonly IUrlRegistry _urlRegistry;
        private readonly ICurrentHttpRequest _currentHttpRequest;

        public ResourceAction(ConnegActionFinder connegActionActionFinder, ActionCallMapper actionCallMapper, IUrlRegistry urlRegistry, ICurrentHttpRequest currentHttpRequest)
        {
            _connegActionActionFinder = connegActionActionFinder;
            _actionCallMapper = actionCallMapper;
            _urlRegistry = urlRegistry;
            _currentHttpRequest = currentHttpRequest;
        }

        //[AsymmetricJson]
        public Resource Execute(ResourceRequest request)
        {
            var baseUrl = _urlRegistry.UrlFor(request);
            var absoluteBaseUrl = _currentHttpRequest.ToFullUrl(baseUrl);

            var actions = _connegActionActionFinder.ActionsForGroup(request.GroupKey).ToArray();

            var apis = createSwaggerAPIs(actions, baseUrl);

            var typeSet = new HashSet<Type>();
            actions.Each(a =>
                            {
                                if(a.HasInput) typeSet.Add(a.InputType());
                                if (a.HasOutput) typeSet.Add(a.OutputType());
                            });

            return new Resource
                       {
                           basePath = absoluteBaseUrl,
                           resourcePath = "/" + request.GroupKey, //SWAGGER HACK - this assumes that the resource path will always be relative to the basePath.
                           apiVersion = Assembly.GetExecutingAssembly().GetVersion(),
                           swaggerVersion = "1.0",
                           apis = apis,
                           models = typeSet.ToArray()
                       };
        }

        private API[] createSwaggerAPIs(IEnumerable<ActionCall> actions, string baseUrl)
        {
            var apis = actions.Select(a =>
                                          {
                                              //UGH we need to make relative URLs for swagger to be happy. 
                                              var pattern = a.ParentChain().Route.Pattern;
                                              var resourceUrl = baseUrl.UrlRelativeTo(pattern);
                                              var description = a.InputType().GetAttribute<DescriptionAttribute>(d => d.Description);

                                              return new API
                                                         {
                                                             path = resourceUrl,
                                                             description = description,
                                                             operations = _actionCallMapper.GetSwaggerOperations(a).ToArray()
                                                         };
                                          }).ToArray();
            return apis;
        }
    }
}