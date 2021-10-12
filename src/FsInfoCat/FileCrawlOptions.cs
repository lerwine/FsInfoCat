using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>File-specific crawl options.</summary>
    [Flags]
    public enum FileCrawlOptions : byte
    {
        /// <summary>No options selected.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_NoOptionsSelected), ShortName = nameof(Properties.Resources.DisplayName_None), Description = nameof(Properties.Resources.Description_FileCrawlOptions_None),
            ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>Do not compare the current <see cref="IFile" /> for redundancies.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_DoNotCompare), ShortName = nameof(Properties.Resources.DisplayName_DoNotCompare),
            Description = nameof(Properties.Resources.Description_FileCrawlOptions_DoNotCompare), ResourceType = typeof(Properties.Resources))]
        DoNotCompare = 1,

        /// <summary>Ignore the current <see cref="IFile" />.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_Ignore), ShortName = nameof(Properties.Resources.DisplayName_Ignore), Description = nameof(Properties.Resources.Description_FileCrawlOptions_Ignore),
            ResourceType = typeof(Properties.Resources))]
        Ignore = 2,

        /// <summary>Mark <see cref="IFile" /> for deletion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForDeletion), ShortName = nameof(Properties.Resources.DisplayName_ToDelete),
            Description = nameof(Properties.Resources.Description_FileCrawlOptions_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 4,

        /// <summary>Mark <see cref="IFile" /> for re-scan.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForRescan), ShortName = nameof(Properties.Resources.DisplayName_Rescan), Description = nameof(Properties.Resources.Description_FileCrawlOptions_FlaggedForRescan),
            ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 8
    }
}

