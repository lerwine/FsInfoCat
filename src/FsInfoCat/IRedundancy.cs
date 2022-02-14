using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IRedundancy : IDbEntity, IHasIdentifierPair, IEquatable<IRedundancy>
    {
        /// <summary>Gets the custom reference value.</summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>Gets custom notes to be associated with the current redundancy.</summary>
        /// <value>The custom notes to associate with the current redundancy.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the primary key of the file in the that belongs to the redundancy set.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="File" /><see cref="IFile">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileId), ResourceType = typeof(Properties.Resources))]
        Guid FileId { get; }

        /// <summary>Gets the file that belongs to the redundancy set.</summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        IFile File { get; }

        /// <summary>Gets the primary key of the redundancy set file belongs to.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="RedundantSet" /><see cref="IRedundantSet">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSetId), ResourceType = typeof(Properties.Resources))]
        Guid RedundantSetId { get; }

        /// <summary>Gets the redundancy set.</summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        IRedundantSet RedundantSet { get; }
    }
}
