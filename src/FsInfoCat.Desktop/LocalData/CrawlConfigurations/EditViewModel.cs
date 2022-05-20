using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
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
            // TODO: Implement CrawlConfigurations.EditViewModel.OnBrowseNewRootFolder
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //IAsyncJob<SubdirectoryListItemWithAncestorNames> job = jobFactory.StartNew("Loading path", "Opening database", directoryInfo, ImportBranchAsync);
            //job.Task.ContinueWith(task =>
            //{
            //    if (task.IsCanceled)
            //        return;
            //    Dispatcher.Invoke(() =>
            //    {
            //        if (task.IsFaulted)
            //        {
            //            if (task.Exception.InnerExceptions.Count == 1)
            //                OnImportBranchFaulted(task.Exception.InnerException, directoryInfo);
            //            else
            //                OnImportBranchFaulted(task.Exception, directoryInfo);
            //        }
            //        else
            //            OnImportBranchCompleted(task.Result, directoryInfo);
            //    }, DispatcherPriority.Background);
            //});
        }

        private void OnImportBranchCompleted(SubdirectoryListItemWithAncestorNames result, DirectoryInfo directoryInfo)
        {
            if (result is null)
                OnBrowseNewRootFolder(directoryInfo);
            else
                Root = new(result);
        }

        private void OnImportBranchFaulted(Exception exception, DirectoryInfo directoryInfo) => MessageBox.Show(Application.Current.MainWindow,
            ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                "There was an unexpected error while importing the subdirectory into the database.\n\nSee logs for further information",
            "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);

        private async Task<SubdirectoryListItemWithAncestorNames> ImportBranchAsync(DirectoryInfo directoryInfo, [DisallowNull] IActivityProgress progress)
        {
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory root = await Subdirectory.FindByFullNameAsync(directoryInfo.FullName, dbContext, progress.Token);
            if (root is null)
                root = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, progress.Token))?.Entity;
            else
            {
                CrawlConfiguration crawlConfiguration = await dbContext.Entry(root).GetRelatedReferenceAsync(d => d.CrawlConfiguration, progress.Token);
                if (crawlConfiguration is not null && crawlConfiguration.Id != Entity.Id)
                {
                    await Dispatcher.ShowMessageBoxAsync($"There is already a configuration defined for that path.", "Configuration exists", MessageBoxButton.OK, MessageBoxImage.Warning, progress.Token);
                    return null;
                }
            }
            Guid id = root.Id;
            return await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, progress.Token);
        }

        protected override IQueryable<CrawlJobLogListItem> GetQueryableCrawlJobLogListing([DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress)
        {
            Guid id = Entity.Id;
            progress.Report("Reading from database");
            return dbContext.CrawlJobListing.Where(j => j.ConfigurationId == id);
        }

        protected override CrawlJobListItemViewModel CreateCrawlJobLogViewModel([DisallowNull] CrawlJobLogListItem entity) => new(entity);

        void INavigatedToNotifiable.OnNavigatedTo()
        {
            if (!IsNew)
                ReloadAsync();
        }

        protected override void ApplyChanges()
        {
            Entity.CreatedOn = CreatedOn;
            Entity.DisplayName = DisplayName;
            Entity.LastCrawlEnd = LastCrawlEnd;
            Entity.LastCrawlStart = LastCrawlStart;
            Entity.LastSynchronizedOn = LastSynchronizedOn;
            Entity.MaxRecursionDepth = MaxRecursionDepth;
            Entity.MaxTotalItems = MaxTotalItems.ResultValue;
            Entity.ModifiedOn = ModifiedOn;
            Entity.NextScheduledStart = NextScheduledStart.ResultValue;
            Entity.Notes = Notes;
            Entity.RescheduleAfterFail = RescheduleAfterFail;
            Entity.RescheduleFromJobEnd = RescheduleFromJobEnd;
            Entity.RescheduleInterval = RescheduleInterval.ResultValue?.ToSeconds();
            Entity.RootId = Root.Entity.Id;
            Entity.StatusValue = StatusValue;
            Entity.TTL = MaxDuration.ResultValue?.ToSeconds();
            Entity.UpstreamId = UpstreamId;
        }

        protected override void ReinitializeFromEntity()
        {
            base.ReinitializeFromEntity();
            LastSynchronizedOn = Entity.LastSynchronizedOn;
            UpstreamId = Entity.UpstreamId;
            SetRootSubdirectory(Entity.RootId);
        }

        protected override IAsyncAction<IActivityEvent> SaveChangesAsync(bool isNew)
        {
            // TODO: Implement CrawlConfigurations.EditViewModel.SaveChangesAsync
            throw new NotImplementedException();
            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //IAsyncJob<CrawlConfigListItemBase> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
            //job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
            //return job;
        }

        private static async Task<CrawlConfigListItemBase> SaveChangesAsync(CrawlConfiguration entity, object invocationState, IActivityProgress progress)
        {
            using IServiceScope scope = Hosting.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            CrawlConfiguration local = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(e => e.Id == entity.Id, progress.Token);
            EntityEntry entry;
            bool isNew = local is null;
            if (isNew)
                entry = dbContext.CrawlConfigurations.Add(entity);
            else
            {
                dbContext.Entry(local).State = EntityState.Detached;
                (entry = dbContext.Entry(entity)).State = EntityState.Modified;
            }
            progress.Report((entry.State == EntityState.Added) ?
                "Inserting new crawl configuration into database" : "Saving crawl configuration record changes to database");
            await dbContext.SaveChangesAsync(progress.Token);
            if (isNew)
            {
                Guid id = entity.Id;
                return await dbContext.CrawlConfigListing.FirstOrDefaultAsync(e => e.Id == id, progress.Token);
            }
            return invocationState is CrawlConfigListItemBase item ? item : null;
        }

        private void OnSaveTaskCompleted(Task<CrawlConfigListItemBase> task)
        {
            if (task.IsCanceled)
                return;
            if (task.IsFaulted)
                _ = MessageBox.Show(Application.Current.MainWindow,
                    ((task.Exception.InnerException is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                        task.Exception.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                        .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                        "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (IsNew)
                RaiseItemInsertedResult(task.Result);
            else
                RaiseItemUpdatedResult();
        }

        void INavigatingFromNotifiable.OnNavigatingFrom(CancelEventArgs e)
        {
            // TODO: Implement CrawlConfigurations.EditViewModel.OnNavigatingFrom
            //if (ApplyChanges())
            //{
            //    switch (MessageBox.Show(Application.Current.MainWindow, "There are unsaved changes. Do you wish to save them before continuing?", "Unsaved Changes",
            //        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            //    {
            //        case MessageBoxResult.Yes:
            //            throw new NotImplementedException();
            //            //IWindowsAsyncJobFactoryService jobFactory = Hosting.GetRequiredService<IWindowsAsyncJobFactoryService>();
            //            //IAsyncJob<CrawlConfigListItemBase> job = jobFactory.StartNew("Saving changes", "Opening database", Entity, InvocationState, SaveChangesAsync);
            //            //job.Task.ContinueWith(task => Dispatcher.Invoke(() => OnSaveTaskCompleted(task)));
            //            e.Cancel = true;
            //            break;
            //        case MessageBoxResult.No:
            //            break;
            //        default:
            //            e.Cancel = true;
            //            break;
            //    }
            //}
        }

        protected override void OnReloadTaskFaulted(Exception exception)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override SubdirectoryListItemViewModel CreateSubdirectoryViewModel(SubdirectoryListItemWithAncestorNames subdirectory) => new(subdirectory);

        protected async override Task<SubdirectoryListItemWithAncestorNames> LoadSubdirectoryAsync(Guid id, LocalDbContext dbContext, IActivityProgress progress) =>
            await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(s => s.Id == id);
    }
}
