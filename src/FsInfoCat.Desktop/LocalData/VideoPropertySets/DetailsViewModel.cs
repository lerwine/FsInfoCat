using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    public class DetailsViewModel : VideoPropertySetDetailsViewModel<VideoPropertySet, FileWithBinaryPropertiesAndAncestorNames, FileWithBinaryPropertiesAndAncestorNamesViewModel>,
        INavigatedToNotifiable, INavigatingFromNotifiable
    {
        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private void RaiseChangesSaved(object args) => ChangesSaved?.Invoke(this, new Commands.CommandEventArgs(args));

        #region Edit Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyPropertyBuilder<DetailsViewModel, Commands.RelayCommand>
            .Register(nameof(Edit))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected void RaiseEditCommand(object parameter) // => EditCommand?.Invoke(this, new(parameter));
        {
            try { OnEditCommand(parameter); }
            finally { EditCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void OnEditCommand(object parameter)
        {
            // TODO: Implement OnEditCommand Logic
        }

        #endregion
        #region Delete Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyPropertyBuilder<DetailsViewModel, Commands.RelayCommand>
            .Register(nameof(Delete))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected void RaiseDeleteCommand(object parameter) // => DeleteCommand?.Invoke(this, new(parameter));
        {
            try { OnDeleteCommand(parameter); }
            finally { DeleteCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void OnDeleteCommand(object parameter)
        {
            // TODO: Implement OnDeleteCommand Logic
            // TODO: Add initialization code to constructor: 
        }

        #endregion

        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<DetailsViewModel, VideoPropertiesListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public VideoPropertiesListItem ListItem { get => (VideoPropertiesListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

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

        public DetailsViewModel([DisallowNull] VideoPropertySet entity, VideoPropertiesListItem listItem) : base(entity)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            ListItem = listItem;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        //private static async Task<VideoPropertySet> EditItemAsync([DisallowNull] VideoPropertiesListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        //{
        //    using IServiceScope serviceScope = Services.CreateScope();
        //    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
        //    Guid id = item.Id;
        //    statusListener.SetMessage("Reading data");
        //    return await dbContext.VideoPropertySets.Include(e => e.Files).FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        //}

        //public static Task EditItemAsync([DisallowNull] VideoPropertiesListItem item, ReturnEventHandler<ItemEditResult> onReturn = null)
        //{
        //    if (item is null)
        //        throw new ArgumentNullException(nameof(item));
        //    IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
        //    return jobFactory.StartNew("Loading database record", "Opening database", item, EditItemAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
        //    {
        //        VideoPropertySet entity = task.Result;
        //        EditViewModel viewModel = new(entity, false) { ListItem = item };
        //        EditPage page = new(viewModel);
        //        if (onReturn is not null)
        //            page.Return += onReturn;
        //        Services.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
        //    }));
        //}

        public static void AddNewItem(ReturnEventHandler<VideoPropertySet> onReturn = null)
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