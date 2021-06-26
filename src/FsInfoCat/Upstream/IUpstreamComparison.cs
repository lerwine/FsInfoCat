namespace FsInfoCat.Upstream
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <seealso cref="IComparison" />
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IUpstreamComparison : IComparison, IUpstreamDbEntity
    {
        /// <summary>
        /// Gets or sets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamFile"/> that represents the baseline file in the comparison.</value>
        new IUpstreamFile Baseline { get; set; }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="IUpstreamFile"/> that represents the correlative file, which is the new or changed file in the comparison.</value>
        new IUpstreamFile Correlative { get; set; }
    }
}
