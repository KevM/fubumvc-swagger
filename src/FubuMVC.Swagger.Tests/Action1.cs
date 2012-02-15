using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FubuMVC.Core;
using FubuMVC.Swagger.Specification;

namespace FubuMVC.Swagger.Tests
{
    [Description("An api description")]
    public class ActionRequest
    {
        [RouteInput, Description("Namey name name"), Required]
        public string input { get; set; }
        
        [QueryString, Description("Queryee query")]
        public string query { get; set; }
        
        [Description("Fishy fish")]
        [AllowableValues("value1", "value2")]
        [DefaultValue("value1")]
        public string redfish { get; set; }

        [Required]
        public bool required { get; set; }

        public bool notrequired { get; set; }
    }

    public class Action1
    {
        public void Execute(ActionRequest request)
        {
        }
    }
}