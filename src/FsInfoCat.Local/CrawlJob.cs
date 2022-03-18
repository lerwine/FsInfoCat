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

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJob other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CrawlJob other) => other is not null && ReferenceEquals(this, other) || ArePropertiesEqual(this);

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
            unchecked
            {
                int hash = EntityExtensions.HashNullable(LogEntityId, 11, 17);
                hash = hash * 17 + ConfigurationId.GetHashCode();
                hash = hash * 17 + MaxRecursionDepth.GetHashCode();
                hash = EntityExtensions.HashNullable(MaxTotalItems, hash, 17);
                hash = EntityExtensions.HashNullable(TTL, hash, 17);
                return hash;
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
