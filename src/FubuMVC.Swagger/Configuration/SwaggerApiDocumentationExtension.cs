using FubuMVC.Core;

namespace FubuMVC.Swagger.Configuration
{
    public class SwaggerApiDocumentationExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<SwaggerConvention>();
            registry.Services(s=>s.AddService<IActionGrouper, APIRouteGrouper>());
			registry.Services(s => s.AddService<IActionFinder, APIRouteFinder>());
        }
    }
}