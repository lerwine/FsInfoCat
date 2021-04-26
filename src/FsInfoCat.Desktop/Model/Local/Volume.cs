using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class Volume : IVolume
    {
        public Volume()
        {
            Notes = "";
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string VolumeName { get; set; }
        public string Identifier { get; set; }
        public Guid FileSystemId { get; set; }
        public System.IO.DriveType Type { get; set; }
        public bool? CaseSensitiveSearch { get; set; }
        public bool? ReadOnly { get; set; }
        public long? MaxNameLength { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual FileSystem FileSystem { get; set; }
        IFileSystem IVolume.FileSystem => FileSystem;
        public virtual Directory RootDirectory { get; set; }
        ISubDirectory IVolume.RootDirectory => RootDirectory;
    }
}
