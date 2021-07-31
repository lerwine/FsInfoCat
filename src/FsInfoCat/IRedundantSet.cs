using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
    /// <seealso cref="IDbEntity" />
    public interface IRedundantSet : IDbEntity
    {
        /// <summary>Gets the primary key value.</summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Id), ResourceType = typeof(Properties.Resources))]
        Guid Id { get; }

        /// <summary>Gets the custom reference value.</summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>Gets custom notes to be associated with the current set of redunant files.</summary>
        /// <value>The custom notes to associate with the current set of redunant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Gets the binary properties in common with all files in the current redundant set.</summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        IBinaryPropertySet BinaryProperties { get; }

        /// <summary>Gets the redundancy entities which represent links to redundant files.</summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IRedundancy> Redundancies { get; }
    }

}

