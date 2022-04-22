using FsInfoCat;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevUtil
{
    public class CrawlJobLogRow
    {
        public Guid Id { get; set; }
        public String RootPath { get; set; }
        public CrawlStatus StatusCode { get; set; }
        public DateTime CrawlStart { get; set; }
        public DateTime? CrawlEnd { get; set; }
        public String StatusMessage { get; set; }
        public String StatusDetail { get; set; }
        public ushort MaxRecursionDepth { get; set; }
        public ulong? MaxTotalItems { get; set; }
        public long? TTL { get; set; }
        public long FoldersProcessed { get; set; }
        public long FilesProcessed { get; set; }
        public Guid ConfigurationId { get; set; }
        public Guid? UpstreamId { get; set; }
        public DateTime? LastSynchronizedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CrawlJobLogRow row &&
                   RootPath == row.RootPath &&
                   StatusCode == row.StatusCode &&
                   CrawlStart == row.CrawlStart &&
                   CrawlEnd == row.CrawlEnd &&
                   StatusMessage == row.StatusMessage &&
                   StatusDetail == row.StatusDetail &&
                   MaxRecursionDepth == row.MaxRecursionDepth &&
                   MaxTotalItems == row.MaxTotalItems &&
                   TTL == row.TTL &&
                   FoldersProcessed == row.FoldersProcessed &&
                   FilesProcessed == row.FilesProcessed &&
                   ConfigurationId.Equals(row.ConfigurationId) &&
                   EqualityComparer<Guid?>.Default.Equals(UpstreamId, row.UpstreamId) &&
                   LastSynchronizedOn == row.LastSynchronizedOn &&
                   CreatedOn == row.CreatedOn &&
                   ModifiedOn == row.ModifiedOn;
        }

    }
}
