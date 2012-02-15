using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Swagger.Specification;

namespace FubuMVC.Swagger.Configuration
{
    public class SwaggerPackageRegistry : FubuPackageRegistry
    {
        public SwaggerPackageRegistry()
        {
            ApplyConvention<SwaggerConvention>();
        
            Services(s =>
                         {
                             s.AddService<IActionGrouper, APIRouteGrouper>();
                             s.ReplaceService<IJsonWriter, NewtonsoftJsonWriter>();
                         });
        }
    }
}