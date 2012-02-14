using System.Linq;
using System.Reflection;
using FubuCore;
using FubuMVC.Core.Http;
using FubuMVC.Core.Urls;
using FubuMVC.Swagger.Specification;

namespace FubuMVC.Swagger.Actions
{
    public class ResourceDiscoveryAction
    {
        private readonly ApiFinder _apiFinder;
        private readonly IUrlRegistry _urlRegistry;
        private readonly ICurrentHttpRequest _currentHttpRequest;

        public ResourceDiscoveryAction(ApiFinder apiFinder, IUrlRegistry urlRegistry, ICurrentHttpRequest currentHttpRequest)
        {
            _apiFinder = apiFinder;
            _urlRegistry = urlRegistry;
            _currentHttpRequest = currentHttpRequest;
        }

        //[AsymmetricJson]
        public ResourceDiscovery Execute()
        {
            var baseUrl = _urlRegistry.UrlFor<ResourceDiscoveryAction>(m => m.Execute());
            var absoluteBaseUrl = _currentHttpRequest.ToFullUrl(baseUrl);

            var apis = _apiFinder
                .ActionsByGroup()
                .Where(a=>a.Any(b=>b.HasOutput && b.OutputType() == typeof(ResourceDiscovery)) == false) //HACK filter out this action
                .Select(s =>
                            {
                                var description = "APIs for {0}".ToFormat(s.Key);

                                //UGH we need to make api urls relative for swagger to be happy. 
                                var resourceAPIRequestUrl = _urlRegistry.UrlFor(new ResourceDiscoveryAPIRequest {GroupKey = s.Key});
                                var resourceUrl = baseUrl.UrlRelativeTo(resourceAPIRequestUrl);

                                return new ResourceDiscoveryAPI
                                           {
                                               path = resourceUrl,
                                               description = description
                                           };
                            });

            
            return new ResourceDiscovery
                       {
                           basePath = absoluteBaseUrl,
                           apiVersion = Assembly.GetExecutingAssembly().GetVersion(),
                           swaggerVersion = "1.0",
                           apis = apis.ToArray()
                       };
        }
    }
}