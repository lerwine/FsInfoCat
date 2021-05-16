using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    [Obsolete]
    public class FileSystem : IFileSystem
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
        public System.Guid DefaultSymbolicNameId { get; set; }

        public virtual HashSet<Volume> Volumes { get; set; }
        IReadOnlyCollection<IVolume> IFileSystem.Volumes => Volumes;
        public virtual HashSet<FsSymbolicName> SymbolicNames { get; set; }
        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames => SymbolicNames;
        public virtual FsSymbolicName DefaultSymbolicName { get; set; }
        IFsSymbolicName IFileSystem.DefaultSymbolicName => DefaultSymbolicName;
    }
}
