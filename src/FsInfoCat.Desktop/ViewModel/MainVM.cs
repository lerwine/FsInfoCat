using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject, IApplicationNavigation
    {
        private ILogger<MainVM> _logger;

        public const string Page_Uri_Local_CrawlConfigurations = "/View/Local/CrawlConfigurationsPage.xaml";
        public const string Page_Uri_Local_FileSystems = "/View/Local/FileSystemsPage.xaml";
        public const string Page_Uri_Local_RedundantSets = "/View/Local/RedundantSetsPage.xaml";
        public const string Page_Uri_Local_SymbolicNames = "/View/Local/SymbolicNamesPage.xaml";
        public const string Page_Uri_Local_Volumes = "/View/Local/VolumesPage.xaml";
        public const string Page_Uri_Local_PersonalTagDefinitions = "/View/Local/PersonalTagDefinitionsPage.xaml";
        public const string Page_Uri_Local_SharedTagDefinitions = "/View/Local/SharedTagDefinitionsPage.xaml";

        #region ViewFileSystems Command Property Members

        private static readonly DependencyPropertyKey ViewFileSystemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewFileSystems),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewFileSystems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewFileSystemsProperty = ViewFileSystemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand NewCrawl => (Commands.RelayCommand)GetValue(NewCrawlProperty);

        private void OnNewCrawl(object parameter)
        {
            if (NavigatedContent is View.Local.CrawlConfigurationsPage crawlConfigurationsPage && crawlConfigurationsPage.DataContext is Local.CrawlConfigurationsPageVM crawlConfigurationsVM)
                crawlConfigurationsVM.RaiseAddNewItem(parameter);
            else
            {
                View.Local.EditCrawlConfigWindow window = new();
                window.ShowDialog();
            }
        }

        #endregion
        #region ViewVolumes Command Property Members

        private static readonly DependencyPropertyKey ViewVolumesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewVolumes),
            typeof(Commands.RelayCommand), typeof(MainVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewVolumes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewVolumesProperty = ViewVolumesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ViewSharedTagDefinitions => (Commands.RelayCommand)GetValue(ViewSharedTagDefinitionsProperty);

        private void OnViewSharedTagDefinitions(object parameter) => NavigateTo(Page_Uri_Local_SharedTagDefinitions);

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

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public CommandBindingCollection CommandBindings => (CommandBindingCollection)GetValue(CommandBindingsProperty);

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

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public object NavigatedContent { get => (object)GetValue(NavigatedContentProperty); set => SetValue(NavigatedContentProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="NavigatedContentProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="NavigatedContentProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnNavigatedContentPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnNavigatedContentPropertyChanged((object)args.OldValue, (object)args.NewValue); }
            finally { NavigatedContentPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="NavigatedContent"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="NavigatedContent"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="NavigatedContent"/> property.</param>
        protected virtual void OnNavigatedContentPropertyChanged(object oldValue, object newValue)
        {
            if (oldValue is Page oldPage && oldPage.DataContext is INotifyNavigationContentChanged oldNavigated)
                oldNavigated.OnNavigatedFrom(this);
            if (newValue is Page newPage && newPage.DataContext is INotifyNavigationContentChanged newNavigated)
                newNavigated.OnNavigatedTo(this);
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
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            _logger.LogInformation("{EventName} event raise: sender = {Sender}; EventArgs= {EventArgs}", nameof(OnClose), sender, e);
            Application.Current.MainWindow?.Close();
        }

        private void NavigateTo(string pageUri)
        {
            _logger.LogInformation("Navigating to {PageUri}", pageUri);
            Navigate(new Uri(pageUri, UriKind.Relative));
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
