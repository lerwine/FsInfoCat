﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat
{
    public interface ICrawlJobLogRow : IDbEntity, ICrawlSettings, IHasSimpleIdentifier
    {
        /// <summary>Gets root path of the crawl.</summary>
        /// <value>The root path of the crawl.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RootPath), ResourceType = typeof(Properties.Resources))]
        string RootPath { get; }

        /// <summary>Gets a value indicating whether the current crawl configuration has been deactivated.</summary>
        /// <value>
        /// <see langword="true" /> if the current crawl configuration has been deactivated; otherwise, <see langword="false" /> to indicate that is
        /// available for use.
        /// </value>
        [Display(Name = nameof(Properties.Resources.DisplayName_StatusCode), ResourceType = typeof(Properties.Resources))]
        CrawlStatus StatusCode { get; }

        /// <summary>Gets the date and time when the crawl was started.</summary>
        /// <value>The date and time when the crawl was started.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlStart), ResourceType = typeof(Properties.Resources))]
        DateTime CrawlStart { get; }

        /// <summary>Gets the date and time when the crawl was finshed.</summary>
        /// <value>The date and time when the crawl was finshed or <see langword="null" /> if the current crawl is still active.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CrawlEnd), ResourceType = typeof(Properties.Resources))]
        DateTime? CrawlEnd { get; }

        /// <summary>Gets the status message.</summary>
        /// <value>The crawl status message.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Message), ResourceType = typeof(Properties.Resources))]
        string StatusMessage { get; }

        /// <summary>Gets the status details.</summary>
        /// <value>The detailed status results.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Details), ResourceType = typeof(Properties.Resources))]
        string StatusDetail { get; }

        /// <summary>Gets the total number of folders crawled.</summary>
        /// <value>The total number of folders crawled.</value>
        long FoldersProcessed { get; }

        /// <summary>Gets the total number of files read.</summary>
        /// <value>The total number of files read.</value>
        long FilesProcessed { get; }
    }
}
