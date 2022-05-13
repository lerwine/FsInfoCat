using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for an entity that represents a group of <see cref="IFile"/> entities which are considered redundant.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    public interface IRedundantSetRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>
        /// Gets the custom reference value.
        /// </summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>
        /// Gets custom notes to be associated with the current set of redunant files.
        /// </summary>
        /// <value>The custom notes to associate with the current set of redunant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>
        /// Gets the redundancy remediation status.
        /// </summary>
        /// <value>The value that indicates the remediation status for all files in the current redundant set.</value>
        RedundancyRemediationStatus Status { get; }

        /// <summary>
        /// Gets the primary key of the binary properties entity that all files in the current redundant set share.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> value of the <see cref="IBinaryPropertySet"/> that all files in the current redundant set share.</value>
        Guid BinaryPropertiesId { get; }
    }
}
