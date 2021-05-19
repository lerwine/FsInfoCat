using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFile : ITimeStampedEntity, IValidatableObject
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the file system name of the file.
        /// </summary>
        /// <value>
        /// The name of the file without the directory path.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the crawl option flags.
        /// </summary>
        /// <value>
        /// The <see cref="FileCrawlFlags"/> value that represents crawling options for the file.
        /// </value>
        FileCrawlFlags Options { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFile"/> has been deleted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> this <see cref="IFile"/> has been deleted; otherwise, <see langword="false"/>.
        /// </value>
        bool Deleted { get;  }

        /// <summary>
        /// Gets the date and time this file was last accessed.
        /// </summary>
        /// <value>
        /// The date and time this file was last accessed.
        /// </value>
        DateTime LastAccessed { get; }

        /// <summary>
        /// Gets the date and time of the last hash calculation.
        /// </summary>
        /// <value>
        /// The date and time of the last hash calculation or <see langword="null"/> if the hash code was never calculated.
        /// </value>
        DateTime? LastHashCalculation { get; }

        /// <summary>
        /// Gets the notes associated with this .<see cref="ISubDirectory"/>.
        /// </summary>
        /// <value>
        /// The notes associated with this .<see cref="ISubDirectory"/>.
        /// </value>
        string Notes { get; }

        /// <summary>
        /// Gets the parent subdirectory.
        /// </summary>
        /// <value>
        /// The <see cref="ISubDirectory"/> that represents the parent subdirectory.
        /// </value>
        ISubDirectory Parent { get; }

        IContentInfo HashInfo { get; }

        IRedundancy Redundancy { get; }

        IReadOnlyCollection<IFileComparison> SourceComparisons { get; }

        IReadOnlyCollection<IFileComparison> TargetComparisons { get; }
    }
}
