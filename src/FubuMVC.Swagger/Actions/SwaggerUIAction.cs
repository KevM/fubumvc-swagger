using System.IO;
using System.Net;
using System.Reflection;
using FubuMVC.Core.Http;

namespace FubuMVC.Swagger.Actions
{
    public class SwaggerUIAction
    {
        private readonly IHttpWriter _writer;

        public SwaggerUIAction(IHttpWriter writer)
        {
            _writer = writer;
        }

        public void Execute(SwaggerUIRequest request)
        {
            const string resourceName = "FubuMVC.Swagger.swagger_ui.index.html"; 

            using(var resourceStream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(resourceName))
            {
                if(resourceStream == null)
                {
                    _writer.WriteResponseCode(HttpStatusCode.NotFound);
                    _writer.Write("Swagger UI resource could not be found.");
                    return;
                }

                string output;
                using (var reader = new StreamReader(resourceStream))
                {
                    output = reader.ReadToEnd();
                }

                //HACK have output javascript, css, and baseURL parameters re-written to actually work.
                _writer.Write(output);
            }
        }
    }

    public class SwaggerUIRequest
    {
    }
}