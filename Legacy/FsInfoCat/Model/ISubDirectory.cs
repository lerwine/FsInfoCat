using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface ISubDirectory : ITimeStampedEntity, IValidatableObject
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets the file system name of the sub-directory.
        /// </summary>
        /// <value>
        /// The name of the subdirectory without the parent path.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the crawl option flags.
        /// </summary>
        /// <value>
        /// The <see cref="FileCrawlFlags"/> value that represents crawling options for the sub-directory.
        /// </value>
        DirectoryCrawlFlags Options { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ISubDirectory"/> has been deleted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> this <see cref="ISubDirectory"/> has been deleted; otherwise, <see langword="false"/>.
        /// </value>
        bool Deleted { get; }

        /// <summary>
        /// Gets the notes associated with this .<see cref="ISubDirectory"/>.
        /// </summary>
        /// <value>
        /// The notes associated with this .<see cref="ISubDirectory"/>.
        /// </value>
        string Notes { get; }

        ISubDirectory Parent { get; }

        IVolume Volume { get; }

        IReadOnlyCollection<IFile> Files { get; }

        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }
    }
}
