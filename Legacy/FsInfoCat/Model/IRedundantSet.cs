using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Represents a grouping of file that have been confirmed to be redundant of one another.
    /// </summary>
    /// <seealso cref="FsInfoCat.Model.ITimeStampedEntity" />
    public interface IRedundantSet : ITimeStampedEntity
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the notes associated with this file redundancy set.
        /// </summary>
        /// <value>
        /// The notes associated with this file redundancy set.
        /// </value>
        string Notes { get; }

        /// <summary>
        /// Gets the database entities that contain information about redundant files.
        /// </summary>
        /// <value>
        /// The <see cref="IRedundancy"/> objects that contain information about redundant files.
        /// </value>
        IReadOnlyCollection<IRedundancy> Redundancies { get; }
    }
}
