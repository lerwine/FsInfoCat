using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobLogEditViewModel<TEntity, TCrawlConfiguration> : CrawlJobRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlJobLog
        where TCrawlConfiguration : ICrawlConfiguration
    {
        public CrawlJobLogEditViewModel([DisallowNull] TEntity entity, TCrawlConfiguration crawlConfiguration) : base(entity)
        {
            // TODO: Implement CrawlJobLogEditViewModel
        }

        public TCrawlConfiguration Configuration => throw new NotImplementedException();

        public ushort MaxRecursionDepth => throw new NotImplementedException();

        public ulong? MaxTotalItems => throw new NotImplementedException();

        public TimeSpanViewModel TTL => throw new NotImplementedException();
    }
}
