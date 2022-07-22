using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// File-specific crawl options.
    /// </summary>
    [Flags]
    public enum FileCrawlOptions : byte
    {
        /// <summary>
        /// No options selected.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.NoOptionsSelected), ShortName = nameof(Properties.Resources.None), Description = nameof(Properties.Resources.NoFileSpecificCrawlOptionsSelected),
            ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>
        /// Do not compare the current <see cref="IFile" /> for redundancies.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DoNotCompare), ShortName = nameof(Properties.Resources.DoNotCompare),
            Description = nameof(Properties.Resources.Description_DoNotCompare), ResourceType = typeof(Properties.Resources))]
        DoNotCompare = 1,

        /// <summary>
        /// Ignore the current <see cref="IFile" />.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.IgnoreFile), ShortName = nameof(Properties.Resources.Ignore), Description = nameof(Properties.Resources.Description_IgnoreFile),
            ResourceType = typeof(Properties.Resources))]
        Ignore = 2,

        /// <summary>
        /// Mark <see cref="IFile" /> for deletion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.FlaggedForDeletion), ShortName = nameof(Properties.Resources.ToDelete),
            Description = nameof(Properties.Resources.Description_FlaggedFlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 4,

        /// <summary>
        /// Mark <see cref="IFile" /> for re-scan.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.FlaggedForRescan), ShortName = nameof(Properties.Resources.Rescan), Description = nameof(Properties.Resources.Description_FlaggedForReScan),
            ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 8
    }
}

