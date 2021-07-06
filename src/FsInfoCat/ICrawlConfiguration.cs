using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Configuration of a file system crawl instance.
    /// </summary>
    public interface ICrawlConfiguration : IDbEntity
    {
        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name for the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the custom notes.
        /// </summary>
        /// <value>The custom notes to associate with the current crawl configuration.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; set; }

        /// <summary>
        /// Gets the root subdirectory of the configured subdirectory crawl.
        /// </summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        ISubdirectory Root { get; }

        /// <summary>
        /// Gets or sets the maximum recursion depth.
        /// </summary>
        /// <value>The maximum recursion depth.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxRecursionDepth), ResourceType = typeof(Properties.Resources))]
        ushort MaxRecursionDepth { get; set; }

        /// <summary>
        /// Gets or sets the maximum total items to crawl.
        /// </summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxTotalItems), ResourceType = typeof(Properties.Resources))]
        ulong MaxTotalItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current crawl configuration is inactive.
        /// </summary>
        /// <value><see langword="true"/> if the current crawl configuration is inactive; otherwise, <see langword="false"/>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; set; }
    }
}
