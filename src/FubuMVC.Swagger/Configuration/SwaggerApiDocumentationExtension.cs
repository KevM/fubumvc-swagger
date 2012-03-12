using FubuMVC.Core;

namespace FubuMVC.Swagger.Configuration
{
    public class SwaggerApiDocumentationExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.ApplyConvention<SwaggerConvention>();
            registry.Services(s=>s.AddService<IActionGrouper, APIRouteGrouper>());
        }
    }
}