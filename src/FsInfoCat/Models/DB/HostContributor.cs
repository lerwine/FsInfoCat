using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FsInfoCat.Models.DB
{
    public class HostContributor : INormalizable
    {
        [Required()]
        [Display(Name = "User ID")]
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

        [Required()]
        [Display(Name = "Host ID")]
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

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Created By")]
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

        // TODO: Move this to HostContributor
        public static async Task<HostContributor> Lookup(DbSet<HostContributor> dbSet, Guid accountID, Guid HostDeviceID)
        {
            IQueryable<HostContributor> contributors = from c in dbSet select c;
            return (await contributors.Where(c => c.AccountID.Equals(accountID) && c.HostDeviceID.Equals(HostDeviceID)).AsNoTracking().ToListAsync()).FirstOrDefault();
        }
    }
}