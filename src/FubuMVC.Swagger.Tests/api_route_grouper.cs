using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Resources.Conneg;
using NUnit.Framework;

namespace FubuMVC.Swagger.Tests
{
    public class api_route_grouper : Context<APIRouteGrouper>
    {
        private IEnumerable<IGrouping<string, ActionCall>> _groupedActions;

        public override void Given()
        {
            var actions = getAPIActions();

            _groupedActions = _cut.Group(actions);
        }

        [Test]
        public void should_find_three_groups()
        {
            _groupedActions.Count().ShouldEqual(3);
        }

        [Test]
        public void should_have_group1_actions()
        {
            var group1Actions = _groupedActions.First(a => a.Key == "group1");
            group1Actions.First().ParentChain().Route.Pattern.ShouldStartWith("api/group1");
        }
        
        [Test]
        public void should_have_group2_actions()
        {
            var group1Actions = _groupedActions.First(a => a.Key == "group2");
            
            group1Actions.Count().ShouldEqual(2);
            group1Actions.First().ParentChain().Route.Pattern.ShouldStartWith("api/group2");
        }

        [Test]
        public void should_have_group3_actions()
        {
            var group1Actions = _groupedActions.First(a => a.Key == "group3");

            group1Actions.Count().ShouldEqual(3);
            group1Actions.First().ParentChain().Route.Pattern.ShouldStartWith("api/group3");
        }

        private static IEnumerable<ActionCall> getAPIActions()
        {
            var graph = new BehaviorGraph();

            graph.AddActionFor("api/group1/{Id}", typeof (Action1));
            graph.AddActionFor("api/group2/{Id}", typeof(Action1));
            graph.AddActionFor("api/group2/foo/", typeof(Action1));
            graph.AddActionFor("api/group3", typeof(Action1));
            graph.AddActionFor("api/group3/foobar", typeof(Action1));
            graph.AddActionFor("api/group3/{star}", typeof(Action1));
            graph.AddActionFor("home", typeof(Action1));
            graph.AddActionFor("home/foo", typeof(Action1));
            graph.AddActionFor("home/foo/baz", typeof(Action1));
            graph.AddActionFor("bar", typeof(Action1));

            return graph.Actions();
        }

        public class Action1
        {
            public void Execute()
            {
            }
        }
    }
}