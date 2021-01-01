using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FsInfoCat.Web.Models;

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
