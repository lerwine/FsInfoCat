using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IComparison" />
    /// <seealso cref="IEquatable{ILocalComparison}" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalFile, ILocalFile}" />
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="ILocalDbContext.Comparisons" />
    /// <seealso cref="ILocalFile.BaselineComparisons" />
    /// <seealso cref="ILocalFile.CorrelativeComparisons" />
    /// <seealso cref="Upstream.Model.IComparison" />
    public interface ILocalComparison : ILocalDbEntity, IComparison, IHasMembershipKeyReference<ILocalFile, ILocalFile>, IEquatable<ILocalComparison>
    {
        /// <summary>
        /// Gets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Baseline { get; }

        /// <summary>
        /// Gets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Correlative { get; }
    }
}
