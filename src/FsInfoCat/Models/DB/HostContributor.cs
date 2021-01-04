using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
#if CORE
using Microsoft.EntityFrameworkCore;
#endif

namespace FsInfoCat.Models.DB
{
    public class HostContributor : INormalizable
    {
#if CORE
        [Required()]
        [Display(Name = "User ID")]
#endif
        public Guid AccountID { get; set; }

        public Account Account { get; set; }

        public string AccountName
        {
            get
            {
                Account host = Account;
                if (null == host)
                    return "";
                string n = host.DisplayName;
                return (string.IsNullOrWhiteSpace(n)) ? host.LoginName : host.DisplayName;
            }
        }

#if CORE
        [Required()]
        [Display(Name = "Host ID")]
#endif
        public Guid HostDeviceID { get; set; }

        public HostDevice Host { get; set; }

        public string HostName
        {
            get
            {
                HostDevice host = Host;
                return (null == host) ? "" : host.Name;
            }
        }

#if CORE
        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
#endif
        public DateTime CreatedOn { get; set; }

#if CORE
        [Editable(false)]
        [Display(Name = "Created By")]
#endif
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (null == account) ? "" : account.Name;
            }
        }

        public void Normalize()
        {
            CreatedOn = ModelHelper.CoerceAsLocalTime(CreatedOn);
            if (null != Account)
                AccountID = Account.AccountID;
            if (null != Host)
                HostDeviceID = Host.HostDeviceID;
            if (null != Creator)
                CreatedBy = Creator.AccountID;
        }
#if CORE

        public static async Task<HostContributor> Lookup(DbSet<HostContributor> dbSet, Guid accountID, Guid HostDeviceID)
        {
            IQueryable<HostContributor> contributors = from c in dbSet select c;
            return (await contributors.Where(c => c.AccountID == accountID && c.HostDeviceID == HostDeviceID).AsNoTracking().ToListAsync()).FirstOrDefault();
        }
#endif
    }
}
