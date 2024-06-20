using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase _db;

        private string _listKey = "HashNames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            var namesList = new HashSet<string>();

            if (_db.KeyExists(_listKey))
            {
                _db.SetMembers(_listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _db.KeyExpire(_listKey, DateTime.Now.AddMinutes(5));
            _db.SetAdd(_listKey, name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await _db.SetRemoveAsync(_listKey, name);

            return RedirectToAction("Index");
        }
    }
}