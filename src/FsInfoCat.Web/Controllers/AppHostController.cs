using FsInfoCat.Models;
using FsInfoCat.Web.Data;
using FsInfoCat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FsInfoCat.Web.Controllers
{
    [Authorize(Roles = ModelHelper.ROLE_NAME_CONTENT_ADMIN)]
    public class AppHostController : Controller
    {
        private readonly FsInfoDataContext _context;
        private readonly ILogger<AppHostController> _logger;

        public AppHostController(FsInfoDataContext context, ILogger<AppHostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await AppHostIndexViewModel.Create(_context.HostDevice, User));
        }
    }
}
