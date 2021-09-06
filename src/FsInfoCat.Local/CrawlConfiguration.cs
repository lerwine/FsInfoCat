using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Root), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Logs), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

        public static async Task<int> DeleteAsync(CrawlConfiguration target, LocalDbContext dbContext, IStatusListener statusListener)
        {
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using IDisposable loggerScope = statusListener.Logger.BeginScope(target.Id);
            statusListener.Logger.LogInformation("Removing CrawlConfiguration {{ Id = {Id}; Path = \"{Path}\" }}", target.Id, target.DisplayName);
            statusListener.SetMessage($"Removing crawl configuration record: {target.DisplayName}");
            EntityEntry<CrawlConfiguration> entry = dbContext.Entry(target);
            CrawlJobLog[] logs = (await entry.GetRelatedCollectionAsync(e => e.Logs, statusListener.CancellationToken)).ToArray();
            int result;
            if (logs.Length > 0)
            {
                dbContext.CrawlJobLogs.RemoveRange(logs);
                result = await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            }
            else
                result = 0;
            dbContext.CrawlConfigurations.Remove(target);
            result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await transaction.CommitAsync(statusListener.CancellationToken);
            return result;
        }
    }
}
