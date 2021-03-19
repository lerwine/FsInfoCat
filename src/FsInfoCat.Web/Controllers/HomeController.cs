using FsInfoCat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FsInfoCat.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FsInfoCat.Web.Data.FsInfoDataContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FsInfoCat.Web.Data.FsInfoDataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
