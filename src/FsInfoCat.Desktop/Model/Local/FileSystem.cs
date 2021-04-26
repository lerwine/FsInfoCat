using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class FileSystem
    {
        public FileSystem()
        {
            Notes = "";
            Volumes = new HashSet<Volume>();
            SymbolicNames = new HashSet<FsSymbolicName>();
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public bool CaseSensitiveSearch { get; set; }
        public bool ReadOnly { get; set; }
        public long MaxNameLength { get; set; }
        public System.IO.DriveType? DefaultDriveType { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<Volume> Volumes { get; set; }
        public virtual ICollection<FsSymbolicName> SymbolicNames { get; set; }
    }
}
