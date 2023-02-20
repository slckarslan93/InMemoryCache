using InMemoryCache.Models;
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
            ////1. Yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}
            //2. Yol
            //if (!_memoryCache.TryGetValue("zaman", out string zamanCache)){}

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callBack", $"{key}-->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product product = new Product { Id = 1, Name = "kalem", Price = 200 };

            _memoryCache.Set<Product>("product:1", product);

            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue("zaman", out string zamanCache);
            _memoryCache.TryGetValue("callBack", out string callBack);
            ViewBag.zaman = zamanCache;
            ViewBag.callBack = callBack;

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            //ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}