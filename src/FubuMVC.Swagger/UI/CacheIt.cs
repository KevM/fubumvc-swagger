using System.Web;
using System.Web.Caching;

namespace FubuMVC.Swagger.UI
{
    public abstract class CacheIt<T> where T : class
    {
        private readonly Cache _cache;
        protected abstract T OnMissing(string key);

        protected CacheIt()
        {
            _cache = HttpContext.Current.Cache;
        }

        public T Get(string key)
        {
            var result = _cache.Get(key);

            if (result == null)
            {
                result = OnMissing(key);
                _cache.Insert(key, result);
            }

            return (T) result;
        }
    }
}