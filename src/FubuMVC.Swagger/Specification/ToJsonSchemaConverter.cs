using System;
using FubuCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using JsonWriter = Newtonsoft.Json.JsonWriter;

namespace FubuMVC.Swagger.Specification
{
    public class ToJsonSchemaConverter : JsonConverter
    {
        private readonly JsonSchemaGenerator _schemaGenerator;

        public ToJsonSchemaConverter()
        {
            _schemaGenerator = new JsonSchemaGenerator
                                   {
                                       UndefinedSchemaIdHandling = UndefinedSchemaIdHandling.UseAssemblyQualifiedName
                                   };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var types = (Type[])value;
            writer.WriteStartObject();

            foreach (var type in types)
            {
                writer.WritePropertyName(type.Name);
                var jsonSchema = _schemaGenerator.Generate(type);
                jsonSchema.Id = type.Name;
                jsonSchema.WriteTo(writer);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            var canBeCastToTypeArray = objectType.CanBeCastTo<Type[]>();

            return canBeCastToTypeArray;
        }

        public override bool CanRead
        {
            get { return false; }
        }
    }
}