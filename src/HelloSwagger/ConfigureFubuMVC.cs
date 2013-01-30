using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Swagger.Configuration;
using HelloSwagger.Handlers;
using HelloSwagger.Handlers.home;
using IApi = FubuMVC.Swagger.Configuration.IApi;

namespace HelloSwagger
{
    public class ApiConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Behaviors
                .Where(x => x.InputType().CanBeCastTo<IApi>())
                .Each(x => x.MakeAsymmetricJson());
        }
    }

    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            Routes.HomeIs<HomeRequest>();

            Policies.Add<ApiConvention>();

            Import<SwaggerApiDocumentationExtension>();
			Import<HandlerConvention>(x => x.MarkerType<HandlerMarker>());
        }
    }
}