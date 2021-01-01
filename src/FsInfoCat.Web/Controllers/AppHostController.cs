using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FsInfoCat.Models;
using FsInfoCat.Models.DB;
using FsInfoCat.Web.Data;
using FsInfoCat.Web.Models;

namespace FsInfoCat.Web.Controllers
{
    [Authorize(Roles = ModelHelper.Role_Name_Any_Contrib)]
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
