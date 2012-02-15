using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Swagger.Actions;
using FubuMVC.Swagger.Specification;

namespace FubuMVC.Swagger
{
    public class ActionCallMapper 
    {
        private readonly ITypeDescriptorCache _typeCache;

        public ActionCallMapper(ITypeDescriptorCache typeCache)
        {
            _typeCache = typeCache;
        }

        public IEnumerable<Operation> GetSwaggerOperations(ActionCall call)
        {
            var parameters = createParameters(call);
            var outputType = call.OutputType();
            var route = call.ParentChain().Route;

            return route.AllowedHttpMethods.Select(verb =>
                                          {
                                              var summary = call.InputType().GetAttribute<DescriptionAttribute>(d => d.Description);

                                              return new Operation
                                                         {
                                                             parameters = parameters.ToArray(),
                                                             httpMethod = verb,
                                                             responseTypeInternal = outputType.FullName,
                                                             responseClass = outputType.Name,
                                                             nickname = call.InputType().Name,
                                                             summary = summary,

                                                             //TODO not sure how we'd support error responses
                                                             errorResponses = new ErrorResponses[0],

                                                             //TODO get notes, nickname, summary from metadata?
                                                         };
                                          });
        }

        private IEnumerable<Parameter> createParameters(ActionCall call)
        {
            if (!call.HasInput) return new Parameter[0];

            var route = call.ParentChain().Route;

            var inputType = call.InputType();
            IEnumerable<PropertyInfo> properties = _typeCache.GetPropertiesFor(inputType).Values;
            return properties.Select(propertyInfo => createParameter(propertyInfo, route));
        }

        public static Parameter createParameter(PropertyInfo propertyInfo, IRouteDefinition route)
        {
            var parameter = new Parameter
                                {
                                    name = propertyInfo.Name,
                                    dataType = propertyInfo.PropertyType.Name,
                                    paramType = "post",
                                    allowMultiple = false,
                                    required = propertyInfo.HasAttribute<RequiredAttribute>(),
                                    description = propertyInfo.GetAttribute<DescriptionAttribute>(a => a.Description),
                                    defaultValue = propertyInfo.GetAttribute<DefaultValueAttribute>(a => a.Value.ToString()),
                                    allowableValues = getAllowableValues(propertyInfo)
                                };

            if (route.Input.RouteParameters.Any(r => r.Name == propertyInfo.Name))
                parameter.paramType = "path";

            if (route.Input.QueryParameters.Any(r => r.Name == propertyInfo.Name))
                parameter.paramType = "query";

            return parameter;
        }

        private static AllowableValues getAllowableValues(ICustomAttributeProvider propertyInfo)
        {
            var allowableValues = propertyInfo.GetAttribute<AllowableValuesAttribute>();

            if(allowableValues == null)
                return null;

            return new AllowableValues {valueType = "LIST", values = allowableValues.AllowableValues};
        }
    }
}