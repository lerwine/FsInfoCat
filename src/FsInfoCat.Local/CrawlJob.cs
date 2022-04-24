using FsInfoCat.Activities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public partial class CrawlJob : ICrawlJob, IEquatable<CrawlJob>
    {
        private DateTime _stopAt;
        private ulong _remainingtotalItems;
        private IActivityProgress _progress;

        public Guid? LogEntityId { get; }

        public Guid ConfigurationId { get; }

        public DateTime CrawlStart { get; }

        public string StatusMessage { get; private set; }

        public string StatusDetail { get; private set; }

        public long FoldersProcessed { get; private set; }

        public long FilesProcessed { get; private set; }

        public ulong TotalCount { get; private set; }

        public ushort MaxRecursionDepth { get; }

        public ulong? MaxTotalItems { get; private set; }

        public long? TTL { get; }

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJob other) => CrawlStart == other.CrawlStart &&
            StatusMessage == other.StatusMessage &&
            StatusDetail == other.StatusDetail &&
            FoldersProcessed == other.FoldersProcessed &&
            FilesProcessed == other.FilesProcessed &&
            MaxRecursionDepth == other.MaxRecursionDepth &&
            MaxTotalItems == other.MaxTotalItems &&
            TTL == other.TTL;

        public bool Equals(CrawlJob other) => other is not null && ReferenceEquals(this, other) || (ArePropertiesEqual(this) && EqualityComparer<Guid?>.Default.Equals(LogEntityId, other.LogEntityId) && ConfigurationId.Equals(other.ConfigurationId) && TotalCount == other.TotalCount);

        public bool Equals(ICrawlJob other)
        {
            if (other is null) return false;
            return (other is CrawlJob crawlJob) ? Equals(crawlJob) : ArePropertiesEqual(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return (obj is CrawlJob crawlJob) ? Equals(crawlJob) : obj is ICrawlJob other && ArePropertiesEqual(other);
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(LogEntityId);
            hash.Add(ConfigurationId);
            hash.Add(CrawlStart);
            hash.Add(StatusMessage);
            hash.Add(StatusDetail);
            hash.Add(FoldersProcessed);
            hash.Add(FilesProcessed);
            hash.Add(TotalCount);
            hash.Add(MaxRecursionDepth);
            hash.Add(MaxTotalItems);
            hash.Add(TTL);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
