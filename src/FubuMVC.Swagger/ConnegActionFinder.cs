using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Swagger
{
    public class ConnegActionFinder
    {
        private readonly BehaviorGraph _graph;
        private readonly IActionGrouper _actionGrouper;
	    private readonly IActionFinder _actionFinder;

	    public ConnegActionFinder(BehaviorGraph graph, IActionGrouper actionGrouper, IActionFinder actionFinder)
        {
            _graph = graph;
            _actionGrouper = actionGrouper;
	        _actionFinder = actionFinder;
        }

        public IEnumerable<ActionCall> Actions()
        {
            var actions = _graph.Actions().Where(_actionFinder.Matches).ToArray();
            return actions;
        }

        //TODO - This should get abstracted out. Everyone will have a different take on their URL structure
        //this method groups actions by the second segment of the URL. 
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

	public interface IActionFinder	
	{
		Func<ActionCall, bool> Matches { get; }
	}
}