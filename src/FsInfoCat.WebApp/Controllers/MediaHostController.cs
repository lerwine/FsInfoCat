using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;

namespace FsInfoCat.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaHostController : ControllerBase
    {
        private readonly FsInfoCat.WebApp.Data.FsInfoDataContext _context;

        public MediaHostController(FsInfoCat.WebApp.Data.FsInfoDataContext context)
        {
            _context = context;
        }

        // private readonly ILogger<MediaHostController> _logger;

        // public MediaHostController(ILogger<MediaHostController> logger)
        // {
        //     _logger = logger;
        // }

        // GET: api/MediaHost/all
        [HttpGet("all")]
        [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<IEnumerable<MediaHost>>> GetAll()
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<IEnumerable<MediaHost>>)null);
            return await _context.MediaHost.ToListAsync();
        }

        // GET: api/MediaHost/get/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<MediaHost>> GetById(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<MediaHost>)null);
            return await _context.MediaHost.FindAsync(id);
        }

        public static readonly Regex DottedNameRegex = new Regex(MediaHost.PATTERN_DOTTED_NAME, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // POST: api/MediaHost
        [HttpPost]
        [Authorize(Roles = AppUser.Role_Name_Crawler)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<MediaHost>>> Register(MediaHostRegRequest host)
        {
            string uc;
            if (null == host || string.IsNullOrWhiteSpace(host.MachineName) || !DottedNameRegex.IsMatch((uc = host.MachineName.ToUpper())))
                return await Task.FromResult(new RequestResponse<MediaHost>(null, "Registration failed."));
            return await _context.MediaHost.FirstOrDefaultAsync(h => String.Equals(uc, h.MachineName, StringComparison.InvariantCulture)).ContinueWith<RequestResponse<MediaHost>>(task =>
            {
                RequestResponse<MediaHost> result;
                if (null != task.Result)
                    return new RequestResponse<MediaHost>(task.Result, (task.Result.IsWindows == host.IsWindows) ? "" : "System type mismatch.");
                result = new RequestResponse<MediaHost>(new MediaHost(uc, host.IsWindows, new Guid(User.Identity.Name)));
                _context.MediaHost.Add(result.Result);
                _context.SaveChanges();
                return result;
            });
        }

        // DELETE: api/MediaHost/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = AppUser.Role_Name_Admin)]
        public async Task<ActionResult<bool>> UnRegister(Guid id)
        {
            return await _context.MediaHost.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result)
                    return false;
                _context.MediaHost.Remove(task.Result);
                _context.SaveChanges();
                return true;
            });
        }

        // GET: api/MediaHost/activate/{id}
        [HttpGet("activate/{id}")]
        [Authorize(Roles = AppUser.Role_Name_Crawler)]
        public async Task<ActionResult<bool>> Activate(Guid id)
        {
            return await _context.MediaHost.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result || !task.Result.IsInactive)
                    return false;
                task.Result.IsInactive = false;
                _context.MediaHost.Update(task.Result);
                _context.SaveChanges();
                return true;
            });
        }

        // GET: api/MediaHost/deactivate/[id]
        [HttpGet("deactivate/{id}")]
        [Authorize(Roles = AppUser.Role_Name_Crawler)]
        public async Task<ActionResult<bool>> Deactivate(Guid id)
        {
            return await _context.MediaHost.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result || task.Result.IsInactive)
                    return false;
                task.Result.IsInactive = true;
                _context.MediaHost.Update(task.Result);
                _context.SaveChanges();
                return true;
            });
        }
    }
}
