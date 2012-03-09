using System.IO;
using System.Text.RegularExpressions;
using FubuMVC.Core.Http;
using System.Web;
using System.Web.Caching;

namespace FubuMVC.Swagger.Actions
{
    public class SwaggerUIAction
    {
        private readonly IHttpWriter _writer;
        private readonly ICurrentHttpRequest _currentRequest;
        private readonly Regex _resourceExpression = new Regex(@"'(?<kind>stylesheets|javascripts)/", RegexOptions.Compiled);
        private readonly Regex _baseUrlExpression = new Regex(@"(?<baseId>id=\""input_baseUrl\"")", RegexOptions.Compiled);

        public SwaggerUIAction(IHttpWriter writer, ICurrentHttpRequest currentRequest)
        {
            _writer = writer;
            _currentRequest = currentRequest;
        }

        public void Execute(SwaggerUIRequest request)
        {
            object cachedPage = HttpContext.Current.Cache.Get("swagger-ui");
            string result;
            if (cachedPage == null)
            {
                //read in swagger index.html
                var swaggerIndexPath = HttpContext.Current.Server.MapPath("~/content/swagger-ui/index.html");
                var content = File.ReadAllText(swaggerIndexPath);

                //update stylesheets and scripts to have urls relative to website root
                var url = _currentRequest.RawUrl();
                var swaggerUIUrl = url.Replace("/api", "/content/swagger-ui");
                content = _resourceExpression.Replace(content, "'" + swaggerUIUrl + "/${kind}/");
                result = _baseUrlExpression.Replace(content, "${baseId} value=\"" + _currentRequest.FullUrl() + "\"");
                HttpContext.Current.Cache.Insert("swagger-ui", result, new CacheDependency(swaggerIndexPath));
            }
            else
                result = (string) cachedPage;

            _writer.Write(result);
        }
    }

    public class SwaggerUIRequest
    {
    }
}