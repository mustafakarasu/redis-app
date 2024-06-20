using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        private string HashKey { get; set; } = "Sozluk";

        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            var list = new Dictionary<string, string>();

            if (Db.KeyExists(HashKey))
            {
                Db.HashGetAll(HashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string val)
        {
            Db.HashSet(HashKey, name, val);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            Db.HashDelete(HashKey, name);
            return RedirectToAction("Index");
        }
    }
}