using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class CrawlStateReportViewModel : ListingViewModel<CrawlConfigReportItem, ReportItemViewModel, ViewModel.Filter.Filter<CrawlConfigReportItem>>, INavigatedToNotifiable
    {
        private ViewModel.Filter.Filter<CrawlConfigReportItem> _currentReportOption;

        #region Owner Attached Property Members

        /// <summary>
        /// The name of the <see cref="OwnerProperty">Owner</see> attached dependency property.
        /// </summary>
        public const string PropertyName_Owner = "Owner";

        private static readonly DependencyPropertyKey OwnerPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, CrawlStateReportViewModel>
            .RegisterAttached(PropertyName_Owner)
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Owner"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OwnerProperty = OwnerPropertyKey.DependencyProperty;

        public static CrawlStateReportViewModel GetOwner([DisallowNull] DependencyObject obj) => (CrawlStateReportViewModel)obj.GetValue(OwnerProperty);

        private static void SetOwner([DisallowNull] DependencyObject obj, CrawlStateReportViewModel value) => obj.SetValue(OwnerPropertyKey, value);

        #endregion
        #region IsSelected Attached Property Members

        /// <summary>
        /// The name of the <see cref="IsSelectedProperty">IsSelected</see> attached dependency property.
        /// </summary>
        public const string PropertyName_IsSelected = "IsSelected";

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, bool>
            .RegisterAttached(PropertyName_IsSelected)
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => RaiseIsSelectedPropertyChanged(d, oldValue, newValue))
            .AsReadWrite();

        public static bool GetIsSelected([DisallowNull] DependencyObject obj) => (bool)obj.GetValue(IsSelectedProperty);

        public static void SetIsSelected([DisallowNull] DependencyObject obj, bool value) => obj.SetValue(IsSelectedProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        /// <param name="obj">The object whose attached property value has changed.</param>
        /// <param name="oldValue">The previous value of the <see cref="IsSelected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsSelected"/> property.</param>
        private static void RaiseIsSelectedPropertyChanged(DependencyObject obj, bool oldValue, bool newValue)
        {
            if (obj is ViewModel.Filter.Filter<CrawlConfigReportItem> item)
                GetOwner(item)?.OnIsSelectedPropertyChanged(item, newValue);
        }

        private void OnIsSelectedPropertyChanged(ViewModel.Filter.Filter<CrawlConfigReportItem> item, bool newValue)
        {
            if (newValue)
                SelectedReportOption = item;
            else if (ReferenceEquals(item, SelectedReportOption))
                SelectedReportOption = ReportOptions.FirstOrDefault(i => i is not null && !ReferenceEquals(i, item));
        }

        #endregion
        #region DisplayText Attached Property Members

        /// <summary>
        /// The name of the <see cref="DisplayTextProperty">DisplayText</see> attached dependency property.
        /// </summary>
        public const string PropertyName_DisplayText = "DisplayText";

        /// <summary>
        /// Identifies the <see cref="DisplayText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayTextProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, string>
            .RegisterAttached(PropertyName_DisplayText)
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public static string GetDisplayText(DependencyObject obj) => obj?.GetValue(DisplayTextProperty) as string;

        public static void SetDisplayText([DisallowNull] DependencyObject obj, string value) => obj.SetValue(DisplayTextProperty, value);

        #endregion
        #region Description Attached Property Members

        /// <summary>
        /// The name of the <see cref="DescriptionProperty">Description</see> attached dependency property.
        /// </summary>
        public const string PropertyName_Description = "Description";

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, string>
            .RegisterAttached(PropertyName_Description)
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public static string GetDescription(DependencyObject obj) => obj?.GetValue(DescriptionProperty) as string;

        public static void SetDescription([DisallowNull] DependencyObject obj, string value) => obj.SetValue(DescriptionProperty, value);

        #endregion
        #region ReportOptions Property Members

        private HashSet<ViewModel.Filter.Filter<CrawlConfigReportItem>> _distinctItems = new();

        /// <summary>
        /// Identifies the <see cref="ReportOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReportOptionsProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>>
            .Register(nameof(ReportOptions))
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnReportOptionsPropertyChanged(oldValue, newValue))
            .CoerseWith((d, baseValue) => (baseValue as ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>) ?? new())
            .AsReadWrite();

        public ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> ReportOptions { get => (ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>)GetValue(ReportOptionsProperty); set => SetValue(ReportOptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReportOptions"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReportOptions"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReportOptions"/> property.</param>
        protected virtual void OnReportOptionsPropertyChanged(ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> oldValue, ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> newValue)
        {
            ViewModel.Filter.Filter<CrawlConfigReportItem> oldOption = _currentReportOption ?? SelectedReportOption;
            foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in _distinctItems)
            {
                if (ReferenceEquals(this, GetOwner(item)))
                    SetOwner(item, null);
            }
            _distinctItems.Clear();
            if (oldValue is not null)
            {
                oldValue.CollectionChanged -= ReportOptions_CollectionChanged;
                foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in oldValue.Where(i => i is not null))
                {
                    if (ReferenceEquals(GetOwner(item), this))
                        SetOwner(item, null);
                }
            }
            foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in newValue.Where(i => i is not null))
            {
                if (_distinctItems.Add(item) && !ReferenceEquals(GetOwner(item), this))
                    SetOwner(item, this);
            }
            IEnumerable<ViewModel.Filter.Filter<CrawlConfigReportItem>> enumerator = _distinctItems.Reverse().SkipWhile(i => !GetIsSelected(i));
            ViewModel.Filter.Filter<CrawlConfigReportItem> newOption = enumerator.FirstOrDefault();
            foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerator.Skip(1).ToArray())
                SetIsSelected(item, false);
            _currentReportOption = (newOption is null) ? _distinctItems.FirstOrDefault() : newOption;
            SelectedReportOption = _currentReportOption;
            newValue.CollectionChanged += ReportOptions_CollectionChanged;
            if ((oldOption is null) ? _currentReportOption is not null : !ReferenceEquals(oldOption, _currentReportOption))
                ReloadAsync(_currentReportOption);
        }

        private void ReportOptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<ViewModel.Filter.Filter<CrawlConfigReportItem>> enumerable;
            ViewModel.Filter.Filter<CrawlConfigReportItem> oldCurrentItem = _currentReportOption, newCurrentItem = _currentReportOption;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if ((enumerable = e.NewItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null)) is not null)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            if (!ReferenceEquals(GetOwner(item), this))
                                SetOwner(item, this);
                            if (_distinctItems.Add(item) && GetIsSelected(item))
                            {
                                if (newCurrentItem is null)
                                    newCurrentItem = item;
                                else
                                    SetIsSelected(item, false);
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in _distinctItems)
                    {
                        if (ReferenceEquals(this, GetOwner(item)))
                            SetOwner(item, null);
                    }
                    _distinctItems.Clear();
                    if (ReportOptions.Count > 0)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in ReportOptions.Where(i => i is not null))
                        {
                            if (!ReferenceEquals(this, GetOwner(item)))
                                SetOwner(item, this);
                            _distinctItems.Add(item);
                        }
                        newCurrentItem = ReportOptions.FirstOrDefault(i => i is not null && GetIsSelected(i));
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if ((enumerable = e.OldItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null)) is not null)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            if (!ReportOptions.Any(o => ReferenceEquals(item, o)))
                            {
                                if (ReferenceEquals(this, GetOwner(item)))
                                    SetOwner(item, null);
                                _distinctItems.Remove(item);
                                if (newCurrentItem is not null && ReferenceEquals(newCurrentItem, item))
                                    newCurrentItem = null;
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if ((enumerable = e.OldItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null)) is not null)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            if (!ReportOptions.Any(o => ReferenceEquals(item, o)))
                            {
                                if (ReferenceEquals(this, GetOwner(item)))
                                    SetOwner(item, null);
                                _distinctItems.Remove(item);
                                if (newCurrentItem is not null && ReferenceEquals(newCurrentItem, item))
                                    newCurrentItem = null;
                            }
                        }
                    }
                    if ((enumerable = e.NewItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null)) is not null)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            if (!ReferenceEquals(GetOwner(item), this))
                                SetOwner(item, this);
                            if (_distinctItems.Add(item) && GetIsSelected(item))
                            {
                                if (newCurrentItem is null)
                                    newCurrentItem = item;
                                else
                                    SetIsSelected(item, false);
                            }
                        }
                    }
                    break;
                default:
                    return;
            }
            if (newCurrentItem is null && (newCurrentItem = ReportOptions.FirstOrDefault(i => GetIsSelected(i))) is null && (oldCurrentItem is null) &&
                    (newCurrentItem = ReportOptions.FirstOrDefault(i => i is not null)) is null)
                SelectedReportOption = _currentReportOption = null;
            else
            {
                SelectedReportOption = _currentReportOption = newCurrentItem;
                SetIsSelected(newCurrentItem, true);
            }
        }

        #endregion
        #region SelectedReportOption Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedReportOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedReportOptionProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, ViewModel.Filter.Filter<CrawlConfigReportItem>>
            .Register(nameof(SelectedReportOption))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnSelectedReportOptionPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ViewModel.Filter.Filter<CrawlConfigReportItem> SelectedReportOption { get => (ViewModel.Filter.Filter<CrawlConfigReportItem>)GetValue(SelectedReportOptionProperty); set => SetValue(SelectedReportOptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedReportOption"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedReportOption"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedReportOption"/> property.</param>
        protected virtual void OnSelectedReportOptionPropertyChanged(ViewModel.Filter.Filter<CrawlConfigReportItem> oldValue, ViewModel.Filter.Filter<CrawlConfigReportItem> newValue)
        {
            if (newValue is not null)
            {
                if (ReferenceEquals(this, GetOwner(newValue)))
                {
                    SetIsSelected(newValue, true);
                    if (oldValue is not null && ReferenceEquals(this, GetOwner(oldValue)))
                        SetIsSelected(oldValue, false);
                }
                else
                    SelectedReportOption = (oldValue is null || !ReferenceEquals(this, GetOwner(oldValue))) ? _currentReportOption : oldValue;
            }
            else if (oldValue is not null && ReferenceEquals(this, GetOwner(newValue)))
                SetIsSelected(oldValue, false);
        }

        #endregion

        public CrawlStateReportViewModel()
        {
            ReportOptions = new();
        }

        private void UpdatePageTitle(ViewModel.Filter.Filter<CrawlConfigReportItem> currentReportOption) => PageTitle = GetDisplayText(currentReportOption).NullIfWhiteSpace() ?? FsInfoCat.Properties.Resources.DisplayName_FSInfoCat;

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentReportOption);

        protected override bool ConfirmItemDelete([DisallowNull] ReportItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override ReportItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigReportItem entity) => new(entity);

        protected override Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigReportItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlConfigReportItem entity) => _currentReportOption?.IsMatch(entity) ?? true;

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ReportItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(new CrawlConfiguration(), null)));
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            dbContext.CrawlConfigReport.Where(e => e.RootId == id);
            CrawlConfiguration fs = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                ReloadAsync(_currentReportOption);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        private DispatcherOperation<PageFunction<ItemFunctionResultEventArgs>> CreateEditPageAsync(CrawlConfiguration crawlConfiguration, SubdirectoryListItemWithAncestorNames selectedRoot, CrawlConfigReportItem listitem,
            [DisallowNull] IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() =>
            {
                if (crawlConfiguration is null || selectedRoot is null)
                {
                    _ = MessageBox.Show(Application.Current.MainWindow, "Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    ReloadAsync(_currentReportOption);
                    return null;
                }
                return new EditPage(new(crawlConfiguration, listitem) { Root = new(selectedRoot) });
            }, DispatcherPriority.Normal, statusListener.CancellationToken);

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ReportItemViewModel listItem, [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration crawlConfiguration;
            SubdirectoryListItemWithAncestorNames selectedRoot;
            CrawlConfigReportItem itemEntity;
            if (listItem is not null)
            {
                using IServiceScope serviceScope = Services.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = listItem.Entity.Id;
                crawlConfiguration = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                if (crawlConfiguration is null)
                    selectedRoot = null;
                else
                {
                    id = crawlConfiguration.RootId;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id);
                }
                return await CreateEditPageAsync(crawlConfiguration, selectedRoot, listItem.Entity, statusListener);
            }

            itemEntity = null;
            selectedRoot = null;
            crawlConfiguration = null;
            while (selectedRoot is null)
            {
                string path = await Dispatcher.InvokeAsync(() =>
                {
                    using System.Windows.Forms.FolderBrowserDialog dialog = new()
                    {
                        Description = "Select root folder",
                        ShowNewFolderButton = false,
                        SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                    };
                    return (dialog.ShowDialog(new WindowOwner()) == System.Windows.Forms.DialogResult.OK) ? dialog.SelectedPath : null;
                }, DispatcherPriority.Background, statusListener.CancellationToken);
                if (string.IsNullOrEmpty(path))
                    return null;
                DirectoryInfo directoryInfo;
                try { directoryInfo = new(path); }
                catch (SecurityException exc)
                {
                    statusListener.Logger.LogError(exc, "Permission denied getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Permission denied while attempting to import subdirectory.", "Security Exception", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (PathTooLongException exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for ({Path} is too long).", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Path is too long. Cannnot import subdirectory as crawl root.", "Path Too Long", MessageBoxButton.OKCancel, MessageBoxImage.Error, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (Exception exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Unable to import subdirectory. See system logs for details.", "File System Error", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                if (directoryInfo is null)
                {
                    selectedRoot = null;
                    crawlConfiguration = null;
                }
                else
                {
                    using IServiceScope serviceScope = Services.CreateScope();
                    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                    Subdirectory root = await Subdirectory.FindByFullNameAsync(path, dbContext, statusListener.CancellationToken);
                    if (root is null)
                    {
                        crawlConfiguration = null;
                        root = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, statusListener.CancellationToken))?.Entity;
                    }
                    else
                        crawlConfiguration = await dbContext.Entry(root).GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
                    Guid id = root.Id;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                }
                if (crawlConfiguration is not null)
                {
                    switch (await Dispatcher.ShowMessageBoxAsync($"There is already a configuration defined for that path. Would you like to edit that configuration, instead?", "Configuration exists", MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.Yes:
                            Guid id = crawlConfiguration.Id;
                            using (IServiceScope serviceScope = Services.CreateScope())
                            {
                                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                                itemEntity = await dbContext.CrawlConfigReport.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                                if (selectedRoot is null)
                                {
                                    id = crawlConfiguration.RootId;
                                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            selectedRoot = null;
                            crawlConfiguration = null;
                            break;
                        default:
                            return null;
                    }
                }
            }
            return await CreateEditPageAsync(crawlConfiguration ?? new(), selectedRoot, itemEntity, statusListener);
        }

        protected override IQueryable<CrawlConfigReportItem> GetQueryableListing(ViewModel.Filter.Filter<CrawlConfigReportItem> options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading report input data from database");
            ParameterExpression parameterExpression = LinqExpression.Parameter(typeof(CrawlConfigReportItem), "entity");
            BinaryExpression binaryExpression = Dispatcher.CheckInvoke(() =>
            {
                if (options is null)
                {
                    statusListener.Logger.LogWarning("No report item filter was selected. Returning all items");
                    return null;
                }
                return options.CreateExpression(parameterExpression);
            });
            if (binaryExpression is null)
                return dbContext.CrawlConfigReport;
            Expression<Func<CrawlConfigReportItem, bool>> expression = LinqExpression.Lambda<Func<CrawlConfigReportItem, bool>>(binaryExpression, parameterExpression);
            return dbContext.CrawlConfigReport.Where(expression);
        }

        protected override void OnApplyFilterOptionsCommand(object parameter) => ReloadAsync(SelectedReportOption);

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ReportItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ReportItemViewModel item)
        {
            UpdatePageTitle(_currentReportOption);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentReportOption);

        protected override void OnReloadTaskCanceled(ViewModel.Filter.Filter<CrawlConfigReportItem> options)
        {
            UpdatePageTitle(_currentReportOption);
            SelectedReportOption = _currentReportOption;
        }

        protected override void OnReloadTaskCompleted(ViewModel.Filter.Filter<CrawlConfigReportItem> options) => _currentReportOption = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, ViewModel.Filter.Filter<CrawlConfigReportItem> options)
        {
            UpdatePageTitle(_currentReportOption);
            SelectedReportOption = _currentReportOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
