using FubuMVC.Core;

namespace FubuMVC.Swagger.Configuration
{
    public class SwaggerPackageRegistry : FubuPackageRegistry
    {
        public SwaggerPackageRegistry()
        {
            ApplyConvention<SwaggerConvention>();
        
            Services(s => s.AddService<IActionGrouper, APIRouteGrouper>());
        }
    }
}