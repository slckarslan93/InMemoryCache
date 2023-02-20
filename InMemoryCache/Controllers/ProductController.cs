using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //1. Yol
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }
            //2. Yol
            if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}