using M = FsInfoCat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Compliance/Redundancy mitigation task.
    /// </summary>
    /// <seealso cref="IMitigationTaskRow" />
    /// <seealso cref="IEquatable{IMitigationTask}" />
    public interface IMitigationTask : IMitigationTaskRow, IEquatable<IMitigationTask>
    {
        /// <summary>
        /// Gets the task's assignment group.
        /// </summary>
        /// <value>The <see cref="IUserGroup"/> responsible for task mitigation.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignmentGroup), ResourceType = typeof(Properties.Resources))]
        IUserGroup AssignmentGroup { get; }

        /// <summary>
        /// Gets the user that the task is assigned to.
        /// </summary>
        /// <value>The <see cref="IUserProfile"/> of the person accountable for task mitigation.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AssignedTo), ResourceType = typeof(Properties.Resources))]
        IUserProfile AssignedTo { get; }

        /// <summary>
        /// Gets the actions to be taken on files.
        /// </summary>
        /// <value>The list of actions to be carried out on specific files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IFileAction> FileActions { get; }

        /// <summary>
        /// Gets the actions to be taken on subdirectories.
        /// </summary>
        /// <value>The list of actions to be carried out on entire subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SubdirectoryActions), ResourceType = typeof(Properties.Resources))]
        IEnumerable<ISubdirectoryAction> SubdirectoryActions { get; }
    }
}
