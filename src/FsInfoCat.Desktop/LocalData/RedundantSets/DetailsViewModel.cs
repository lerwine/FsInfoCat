using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public class DetailsViewModel : RedundantSetRowViewModel<RedundantSet>, INavigatedToNotifiable, INavigatingFromNotifiable
    {
        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));

        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<DetailsViewModel, RedundantSetListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public RedundantSetListItem ListItem { get => (RedundantSetListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyPropertyBuilder<DetailsViewModel, Guid?>
            .Register(nameof(UpstreamId))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyPropertyBuilder<DetailsViewModel, DateTime?>
            .Register(nameof(LastSynchronizedOn))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion

        public DetailsViewModel(RedundantSet entity, RedundantSetListItem listItem) : base(entity)
        {
            ListItem = listItem;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            // TODO: Implement DetailsViewModel
        }

        //private static async Task<RedundantSet> EditItemAsync([DisallowNull] RedundantSetListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        //{
        //    using IServiceScope serviceScope = Services.CreateScope();
        //    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
        //    Guid id = item.Id;
        //    statusListener.SetMessage("Reading data");
        //    return await dbContext.RedundantSets.Include(e => e.BinaryProperties).Include(e => e.Redundancies).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        //}

        //public static Task EditItemAsync([DisallowNull] RedundantSetListItem item, ReturnEventHandler<ItemEditResult> onReturn = null)
        //{
        //    if (item is null)
        //        throw new ArgumentNullException(nameof(item));
        //    IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
        //    return jobFactory.StartNew("Loading database record", "Opening database", item, EditItemAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
        //    {
        //        RedundantSet entity = task.Result;
        //        EditViewModel viewModel = new(entity, false) { ListItem = item };
        //        EditPage page = new(viewModel);
        //        if (onReturn is not null)
        //            page.Return += onReturn;
        //        Services.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
        //    }));
        //}

        public static void AddNewItem(ReturnEventHandler<RedundantSet> onReturn = null)
        {
            // TODO: Implement AddNewItem
        }

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            // TODO: Load option lists from database
            throw new NotImplementedException();
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            // TODO: Prompt to lose changes if not saved
            throw new NotImplementedException();
        }
    }
}
