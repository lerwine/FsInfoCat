using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;
using FsInfoCat.Web.Data;

namespace FsInfoCat.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HostDeviceController : ControllerBase
    {
        private readonly FsInfoDataContext _context;
        private readonly ILogger<HostDeviceController> _logger;

        public HostDeviceController(FsInfoDataContext context, ILogger<HostDeviceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/HostDevice/get/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<HostDevice>> GetById(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<HostDevice>)null);
            return await _context.HostDevice.FindAsync(id);
        }

        public static readonly Regex MachineNameRegex = new Regex(HostDevice.PATTERN_MACHINE_NAME, RegexOptions.Compiled);
        // POST: api/HostDevice
        [HttpPost]
        [Authorize(Roles = AppUser.Role_Name_Crawler)]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<HostDevice>>> Register(HostDeviceRegRequest host)
        {
            string uc;
            if (null == host || string.IsNullOrWhiteSpace(host.MachineName) || !MachineNameRegex.IsMatch((uc = host.MachineName.ToUpper())))
                return await Task.FromResult(new RequestResponse<HostDevice>(null, "Registration failed."));
            return await _context.HostDevice.FirstOrDefaultAsync(h => String.Equals(uc, host.MachineName, StringComparison.InvariantCulture)).ContinueWith<RequestResponse<HostDevice>>(task =>
            {
                RequestResponse<HostDevice> result;
                if (null != task.Result)
                    return new RequestResponse<HostDevice>(task.Result, (task.Result.IsWindows == host.IsWindows) ? "" : "System type mismatch.");
                result = new RequestResponse<HostDevice>(new HostDevice(uc, host.IsWindows, new Guid(User.Identity.Name)));
                _context.HostDevice.Add(result.Result);
                _context.SaveChanges();
                return result;
            });
        }

        // DELETE: api/HostDevice/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = AppUser.Role_Name_Admin)]
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
    }
}
