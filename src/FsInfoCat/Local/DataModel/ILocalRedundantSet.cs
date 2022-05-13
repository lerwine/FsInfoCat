using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public interface ILocalRedundantSetRow : ILocalDbEntity, IRedundantSetRow { }

    /// <summary>
    /// Represents a set of files that have the same size, Hash and remediation status.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundantSet" />
    public interface ILocalRedundantSet : ILocalRedundantSetRow, IRedundantSet
    {
        /// <summary>
        /// Gets the binary properties in common with all files in the current redundant set.
        /// </summary>
        /// <value>The binary properties in common with all files in the current redundant set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_BinaryProperties), ResourceType = typeof(Properties.Resources))]
        new ILocalBinaryPropertySet BinaryProperties { get; }

        /// <summary>
        /// Gets the redundancy entities which represent links to redundant files.
        /// </summary>
        /// <value>The redundancy entities which represent links to redundant files.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Redundancies), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<ILocalRedundancy> Redundancies { get; }
    }

    public interface ILocalRedundantSetListItem : IRedundantSetListItem, ILocalRedundantSetRow { }
}
