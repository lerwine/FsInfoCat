namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class CrawlStatusItemVM<TEntity>(Model.CrawlStatus crawlStatus) : EnumValueViewModel<Model.CrawlStatus>(crawlStatus)
        where TEntity : class, Model.ICrawlConfigurationRow
    {
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
