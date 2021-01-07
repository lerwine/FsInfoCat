using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FsInfoCat.Models.DB;
using FsInfoCat.Models;
using FsInfoCat.Web.Data;
using System.ComponentModel.DataAnnotations;
using FsInfoCat.Models.HostDevices;

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
        [Authorize(Roles = ModelHelper.Role_Name_Viewer)]
        public async Task<ActionResult<HostDevice>> GetById(Guid id)
        {
            return await _context.HostDevice.FindAsync(id);
        }

        public static async Task<RequestResponse<HostDevice>> Register(Guid userId, FsInfoDataContext dbContext, HostDeviceRegRequest request)
        {
            if (null == request)
                return await Task.FromResult(new RequestResponse<HostDevice>(null, "Invalid request."));
            IList<ValidationResult> validation = request.ValidateAll();
            if (validation.Count > 0)
                return await Task.FromResult(new RequestResponse<HostDevice>(null, validation[0].ErrorMessage));
            HostDevice matching = await ViewModelHelper.LookUp(dbContext.HostDevice, request.MachineName, request.MachineIdentifer);
            RequestResponse<HostDevice> result;
            if (null == matching)
            {
                result = new RequestResponse<HostDevice>(new HostDevice(request, userId));
                dbContext.HostDevice.Add(result.Result);
                await dbContext.SaveChangesAsync();
            }
            else if (request.IsWindows != matching.IsWindows)
                result = new RequestResponse<HostDevice>(matching, "System type mismatch");
            else
            {
                if (request.DisplayName != matching.DisplayName || request.MachineIdentifer != matching.MachineIdentifer || request.MachineName != matching.MachineName)
                {
                    matching.DisplayName = request.DisplayName;
                    matching.MachineIdentifer = request.MachineIdentifer;
                    matching.MachineName = request.MachineName;
                    matching.ModifiedOn = DateTime.UtcNow;
                    matching.ModifiedBy = userId;
                    dbContext.HostDevice.Update(matching);
                    await dbContext.SaveChangesAsync();
                }
                result = new RequestResponse<HostDevice>(matching);
            }
            return result;
        }

        // POST: api/HostDevice/Register
        [HttpPost]
        [Authorize(Roles = ModelHelper.Role_Name_App_Contrib)]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<HostDevice>>> Register(HostDeviceRegRequest host)
        {
            return await Register(new Guid(User.Identity.Name), _context, host);
        }

        // DELETE: api/HostDevice/UnRegister/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = ModelHelper.Role_Name_Admin)]
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
