using FsInfoCat.Local.Model;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel.Filter.CrawlConfig
{
    public sealed class RelativeNextScheduledStart : NullableSchedulableDateTimeFilter<CrawlConfigReportItem>
    {
        public RelativeNextScheduledStart() : base(nameof(Model.ICrawlConfigReportItem.NextScheduledStart)) { }

        protected override DateTime? GetMemberValue([DisallowNull] CrawlConfigReportItem crawlConfiguration) => crawlConfiguration.NextScheduledStart;
    }
}
