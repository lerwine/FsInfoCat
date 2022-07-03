using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a file system crawl job.
    /// </summary>
    /// <seealso cref="ICrawlSettings" />
    /// <seealso cref="IEquatable{ICrawlJob}" />
    public interface ICrawlJob : ICrawlSettings, IEquatable<ICrawlJob>
    {
        /// <summary>
        /// Gets the date and time when the crawl was started.
        /// </summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(Properties.Resources))]
        DateTime CrawlStart { get; }

        /// <summary>
        /// Gets the status message.
        /// </summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string StatusMessage { get; }

        /// <summary>
        /// Gets the status details.
        /// </summary>
        /// <value>The detailed status results.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string StatusDetail { get; }

        /// <summary>
        /// Gets the total number of folders crawled.
        /// </summary>
        /// <value>The total number of folders crawled.</value>
        long FoldersProcessed { get; }

        /// <summary>
        /// Gets the total number of files read.
        /// </summary>
        /// <value>The total number of files read.</value>
        long FilesProcessed { get; }
    }
}
