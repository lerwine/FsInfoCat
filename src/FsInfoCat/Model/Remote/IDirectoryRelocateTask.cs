using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IDirectoryRelocateTask : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        AppTaskStatus Status { get; }
        PriorityLevel Priority { get; }
        string ShortDescription { get; }
        Guid? AssignmentGroupId { get; }
        Guid? AssignedToId { get; }
        string Notes { get; }
        bool IsInactive { get; }
        Guid TargetDirectoryId { get; }

        IReadOnlyCollection<IRemoteSubDirectory> SourceDirectories { get; }
        IUserGroup AssignmentGroup { get; }
        IUserProfile AssignedTo { get; }
        IRemoteSubDirectory TargetDirectory { get; }
    }
}
