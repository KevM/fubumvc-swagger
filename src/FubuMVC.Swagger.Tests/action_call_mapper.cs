using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Swagger.Specification;
using NUnit.Framework;

namespace FubuMVC.Swagger.Tests
{
    public class action_call_mapper 
    {
        private Operation _result;

        [SetUp]
        public void Given()
        {
            var cut = new ActionCallMapper(new TypeDescriptorCache());
            
            var graph = new BehaviorGraph();
            var chain = graph.AddActionFor("api/group1/{input}", typeof(Action1));
            chain.Route.AddHttpMethodConstraint("POST");
            var action = graph.Actions().First();
            
            _result = cut.GetSwaggerOperations(action).First();
        }

        [Test]
        public void httpMethod_should_match_verb()
        {
            _result.httpMethod.ShouldEqual("POST");
        }

        [Test]
        public void summary_should_be_based_on_annotation()
        {
            var inputTypeDescription = typeof (ActionRequest).GetAttribute<System.ComponentModel.DescriptionAttribute>().Description;

            _result.summary.ShouldEqual(inputTypeDescription);
        }

        [Test]
        public void nickname_should_be_input_type_name()
        {
            _result.nickname.ShouldEqual(typeof(ActionRequest).Name);
        }

        [Test]
        public void response_class_should_be_output_type_name()
        {
            _result.responseClass.ShouldEqual(typeof(ActionResult).Name);
        }

        [Test]
        public void response_type_internal_should_be_output_type_fullname()
        {
            _result.responseTypeInternal.ShouldEqual(typeof(ActionResult).FullName);
        }

        [Test]
        public void parameters_should_match_input_type_properties()
        {
            _result.parameters.Count().ShouldEqual(typeof (ActionRequest).GetProperties().Count());
        }
    }

    public class action_call_mapper_get_route_verbs
    {
        private IRouteDefinition _route;

        [SetUp]
        public void Given()
        {
            var graph = new BehaviorGraph();
            _route = graph.AddActionFor("api/group1/{input}", typeof (Action1)).Route;
        }

        [Test]
        public void when_no_methods_are_allowed_default_to_GET()
        {
            var result = ActionCallMapper.getRouteVerbs(_route).ToArray();

            result.Count().ShouldEqual(1);
            result[0].ShouldEqual("GET");
        }

        [Test]
        public void should_return_allowed_route_methods()
        {
            _route.AddHttpMethodConstraint("POST");
            _route.AddHttpMethodConstraint("PUT");
            
            var result = ActionCallMapper.getRouteVerbs(_route).ToArray();

            result.Count().ShouldEqual(2);
            result.Contains("POST");
            result.Contains("PUT");
        }
    }
}