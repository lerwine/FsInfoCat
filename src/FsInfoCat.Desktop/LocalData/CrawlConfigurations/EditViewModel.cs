using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class EditViewModel : CrawlConfigurationDetailsViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>,
        INavigatedToNotifiable, INavigatingFromNotifiable
    {
        #region SaveChanges Command Property Members

        /// <summary>
        /// Occurs when the <see cref="SaveChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesSaved;

        private static readonly DependencyPropertyKey SaveChangesPropertyKey = DependencyPropertyBuilder<EditViewModel, Commands.RelayCommand>
            .Register(nameof(SaveChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SaveChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveChangesProperty = SaveChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveChanges => (Commands.RelayCommand)GetValue(SaveChangesProperty);

        /// <summary>
        /// Called when the <see cref="SaveChanges">SaveChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SaveChanges" />.</param>
        protected virtual void OnSaveChangesCommand(object parameter)
        {
            // TODO: Implement OnSaveChangesCommand Logic
        }

        #endregion
        #region DiscardChanges Command Property Members

        /// <summary>
        /// Occurs when the <see cref="DiscardChanges"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ChangesDiscarded;

        private static readonly DependencyPropertyKey DiscardChangesPropertyKey = DependencyPropertyBuilder<EditViewModel, Commands.RelayCommand>
            .Register(nameof(DiscardChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="DiscardChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DiscardChangesProperty = DiscardChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand DiscardChanges => (Commands.RelayCommand)GetValue(DiscardChangesProperty);

        /// <summary>
        /// Called when the <see cref="DiscardChanges">DiscardChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DiscardChanges" />.</param>
        protected virtual void OnDiscardChangesCommand(object parameter)
        {
            // TODO: Implement OnDiscardChangesCommand Logic
        }

        #endregion
        #region ListItem Property Members

        private static readonly DependencyPropertyKey ListItemPropertyKey = DependencyPropertyBuilder<EditViewModel, CrawlConfigListItem>
            .Register(nameof(ListItem))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ListItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListItemProperty = ListItemPropertyKey.DependencyProperty;

        public CrawlConfigListItem ListItem { get => (CrawlConfigListItem)GetValue(ListItemProperty); private set => SetValue(ListItemPropertyKey, value); }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyPropertyBuilder<EditViewModel, Guid?>
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

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyPropertyBuilder<EditViewModel, DateTime?>
            .Register(nameof(LastSynchronizedOn))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
        #region BrowseNewRootFolder Command Property Members

        private static readonly DependencyPropertyKey BrowseNewRootFolderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BrowseNewRootFolder),
            typeof(Commands.RelayCommand), typeof(EditViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BrowseNewRootFolder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrowseNewRootFolderProperty = BrowseNewRootFolderPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the command object for picking a new root subdirectory.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that is invoked when the user wishes to pick a new root subdirectory.</value>
        public Commands.RelayCommand BrowseNewRootFolder => (Commands.RelayCommand)GetValue(BrowseNewRootFolderProperty);

        #endregion
        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyPropertyBuilder<EditViewModel, bool>
            .Register(nameof(IsNew))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsNew"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew { get => (bool)GetValue(IsNewProperty); private set => SetValue(IsNewPropertyKey, value); }

        #endregion

        public EditViewModel([DisallowNull] CrawlConfiguration entity, bool isNew) : base(entity)
        {
            SetValue(SaveChangesPropertyKey, new Commands.RelayCommand(OnSaveChangesCommand));
            SetValue(DiscardChangesPropertyKey, new Commands.RelayCommand(OnDiscardChangesCommand));
            SetValue(BrowseNewRootFolderPropertyKey, new Commands.RelayCommand(OnBrowseNewRootFolder));
            IsNew = isNew;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        public static void AddNewItem(ReturnEventHandler<CrawlConfiguration> onReturn = null)
        {
            // TODO: Implement AddNewItem
        }

        private void OnBrowseNewRootFolder(object parameter)
        {
            // TODO: Implement OnBrowseNewRootFolder Logic
        }

        protected override void OnRefreshCrawlJobLogsCommand(object parameter)
        {
            // TODO: Implement OnRefreshCrawlJobLogsCommand(parameter)
            throw new NotImplementedException();
        }

        internal static void NavigateToEditPage(CrawlConfigListItem item, CrawlConfiguration entity, ReturnEventHandler<ItemEditResult> onReturn = null)
        {
            EditViewModel viewModel = new(entity, false) { ListItem = item };
            EditPage page = new(viewModel);
            if (onReturn is not null)
                page.Return += onReturn;
            Services.ServiceProvider.GetRequiredService<IApplicationNavigation>().Navigate(page);
        }

        protected override void OnAddNewCrawlJobLogCommand(object parameter)
        {
            // TODO: Implement OnAddNewCrawlJobLogCommand(parameter)
            throw new NotImplementedException();
        }

        protected override void OnCrawlJobLogEditCommand([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            // TODO: Implement GetQueryableCrawlJobLogListing(CrawlJobListItemViewModel, parameter)
            throw new NotImplementedException();
        }

        protected override bool ConfirmCrawlJobLogDelete([DisallowNull] CrawlJobListItemViewModel item, object parameter)
        {
            // TODO: Implement GetQueryableCrawlJobLogListing(CrawlJobListItemViewModel, parameter)
            throw new NotImplementedException();
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement GetQueryableCrawlJobLogListing(LocalDbContext, IWindowsStatusListener)
            throw new NotImplementedException();
        }

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity)
        {
            // TODO: Implement CreateCrawlJobLogViewModel(CrawlJobLogListItem)
            throw new NotImplementedException();
        }

        protected override Task<int> DeleteCrawlJobLogFromDbContextAsync([DisallowNull] CrawlJobLogListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement DeleteCrawlJobLogFromDbContextAsync(CrawlJobLogListItem, LocalDbContext, IWindowsStatusListener)
            throw new NotImplementedException();
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
