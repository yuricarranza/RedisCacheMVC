using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisTestWebMVC.Models;
using RedisTestWebMVC.Repository;
using System.Diagnostics;
using System.Text.Json;

namespace RedisTestWebMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            List<User>? users;
            string message = string.Empty;
            string recordKey = $"Users_{DateTime.Now:yyyyMMdd}";
            Stopwatch stopwatch = Stopwatch.StartNew();
            var jsonData = await _cache.GetStringAsync(recordKey);            

            if (jsonData is null) // Data not available in the Cache
            {
                users = await _userRepository.GetUsersAsync(); // Read data from database
                var options = new DistributedCacheEntryOptions();

                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                options.SlidingExpiration = null;

                var jsonUsersData = JsonSerializer.Serialize(users);
                await _cache.SetStringAsync(recordKey, jsonUsersData, options);
                stopwatch.Stop();
                message = $"Got from database, took {stopwatch.Elapsed.TotalMilliseconds} millisecons";
            }
            else
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonData);
                stopwatch.Stop();
                message = $"Got from cache, took {stopwatch.Elapsed.TotalMilliseconds} millisecons";
            }
            ViewData["Message"] = message;
            return View(users); // Return data
        }
    }
}
