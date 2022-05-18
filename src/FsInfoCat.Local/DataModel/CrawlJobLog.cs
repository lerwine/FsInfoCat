using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Log of crawl job results.
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlJobLog" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class CrawlJobLog : CrawlJobLogRow, ILocalCrawlJobLog, IEquatable<CrawlJobLog>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private readonly CrawlConfigurationReference _crawlConfiguration;

    // TODO: Document CrawlJobLog class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override Guid ConfigurationId { get => _crawlConfiguration.Id; set => _crawlConfiguration.SetId(value); }

        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>
        /// The configuration for the file system crawl.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Configuration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlConfiguration Configuration { get => _crawlConfiguration.Entity; set => _crawlConfiguration.Entity = value; }

        ICrawlConfiguration ICrawlJobLog.Configuration => Configuration;

        ILocalCrawlConfiguration ILocalCrawlJobLog.Configuration => Configuration;

        public CrawlJobLog() { _crawlConfiguration = new(SyncRoot); }

        internal static void OnBuildEntity(EntityTypeBuilder<CrawlJobLog> builder) => _ = builder.HasOne(sn => sn.Configuration).WithMany(d => d.Logs).HasForeignKey(nameof(ConfigurationId)).IsRequired().OnDelete(DeleteBehavior.Restrict);

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobLog other) => ArePropertiesEqual((ILocalCrawlJobLogRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJobLog other) => ArePropertiesEqual((ICrawlJobLogRow)other) && ConfigurationId.Equals(other?.Configuration.Id ?? Guid.Empty);

        public bool Equals(CrawlJobLog other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)) &&
            ConfigurationId.Equals(other.ConfigurationId));

        public bool Equals(ILocalCrawlJobLog other)
        {
            if (other is null) return false;
            if (other is CrawlJobLog crawlJobLog) return Equals(crawlJobLog);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(ICrawlJobLog other)
        {
            if (other is null) return false;
            if (other is CrawlJobLog crawlJobLog) return Equals(crawlJobLog);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalCrawlJobLog local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
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

        protected class CrawlConfigurationReference : ForeignKeyReference<CrawlConfiguration>, IForeignKeyReference<ILocalCrawlConfiguration>, IForeignKeyReference<ICrawlConfiguration>
        {
            internal CrawlConfigurationReference(object syncRoot) : base(syncRoot) { }

            ILocalCrawlConfiguration IForeignKeyReference<ILocalCrawlConfiguration>.Entity => Entity;

            ICrawlConfiguration IForeignKeyReference<ICrawlConfiguration>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalCrawlConfiguration>>.Equals(IForeignKeyReference<ILocalCrawlConfiguration> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ICrawlConfiguration>>.Equals(IForeignKeyReference<ICrawlConfiguration> other)
            {
                throw new NotImplementedException();
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
