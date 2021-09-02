using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigEditViewModel<TEntity> : CrawlConfigurationRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfiguration
    {
        // TODO: Implement CrawlConfigEditViewModel properties. Add related item like with FileSystem view model
        public EnumValuePickerVM<CrawlStatus> StatusValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeViewModel LastCrawlStart { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeViewModel LastCrawlEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeViewModel NextScheduledStart { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpanViewModel RescheduleInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpanViewModel TTL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CrawlConfigEditViewModel([DisallowNull] TEntity entity) : base(entity)
        {

        }
    }
}
