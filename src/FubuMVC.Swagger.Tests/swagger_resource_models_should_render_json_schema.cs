using System;
using System.IO;
using FubuMVC.Swagger.Specification;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace FubuMVC.Swagger.Tests
{
    [JsonObjectAttribute("1")]
    public class TestOutputModel
    {
        public string id { get; set; }
        public int count { get; set; }
        public DateTime when { get; set; }
    }

    [JsonObjectAttribute("2")]
    public class TestOutputModel2
    {
        public string id { get; set; }
    }

    [JsonObject("3")]
    public class TestOutputModel3
    {
        public string id { get; set; }
        public TestOutputModel2[] Output2Models { get; set; }
    }

    public class HasNested
    {
        public string nested { get; set; }
        public TestOutputModel2 Output2Model { get; set; }
        public TestOutputModel3[] Output3Models { get; set; }
    }

    [TestFixture]
    public class swagger_resource_models_should_render_json_schema
    {
        private Resource _resource;

        [SetUp]
        public void beforeEach()
        {
            _resource = new Resource
            {
                apiVersion = "1.0",
                basePath = "basePath",
                resourcePath = "resourcePath",
                swaggerVersion = "wwee!",
                apis = new[] { new API { path = "apiPath", description = "wee!", operations = new Operation[] { } } },
            };
        }

        [Test]
        public void one_model()
        {
            var type = typeof(TestOutputModel);
            _resource.models = new[] { type };
            var modelJsonSchema = getJsonSchema(typeof(TestOutputModel));

            var json = serializeToJson(_resource);

            StringAssert.Contains(modelJsonSchema, json);
        }

        [Test]
        public void another_model()
        {
            var type = typeof(TestOutputModel3);
            _resource.models = new[] { type };
            var modelJsonSchema = getJsonSchema(type);

            var json = serializeToJson(_resource);

            StringAssert.Contains(modelJsonSchema, json);
        }

        [Test]
        public void multiple_models()
        {
            var type1 = typeof(TestOutputModel);
            var type2 = typeof(TestOutputModel2);
            _resource.models = new[] { type1, type2 };
            var type1JsonSchema = getJsonSchema(type1);
            var type2JsonSchema = getJsonSchema(type2);

            var json = serializeToJson(_resource);

            StringAssert.Contains(type1JsonSchema, json);
            StringAssert.Contains(type2JsonSchema, json);
        }

        [Test]
        [Explicit("According to the swagger spec. This test should fail as nested schemas should not be populated.")]
        public void nested()
        {
            var type = typeof(HasNested);
            _resource.models = new[] { type };
            var jsonSchema = getJsonSchema(type);

            var json = serializeToJson(_resource);

            StringAssert.Contains(jsonSchema, json);
        }

        private static string serializeToJson(Resource resource)
        {
            var json = JsonConvert.SerializeObject(resource, Formatting.None);
            return json;
        }

        private static string getJsonSchema(Type type)
        {
            var sw = new StringWriter();
            var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None };
            var jSchema = new JsonSchemaGenerator { UndefinedSchemaIdHandling = UndefinedSchemaIdHandling.UseAssemblyQualifiedName }.Generate(type);
            jSchema.Id = type.Name;
            jSchema.WriteTo(jtw);

            return sw.ToString();
        }
    }
}