using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public enum ActivityCode
    {
        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlingFileSystem), ShortName = nameof(Properties.Resources.DisplayName_Crawling), Description = nameof(Properties.Resources.DisplayName_CrawlingFileSystem), ResourceType = typeof(Properties.Resources))]
        CrawlingFileSystem = 1,

        [Display(Name = nameof(Properties.Resources.DisplayName_GettingDriveInfo), ShortName = nameof(Properties.Resources.DisplayName_GettingDriveInfo), Description = nameof(Properties.Resources.DisplayName_GettingDriveInfo), ResourceType = typeof(Properties.Resources))]
        GettingDriveInfo = 2,
        DeletingVolume = 3,
        DeletingCrawlConfiguration = 4,
        DeletingSubdirectory = 5,
        DeletingFile = 6,
        ImportingSubdirectory = 7,
        SettingBranchIncomplete = 8
    }
}
