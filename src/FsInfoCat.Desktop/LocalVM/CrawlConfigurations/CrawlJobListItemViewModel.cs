﻿using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.LocalVM.CrawlConfigurations
{
    public class CrawlJobListItemViewModel : CrawlJobListItemViewModel<CrawlJobLogListItem>
    {
        public CrawlJobListItemViewModel([DisallowNull] CrawlJobLogListItem entity) : base(entity) { }
    }
}