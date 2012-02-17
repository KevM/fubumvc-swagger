using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelloSwagger.Handlers.api.lists.show
{
    public class ListsOf
    {
        public static Dictionary<string, ListElement[]> Lists { get; set; }

        static ListsOf()
        {
            Lists = new Dictionary<string, ListElement[]>
                         {
                             {"Astronauts", new[]{new ListElement("Michael Collins", 3), new ListElement("Buzz Aldrin", 2) {IsDefault = true}, new ListElement("Neil Armstrong", 1) }},
                             {"Latin American Artists", new[]{new ListElement("Raúl Anguiano", 3), new ListElement("Veronica Ruiz de Velasco", 2), new ListElement("Carlos Francisco Chang Marín", 1) }},
                             {"American Scientists", new[]{new ListElement("Samuel Wendell Williston", 3), new ListElement("William Hultz Walker", 2), new ListElement("Philip Morrison", 1) }}
                         };
        }
    }

    public class get_Name_handler
    {
        public ShowModel Execute(ShowRequest request)
        {
            var result = new ShowModel { Name = request.Name };

            if (!ListsOf.Lists.ContainsKey(request.Name))
            {
                //should throw 404
                return result;
            }

            result.Elements = ListsOf.Lists[request.Name];
            return result;
        }
    }

    [Description("Display all elements of a given list.")]
    public class ShowRequest : IApi 
    {
        [Required, Description("Get list by name")]
        public string Name { get; set; }
    }

    public class ShowModel 
    {
        public ShowModel()
        {
            Elements = new ListElement[0];
        }

        public string Name { get; set; }
        public ListElement[] Elements { get; set; }
    }

    public class ListElement
    {
        public ListElement(string title, int rank)
        {
            Rank = rank;
            Title = title;
        }

        public string Title { get; set; }
        public int Rank { get; set; }
        public bool IsDefault { get; set; }
    }
}