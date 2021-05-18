using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IContentHash : ITimeStampedEntity, IValidatableObject
    {
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier that is used as the prmary database key.
        /// </value>
        Guid Id { get; }
        long Length { get; }
        IReadOnlyCollection<byte> Data { get; }
        IReadOnlyCollection<IFile> Files { get; }
    }
}
