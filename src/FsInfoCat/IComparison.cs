using System;

namespace FsInfoCat
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    public interface IComparison : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="Baseline"/>.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid BaselineId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="Correlative"/>, which is the new or changed file that is being compared to a <see cref="Baseline"/> file.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        Guid CorrelativeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Baseline"/> and <see cref="Correlative"/> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="Baseline"/> and <see cref="Correlative"/> are identical byte-for-byte; otherwise, <see langword="false"/>.</value>
        bool AreEqual { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="Baseline"/> was compared to <see cref="Correlative"/>.</value>
        DateTime ComparedOn { get; set; }

        /// <summary>
        /// Gets or sets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile"/> that represents the baseline file in the comparison.</value>
        IFile Baseline { get; set; }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IFile"/> that represents the correlative file, which is the new or changed file in the comparison.</value>
        IFile Correlative { get; set; }
    }
}
