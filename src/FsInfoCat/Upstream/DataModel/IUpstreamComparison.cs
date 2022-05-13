using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IComparison" />
    public interface IUpstreamComparison : IUpstreamDbEntity, IComparison, IHasMembershipKeyReference<IUpstreamFile, IUpstreamFile>
    {
        /// <summary>
        /// Gets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Baseline { get; }

        /// <summary>
        /// Gets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile Correlative { get; }
    }
}
