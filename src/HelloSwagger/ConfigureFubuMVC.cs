using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Spark;
using FubuMVC.Swagger.Configuration;
using HelloSwagger.Handlers;
using HelloSwagger.Handlers.home;

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
            IncludeDiagnostics(true);

            ApplyHandlerConventions<HandlerMarker>();

            this.UseSpark();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();

            Routes.HomeIs<HomeRequest>();

            ApplyConvention<ApiConvention>();

            Import<SwaggerApiDocumentationExtension>();
        }
    }
}