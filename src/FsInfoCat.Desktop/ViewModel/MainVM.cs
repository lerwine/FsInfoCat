using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject, IApplicationNavigation
    {
        private readonly ILogger<MainVM> _logger;

        public const string Page_Uri_Local_CrawlConfigurations = "/LocalData/CrawlConfigurations/ListingPage.xaml";
        public const string Page_Uri_Local_FileSystems = "/LocalData/FileSystems/ListingPage.xaml";
        public const string Page_Uri_Local_RedundantSets = "/LocalData/RedundantSets/ListingPage.xaml";
        public const string Page_Uri_Local_SymbolicNames = "/LocalData/SymbolicNames/ListingPage.xaml";
        public const string Page_Uri_Local_Volumes = "/LocalData/Volumes/ListingPage.xaml";
        public const string Page_Uri_Local_PersonalTagDefinitions = "/LocalData/PersonalTagDefinitions/ListingPage.xaml";
        public const string Page_Uri_Local_SharedTagDefinitions = "/LocalData/SharedTagDefinitions/ListingPage.xaml";
        public const string Page_Uri_Local_CrawlLogs = "/LocalData/CrawlLogs/ListingPage.xaml";
        public const string Page_Uri_Local_SummaryPropertySets = "/LocalData/SummaryPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_DocumentPropertySets = "/LocalData/DocumentPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_AudioPropertySets = "/LocalData/AudioPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_DRMPropertySets = "/LocalData/DRMPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_GPSPropertySets = "/LocalData/GPSPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_ImagePropertySets = "/LocalData/ImagePropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_MediaPropertySets = "/LocalData/MediaPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_MusicPropertySets = "/LocalData/MusicPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_PhotoPropertySets = "/LocalData/PhotoPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_RecordedTVPropertySets = "/LocalData/RecordedTVPropertySets/ListingPage.xaml";
        public const string Page_Uri_Local_VideoPropertySets = "/LocalData/VideoPropertySets/ListingPage.xaml";

        #region WindowTitle Property Members

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyPropertyBuilder<MainVM, string>
            .Register(nameof(WindowTitle))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="WindowTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle { get => GetValue(WindowTitleProperty) as string; private set => SetValue(WindowTitlePropertyKey, value); }

        #endregion
        #region ViewFileSystems Command Property Members

        private static readonly DependencyPropertyKey ViewFileSystemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewFileSystems),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewFileSystems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewFileSystemsProperty = ViewFileSystemsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewFileSystems => (Commands.RelayCommand)GetValue(ViewFileSystemsProperty);

        private void OnViewFileSystems(object parameter) => NavigateTo(Page_Uri_Local_FileSystems);

        #endregion
        #region ViewCrawlConfigurations Command Property Members

        private static readonly DependencyPropertyKey ViewCrawlConfigurationsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewCrawlConfigurations),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewCrawlConfigurations"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewCrawlConfigurationsProperty = ViewCrawlConfigurationsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewCrawlConfigurations => (Commands.RelayCommand)GetValue(ViewCrawlConfigurationsProperty);

        private void OnViewCrawlConfigurations(object parameter) => NavigateTo(Page_Uri_Local_CrawlConfigurations);

        #endregion
        #region NewCrawl Command Property Members

        private static readonly DependencyPropertyKey NewCrawlPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewCrawl),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NewCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NewCrawlProperty = NewCrawlPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewCrawl => (Commands.RelayCommand)GetValue(NewCrawlProperty);

        private void OnNewCrawl(object parameter)
        {
            // TODO: Implement OnNewCrawl(object)
            throw new NotImplementedException();
            //if (NavigatedContent is View.Local.CrawlConfigurationsPage crawlConfigurationsPage && crawlConfigurationsPage.DataContext is Local.CrawlConfigurationsPageVM crawlConfigurationsVM)
            //    crawlConfigurationsVM.RaiseAddNewItem(parameter);
            //else
            //{
            //    View.Local.EditCrawlConfigWindow window = new();
            //    window.ShowDialog();
            //}
        }

        #endregion
        #region ViewVolumes Command Property Members

        private static readonly DependencyPropertyKey ViewVolumesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewVolumes),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewVolumes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewVolumesProperty = ViewVolumesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewVolumes => (Commands.RelayCommand)GetValue(ViewVolumesProperty);

        private void OnViewVolumes(object parameter) => NavigateTo(Page_Uri_Local_Volumes);

        #endregion
        #region ViewSymbolicNames Command Property Members

        private static readonly DependencyPropertyKey ViewSymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewSymbolicNames),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewSymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewSymbolicNamesProperty = ViewSymbolicNamesPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewSymbolicNames => (Commands.RelayCommand)GetValue(ViewSymbolicNamesProperty);

        private void OnViewSymbolicNames(object parameter) => NavigateTo(Page_Uri_Local_SymbolicNames);

        #endregion
        #region ViewRedundancySets Command Property Members

        private static readonly DependencyPropertyKey ViewRedundancySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewRedundancySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewRedundancySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewRedundancySetsProperty = ViewRedundancySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewRedundancySets => (Commands.RelayCommand)GetValue(ViewRedundancySetsProperty);

        private void OnViewRedundancySets(object parameter) => NavigateTo(Page_Uri_Local_RedundantSets);

        #endregion
        #region ViewPersonalTagDefinitions Command Property Members

        private static readonly DependencyPropertyKey ViewPersonalTagDefinitionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewPersonalTagDefinitions),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewPersonalTagDefinitions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewPersonalTagDefinitionsProperty = ViewPersonalTagDefinitionsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewPersonalTagDefinitions => (Commands.RelayCommand)GetValue(ViewPersonalTagDefinitionsProperty);

        private void OnViewPersonalTagDefinitions(object parameter) => NavigateTo(Page_Uri_Local_PersonalTagDefinitions);

        #endregion
        #region ViewSharedTagDefinitions Command Property Members

        private static readonly DependencyPropertyKey ViewSharedTagDefinitionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewSharedTagDefinitions),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewSharedTagDefinitions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewSharedTagDefinitionsProperty = ViewSharedTagDefinitionsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewSharedTagDefinitions => (Commands.RelayCommand)GetValue(ViewSharedTagDefinitionsProperty);

        private void OnViewSharedTagDefinitions(object parameter) => NavigateTo(Page_Uri_Local_SharedTagDefinitions);

        #endregion
        #region ViewSummaryPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewSummaryPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewSummaryPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewSummaryPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewSummaryPropertySetsProperty = ViewSummaryPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewSummaryPropertySets => (Commands.RelayCommand)GetValue(ViewSummaryPropertySetsProperty);

        private void OnViewSummaryPropertySets(object parameter) => NavigateTo(Page_Uri_Local_SummaryPropertySets);

        #endregion
        #region ViewCrawlLogs Command Property Members

        private static readonly DependencyPropertyKey ViewCrawlLogsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewCrawlLogs),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewCrawlLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewCrawlLogsProperty = ViewCrawlLogsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewCrawlLogs => (Commands.RelayCommand)GetValue(ViewCrawlLogsProperty);

        private void OnViewCrawlLogs(object parameter) => NavigateTo(Page_Uri_Local_CrawlLogs);

        #endregion
        #region ViewDocumentPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewDocumentPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewDocumentPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewDocumentPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewDocumentPropertySetsProperty = ViewDocumentPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewDocumentPropertySets => (Commands.RelayCommand)GetValue(ViewDocumentPropertySetsProperty);

        private void OnViewDocumentPropertySets(object parameter) => NavigateTo(Page_Uri_Local_DocumentPropertySets);

        #endregion
        #region ViewAudioPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewAudioPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewAudioPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewAudioPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewAudioPropertySetsProperty = ViewAudioPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewAudioPropertySets => (Commands.RelayCommand)GetValue(ViewAudioPropertySetsProperty);

        private void OnViewAudioPropertySets(object parameter) => NavigateTo(Page_Uri_Local_AudioPropertySets);

        #endregion
        #region ViewDRMPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewDRMPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewDRMPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewDRMPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewDRMPropertySetsProperty = ViewDRMPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewDRMPropertySets => (Commands.RelayCommand)GetValue(ViewDRMPropertySetsProperty);

        private void OnViewDRMPropertySets(object parameter) => NavigateTo(Page_Uri_Local_DRMPropertySets);

        #endregion
        #region ViewGPSPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewGPSPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewGPSPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewGPSPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewGPSPropertySetsProperty = ViewGPSPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewGPSPropertySets => (Commands.RelayCommand)GetValue(ViewGPSPropertySetsProperty);

        private void OnViewGPSPropertySets(object parameter) => NavigateTo(Page_Uri_Local_GPSPropertySets);

        #endregion
        #region ViewImagePropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewImagePropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewImagePropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewImagePropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewImagePropertySetsProperty = ViewImagePropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewImagePropertySets => (Commands.RelayCommand)GetValue(ViewImagePropertySetsProperty);

        private void OnViewImagePropertySets(object parameter) => NavigateTo(Page_Uri_Local_ImagePropertySets);

        #endregion
        #region ViewMediaPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewMediaPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewMediaPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewMediaPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewMediaPropertySetsProperty = ViewMediaPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewMediaPropertySets => (Commands.RelayCommand)GetValue(ViewMediaPropertySetsProperty);

        private void OnViewMediaPropertySets(object parameter) => NavigateTo(Page_Uri_Local_MediaPropertySets);

        #endregion
        #region ViewMusicPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewMusicPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewMusicPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewMusicPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewMusicPropertySetsProperty = ViewMusicPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewMusicPropertySets => (Commands.RelayCommand)GetValue(ViewMusicPropertySetsProperty);

        private void OnViewMusicPropertySets(object parameter) => NavigateTo(Page_Uri_Local_MusicPropertySets);

        #endregion
        #region ViewPhotoPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewPhotoPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewPhotoPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewPhotoPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewPhotoPropertySetsProperty = ViewPhotoPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewPhotoPropertySets => (Commands.RelayCommand)GetValue(ViewPhotoPropertySetsProperty);

        private void OnViewPhotoPropertySets(object parameter) => NavigateTo(Page_Uri_Local_PhotoPropertySets);

        #endregion
        #region ViewRecordedTVPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewRecordedTVPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewRecordedTVPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewRecordedTVPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewRecordedTVPropertySetsProperty = ViewRecordedTVPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewRecordedTVPropertySets => (Commands.RelayCommand)GetValue(ViewRecordedTVPropertySetsProperty);

        private void OnViewRecordedTVPropertySets(object parameter) => NavigateTo(Page_Uri_Local_RecordedTVPropertySets);

        #endregion
        #region ViewVideoPropertySets Command Property Members

        private static readonly DependencyPropertyKey ViewVideoPropertySetsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewVideoPropertySets),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewVideoPropertySets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewVideoPropertySetsProperty = ViewVideoPropertySetsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewVideoPropertySets => (Commands.RelayCommand)GetValue(ViewVideoPropertySetsProperty);

        private void OnViewVideoPropertySets(object parameter) => NavigateTo(Page_Uri_Local_VideoPropertySets);

        #endregion
        #region CommandBindings Property Members

        public static CommandBindingCollection GetAttachedBindings(DependencyObject obj)
        {
            return (CommandBindingCollection)obj.GetValue(AttachedBindingsProperty);
        }

        public static void SetAttachedBindings(DependencyObject obj, CommandBindingCollection value)
        {
            obj.SetValue(AttachedBindingsProperty, value);
        }


        public static readonly DependencyProperty AttachedBindingsProperty = DependencyProperty.RegisterAttached("AttachedBindings", typeof(CommandBindingCollection), typeof(MainVM),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                if (d is UIElement element)
                {
                    element.CommandBindings.Clear();
                    if (e.NewValue is CommandBindingCollection items)
                        element.CommandBindings.AddRange(items);
                }
            }));

        private static readonly DependencyPropertyKey CommandBindingsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CommandBindings), typeof(CommandBindingCollection), typeof(MainVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CommandBindings"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandBindingsProperty = CommandBindingsPropertyKey.DependencyProperty;

        public CommandBindingCollection CommandBindings => (CommandBindingCollection)GetValue(CommandBindingsProperty);

        #endregion
        #region PageTitle Property Members

        /// <summary>
        /// Identifies the <see cref="PageTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty = DependencyPropertyBuilder<MainVM, string>
            .Register(nameof(PageTitle))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MainVM)?.OnPageTitlePropertyChanged(newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string PageTitle { get => GetValue(PageTitleProperty) as string; set => SetValue(PageTitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PageTitle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PageTitle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PageTitle"/> property.</param>
        protected virtual void OnPageTitlePropertyChanged(string newValue) => WindowTitle = (newValue.Length > 0) ?
            string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_FSInfoCatTitle, newValue) : FsInfoCat.Properties.Resources.DisplayName_FSInfoCat;

        #endregion

        #region NavigatedContent Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="NavigatedContent"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler NavigatedContentPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="NavigatedContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NavigatedContentProperty = DependencyProperty.Register(nameof(NavigatedContent), typeof(object), typeof(MainVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as MainVM)?.OnNavigatedContentPropertyChanged(e)));

        public object NavigatedContent { get => (object)GetValue(NavigatedContentProperty); set => SetValue(NavigatedContentProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="NavigatedContentProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="NavigatedContentProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnNavigatedContentPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnNavigatedContentPropertyChanged(args.OldValue, args.NewValue); }
            finally { NavigatedContentPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="NavigatedContent"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="NavigatedContent"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="NavigatedContent"/> property.</param>
        protected virtual void OnNavigatedContentPropertyChanged(object oldValue, object newValue)
        {
            if (oldValue is FrameworkElement oldContent)
            {
                if (oldContent is Page)
                    BindingOperations.ClearBinding(this, PageTitleProperty);
                if (oldContent.DataContext is INavigatedFromNotifiable navigatedFrom)
                    navigatedFrom.OnNavigatedFrom();
            }
            if (newValue is FrameworkElement newContent)
            {
                if (newContent is Page page)
                    BindingOperations.SetBinding(this, Page.TitleProperty, new Binding(nameof(Page.TitleProperty)) { Source = page }).UpdateTarget();
                else
                    PageTitle = "";
                if (newContent.DataContext is INavigatedToNotifiable navigatedTo)
                    navigatedTo.OnNavigatedTo();
            }
            else
                PageTitle = "";
        }

        #endregion

        public MainVM(ILogger<MainVM> logger)
        {
            _logger = logger;
            SetValue(CommandBindingsPropertyKey, new CommandBindingCollection
            {
                new CommandBinding(ApplicationCommands.Close, OnClose)
            });
            SetValue(ViewFileSystemsPropertyKey, new Commands.RelayCommand(OnViewFileSystems));
            SetValue(ViewCrawlConfigurationsPropertyKey, new Commands.RelayCommand(OnViewCrawlConfigurations));
            SetValue(NewCrawlPropertyKey, new Commands.RelayCommand(OnNewCrawl));
            SetValue(ViewVolumesPropertyKey, new Commands.RelayCommand(OnViewVolumes));
            SetValue(ViewSymbolicNamesPropertyKey, new Commands.RelayCommand(OnViewSymbolicNames));
            SetValue(ViewRedundancySetsPropertyKey, new Commands.RelayCommand(OnViewRedundancySets));
            SetValue(ViewPersonalTagDefinitionsPropertyKey, new Commands.RelayCommand(OnViewPersonalTagDefinitions));
            SetValue(ViewSharedTagDefinitionsPropertyKey, new Commands.RelayCommand(OnViewSharedTagDefinitions));
            SetValue(ViewSummaryPropertySetsPropertyKey, new Commands.RelayCommand(OnViewSummaryPropertySets));
            SetValue(ViewDocumentPropertySetsPropertyKey, new Commands.RelayCommand(OnViewDocumentPropertySets));
            SetValue(ViewAudioPropertySetsPropertyKey, new Commands.RelayCommand(OnViewAudioPropertySets));
            SetValue(ViewCrawlLogsPropertyKey, new Commands.RelayCommand(OnViewCrawlLogs));
            SetValue(ViewDRMPropertySetsPropertyKey, new Commands.RelayCommand(OnViewDRMPropertySets));
            SetValue(ViewGPSPropertySetsPropertyKey, new Commands.RelayCommand(OnViewGPSPropertySets));
            SetValue(ViewImagePropertySetsPropertyKey, new Commands.RelayCommand(OnViewImagePropertySets));
            SetValue(ViewMediaPropertySetsPropertyKey, new Commands.RelayCommand(OnViewMediaPropertySets));
            SetValue(ViewMusicPropertySetsPropertyKey, new Commands.RelayCommand(OnViewMusicPropertySets));
            SetValue(ViewPhotoPropertySetsPropertyKey, new Commands.RelayCommand(OnViewPhotoPropertySets));
            SetValue(ViewRecordedTVPropertySetsPropertyKey, new Commands.RelayCommand(OnViewRecordedTVPropertySets));
            SetValue(ViewVideoPropertySetsPropertyKey, new Commands.RelayCommand(OnViewVideoPropertySets));
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            _logger.LogInformation("{EventName} event raise: sender = {Sender}; EventArgs= {EventArgs}", nameof(OnClose), sender, e);
            Application.Current.MainWindow?.Close();
        }

        private void NavigateTo(string pageUri)
        {
            _logger.LogInformation("Navigating to {PageUri}", pageUri);
            _ = Navigate(new Uri(pageUri, UriKind.Relative));
        }

        private bool P_TryGetNavigationService(out NavigationService navigationService)
        {
            if (NavigatedContent is DependencyObject navigatedContent)
                return (navigationService = NavigationService.GetNavigationService(navigatedContent)) is not null;
            navigationService = null;
            return false;
        }

        private NavigationService P_GetNavigationService() => (NavigatedContent is DependencyObject navigatedContent) ? NavigationService.GetNavigationService(navigatedContent) : null;

        internal bool TryGetNavigationService(out NavigationService navigationService)
        {
            VerifyAccess();
            return (navigationService = P_GetNavigationService()) is not null;
        }

        internal NavigationService GetNavigationService()
        {
            VerifyAccess();
            return P_GetNavigationService();
        }

        internal Uri GetSource() => Dispatcher.CheckInvoke(() => P_GetNavigationService()?.Source);

        internal Uri GetCurrentSource() => Dispatcher.CheckInvoke(() => P_GetNavigationService()?.CurrentSource);

        internal object GetContent() => Dispatcher.CheckInvoke(() => P_GetNavigationService()?.Content);

        internal bool CanGoForward() => Dispatcher.CheckInvoke(() => P_GetNavigationService()?.CanGoForward ?? false);

        internal bool CanGoBack() => Dispatcher.CheckInvoke(() => P_GetNavigationService()?.CanGoBack ?? false);

        Uri IApplicationNavigation.Source => GetSource();

        Uri IApplicationNavigation.CurrentSource => GetCurrentSource();

        object IApplicationNavigation.Content => GetContent();

        bool IApplicationNavigation.CanGoForward => CanGoForward();

        bool IApplicationNavigation.CanGoBack => CanGoBack();

        public bool GoBack() => Dispatcher.CheckInvoke(() =>
        {
            if (P_TryGetNavigationService(out NavigationService navigationService) && navigationService.CanGoBack)
            {
                _logger.LogInformation("Navigating back");
                navigationService.GoBack();
                return true;
            }
            return false;
        });

        public bool GoForward() => Dispatcher.CheckInvoke(() =>
        {
            if (P_TryGetNavigationService(out NavigationService navigationService) && navigationService.CanGoForward)
            {
                _logger.LogInformation("Navigating forward");
                navigationService.GoForward();
                return true;
            }
            return false;
        });

        public bool Navigate(Uri source, object navigationState, bool sandboxExternalContent)
        {
            _logger.LogInformation("Navigating to URI {source} (navigationState: {navigationState}, sandboxExternalContent: {sandboxExternalContent})",
                source, navigationState, sandboxExternalContent);
            return Dispatcher.CheckInvoke(() =>
                P_TryGetNavigationService(out NavigationService navigationService) && navigationService.Navigate(source, navigationState, sandboxExternalContent));
        }

        public bool Navigate(Uri source, object navigationState)
        {
            _logger.LogInformation("Navigating to URI {source} (navigationState: {navigationState})", source, navigationState);
            return Dispatcher.CheckInvoke(() =>
                P_TryGetNavigationService(out NavigationService navigationService) && navigationService.Navigate(source, navigationState));
        }

        public bool Navigate(Uri source)
        {
            _logger.LogInformation("Navigating to URI {source}", source);
            return Dispatcher.CheckInvoke(() => P_TryGetNavigationService(out NavigationService navigationService) && navigationService.Navigate(source));
        }

        public bool Navigate(object root, object navigationState)
        {
            _logger.LogInformation("Navigating to Content {root} (navigationState: {navigationState})", root, navigationState);
            return Dispatcher.CheckInvoke(() =>
                P_TryGetNavigationService(out NavigationService navigationService) && navigationService.Navigate(root, navigationState));
        }

        public bool Navigate(object root)
        {
            _logger.LogInformation("Navigating to Content {root}", root);
            return Dispatcher.CheckInvoke(() => P_TryGetNavigationService(out NavigationService navigationService) && navigationService.Navigate(root));
        }

        public void Refresh()
        {
            _logger.LogInformation("Refreshing current content");
            Dispatcher.CheckInvoke(() => P_GetNavigationService()?.Refresh());
        }
    }
}
