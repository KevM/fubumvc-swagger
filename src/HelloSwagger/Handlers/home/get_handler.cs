namespace HelloSwagger.Handlers.home
{
    public class get_handler
    {
        public HomeModel Execute(HomeRequest request)
        {
            return new HomeModel();
        }
    }

    public class HomeRequest { }

    public class HomeModel 
    {
    }
}