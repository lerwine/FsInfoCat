using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IItemTagRow : IDbEntity, IHasIdentifierPair
    {
        /// <summary>Gets the primary key value that references the <see cref="Tagged"/> entity.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
        Guid TaggedId { get; }

        /// <summary>Gets the primary key value that references the <see cref="Definition"/> entity.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as part of the current entity's primary key the database.</value>
        Guid DefinitionId { get; }

        /// <summary>Gets custom notes to be associated with the current file system item.</summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }
    }
}
