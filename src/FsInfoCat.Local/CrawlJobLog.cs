using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlJobLog" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlJobLog : CrawlJobLogRow, ILocalCrawlJobLog, IEquatable<CrawlJobLog>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private CrawlConfiguration _crawlConfiguration;

        public override Guid ConfigurationId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _crawlConfiguration?.Id;
                    if (id.HasValue && id.Value != base.ConfigurationId)
                    {
                        base.ConfigurationId = id.Value;
                        return id.Value;
                    }
                    return base.ConfigurationId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _crawlConfiguration?.Id;
                    if (id.HasValue && id.Value != value)
                        _crawlConfiguration = null;
                    base.ConfigurationId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>
        /// The configuration for the file system crawl.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Configuration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_crawlConfiguration))]
        public CrawlConfiguration Configuration
        {
            get => _crawlConfiguration;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_crawlConfiguration is not null)
                            base.ConfigurationId = Guid.Empty;
                    }
                    else
                        base.ConfigurationId = value.Id;
                    _crawlConfiguration = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        ICrawlConfiguration ICrawlJobLog.Configuration => Configuration;

        ILocalCrawlConfiguration ILocalCrawlJobLog.Configuration => Configuration;

        internal static void OnBuildEntity(EntityTypeBuilder<CrawlJobLog> builder) => _ = builder.HasOne(sn => sn.Configuration).WithMany(d => d.Logs).HasForeignKey(nameof(ConfigurationId)).IsRequired().OnDelete(DeleteBehavior.Restrict);

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobLog other) => ArePropertiesEqual((ILocalCrawlJobLogRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJobLog other) => ArePropertiesEqual((ICrawlJobLogRow)other) && ConfigurationId.Equals(other?.Configuration.Id ?? Guid.Empty);

        public bool Equals(CrawlJobLog other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (TryGetId(out Guid id))
                return other.TryGetId(out Guid g) && id.Equals(g);
            return !other.TryGetId(out _) && ArePropertiesEqual(other) && ConfigurationId.Equals(other.ConfigurationId);
        }

        public bool Equals(ICrawlJobLog other)
        {
            if (other is null) return false;
            if (other is CrawlJobLog crawlJobLog) return Equals(crawlJobLog);
            if (TryGetId(out Guid id)) id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalCrawlJobLog localJobLog)
                return ArePropertiesEqual(localJobLog);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(ICrawlJob other)
        {
            if (other is null) return false;
            if (other is CrawlJobLog crawlJobLog) return Equals(crawlJobLog);
            if (other is ICrawlJobLogRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlJobLog localJobLog)
                    return ArePropertiesEqual(localJobLog);
                if (row is ICrawlJobLog jobLog)
                    return ArePropertiesEqual(jobLog);
                if (row is (ILocalCrawlJobLogRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is CrawlJobLog crawlJobLog) return Equals(crawlJobLog);
            if (obj is ICrawlJobLogRow row)
            {
                if (TryGetId(out Guid id)) id.Equals(row.Id);
                if (!row.Id.Equals(Guid.Empty)) return false;
                if (row is ILocalCrawlJobLog localJobLog)
                    return ArePropertiesEqual(localJobLog);
                if (row is ICrawlJobLog jobLog)
                    return ArePropertiesEqual(jobLog);
                if (row is (ILocalCrawlJobLogRow localRow))
                    return ArePropertiesEqual(localRow);
                return ArePropertiesEqual(row);
            }
            return obj is ICrawlJob job && ArePropertiesEqual(job);
        }
    }
}
