using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Upstream
{
    public interface IMitigationTaskListItem : IMitigationTaskRow
    {
        Guid? AssignmentGroupId { get; }

        string AssignmentGroupName { get; }

        Guid? AssignedToId { get; }

        string AssignedToDisplayName { get; }

        string AssignedToFirstName { get; }

        string AssignedToLastName { get; }

        int? AssignedToDbPrincipalId { get; }

        ByteValues AssignedToSID { get; }

        long FileActionCount { get; }

        long SubdirectoryActionCount { get; }
    }
}
