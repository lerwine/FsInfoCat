using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;

namespace FsInfoCat.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<MediaHost>>> GetAll()
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<IEnumerable<MediaHost>>)null);
            return await _context.MediaHost.ToListAsync();
        }

        // GET: api/MediaHost/get/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaHost>> GetById(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<MediaHost>)null);
            return await _context.MediaHost.FindAsync(id);
        }

        public static readonly Regex DottedNameRegex = new Regex(MediaHost.PATTERN_DOTTED_NAME, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // POST: api/MediaHost
        [HttpPost]
        public async Task<ActionResult<MediaHost>> Register(MediaHostRegRequest host)
        {
            string uc;
            if (null == host || string.IsNullOrWhiteSpace(host.MachineName) || !DottedNameRegex.IsMatch((uc = host.MachineName.ToUpper())) || null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((MediaHost)null);
            return await _context.MediaHost.FirstOrDefaultAsync(h => String.Equals(uc, h.MachineName, StringComparison.InvariantCulture)).ContinueWith<MediaHost>(task =>
            {
                if (null != task.Result)
                {
                    if (task.Result.IsWindows != host.IsWindows)
                        return null;
                    return task.Result;
                }
                MediaHost result = new MediaHost();
                result.HostID = Guid.NewGuid();
                result.CreatedBy = result.ModifiedBy = User.Identity.Name;
                result.CreatedOn = result.ModifiedOn = DateTime.Now;
                result.IsInactive = false;
                result.IsWindows = host.IsWindows;
                result.MachineName = uc;
                _context.MediaHost.Add(result);
                _context.SaveChanges();
                return result;
            });
        }

        // DELETE: api/MediaHost/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> UnRegister(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult(false);
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
        public async Task<ActionResult<bool>> Activate(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult(false);
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
        public async Task<ActionResult<bool>> Deactivate(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult(false);
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
