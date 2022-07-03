using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that has a single <see cref="Guid"/> value as the primary key.
    /// </summary>
    public interface IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>
        /// Attempts to get the primary key value.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns><see langword="true"/> if the primary key value ahas been set; otherwise, <see langword="false"/>.</returns>
        bool TryGetId(out Guid id);
    }
}
