using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates crawl status for the a <see cref="ISubdirectory" />
    /// </summary>
    public enum DirectoryStatus : byte
    {
        /// <summary>
        /// Not all items in the current <see cref="ISubdirectory" /> have been crawled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryStatus_Incomplete), ShortName = nameof(Properties.Resources.DisplayName_DirectoryStatus_Incomplete),
            Description = nameof(Properties.Resources.Description_DirectoryStatus_Incomplete), ResourceType = typeof(Properties.Resources))]
        Incomplete = 0,

        /// <summary>
        /// All items in the current <see cref="ISubdirectory" /> have been crawled.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryStatus_Complete), ShortName = nameof(Properties.Resources.DisplayName_DirectoryStatus_Complete),
            Description = nameof(Properties.Resources.Description_DirectoryStatus_Complete), ResourceType = typeof(Properties.Resources))]
        Complete = 1,

        /// <summary>
        /// An error occurred while trying to enumerate items in the current <see cref="ISubdirectory" />.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessError), ShortName = nameof(Properties.Resources.DisplayName_Error),
            Description = nameof(Properties.Resources.Description_DirectoryStatus_AccessError), ResourceType = typeof(Properties.Resources))]
        AccessError = 2,

        /// <summary>
        /// The current <see cref="ISubdirectory" /> has been deleted.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_Deleted), ShortName = nameof(Properties.Resources.DisplayName_Deleted),
            Description = nameof(Properties.Resources.Description_DirectoryStatus_Deleted), ResourceType = typeof(Properties.Resources))]
        Deleted = 3
    }

}

