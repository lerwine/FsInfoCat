using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel.Filter.CrawlConfig
{
    public sealed class RelativeLastCrawlEnd : NullableDateTimeFilter<CrawlConfigReportItem>
    {
        public RelativeLastCrawlEnd() : base(nameof(ICrawlConfigReportItem.LastCrawlEnd)) { }

        protected override DateTime? GetMemberValue([DisallowNull] CrawlConfigReportItem crawlConfiguration) => crawlConfiguration.LastCrawlEnd;
    }
}
