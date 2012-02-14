using FubuMVC.Core;

namespace FubuMVC.Swagger
{
    public class SwaggerPackageRegistry : FubuPackageRegistry
    {
        public SwaggerPackageRegistry()
        {
            ApplyConvention<SwaggerConvention>();
        }
    }
}