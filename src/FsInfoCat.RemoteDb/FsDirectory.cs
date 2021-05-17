using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        IReadOnlyCollection<IFile> ISubDirectory.Files => Files;

        IReadOnlyCollection<IRemoteFile> IRemoteSubDirectory.Files => throw new NotImplementedException();

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => SubDirectories;

        IReadOnlyCollection<IRemoteSubDirectory> IRemoteSubDirectory.SubDirectories => throw new NotImplementedException();

        IRemoteSubDirectory IRemoteSubDirectory.Parent => throw new NotImplementedException();

        ISubDirectory ISubDirectory.Parent => throw new NotImplementedException();

        IRemoteVolume IRemoteSubDirectory.Volume => throw new NotImplementedException();

        IVolume ISubDirectory.Volume => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
