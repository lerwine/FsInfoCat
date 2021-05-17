using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IUserGroup : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string Name { get; }
        UserRole Roles { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IUserProfile> Members { get; }
        IReadOnlyCollection<IDirectoryRelocateTask> DirectoryRelocationTasks { get; }
        IReadOnlyCollection<IFileRelocateTask> FileRelocationTasks { get; }
    }
}
