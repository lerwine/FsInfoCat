using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{

    /// <summary>Log of crawl job results.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlJobLog" />
    public class CrawlJobLog : CrawlJobLogRow, ILocalCrawlJobLog
    {
        private readonly IPropertyChangeTracker<CrawlConfiguration> _configuration;

        /// <summary>
        /// Gets the configuration source for the file system crawl.
        /// </summary>
        /// <value>
        /// The configuration for the file system crawl.
        /// </value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Configuration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlConfiguration Configuration
        {
            get => _configuration.GetValue();
            set
            {
                if (_configuration.SetValue(value))
                    ConfigurationId = value?.Id ?? Guid.Empty;
            }
        }

        ICrawlConfiguration ICrawlJobLog.Configuration => Configuration;

        ILocalCrawlConfiguration ILocalCrawlJobLog.Configuration => Configuration;

        public CrawlJobLog()
        {
            _configuration = AddChangeTracker<CrawlConfiguration>(nameof(Configuration), null);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<CrawlJobLog> builder)
        {
            _ = builder.HasOne(sn => sn.Configuration).WithMany(d => d.Logs).HasForeignKey(nameof(ConfigurationId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
