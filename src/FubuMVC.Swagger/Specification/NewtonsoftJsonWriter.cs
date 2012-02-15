using FubuMVC.Core.Runtime;
using Newtonsoft.Json;

namespace FubuMVC.Swagger.Specification
{
    public class NewtonsoftJsonWriter : IJsonWriter
    {
        private readonly IOutputWriter _outputWriter;

        public NewtonsoftJsonWriter(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public void Write(object output)
        {
            Write(output, MimeType.Json.ToString());
        }

        public void Write(object output, string mimeType)
        {
            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var json = JsonConvert.SerializeObject(output, Formatting.None, serializerSettings);
            _outputWriter.Write(mimeType, json);
        }
    }
}