using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IUserGroup : IUpstreamTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        string Name { get; }
        UserRole Roles { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IUserGroupMembership> Members { get; }
        IReadOnlyCollection<IDirectoryRelocateTask> DirectoryRelocationTasks { get; }
        IReadOnlyCollection<IFileRelocateTask> FileRelocationTasks { get; }
    }
}
