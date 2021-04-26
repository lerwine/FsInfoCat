using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class FileSystem : IFileSystem
    {
        private readonly ReadOnlyCollectionDelegateWrapper<Volume, IVolume> _volumesWrapper;
        private readonly ReadOnlyCollectionDelegateWrapper<FsSymbolicName, IFsSymbolicName> _symbolicNamesWrapper;
        public FileSystem()
        {
            Notes = "";
            Volumes = new HashSet<Volume>();
            SymbolicNames = new HashSet<FsSymbolicName>();
            _volumesWrapper = new ReadOnlyCollectionDelegateWrapper<Volume, IVolume>(() => Volumes);
            _symbolicNamesWrapper = new ReadOnlyCollectionDelegateWrapper<FsSymbolicName, IFsSymbolicName>(() => SymbolicNames);
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

        public virtual ICollection<Volume> Volumes { get; set; }
        IReadOnlyCollection<IVolume> IFileSystem.Volumes => _volumesWrapper;
        public virtual ICollection<FsSymbolicName> SymbolicNames { get; set; }
        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames => _symbolicNamesWrapper;
        public virtual FsSymbolicName DefaultSymbolicName { get; set; }
        IFsSymbolicName IFileSystem.DefaultSymbolicName => DefaultSymbolicName;
    }
}
