using NUnit.Framework;

namespace FubuMVC.Swagger.Tests
{
    [TestFixture]
    public class base_url_relative_to_target
    {
        [Test]
        public void resource_relativity()
        {
            var result = "/api/resource.json".UrlRelativeTo("/api/foo/resource.json");

            result.ShouldEqual("/foo/resource.json");
        }
        
        [Test]
        public void target_without_leading_slash_should_work()
        {
            var result = "/api/resource.json".UrlRelativeTo("api/foo/resource.json");

            result.ShouldEqual("/foo/resource.json");
        }

        [Test]
        public void base_without_leading_slash_should_work()
        {
            var result = "api/resource.json".UrlRelativeTo("/api/foo/resource.json");

            result.ShouldEqual("/foo/resource.json");
        }

        [Test]
        public void no_leading_slashs_should_work()
        {
            var result = "api/resource.json".UrlRelativeTo("api/foo/resource.json");

            result.ShouldEqual("/foo/resource.json");
        }
    }
}