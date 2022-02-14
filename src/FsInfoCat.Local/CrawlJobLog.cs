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

        public bool Equals(CrawlJobLog other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ICrawlJobLog other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
