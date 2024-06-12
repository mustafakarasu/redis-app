using System;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                // options.SlidingExpiration = TimeSpan.FromSeconds(10);
                Priority = CacheItemPriority.High
            };

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key} -> {value} => Sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            var product = new Product { Id = 1, Name = "Kalem", Price = 200 };

            _memoryCache.Set<Product>("product:1", product);
            _memoryCache.Set<double>("money", 100.99);

            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string timeCache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = timeCache;
            ViewBag.callback = callback;

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}