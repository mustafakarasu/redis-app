using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase _db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            _db.StringSet("name", "Mustafa Karasu");
            _db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var value = _db.StringLength("name");

            // db.StringIncrement("iyaretci", 10);
            // var count = db.StringDecrementAsync("ziyaretci", 1).Result;

            _db.StringDecrementAsync("ziyaretci", 10).Wait();

            ViewBag.value = value.ToString();

            return View();
        }
    }
}