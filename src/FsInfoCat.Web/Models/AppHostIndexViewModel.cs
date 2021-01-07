using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FsInfoCat.Models;
using FsInfoCat.Models.DB;
using FsInfoCat.Models.HostDevices;
using Microsoft.EntityFrameworkCore;

namespace FsInfoCat.Web.Models
{
    public class AppHostIndexViewModel : HostDevice
    {
        public bool IsRegistered { get; set; }

        private AppHostIndexViewModel(HostDevice host) : base(host)
        {
            IsRegistered = true;
        }

        private AppHostIndexViewModel(HostDeviceRegRequest request, Guid createdBy) : base(request, createdBy)
        {
            IsRegistered = false;
            AllowCrawl = false;
        }

        public static async Task<AppHostIndexViewModel> Create(DbSet<HostDevice> dbSet, ClaimsPrincipal user)
        {
            HostDeviceRegRequest regRequest = HostDeviceRegRequest.CreateForLocal();
            HostDevice host = await ViewModelHelper.LookUp(dbSet, regRequest.MachineName, regRequest.MachineIdentifer);
            if (null == host)
                return new AppHostIndexViewModel(regRequest, new Guid(user.Identity.Name));
            return new AppHostIndexViewModel(host);
        }
    }
}
