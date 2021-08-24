using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Local
{
    /// <summary>Specifies the configuration of a file system crawl.</summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalCrawlConfiguration" />
    public class CrawlConfiguration : CrawlConfigurationRow, ILocalCrawlConfiguration, ISimpleIdentityReference<CrawlConfiguration>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Subdirectory> _root;
        private HashSet<CrawlJobLog> _logs = new();

        #endregion

        #region Properties

        /// <summary>Gets the starting subdirectory for the configured subdirectory crawl.</summary>
        /// <value>The root subdirectory of the configured subdirectory crawl.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Root), ResourceType = typeof(Properties.Resources))]
        public Subdirectory Root
        {
            get => _root.GetValue(); set
            {
                if (_root.SetValue(value))
                    RootId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>Gets the crawl log entries.</summary>
        /// <value>The crawl log entries.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Logs), ResourceType = typeof(Properties.Resources))]
        public virtual HashSet<CrawlJobLog> Logs
        {
            get => _logs;
            set => CheckHashSetChanged(_logs, value, h => _logs = h);
        }

        #endregion

        ILocalSubdirectory ILocalCrawlConfiguration.Root => Root;

        ISubdirectory ICrawlConfiguration.Root => Root;

        IEnumerable<ICrawlJobLog> ICrawlConfiguration.Logs => Logs.Cast<ICrawlJobLog>();

        IEnumerable<ILocalCrawlJobLog> ILocalCrawlConfiguration.Logs => Logs.Cast<ILocalCrawlJobLog>();

        CrawlConfiguration IIdentityReference<CrawlConfiguration>.Entity => this;

        public CrawlConfiguration()
        {
            _root = AddChangeTracker<Subdirectory>(nameof(Root), null);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<CrawlConfiguration> builder)
        {
            builder.HasOne(s => s.Root).WithOne(c => c.CrawlConfiguration).HasForeignKey<CrawlConfiguration>(nameof(RootId)).OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnRootIdChanged(Guid value)
        {
            Subdirectory nav = _root.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _root.SetValue(null);
        }
    }
}
