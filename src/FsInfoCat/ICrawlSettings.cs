﻿using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    /// <summary>Specifies the settings for a file system crawl.</summary>
    public interface ICrawlSettings
    {
        /// <summary>Gets the maximum recursion depth.</summary>
        /// <value>
        /// The maximum sub-folder recursion depth, where a value less than <c>1</c> only crawls the <see cref="Root" /> <see cref="ISubdirectory" />,
        /// a value will crawl 1 sub-folder deep, and so on.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxRecursionDepth), ResourceType = typeof(Properties.Resources))]
        ushort MaxRecursionDepth { get; }

        /// <summary>Gets the maximum total items to crawl.</summary>
        /// <value>The maximum total items to crawl, including both files and subdirectories.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MaxTotalItems), ResourceType = typeof(Properties.Resources))]
        ulong? MaxTotalItems { get; }

        /// <summary>Gets the maximum duration of the crawl.</summary>
        /// <value>The maximum duration of the crawl, in seconds. This value should never be less than <c>1</c>.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_TTL), ResourceType = typeof(Properties.Resources))]
        long? TTL { get; }
    }

}
