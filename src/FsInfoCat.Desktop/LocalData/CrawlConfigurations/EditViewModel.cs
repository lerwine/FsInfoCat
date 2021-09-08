using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class EditViewModel : CrawlConfigurationDetailsViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>
    {
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

        public EditViewModel([DisallowNull] CrawlConfiguration entity, [AllowNull] SubdirectoryListItemViewModel root, bool isNew) : base(entity, root)
        {
            SetValue(BrowseNewRootFolderPropertyKey, new Commands.RelayCommand(OnBrowseNewRootFolder));
            IsNew = isNew;
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
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
    }
}
