using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class Directory : ISubDirectory
    {
        private readonly ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory> _subdirectoriesWrapper;
        private readonly ReadOnlyCollectionDelegateWrapper<File, IFile> _filesWrapper;

        public Directory()
        {
            SubDirectories = new HashSet<Directory>();
            Files = new HashSet<File>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
            _subdirectoriesWrapper = new ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory>(() => SubDirectories);
            _filesWrapper = new ReadOnlyCollectionDelegateWrapper<File, IFile>(() => Files);
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DirectoryCrawlFlags CrawlFlags { get; set; }

        public Guid? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
        public Guid? SourceRelocationTaskId { get; set; }
        public virtual Volume Volume { get; set; }
        IVolume ISubDirectory.Volume => Volume;
        public virtual ICollection<Directory> SubDirectories { get; set; }
        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => _subdirectoriesWrapper;
        public virtual Directory Parent { get; set; }
        ISubDirectory ISubDirectory.Parent => Parent;
        public virtual ICollection<File> Files { get; set; }
        IReadOnlyCollection<IFile> ISubDirectory.Files => _filesWrapper;
        public virtual DirectoryRelocateTask SourceRelocationTask { get; set; }
        public virtual ICollection<FileRelocateTask> FileRelocationTasks { get; set; }
        public virtual ICollection<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; set; }
    }
}
