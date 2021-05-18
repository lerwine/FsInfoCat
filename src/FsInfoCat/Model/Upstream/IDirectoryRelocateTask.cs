using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IDirectoryRelocateTask : IUpstreamTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        AppTaskStatus Status { get; }
        PriorityLevel Priority { get; }
        string ShortDescription { get; }
        string Notes { get; }
        bool IsInactive { get; }

        IReadOnlyCollection<IUpstreamSubDirectory> SourceDirectories { get; }
        IUserGroup AssignmentGroup { get; }
        IUserProfile AssignedTo { get; }
        IUpstreamSubDirectory TargetDirectory { get; }
    }
}
