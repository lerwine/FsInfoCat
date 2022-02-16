using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlJobLog" />
    public class CrawlJobLog : CrawlJobLogRow, ILocalCrawlJobLog, IEquatable<CrawlJobLog>
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
                    {
                        base.ConfigurationId = value.Id;
                        _crawlConfiguration = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        ICrawlConfiguration ICrawlJobLog.Configuration => Configuration;

        ILocalCrawlConfiguration ILocalCrawlJobLog.Configuration => Configuration;

        internal static void OnBuildEntity(EntityTypeBuilder<CrawlJobLog> builder)
        {
            _ = builder.HasOne(sn => sn.Configuration).WithMany(d => d.Logs).HasForeignKey(nameof(ConfigurationId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalCrawlJobLog other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ICrawlJobLog other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(CrawlJobLog other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(ICrawlJobLog other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ICrawlJob other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 23;
                    hash = (Configuration is null) ? (ConfigurationId.Equals(Guid.Empty) ? hash * 109 : hash * 109 + ConfigurationId.GetHashCode()) : hash * 109 + (Configuration?.GetHashCode() ?? 0);
                    hash = hash * 31 + RootPath.GetHashCode();
                    hash = hash * 31 + MaxRecursionDepth.GetHashCode();
                    hash = MaxTotalItems.HasValue ? hash * 31 + (MaxTotalItems ?? default).GetHashCode() : hash * 31;
                    hash = TTL.HasValue ? hash * 31 + (TTL ?? default).GetHashCode() : hash * 31;
                    hash = UpstreamId.HasValue ? hash * 31 + (UpstreamId ?? default).GetHashCode() : hash * 31;
                    hash = LastSynchronizedOn.HasValue ? hash * 31 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 31;
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
