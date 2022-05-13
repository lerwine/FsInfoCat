using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="ICrawlSettings" />
    public interface ICrawlJobLog : ICrawlJobLogRow, IEquatable<ICrawlJobLog>
    {
        /// <summary>Gets the configuration source for the file system crawl.</summary>
        /// <value>The configuration for the file system crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Configuration), ResourceType = typeof(Properties.Resources))]
        ICrawlConfiguration Configuration { get; }
    }
}
