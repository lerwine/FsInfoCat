using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface IRedundantSetRow : IDbEntity, IHasSimpleIdentifier
    {
        /// <summary>Gets the custom reference value.</summary>
        /// <value>The custom reference value which can be used to refer to external information regarding redundancy remediation, such as a ticket number.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Reference), ResourceType = typeof(Properties.Resources))]
        string Reference { get; }

        /// <summary>Gets custom notes to be associated with the current set of redunant files.</summary>
        /// <value>The custom notes to associate with the current set of redunant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }
        
        RedundancyRemediationStatus Status { get; }

        Guid BinaryPropertiesId { get; }
    }
}
