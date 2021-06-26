namespace FsInfoCat.Local
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 local files.
    /// </summary>
    /// <seealso cref="IComparison" />
    /// <seealso cref="ILocalDbEntity" />
    public interface ILocalComparison : IComparison, ILocalDbEntity
    {
        /// <summary>
        /// Gets or sets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="ILocalFile"/> that represents the baseline file in the comparison.</value>
        new ILocalFile Baseline { get; set; }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="ILocalFile"/> that represents the correlative file, which is the new or changed file in the comparison.</value>
        new ILocalFile Correlative { get; set; }
    }
}
