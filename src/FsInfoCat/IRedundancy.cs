using System;

namespace FsInfoCat
{
    public interface IRedundancy : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key of the file in the that belongs to the redundancy set.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="File"/> <see cref="IFile">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the redundancy set file belongs to.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="RedundantSet"/> <see cref="IRedundantSet">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid RedundantSetId { get; set; }

        string Reference { get; set; }

        string Notes { get; set; }

        IFile File { get; set; }

        IRedundantSet RedundantSet { get; set; }
    }
}
