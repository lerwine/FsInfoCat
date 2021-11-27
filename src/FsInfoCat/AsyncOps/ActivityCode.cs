using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.AsyncOps
{
    public enum ActivityCode
    {
        /// <summary>An unknown/unspecified error has occurred.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlingFileSystem), ShortName = nameof(Properties.Resources.DisplayName_Crawling), Description = nameof(Properties.Resources.DisplayName_CrawlingFileSystem), ResourceType = typeof(Properties.Resources))]
        CrawlingFileSystem = 1,

        [Display(Name = nameof(Properties.Resources.DisplayName_GettingDriveInfo), ShortName = nameof(Properties.Resources.DisplayName_GettingDriveInfo), Description = nameof(Properties.Resources.DisplayName_GettingDriveInfo), ResourceType = typeof(Properties.Resources))]
        GettingDriveInfo = 2,

        [Display(Name = nameof(Properties.Resources.DisplayName_DeletingVolume), ShortName = nameof(Properties.Resources.DisplayName_DeletingVolume), Description = nameof(Properties.Resources.DisplayName_DeletingVolume), ResourceType = typeof(Properties.Resources))]
        DeletingVolume = 3,

        [Display(Name = nameof(Properties.Resources.DisplayName_DeletingCrawlConfiguration), ShortName = nameof(Properties.Resources.DisplayName_DeletingCrawlConfiguration), Description = nameof(Properties.Resources.DisplayName_DeletingCrawlConfiguration), ResourceType = typeof(Properties.Resources))]
        DeletingCrawlConfiguration = 4,

        [Display(Name = nameof(Properties.Resources.DisplayName_DeletingSubdirectory), ShortName = nameof(Properties.Resources.DisplayName_DeletingSubdirectory), Description = nameof(Properties.Resources.DisplayName_DeletingSubdirectory), ResourceType = typeof(Properties.Resources))]
        DeletingSubdirectory = 5,

        [Display(Name = nameof(Properties.Resources.DisplayName_DeletingFile), ShortName = nameof(Properties.Resources.DisplayName_DeletingFile), Description = nameof(Properties.Resources.DisplayName_DeletingFile), ResourceType = typeof(Properties.Resources))]
        DeletingFile = 6,

        [Display(Name = nameof(Properties.Resources.DisplayName_ImportingSubdirectory), ShortName = nameof(Properties.Resources.DisplayName_ImportingSubdirectory), Description = nameof(Properties.Resources.DisplayName_ImportingSubdirectory), ResourceType = typeof(Properties.Resources))]
        ImportingSubdirectory = 7,

        [Display(Name = nameof(Properties.Resources.DisplayName_SettingBranchIncomplete), ShortName = nameof(Properties.Resources.DisplayName_SettingBranchIncomplete), Description = nameof(Properties.Resources.DisplayName_SettingBranchIncomplete), ResourceType = typeof(Properties.Resources))]
        SettingBranchIncomplete = 8
    }
}
