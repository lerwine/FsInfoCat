using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IHasSimpleIdentifier
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }
    }
}
