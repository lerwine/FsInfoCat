using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.Local
{
    public class DirectoryRelocateTask
    {
        public DirectoryRelocateTask()
        {
            Notes = "";
            SourceDirectories = new HashSet<Directory>();
        }

        public Guid Id { get; set; }
        public AppTaskStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }
        public string ShortDescription { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public Guid TargetDirectoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual ICollection<Directory> SourceDirectories { get; set; }
        public virtual Directory TargetDirectory { get; set; }
    }
}
