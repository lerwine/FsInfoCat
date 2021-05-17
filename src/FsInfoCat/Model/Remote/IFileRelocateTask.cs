using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model.Remote
{
    public interface IFileRelocateTask : IRemoteTimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        AppTaskStatus Status { get; }
        PriorityLevel Priority { get; }
        string ShortDescription { get; }
        Guid TargetDirectoryId { get; }
        Guid? AssignmentGroupId { get; }
        Guid? AssignedToId { get; }
        string Notes { get; }

        IReadOnlyCollection<IRemoteFile> Files { get; }
        IRemoteSubDirectory TargetDirectory { get; }
        IUserGroup AssignmentGroup { get; }
        IUserProfile AssignedTo { get; }
    }
}
