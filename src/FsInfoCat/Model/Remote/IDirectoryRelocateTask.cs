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
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IRemoteSubDirectory> SourceDirectories { get; }
        IUserGroup AssignmentGroup { get; }
        IUserProfile AssignedTo { get; }
        IRemoteSubDirectory TargetDirectory { get; }
    }
}
