using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for an entity that defines an association between an <see cref="IRedundantSet"/> and an <see cref="IFile"/>.
    /// </summary>
    /// <seealso cref="IDbContext.Redundancies"/>
    public interface IRedundancy : IDbEntity, IHasMembershipKeyReference<IRedundantSet, IFile>, IEquatable<IRedundancy>
    {
        /// <summary>
        /// Gets the custom reference value.
        /// </summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>
        /// Gets custom notes to be associated with the current redundancy.
        /// </summary>
        /// <value>The custom notes to associate with the current redundancy.</value>
        [Display(Name = nameof(Properties.Resources.Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Gets the primary key of the file in the that belongs to the redundancy set.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="File" /><see cref="IFile">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.FileId), ResourceType = typeof(Properties.Resources))]
        Guid FileId { get; }

        /// <summary>
        /// Gets the file that belongs to the redundancy set.
        /// </summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.File), ResourceType = typeof(Properties.Resources))]
        IFile File { get; }

        /// <summary>
        /// Gets the primary key of the redundancy set file belongs to.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="IRedundantSet">entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.RedundantSetId), ResourceType = typeof(Properties.Resources))]
        Guid RedundantSetId { get; }

        /// <summary>
        /// Gets the redundancy set.
        /// </summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.RedundantSet), ResourceType = typeof(Properties.Resources))]
        IRedundantSet RedundantSet { get; }

        /// <summary>
        /// Gets the value of the <see cref="FileId" /> property or the unique identifier of the <see cref="File" /> entity if it has been assigned.
        /// </summary>
        /// <param name="fileId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IFile" /> baseline entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetFileId(out Guid fileId);

        /// <summary>
        /// Gets value of the <see cref="RedundantSetId" /> property or the unique identifier of the <see cref="RedundantSet" /> entity if it has been assigned.
        /// </summary>
        /// <param name="redundantSetId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IRedundantSet" /> correlative entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetRedundantSetId(out Guid redundantSetId);
    }
}
