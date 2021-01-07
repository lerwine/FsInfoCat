using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FsInfoCat.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace FsInfoCat.Web
{
    public static class ViewModelHelper
    {
        public static async Task<HostContributor> Lookup(DbSet<HostContributor> dbSet, Guid accountID, Guid HostDeviceID)
        {
            IQueryable<HostContributor> contributors = from c in dbSet select c;
            return (await contributors.Where(c => c.AccountID == accountID && c.HostDeviceID == HostDeviceID).AsNoTracking().ToListAsync()).FirstOrDefault();
        }

        public static async Task<HostDevice> LookUp(DbSet<HostDevice> dbSet, string machineName, string machineIdentifer)
        {
            if (string.IsNullOrWhiteSpace(machineName))
                return null;
            IQueryable<HostDevice> hostDevices = from d in dbSet select d;
            if (string.IsNullOrWhiteSpace(machineIdentifer))
                hostDevices = hostDevices.Where(h => machineName == h.MachineName);
            else
                hostDevices = hostDevices.Where(h => machineName == h.MachineName && machineIdentifer == h.MachineIdentifer);
            return (await hostDevices.AsNoTracking().ToListAsync()).FirstOrDefault();
        }

        public static async Task<UserCredential> LookUp(DbSet<UserCredential> dbSet, Guid accountId)
        {
            IQueryable<UserCredential> userCredentials = from d in dbSet select d;
            userCredentials = userCredentials.Where(h => h.AccountID == accountId);
            return (await userCredentials.AsNoTracking().ToListAsync()).FirstOrDefault();
        }

        public static async Task<List<Volume>> GetByHost(Guid HostDeviceID, DbSet<Volume> dbSet)
        {
            IQueryable<Volume> volumes = from d in dbSet select d;
            return await volumes.Where(v => v.HostDeviceID.HasValue && v.HostDeviceID.Value.Equals(HostDeviceID)).AsNoTracking().ToListAsync();
        }
    }
}
