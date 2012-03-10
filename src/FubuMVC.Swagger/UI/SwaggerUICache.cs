using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using FubuMVC.Core.Http;

namespace FubuMVC.Swagger.UI
{
    public class SwaggerUICache : CacheIt<SwaggerUIIndex>
    {
        private readonly Regex _resourceExpression = new Regex(@"'(?<kind>stylesheets|javascripts)/", RegexOptions.Compiled);
        private readonly Regex _baseUrlExpression = new Regex(@"(?<baseId>id=\""input_baseUrl\"")", RegexOptions.Compiled);
        private readonly ICurrentHttpRequest _currentRequest;

        public SwaggerUICache(ICurrentHttpRequest currentRequest)
        {
            _currentRequest = currentRequest;
        }

        public SwaggerUIIndex Get()
        {
            return this.Get("swagger-ui-cached-index");
        }

        protected override SwaggerUIIndex OnMissing(string key)
        {
            var swaggerIndexPath = HttpContext.Current.Server.MapPath("~/content/swagger-ui/index.html");

            if(!File.Exists(swaggerIndexPath))
            {
                //load from resource
            }
            var content = File.ReadAllText(swaggerIndexPath);

            var rewrittenContents = RewriteSwaggerUI(content);

            return new SwaggerUIIndex { PageContents = rewrittenContents };
        }

        // hacky assumes /api is this action's route
        private string RewriteSwaggerUI(string content)
        {
            //update stylesheets and scripts to have urls relative to website root
            var url = _currentRequest.RawUrl();
            var swaggerUIUrl = url.Replace("/api", "/content/swagger-ui");
            content = _resourceExpression.Replace(content, "'" + swaggerUIUrl + "/${kind}/");
            var result = _baseUrlExpression.Replace(content, "${baseId} value=\"" + ToPublicUrl(url) + "\"");
            return result;
        }

        public string ToPublicUrl(string relativeUri)
        {
            var httpContext = HttpContext.Current;

            var uriBuilder = new UriBuilder
            {
                Host = httpContext.Request.Url.Host,
                Path = "/",
                Port = 80,
                Scheme = "http",
            };

            if (httpContext.Request.IsLocal)
            {
                uriBuilder.Port = httpContext.Request.Url.Port;
            }

            return new Uri(uriBuilder.Uri, relativeUri).AbsoluteUri;
        }


    }
}