using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class Directory
    {
        public Directory()
        {
            SubDirectories = new HashSet<Directory>();
            Files = new HashSet<File>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DirectoryCrawlFlags CrawlFlags { get; set; }

        public Guid? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
        public Guid? SourceRelocationTaskId { get; set; }

        public virtual Volume Volume { get; set; }
        public virtual ICollection<Directory> SubDirectories { get; set; }
        public virtual Directory Parent { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual DirectoryRelocateTask SourceRelocationTask { get; set; }
        public virtual ICollection<FileRelocateTask> FileRelocationTasks { get; set; }
        public virtual ICollection<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; set; }
    }
}
