using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IEquatable{IComparison}" />
    /// <seealso cref="IHasMembershipKeyReference{IFile, IFile}" />
    /// <seealso cref="Upstream.IUpstreamComparison" />
    /// <seealso cref="Local.ILocalComparison" />
    /// <seealso cref="IDbContext.Comparisons" />
    /// <seealso cref="IFile.BaselineComparisons" />
    /// <seealso cref="IFile.CorrelativeComparisons" />
    public interface IComparison : IDbEntity, IEquatable<IComparison>, IHasMembershipKeyReference<IFile, IFile>
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="Baseline" /> and <see cref="Correlative" /> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true" /> if <see cref="Baseline" /> and <see cref="Correlative" /> are identical byte-for-byte; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AreEqual), ResourceType = typeof(Properties.Resources))]
        bool AreEqual { get; }

        /// <summary>
        /// Gets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="Baseline" /> was compared to <see cref="Correlative" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ComparedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ComparedOn { get; }

        /// <summary>
        /// Gets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="Baseline" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_BaselineId), ResourceType = typeof(Properties.Resources))]
        Guid BaselineId { get; }

        /// <summary>
        /// Gets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile" /> that represents the baseline file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Baseline), ResourceType = typeof(Properties.Resources))]
        IFile Baseline { get; }

        /// <summary>
        /// Gets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the foreign key that refers to the <see cref="Correlative" /><see cref="IFile">file entity</see>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CorrelativeId), ResourceType = typeof(Properties.Resources))]
        Guid CorrelativeId { get; }

        /// <summary>
        /// Gets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Correlative), ResourceType = typeof(Properties.Resources))]
        IFile Correlative { get; }

        /// <summary>
        /// Gets the value of the <see cref="BaselineId" /> property or the unique identifier of the <see cref="Baseline" /> entity if it has been assigned.
        /// </summary>
        /// <param name="baselineId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IFile" /> baseline entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetBaselineId(out Guid baselineId);

        /// <summary>
        /// Gets value of the <see cref="CorrelativeId" /> property or the unique identifier of the <see cref="Correlative" /> entity if it has been assigned.
        /// </summary>
        /// <param name="correlativeId">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier for the associated <see cref="IFile" /> correlative entity has been set; otherwise, <see langword="false" />.</returns>
        bool TryGetCorrelativeId(out Guid correlativeId);
    }
}
