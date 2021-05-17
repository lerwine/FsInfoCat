using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class UserGroup : IUserGroup
    {
        public Guid Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public UserRole Roles => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public HashSet<UserProfile> Members => throw new NotImplementedException();

        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks => throw new NotImplementedException();

        public HashSet<FileRelocateTask> FileRelocationTasks => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IReadOnlyCollection<IUserProfile> IUserGroup.Members => throw new NotImplementedException();

        IReadOnlyCollection<IDirectoryRelocateTask> IUserGroup.DirectoryRelocationTasks => throw new NotImplementedException();

        IReadOnlyCollection<IFileRelocateTask> IUserGroup.FileRelocationTasks => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
