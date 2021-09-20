namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class CrawlStatusItemVM<TEntity> : EnumValueViewModel<CrawlStatus>
        where TEntity : class, ICrawlConfigurationRow
    {
        public CrawlStatusItemVM(CrawlStatus crawlStatus) : base(crawlStatus) { }

        protected override void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedPropertyChanged(oldValue, newValue);
            if (newValue)
                CrawlStatusOptions<TEntity>.GetOwner(this)?.Select(Value.Value);
            else
                CrawlStatusOptions<TEntity>.GetOwner(this)?.Deselect(Value.Value);
        }
    }
}
