using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Indicates crawl status for the a <see cref="ISubdirectory" />
    /// </summary>
    public enum DirectoryStatus : byte
    {
        /// <summary>
        /// Not all items in the current <see cref="ISubdirectory" /> have been crawled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CrawlIncomplete), ShortName = nameof(Properties.Resources.CrawlIncomplete),
            Description = nameof(Properties.Resources.Description_CrawlIncomplete), ResourceType = typeof(Properties.Resources))]
        Incomplete = 0,

        /// <summary>
        /// All items in the current <see cref="ISubdirectory" /> have been crawled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.CrawlCompleted), ShortName = nameof(Properties.Resources.CrawlCompleted),
            Description = nameof(Properties.Resources.Description_DirectoryStatusComplete), ResourceType = typeof(Properties.Resources))]
        Complete = 1,

        /// <summary>
        /// An error occurred while trying to enumerate items in the current <see cref="ISubdirectory" />.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.AccessError), ShortName = nameof(Properties.Resources.Error),
            Description = nameof(Properties.Resources.DirectoryAccessError), ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>
        /// The current <see cref="ISubdirectory" /> has been deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Deleted), ShortName = nameof(Properties.Resources.Deleted),
            Description = nameof(Properties.Resources.DirectoryDeleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 3
    }
}

