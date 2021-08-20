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

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject
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

        private void OnViewFileSystems(object parameter)
        {
            Source = new Uri(Page_Uri_Local_FileSystems, UriKind.Relative);
        }

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

        private void OnViewCrawlConfigurations(object parameter)
        {
            Source = new Uri(Page_Uri_Local_CrawlConfigurations, UriKind.Relative);
        }

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

        private void OnViewVolumes(object parameter)
        {
            Source = new Uri(Page_Uri_Local_Volumes, UriKind.Relative);
        }

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

        private void OnViewSymbolicNames(object parameter)
        {
            Source = new Uri(Page_Uri_Local_SymbolicNames, UriKind.Relative);
        }

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

        private void OnViewRedundancySets(object parameter)
        {
            Source = new Uri(Page_Uri_Local_RedundantSets, UriKind.Relative);
        }

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

        private void OnViewPersonalTagDefinitions(object parameter)
        {
            Source = new Uri(Page_Uri_Local_PersonalTagDefinitions, UriKind.Relative);
        }

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

        private void OnViewSharedTagDefinitions(object parameter)
        {
            Source = new Uri(Page_Uri_Local_SharedTagDefinitions, UriKind.Relative);
        }

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
        #region Source Property Members

        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(MainVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as MainVM)?.OnSourcePropertyChanged((Uri)e.OldValue, (Uri)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public Uri Source { get => (Uri)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Source"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Source"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Source"/> property.</param>
        private void OnSourcePropertyChanged(Uri oldValue, Uri newValue)
        {
            // TODO: Implement OnSourcePropertyChanged Logic
        }

        #endregion
        public MainVM()
        {
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
            Source = new Uri(Page_Uri_Local_CrawlConfigurations, UriKind.Relative);
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e) => Application.Current.MainWindow?.Close();
    }
}
