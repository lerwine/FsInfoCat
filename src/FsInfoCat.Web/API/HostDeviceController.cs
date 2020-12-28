using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models.DB;
using FsInfoCat.Models;
using FsInfoCat.Web.Data;
using System.ComponentModel.DataAnnotations;

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

        // GET: api/HostDevice/GetById/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = AppUser.Role_Name_Viewer)]
        public async Task<ActionResult<HostDevice>> GetById(Guid id)
        {
            if (null == User || null == User.Identity || !User.Identity.IsAuthenticated)
                return await Task.FromResult((ActionResult<HostDevice>)null);
            return await _context.HostDevice.FindAsync(id);
        }


        // POST: api/HostDevice/Register
        [HttpPost]
        [Authorize(Roles = AppUser.Role_Name_Crawler)]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<HostDevice>>> Register(HostDeviceRegRequest host)
        {
            if (null != host)
                return await Task.FromResult(new RequestResponse<HostDevice>(null, "Invalid request."));
            IList<ValidationResult> validation = host.ValidateAll();
            if (validation.Count > 0)
                return await Task.FromResult(new RequestResponse<HostDevice>(null, validation[0].ErrorMessage));
            IQueryable<HostDevice> hostDevices = from d in _context.HostDevice select d;
            if (host.MachineIdentifer.Length == 0)
                hostDevices = hostDevices.Where(h => string.Equals(host.MachineName, h.MachineName, StringComparison.InvariantCulture));
            else
                hostDevices = hostDevices.Where(h => string.Equals(host.MachineName, h.MachineName, StringComparison.InvariantCulture) ||
                    string.Equals(host.MachineIdentifer, h.MachineIdentifer, StringComparison.InvariantCulture));
            HostDevice matching = (await hostDevices.AsNoTracking().ToListAsync()).FirstOrDefault();
            RequestResponse<HostDevice> result;
            if (null == matching)
            {
                result = new RequestResponse<HostDevice>(new HostDevice(host, new Guid(User.Identity.Name)));
                _context.HostDevice.Add(result.Result);
                await _context.SaveChangesAsync();
            }
            else if (host.IsWindows != matching.IsWindows)
                result = new RequestResponse<HostDevice>(matching, "System type mismatch");
            else
            {
                if (host.DisplayName != matching.DisplayName || host.MachineIdentifer != matching.MachineIdentifer || host.MachineName != matching.MachineName)
                {
                    matching.DisplayName = host.DisplayName;
                    matching.MachineIdentifer = host.MachineIdentifer;
                    matching.MachineName = host.MachineName;
                    matching.ModifiedOn = DateTime.UtcNow;
                    matching.ModifiedBy = new Guid(User.Identity.Name);
                    _context.HostDevice.Update(matching);
                    await _context.SaveChangesAsync();
                }
                result = new RequestResponse<HostDevice>(matching);
            }
            return result;
        }

        // DELETE: api/HostDevice/UnRegister/{id}
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
