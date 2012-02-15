using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Resources.Conneg;

namespace FubuMVC.Swagger
{
    public class ConnegActionFinder
    {
        private readonly BehaviorGraph _graph;
        private readonly IActionGrouper _actionGrouper;

        public ConnegActionFinder(BehaviorGraph graph, IActionGrouper actionGrouper)
        {
            _graph = graph;
            _actionGrouper = actionGrouper;
        }

        public IEnumerable<ActionCall> Actions()
        {
            var actions = _graph.Actions().Where(a => a.ParentChain().HasConnegOutput()).ToArray();
            return actions;
        }

        public IEnumerable<IGrouping<string, ActionCall>> ActionsByGroup()
        {
            return _actionGrouper.Group(Actions());
        }

        public IEnumerable<ActionCall> ActionsForGroup(string name)
        {
            var group = ActionsByGroup().FirstOrDefault(g => g.Key.ToLowerInvariant() == name);

            return @group ?? (IEnumerable<ActionCall>) new ActionCall[0];
        }
    }
}