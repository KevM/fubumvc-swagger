using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using NUnit.Framework;

namespace FubuMVC.Swagger.Tests
{
    [TestFixture]
    public class when_creating_parameters_paramTypes
    {
        private IRouteDefinition _route;

        [SetUp]
        public void Given()
        {
            var graph = new BehaviorGraph();
            var chain = graph.AddActionFor("api/action1/{input}/", typeof(Action1));

            _route = chain.Route;
        }

        [Test]
        public void for_route_input_should_have_paramType_path()
        {
            var property = ReflectionHelper.GetProperty<ActionRequest>(a => a.input);
            
            var result = ActionCallMapper.createParameter(property, _route);

            result.paramType.ShouldEqual("path");
        }

        [Test]
        public void for_querystring_input_should_have_paramType_query()
        {
            var property = ReflectionHelper.GetProperty<ActionRequest>(a => a.query);

            var result = ActionCallMapper.createParameter(property, _route);

            result.paramType.ShouldEqual("query");
        }

        [Test]
        public void for_normal_input_property_should_have_paramType_post()
        {
            var property = ReflectionHelper.GetProperty<ActionRequest>(a => a.redfish);

            var result = ActionCallMapper.createParameter(property, _route);

            result.paramType.ShouldEqual("post");
        }
    }
}