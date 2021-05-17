using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.Local
{
    [System.Obsolete("Use FsInfoCat.LocalDb.SymbolicName or FsInfoCat.Model.Local.ILocalSymbolicName")]
    public class FsSymbolicName : IFsSymbolicName
    {
        public FsSymbolicName()
        {
            Notes = "";
            DefaultFileSystems = new HashSet<FileSystem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FileSystemId { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual FileSystem FileSystem { get; set; }
        public virtual ICollection<FileSystem> DefaultFileSystems { get; set; }
    }
}
