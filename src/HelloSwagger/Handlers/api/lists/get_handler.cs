using System.ComponentModel;
using System.Linq;
using FubuMVC.Swagger.Configuration;
using HelloSwagger.Handlers.api.lists.show;

namespace HelloSwagger.Handlers.api.lists
{
    public class get_handler
    {
        public ListsModel Execute(ListsRequest request)
        {
            return new ListsModel {Lists = ListsOf.Lists.Keys.ToArray()};
        }
    }

    [Description("A listing of lists")]
    public class ListsRequest : IApi 
    {
    }

    public class ListsModel 
    {
        public string[] Lists { get; set; }
    }
}