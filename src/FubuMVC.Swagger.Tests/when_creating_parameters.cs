using System.Linq;
using System.Reflection;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Swagger.Specification;
using NUnit.Framework;

namespace FubuMVC.Swagger.Tests
{
    [TestFixture]
    public class when_creating_parameters
    {
        private Parameter _result;
        private PropertyInfo _property;
        private IRouteDefinition _route;

        [SetUp]
        public void Given()
        {
            var graph = new BehaviorGraph();
            var chain = graph.AddActionFor("api/action1/{input}/", typeof (Action1));

            _route = chain.Route;
            _property = ReflectionHelper.GetProperty<ActionRequest>(a => a.redfish);

            _result = ActionCallMapper.createParameter(_property, _route);
        }
        
        [Test]
        public void allow_multiple_should_be_false()
        {
            _result.allowMultiple.ShouldBeFalse();  
        }

        [Test]
        public void parameter_name_should_match_property()
        {
            _result.name.ShouldEqual(_property.Name);
        }

        [Test]
        public void data_type_should_match_property()
        {
            _result.dataType.ShouldEqual(_property.PropertyType.Name);
        }
        
        [Test]
        public void description_should_match_annotation()
        {
            _result.description.ShouldEqual(_property.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description);
        }

        [Test]
        public void default_value_should_match_annotation()
        {
            _result.defaultValue.ShouldEqual(_property.GetAttribute<System.ComponentModel.DefaultValueAttribute>().Value);
        }

        [Test]
        public void allowable_value_should_match_annotation()
        {
            var allowableValues = _result.allowableValues;
            allowableValues.valueType.ShouldEqual("LIST");

            var values = allowableValues.values;
            values.Count().ShouldEqual(2);
            values[0].ShouldEqual("value1");
            values[1].ShouldEqual("value2");
        }

        [Test]
        public void required_should_be_false_when_not_annotated()
        {
            _result.required.ShouldBeFalse();
        }
        
        [Test]
        public void required_should_be_pulled_from_data_annotation()
        {
            _property = ReflectionHelper.GetProperty<ActionRequest>(a => a.required);

            var result = ActionCallMapper.createParameter(_property, _route);

            result.required.ShouldBeTrue();
        }

    }
}