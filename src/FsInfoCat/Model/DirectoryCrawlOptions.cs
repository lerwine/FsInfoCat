using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Directory-specific crawl option flags.
    /// </summary>
    [Flags]
    public enum DirectoryCrawlOptions : byte
    {
        /// <summary>
        /// No options selected.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_NoOptionsSelected), ShortName = nameof(Properties.Resources.DisplayName_None),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_None), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>
        /// Do not crawl nested <see cref="ISubdirectory">sub-directories</see>.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_SkipSubdirectories), ShortName = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_SkipSubdirectories),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_SkipSubdirectories), ResourceType = typeof(Properties.Resources))]
        SkipSubdirectories = 1,

        /// <summary>
        /// Do not crawl the current <see cref="ISubdirectory" />.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_Skip), ShortName = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_Skip),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_Skip), ResourceType = typeof(Properties.Resources))]
        Skip = 2,

        /// <summary>
        /// Do not compare files for redundancies.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_DoNotCompareFiles), ShortName = nameof(Properties.Resources.DisplayName_DirectoryCrawlOptions_DoNotCompareFiles),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_DoNotCompareFiles), ResourceType = typeof(Properties.Resources))]
        DoNotCompareFiles = 4,

        /// <summary>
        /// Do not show contents of <see cref="ISubdirectory" /> in public result displays.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_DoNotShow), ShortName = nameof(Properties.Resources.DisplayName_Hide),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_DoNotShow), ResourceType = typeof(Properties.Resources))]
        DoNotShow = 8,

        /// <summary>
        /// Mark <see cref="ISubdirectory" /> for deletion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForDeletion), ShortName = nameof(Properties.Resources.DisplayName_ToDelete),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 16,

        /// <summary>
        /// Mark <see cref="ISubdirectory" /> for re-scan.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForRescan), ShortName = nameof(Properties.Resources.DisplayName_Rescan),
            Description = nameof(Properties.Resources.Description_DirectoryCrawlOptions_FlaggedForRescan), ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 32
    }

}

