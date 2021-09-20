using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public sealed class EditViewModel : CrawlConfigurationEditViewModel<CrawlConfiguration, SubdirectoryListItemWithAncestorNames, SubdirectoryListItemViewModel, CrawlJobLogListItem, CrawlJobListItemViewModel>,
        INavigatedToNotifiable, INavigatingFromNotifiable
    {
        internal CrawlConfigListItemBase ListItem { get; }

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
        private readonly ILogger<EditViewModel> _logger;

        /// <summary>
        /// Gets the command object for picking a new root subdirectory.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that is invoked when the user wishes to pick a new root subdirectory.</value>
        public Commands.RelayCommand BrowseNewRootFolder => (Commands.RelayCommand)GetValue(BrowseNewRootFolderProperty);

        #endregion

        public EditViewModel([DisallowNull] CrawlConfiguration tableEntity, CrawlConfigListItemBase itemEntity) : base(tableEntity, itemEntity is null, itemEntity)
        {
            _logger = App.GetLogger(this);
            SetValue(BrowseNewRootFolderPropertyKey, new Commands.RelayCommand(OnBrowseNewRootFolder));
            ListItem = itemEntity;
            UpstreamId = tableEntity.UpstreamId;
            LastSynchronizedOn = tableEntity.LastSynchronizedOn;
        }

        private void OnBrowseNewRootFolder(object parameter)
        {
            DirectoryInfo directoryInfo = null;
            while (directoryInfo is null)
            {
                using System.Windows.Forms.FolderBrowserDialog dialog = new()
                {
                    Description = "Select root folder",
                    ShowNewFolderButton = false,
                    SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                };
                if (dialog.ShowDialog(new WindowOwner()) != System.Windows.Forms.DialogResult.OK)
                    return;

                try { directoryInfo = new(dialog.SelectedPath); }
                catch (SecurityException exc)
                {
                    _logger.LogError(exc, "Permission denied getting directory information for {Path}.", dialog.SelectedPath);
                    MessageBox.Show(Application.Current.MainWindow, $"Permission denied while attempting to import subdirectory.", "Security Exception", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    directoryInfo = null;
                }
                catch (PathTooLongException exc)
                {
                    _logger.LogError(exc, "Error getting directory information for ({Path} is too long).", dialog.SelectedPath);
                    MessageBox.Show(Application.Current.MainWindow, $"Path is too long. Cannnot import subdirectory as crawl root.", "Path Too Long", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    directoryInfo = null;
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "Error getting directory information for {Path}.", dialog.SelectedPath);
                    MessageBox.Show(Application.Current.MainWindow, $"Unable to import subdirectory. See system logs for details.", "File System Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    directoryInfo = null;
                }
            }
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            IAsyncJob<SubdirectoryListItemWithAncestorNames> job = jobFactory.StartNew("Loading path", "Opening database", directoryInfo, ImportBranchAsync);
            job.Task.ContinueWith(task =>
            {
                if (task.IsCanceled)
                    return;
                Dispatcher.Invoke(() =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception.InnerExceptions.Count == 1)
                            OnImportBranchFaulted(task.Exception.InnerException, directoryInfo);
                        else
                            OnImportBranchFaulted(task.Exception, directoryInfo);
                    }
                    else
                        OnImportBranchCompleted(task.Result, directoryInfo);
                }, DispatcherPriority.Background);
            });
        }

        private void OnImportBranchCompleted(SubdirectoryListItemWithAncestorNames result, DirectoryInfo directoryInfo)
        {
            if (result is null)
                OnBrowseNewRootFolder(directoryInfo);
            else
                Root = new(result);
        }

        private void OnImportBranchFaulted(Exception exception, DirectoryInfo directoryInfo) => MessageBox.Show(Application.Current.MainWindow,
            ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                "There was an unexpected error while importing the subdirectory into the databse.\n\nSee logs for further information",
            "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

        private async Task<SubdirectoryListItemWithAncestorNames> ImportBranchAsync(DirectoryInfo directoryInfo, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory root = await Subdirectory.FindByFullNameAsync(directoryInfo.FullName, dbContext, statusListener.CancellationToken);
            if (root is null)
                root = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, statusListener.CancellationToken))?.Entity;
            else
            {
                CrawlConfiguration crawlConfiguration = await dbContext.Entry(root).GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
                if (crawlConfiguration is not null && crawlConfiguration.Id != Entity.Id)
                {
                    await Dispatcher.ShowMessageBoxAsync($"There is already a configuration defined for that path.", "Configuration exists", MessageBoxButton.OK, MessageBoxImage.Warning, statusListener.CancellationToken);
                    return null;
                }
            }
            Guid id = root.Id;
            return await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            Guid id = Entity.Id;
            statusListener.SetMessage("Reading from database");
            return dbContext.CrawlJobListing.Where(j => j.ConfigurationId == id);
        }

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            if (!IsNew)
                ReloadAsync();
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            // TODO: Prompt to lose changes if not saved
        }

        protected override void OnReloadTaskFaulted(Exception exception)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override SubdirectoryListItemViewModel CreateSubdirectoryViewModel(SubdirectoryListItemWithAncestorNames subdirectory) => new(subdirectory);

        protected async override Task<SubdirectoryListItemWithAncestorNames> LoadSubdirectoryAsync(Guid id, LocalDbContext dbContext, IWindowsStatusListener statusListener) =>
            await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(s => s.Id == id);
    }
}
