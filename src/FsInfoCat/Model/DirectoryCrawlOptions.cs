using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Directory-specific crawl option flags.
    /// </summary>
    [Flags]
    public enum DirectoryCrawlOptions2 : byte
    {
        /// <summary>
        /// No options selected.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.NoOptionsSelected), ShortName = nameof(Properties.Resources.None),
            Description = nameof(Properties.Resources.Description_NoOptionsSelected), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>
        /// Do not crawl nested <see cref="ISubdirectory">sub-directories</see>.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.SkipChildSubdirectories), ShortName = nameof(Properties.Resources.SkipChildSubdirectories),
            Description = nameof(Properties.Resources.Description_SkipSubdirectorySubdirectories), ResourceType = typeof(Properties.Resources))]
        SkipSubdirectories = 1,

        /// <summary>
        /// Do not crawl the current <see cref="ISubdirectory" />.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.SkipSubdirectory), ShortName = nameof(Properties.Resources.SkipSubdirectory),
            Description = nameof(Properties.Resources.Description_SkipSubdirectory), ResourceType = typeof(Properties.Resources))]
        Skip = 2,

        /// <summary>
        /// Do not compare files for redundancies.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DoNotCompareFiles), ShortName = nameof(Properties.Resources.DoNotCompareFiles),
            Description = nameof(Properties.Resources.Description_DoNotCompareFiles), ResourceType = typeof(Properties.Resources))]
        DoNotCompareFiles = 4,

        /// <summary>
        /// Do not show contents of <see cref="ISubdirectory" /> in public result displays.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.DoNotShow), ShortName = nameof(Properties.Resources.Hide),
            Description = nameof(Properties.Resources.Description_DoNotShow), ResourceType = typeof(Properties.Resources))]
        DoNotShow = 8,

        /// <summary>
        /// Mark <see cref="ISubdirectory" /> for deletion.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.FlaggedForDeletion), ShortName = nameof(Properties.Resources.ToDelete),
            Description = nameof(Properties.Resources.Description_FlaggedForDeletion), ResourceType = typeof(Properties.Resources))]
        FlaggedForDeletion = 16,

        /// <summary>
        /// Mark <see cref="ISubdirectory" /> for re-scan.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.FlaggedForRescan), ShortName = nameof(Properties.Resources.Rescan),
            Description = nameof(Properties.Resources.DirectoryFlaggedForReScan), ResourceType = typeof(Properties.Resources))]
        FlaggedForRescan = 32
    }
}

