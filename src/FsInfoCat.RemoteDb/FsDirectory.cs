using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;

namespace FsInfoCat.RemoteDb
{
    public class FsDirectory : IRemoteSubDirectory
    {
        public FsDirectory()
        {
            SubDirectories = new HashSet<FsDirectory>();
            Files = new HashSet<FsFile>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            TargetDirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public FsDirectory Parent { get; set; }

        public HashSet<FsDirectory> SubDirectories { get; set; }

        public Volume Volume { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public HashSet<FsFile> Files { get; set; }

        public HashSet<FileRelocateTask> FileRelocationTasks { get; private set; }

        public HashSet<DirectoryRelocateTask> TargetDirectoryRelocationTasks { get; private set; }

        IReadOnlyCollection<IFile> ISubDirectory.Files => Files;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => SubDirectories;
    }
}
