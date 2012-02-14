namespace FubuMVC.Swagger
{
    public static class StringExtensions
    {
        public static string UrlRelativeTo(this string baseUrl, string targetUrl)
        {
            var b = baseUrl;
            var t = targetUrl;

            var lastSlashIndex = b.LastIndexOf('/');
            if (lastSlashIndex == -1) return t;

            //find base url 
            b = b.Substring(0, lastSlashIndex + 1);

            //normalize leading slash on base and target
            if (!b.StartsWith("/")) b = "/" + b;
            if (!t.StartsWith("/")) t = "/" + t;

            //if base and target roots are the same remove the base root from target (making it relative).
            if (t.StartsWith(b))
                t = t.Remove(0, b.Length);

            //add a leading slash to the target because that makes swagger happy
            if (!t.StartsWith("/")) t = "/" + t;

            return t;
        }
    }
}