using System;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Swagger.Configuration
{
	public class APIRouteFinder : IActionFinder
	{
		public Func<ActionCall, bool> Matches { get { return isApi;}}

		private static bool isApi(ActionCall actionCall)
		{
			return actionCall.ParentChain().InputType().CanBeCastTo<IApi>();
		}
	}
}