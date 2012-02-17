using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Swagger.Actions;

namespace FubuMVC.Swagger
{
    public class SwaggerConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            //add resource discovery action and force it to return JSON
            graph.AddActionFor("api", typeof(SwaggerUIAction));

            ////TODO should this route '/api' be configurable?
            //add resource discovery action and force it to return JSON
            graph.AddActionFor("api/resources.json", typeof(ResourceDiscoveryAction)).MakeAsymmetricJson();

            //add resource action and force it to return JSON
            graph.AddActionFor("api/{GroupKey}.json", typeof(ResourceAction)).MakeAsymmetricJson();
        }
    }
}
