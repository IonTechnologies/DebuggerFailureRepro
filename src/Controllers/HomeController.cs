using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using debug_failure.Models;
using Microsoft.AspNetCore.Authorization;
using debug_failure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace debug_failure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDataContext _context;

        public ILogger<HomeController> Logger => _logger;

        public HomeController(ApplicationDataContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.FirstAsync();
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
