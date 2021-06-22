using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IPropertySet : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets the files that have this property set.
        /// </summary>
        IEnumerable<IFile> Files { get; }
    }
}
