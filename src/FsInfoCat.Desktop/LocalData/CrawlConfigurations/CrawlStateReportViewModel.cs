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
using System.Linq.Expressions;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using LinqExpression = System.Linq.Expressions.Expression;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class CrawlStateReportViewModel : ListingViewModel<CrawlConfigReportItem, ReportItemViewModel, ViewModel.Filter.Filter<CrawlConfigReportItem>>
    {
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
            .OnChanged((d, oldValue, newValue) => RaiseIsSelectedPropertyChanged(d, newValue))
            .AsReadWrite();

        public static bool GetIsSelected([DisallowNull] DependencyObject obj) => (bool)obj.GetValue(IsSelectedProperty);

        public static void SetIsSelected([DisallowNull] DependencyObject obj, bool value) => obj.SetValue(IsSelectedProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        /// <param name="obj">The object whose attached property value has changed.</param>
        /// <param name="oldValue">The previous value of the <see cref="IsSelected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsSelected"/> property.</param>
        private static void RaiseIsSelectedPropertyChanged(DependencyObject obj, bool newValue)
        {
            if (obj is ViewModel.Filter.Filter<CrawlConfigReportItem> item)
                GetOwner(item)?.OnIsSelectedPropertyChanged(item, newValue);
        }

        private void OnIsSelectedPropertyChanged(ViewModel.Filter.Filter<CrawlConfigReportItem> item, bool newValue)
        {
            _logger.LogDebug("Invoked {MethodName}(item: {item}, newValue: {newValue})", nameof(OnIsSelectedPropertyChanged), item, newValue);
            if (newValue)
                SelectedReportIndex = ReportOptions.IndexOf(item);
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

        private readonly HashSet<ViewModel.Filter.Filter<CrawlConfigReportItem>> _distinctItems = new();

        /// <summary>
        /// Identifies the <see cref="ReportOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReportOptionsProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>>
            .Register(nameof(ReportOptions))
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnReportOptionsPropertyChanged(oldValue, newValue))
            .CoerseWith((d, baseValue) => (baseValue as ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>) ?? new())
            .AsReadWrite();
        private readonly ILogger<CrawlStateReportViewModel> _logger;

        public ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> ReportOptions { get => (ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>>)GetValue(ReportOptionsProperty); set => SetValue(ReportOptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReportOptions"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReportOptions"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReportOptions"/> property.</param>
        protected virtual void OnReportOptionsPropertyChanged(ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> oldValue, ObservableCollection<ViewModel.Filter.Filter<CrawlConfigReportItem>> newValue)
        {
            _logger.LogDebug("Invoked {MethodName}(oldValue: {oldValue}, newValue: {newValue})", nameof(OnReportOptionsPropertyChanged), oldValue, newValue);
            foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in _distinctItems)
            {
                if (ReferenceEquals(this, GetOwner(item)))
                    SetOwner(item, null);
            }
            _distinctItems.Clear();
            if (oldValue is not null)
                oldValue.CollectionChanged -= ReportOptions_CollectionChanged;
            if (newValue is not null)
            {
                newValue.CollectionChanged += ReportOptions_CollectionChanged;
                foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in newValue.Where(i => i is not null && _distinctItems.Add(i)))
                    SetOwner(item, this);
                if (_distinctItems.Count > 0)
                {
                    IEnumerable<ViewModel.Filter.Filter<CrawlConfigReportItem>> distinctItems = _distinctItems.Reverse().SkipWhile(i => !GetIsSelected(i));
                    ViewModel.Filter.Filter<CrawlConfigReportItem> newOption = distinctItems.FirstOrDefault();
                    if (newOption is not null)
                    {
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in distinctItems.Skip(1).Where(i => GetIsSelected(i)).ToArray())
                            SetIsSelected(item, false);
                    }
                    else
                        newOption = _distinctItems.FirstOrDefault();
                    int index = ReportOptions.IndexOf(newOption);
                    if (index == SelectedReportIndex)
                        ReloadAsync(newOption);
                    else
                    {
                        _logger.LogDebug("Changing {PropertyName} from {oldValue} to {newValue}", nameof(OnReportOptionsPropertyChanged), SelectedReportIndex, index);
                        SelectedReportIndex = index;
                    }
                    return;
                }
            }
            if (SelectedReportIndex != -1)
                SelectedReportIndex = -1;
            else
            {
                ClearItems();
                SelectedReportOption = null;
            }
        }

        private void ReportOptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _logger.LogDebug("Invoked {MethodName}(sender: {sender}, e: {e})", nameof(ReportOptions_CollectionChanged), sender, e);
            IEnumerable<ViewModel.Filter.Filter<CrawlConfigReportItem>> enumerable;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if ((enumerable = e.NewItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null && _distinctItems.Add(i))) is not null)
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            SetOwner(item, this);
                            if (GetIsSelected(item))
                            {
                                if (SelectedReportIndex < 0)
                                    SelectedReportIndex = ReportOptions.IndexOf(item);
                                else
                                    SetIsSelected(item, false);
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
                            }
                        }
                        if (SelectedReportOption is not null && enumerable.Contains(SelectedReportOption) && ReportOptions.IndexOf(SelectedReportOption) != SelectedReportIndex)
                            SelectedReportIndex = -1;
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
                            }
                        }
                    }
                    int selectedReportIndex = (SelectedReportOption is not null && enumerable.Contains(SelectedReportOption) && ReportOptions.IndexOf(SelectedReportOption) != SelectedReportIndex) ? -1 : SelectedReportIndex;
                    if ((enumerable = e.NewItems?.OfType<ViewModel.Filter.Filter<CrawlConfigReportItem>>().Where(i => i is not null && _distinctItems.Add(i))) is not null)
                        foreach (ViewModel.Filter.Filter<CrawlConfigReportItem> item in enumerable)
                        {
                            SetOwner(item, this);
                            if (GetIsSelected(item))
                            {
                                if (selectedReportIndex < 0)
                                    selectedReportIndex = ReportOptions.IndexOf(item);
                                else
                                    SetIsSelected(item, false);
                            }
                        }
                    if (selectedReportIndex != SelectedReportIndex)
                        SelectedReportIndex = selectedReportIndex;
                    else if (!ReferenceEquals(ReportOptions[selectedReportIndex], SelectedReportOption))
                        ReloadAsync(ReportOptions[selectedReportIndex]);
                    break;
                default:
                    return;
            }
        }

        #endregion
        #region SelectedReportIndex Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedReportIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedReportIndexProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, int>
            .Register(nameof(SelectedReportIndex))
            .DefaultValue(-1)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnSelectedReportIndexPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int SelectedReportIndex
        {
            get => (int)GetValue(SelectedReportIndexProperty); set
            {
                _logger.LogDebug("Invoked {PropertyName} setter (value: {value})", nameof(SelectedReportIndex), value);
                SetValue(SelectedReportIndexProperty, value);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedReportIndex"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedReportIndex"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedReportIndex"/> property.</param>
        protected virtual void OnSelectedReportIndexPropertyChanged(int oldValue, int newValue)
        {
            _logger.LogDebug("Invoked {MethodName}(oldValue: {oldValue}, newValue: {newValue})", nameof(OnSelectedReportIndexPropertyChanged), oldValue, newValue);
            ViewModel.Filter.Filter<CrawlConfigReportItem> oldItem = (oldValue < 0 || oldValue >= ReportOptions.Count) ? null : ReportOptions[oldValue];
            ViewModel.Filter.Filter<CrawlConfigReportItem> newItem = (newValue < 0 || newValue >= ReportOptions.Count) ? null : ReportOptions[newValue];

            if (newItem is not null)
            {
                SetIsSelected(newItem, true);
                if (oldItem is not null && ReferenceEquals(this, GetOwner(oldItem)))
                    SetIsSelected(oldItem, false);
                if (!DesignerProperties.GetIsInDesignMode(this))
                    ReloadAsync(newItem);
            }
            else
            {
                if (oldItem is not null && ReferenceEquals(this, GetOwner(oldItem)))
                    SetIsSelected(oldItem, false);
                ClearItems();
            }
        }

        #endregion
        #region SelectedReportOption Property Members

        private static readonly DependencyPropertyKey SelectedReportOptionPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, ViewModel.Filter.Filter<CrawlConfigReportItem>>
            .Register(nameof(SelectedReportOption))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SelectedReportOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedReportOptionProperty = SelectedReportOptionPropertyKey.DependencyProperty;

        public ViewModel.Filter.Filter<CrawlConfigReportItem> SelectedReportOption { get => (ViewModel.Filter.Filter<CrawlConfigReportItem>)GetValue(SelectedReportOptionProperty); private set => SetValue(SelectedReportOptionPropertyKey, value); }

        #endregion

        public CrawlStateReportViewModel()
        {
            _logger = App.GetLogger(this);
            ReportOptions = new();
        }

        private void UpdatePageTitle(ViewModel.Filter.Filter<CrawlConfigReportItem> currentReportOption) => PageTitle = GetDisplayText(currentReportOption).NullIfWhiteSpace() ?? FsInfoCat.Properties.Resources.DisplayName_FSInfoCat;

        protected override bool ConfirmItemDelete([DisallowNull] ReportItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this crawl configuration from the database?",
            "Delete Crawl Configuration", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override ReportItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigReportItem entity) => new(entity);

        private void Item_OpenRootSubdirectoryCommand(object sender, Commands.CommandEventArgs e)
        {
            // TODO: Implement Item_OpenRootSubdirectoryCommand
            MessageBox.Show("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void Item_ShowCrawlActivityRecordsCommand(object sender, Commands.CommandEventArgs e)
        {
            // TODO: Implement Item_ShowCrawlActivityRecordsCommand
            MessageBox.Show("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void Item_StartCrawlCommand(object sender, Commands.CommandEventArgs e)
        {
            // TODO: Implement Item_StartCrawlCommand
            MessageBox.Show("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void Item_StopCrawlCommand(object sender, Commands.CommandEventArgs e)
        {
            // TODO: Implement Item_StopCrawlCommand
            MessageBox.Show("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        protected override void AddItem(ReportItemViewModel item)
        {
            base.AddItem(item);
            item.OpenRootSubdirectoryCommand += Item_OpenRootSubdirectoryCommand;
            item.ShowCrawlActivityRecordsCommand += Item_ShowCrawlActivityRecordsCommand;
            item.StartCrawlCommand += Item_StartCrawlCommand;
            item.StopCrawlCommand += Item_StopCrawlCommand;
        }

        protected override bool RemoveItem(ReportItemViewModel item)
        {
            if (base.RemoveItem(item))
            {
                item.OpenRootSubdirectoryCommand -= Item_OpenRootSubdirectoryCommand;
                item.ShowCrawlActivityRecordsCommand -= Item_ShowCrawlActivityRecordsCommand;
                item.StartCrawlCommand -= Item_StartCrawlCommand;
                item.StopCrawlCommand -= Item_StopCrawlCommand;
                return true;
            }
            return false;
        }

        protected override ReportItemViewModel[] ClearItems()
        {
            ReportItemViewModel[] removedItems = base.ClearItems();
            foreach (ReportItemViewModel item in removedItems)
            {
                item.OpenRootSubdirectoryCommand -= Item_OpenRootSubdirectoryCommand;
                item.ShowCrawlActivityRecordsCommand -= Item_ShowCrawlActivityRecordsCommand;
                item.StartCrawlCommand -= Item_StartCrawlCommand;
                item.StopCrawlCommand -= Item_StopCrawlCommand;
            }
            return removedItems;
        }

        protected override Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigReportItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            // TODO: Implement DeleteEntityFromDbContextAsync
            Dispatcher.ShowMessageBoxAsync("You  have invoked a command which has not yet been implemented.", "Not Implemented", MessageBoxButton.OK, MessageBoxImage.Hand, statusListener.CancellationToken);
            throw new NotImplementedException($"{nameof(DeleteEntityFromDbContextAsync)} not implemented");
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlConfigReportItem entity) => SelectedReportOption?.IsMatch(entity) ?? true;

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
                ReloadAsync(SelectedReportOption);
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
                    ReloadAsync(SelectedReportOption);
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
                        EntityEntry<Subdirectory> entry = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, statusListener.CancellationToken));
                        if (entry is not null)
                        {
                            if (entry.State == EntityState.Added)
                                await dbContext.SaveChangesAsync(statusListener.CancellationToken);
                            root = entry.Entity;
                        }
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
                    "There was an unexpected error while deleting the item from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ReportItemViewModel item)
        {
            UpdatePageTitle(SelectedReportOption);
            SelectedReportIndex = ReportOptions.IndexOf(SelectedReportOption);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(SelectedReportOption);

        protected override void OnReloadTaskCanceled(ViewModel.Filter.Filter<CrawlConfigReportItem> options)
        {
            _logger.LogDebug("Invoked {MethodName}(options: {options})", nameof(OnReloadTaskCanceled), options);
            UpdatePageTitle(SelectedReportOption);
            SelectedReportIndex = ReportOptions.IndexOf(SelectedReportOption);
        }

        protected override void OnReloadTaskCompleted(ViewModel.Filter.Filter<CrawlConfigReportItem> options)
        {
            _logger.LogDebug("Invoked {MethodName}(options: {options})", nameof(OnReloadTaskCompleted), options);
            SelectedReportOption = options;
        }

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, ViewModel.Filter.Filter<CrawlConfigReportItem> options)
        {
            _logger.LogDebug("Invoked {MethodName}(options: {options})", nameof(OnReloadTaskFaulted), options);
            UpdatePageTitle(SelectedReportOption);
            SelectedReportIndex = ReportOptions.IndexOf(SelectedReportOption);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
