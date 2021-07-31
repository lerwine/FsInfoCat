using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>File-specific crawl options.</summary>
    [Flags]
    public enum FileCrawlOptions : byte
    {
        /// <summary>No options selected.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_NoOptionsSelected), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>Do not compare the current <see cref="IFile" /> for redundancies.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_DoNotCompare), ResourceType = typeof(Properties.Resources))]
        DoNotCompare = 1,

        /// <summary>Ignore the current <see cref="IFile" />.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileCrawlOptions_Ignore), ResourceType = typeof(Properties.Resources))]
        Ignore = 2,

        /// <summary>Mark <see cref="IFile" /> for deletion.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 4,

        /// <summary>Mark <see cref="IFile" /> for re-scan.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_FlaggedForRescan), ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 8
    }

}

