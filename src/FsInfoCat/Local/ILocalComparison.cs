using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IComparison" />
    public interface ILocalComparison : ILocalDbEntity, IComparison, IHasMembershipKeyReference<ILocalFile, ILocalFile>
    {
        /// <summary>Gets the baseline file in the comparison.</summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Baseline { get; }

        /// <summary>Gets the correlative file in the comparison.</summary>
        /// <value>The generic <see cref="ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        new ILocalFile Correlative { get; }
    }
}
