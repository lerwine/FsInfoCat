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
using FsInfoCat.Models.DB;

namespace FsInfoCat.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class HostDeviceController : ControllerBase
    {
        private readonly FsInfoCat.WebApp.Data.FsInfoDataContext _context;

        public HostDeviceController(FsInfoCat.WebApp.Data.FsInfoDataContext context)
        {
            _context = context;
        }

        // private readonly ILogger<HostDeviceController> _logger;

        // public HostDeviceController(ILogger<HostDeviceController> logger)
        // {
        //     _logger = logger;
        // }

        // GET: api/HostDevice/all
        [HttpGet("all")]
        // [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<IEnumerable<HostDevice>>> GetAll()
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<IEnumerable<HostDevice>>)null);
            return await _context.HostDevice.ToListAsync();
        }

        // GET: api/HostDevice/get/{id}
        [HttpGet("{id}")]
        // [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<HostDevice>> GetById(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<HostDevice>)null);
            return await _context.HostDevice.FindAsync(id);
        }

        public static readonly Regex DottedNameRegex = new Regex(ModelHelper.PATTERN_DOTTED_NAME, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        // POST: api/HostDevice
        [HttpPost]
        // [Authorize(Roles = AppUser.Role_Name_Crawler)]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<HostDevice>>> Register(HostDeviceRegRequest host)
        {
            string machineName;
            if (null == host || string.IsNullOrWhiteSpace(host.MachineName) || !DottedNameRegex.IsMatch((machineName = host.MachineName.ToUpper())))
                return await Task.FromResult(new RequestResponse<HostDevice>(null, "Registration failed."));
            return await _context.HostDevice.FirstOrDefaultAsync(h => String.Equals(machineName, h.MachineName, StringComparison.InvariantCulture)).ContinueWith<RequestResponse<HostDevice>>(task =>
            {
                RequestResponse<HostDevice> result;
                if (null != task.Result)
                    return new RequestResponse<HostDevice>(task.Result, (task.Result.IsWindows == host.IsWindows) ? "" : "System type mismatch.");
                result = new RequestResponse<HostDevice>(new HostDevice(host.MachineIdentifer, machineName, host.IsWindows, new Guid(User.Identity.Name)));
                _context.HostDevice.Add(result.Result);
                _context.SaveChanges();
                return result;
            });
        }

        // DELETE: api/HostDevice/{id}
        [HttpDelete("{id}")]
        // [Authorize(Roles = AppUser.Role_Name_Admin)]
        public async Task<ActionResult<bool>> UnRegister(Guid id)
        {
            return await _context.HostDevice.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result)
                    return false;
                _context.HostDevice.Remove(task.Result);
                _context.SaveChanges();
                return true;
            });
        }

        // GET: api/HostDevice/activate/{id}
        [HttpGet("activate/{id}")]
        // [Authorize(Roles = AppUser.Role_Name_Crawler)]
        public async Task<ActionResult<bool>> Activate(Guid id)
        {
            return await _context.HostDevice.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result || !task.Result.IsInactive)
                    return false;
                task.Result.IsInactive = false;
                _context.HostDevice.Update(task.Result);
                _context.SaveChanges();
                return true;
            });
        }

        // GET: api/HostDevice/deactivate/[id]
        [HttpGet("deactivate/{id}")]
        // [Authorize(Roles = AppUser.Role_Name_Crawler)]
        public async Task<ActionResult<bool>> Deactivate(Guid id)
        {
            return await _context.HostDevice.FindAsync(id).AsTask().ContinueWith<Boolean>(task => {
                if (null == task.Result || task.Result.IsInactive)
                    return false;
                task.Result.IsInactive = true;
                _context.HostDevice.Update(task.Result);
                _context.SaveChanges();
                return true;
            });
        }
    }
}
