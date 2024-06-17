using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly RedisService RedisService;
        protected readonly IDatabase Db;

        protected BaseController(RedisService redisService)
        {
            RedisService = redisService;
            Db = RedisService.GetDb(1);
        }
    }
}