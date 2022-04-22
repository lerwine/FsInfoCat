using FsInfoCat.Activities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
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
