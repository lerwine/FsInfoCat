using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Upstream
{
    public interface IFileRelocateTask : IUpstreamTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        AppTaskStatus Status { get; }
        PriorityLevel Priority { get; }
        string ShortDescription { get; }
        string Notes { get; }

        IReadOnlyCollection<IUpstreamFile> Files { get; }
        IUpstreamSubDirectory TargetDirectory { get; }
        IUserGroup AssignmentGroup { get; }
        IUserProfile AssignedTo { get; }
    }
}
