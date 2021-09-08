using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobLogEditViewModel<TEntity, TCrawlConfigEntity, TCrawlConfigViewModel> : CrawlJobRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlJobLog
        where TCrawlConfigEntity : DbEntity, ICrawlConfigurationListItem
        where TCrawlConfigViewModel : CrawlConfigListItemViewModel<TCrawlConfigEntity>
    {
        public CrawlJobLogEditViewModel([DisallowNull] TEntity entity, TCrawlConfigViewModel crawlConfiguration) : base(entity)
        {
            // TODO: Implement CrawlJobLogEditViewModel
        }

        public TCrawlConfigEntity Configuration => throw new NotImplementedException();

        public ushort MaxRecursionDepth => throw new NotImplementedException();

        public ulong? MaxTotalItems => throw new NotImplementedException();

        public TimeSpanViewModel TTL => throw new NotImplementedException();
    }
}
