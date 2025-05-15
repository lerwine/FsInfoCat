using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Specifies the settings for a file system crawl.
    /// </summary>
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="ICrawlJobLogRow" />
    public interface ICrawlSettings
    {
        /// <summary>
        /// Gets the maximum recursion depth.
        /// </summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the root <see cref="Model.ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Display(Name = nameof(Properties.Resources.MaxRecursionDepth), ResourceType = typeof(Properties.Resources))]
        ushort MaxRecursionDepth { get; }

        /// <summary>
        /// Gets the maximum total items to crawl.
        /// </summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.MaxTotalItems), ResourceType = typeof(Properties.Resources))]
        ulong? MaxTotalItems { get; }

        /// <summary>
        /// Gets the maximum duration of the crawl.
        /// </summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Display(Name = nameof(Properties.Resources.TTL), ResourceType = typeof(Properties.Resources))]
        long? TTL { get; }
    }
}
