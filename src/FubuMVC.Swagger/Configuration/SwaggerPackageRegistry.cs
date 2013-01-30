using FubuMVC.Core;

namespace FubuMVC.Swagger.Configuration
{
    public class SwaggerPackageRegistry : FubuPackageRegistry
    {
        public SwaggerPackageRegistry()
        {
			Policies.Add<SwaggerConvention>();
        
            Services(s => s.AddService<IActionGrouper, APIRouteGrouper>());
			Services(s => s.AddService<IActionFinder, APIRouteFinder>());
        }
    }
}