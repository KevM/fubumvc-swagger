using FubuMVC.Core.Http;
using FubuMVC.Swagger.UI;

namespace FubuMVC.Swagger.Actions
{
    public class SwaggerUIAction
    {
        private readonly IHttpWriter _writer;
        private readonly SwaggerUICache _cache;

 
        public SwaggerUIAction(IHttpWriter writer, SwaggerUICache cache)
        {
            _writer = writer;
            _cache = cache;
        }

        public void Execute(SwaggerUIRequest request)
        {
            _writer.Write(_cache.Get().PageContents);
        }
    }

    public class SwaggerUIRequest
    {
    }
}