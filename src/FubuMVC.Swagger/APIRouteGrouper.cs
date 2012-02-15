using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Swagger
{
    public interface IActionGrouper
    {
        IEnumerable<IGrouping<string, ActionCall>> Group(IEnumerable<ActionCall> actions);
    }

    /// <summary>
    /// This will group actions by the second segment of the URL /api/{group}/. 
    /// </summary>
    public class APIRouteGrouper : IActionGrouper
    {
        public IEnumerable<IGrouping<string, ActionCall>> Group(IEnumerable<ActionCall> actions)
        {
            var partGroupExpression = new Regex(@"^/?api/(?<group>[a-zA-Z0-9_\-\.\+]+)/?");

            var groups = actions.GroupBy(a =>
                                             {
                                                 var pattern = a.ParentChain().Route.Pattern;
                                                 var match = partGroupExpression.Match(pattern);
                                                 return match.Success ? match.Groups["group"].Value : null;
                                             });

            return groups.Where(g => g.Key.IsNotEmpty()).ToArray();
        }
    }
}