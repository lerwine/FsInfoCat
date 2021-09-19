namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class CrawlStatusItemVM : EnumValueViewModel<CrawlStatus>
    {
        public CrawlStatusItemVM(CrawlStatus crawlStatus) : base(crawlStatus) { }

        protected override void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedPropertyChanged(oldValue, newValue);
            if (newValue)
                CrawlStatusOptions.GetOwner(this)?.Select(Value.Value);
            else
                CrawlStatusOptions.GetOwner(this)?.Deselect(Value.Value);
        }
    }
}
