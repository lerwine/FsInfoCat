using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFsSymbolicName : ITimeStampedEntity, IValidatableObject
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }
        string Name { get; }
        string Notes { get; }
        bool IsInactive { get; }
        IFileSystem FileSystem { get; }
        IReadOnlyCollection<IFileSystem> FileSystemDefaults { get; }
    }
}
