using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a confirmed file redundancy.
    /// </summary>
    /// <seealso cref="FsInfoCat.Model.ITimeStampedEntity" />
    /// <seealso cref="System.ComponentModel.DataAnnotations.IValidatableObject" />
    public interface IRedundancy : ITimeStampedEntity, IValidatableObject
    {
        Guid FileId { get; }

        Guid RedundantSetId { get; }

        /// <summary>
        /// Gets the dispositional status of the of the file.
        /// </summary>
        /// <value>
        /// The <see cref="FileRedundancyStatus"/> value that indicates the justification status.
        /// </value>
        FileRedundancyStatus Status { get; }

        /// <summary>
        /// Gets the notes associated with this file redundancy set.
        /// </summary>
        /// <value>
        /// The notes associated with this file redundancy set.
        /// </value>
        string Notes { get; }

        /// <summary>
        /// Gets the database entity that containins file system information about the redundant file.
        /// </summary>
        /// <value>
        /// The <see cref="IFile"/> object that containins file system information about the redundant file.
        /// </value>
        IFile TargetFile { get; }

        /// <summary>
        /// Gets the database entity that represents a set of redundant files.
        /// </summary>
        /// <value>
        /// The <see cref="IRedundantSet"/> object that represents a set of redundant files.
        /// </value>
        IRedundantSet RedundantSet { get; }
    }
}
