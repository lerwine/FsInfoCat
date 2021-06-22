using System.Collections.Generic;

namespace FsInfoCat.Upstream
{
    public interface IMitigationTask : IUpstreamDbEntity
    {
        string ShortDescription { get; set; }

        string Notes { get; set; }

        TaskStatus Status { get; set; }

        PriorityLevel Priority { get; }

        IUserGroup AssignmentGroup { get; set; }

        IUserProfile AssignedTo { get; set; }

        IEnumerable<IFileAction> FileActions { get; set; }

        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; set; }
    }
}
