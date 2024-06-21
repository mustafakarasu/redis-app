using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase _db;

        private string _listKey = "SortedSetNames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            var list = new HashSet<string>();

            if (_db.KeyExists(_listKey))
            {
                _db.SortedSetScan(_listKey).ToList().ForEach(number =>
                {
                    list.Add(number.ToString());
                });

                _db.SortedSetRangeByRank(_listKey, 0, 5, order: Order.Descending)
                    .ToList()
                    .ForEach(number =>
                    {
                        list.Add(number.ToString());
                    });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            _db.SortedSetAdd(_listKey, name, score);
            _db.KeyExpire(_listKey, DateTime.Now.AddMinutes(1));
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _db.SortedSetRemove(_listKey, name);

            return RedirectToAction("Index");
        }
    }
}