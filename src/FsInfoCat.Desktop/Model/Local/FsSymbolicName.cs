using System;

namespace FsInfoCat.Desktop.Model.Local
{
    public class FsSymbolicName
    {
        public FsSymbolicName()
        {
            Notes = "";
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FileSystemId { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual FileSystem FileSystem { get; set; }
    }
}
