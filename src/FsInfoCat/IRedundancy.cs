using System;
using System.ComponentModel.DataAnnotations;

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

        // TODO: Add [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; set; }

        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; set; }

        // TODO: Add [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        IFile File { get; set; }

        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        IRedundantSet RedundantSet { get; set; }
    }
}
