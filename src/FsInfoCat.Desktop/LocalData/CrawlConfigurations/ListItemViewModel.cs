using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class ListItemViewModel : CrawlConfigListItemViewModel<CrawlConfigListItem>, ILocalCrudEntityRowViewModel<CrawlConfigListItem>
    {
        #region SynchronizeNow Command Property Members

        /// <summary>
        /// Occurs when the <see cref="SynchronizeNow"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> SynchronizeNowCommand;

        private static readonly DependencyPropertyKey SynchronizeNowPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(SynchronizeNow))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SynchronizeNow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SynchronizeNowProperty = SynchronizeNowPropertyKey.DependencyProperty;

        public Commands.RelayCommand SynchronizeNow => (Commands.RelayCommand)GetValue(SynchronizeNowProperty);

        /// <summary>
        /// Called when the SynchronizeNow event is raised by <see cref="SynchronizeNow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected void RaiseSynchronizeNowCommand(object parameter) => SynchronizeNowCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnSynchronizeNowCommand(parameter); }
        //   finally { SynchronizeNowCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="SynchronizeNow">SynchronizeNow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SynchronizeNow" />.</param>
        protected virtual void OnSynchronizeNowCommand(object parameter)
        {
            // TODO: Implement OnSynchronizeNowCommand Logic
        }

        #endregion
        #region OpenRootSubdirectory Command Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenRootSubdirectory"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenRootSubdirectoryCommand;

        private static readonly DependencyPropertyKey OpenRootSubdirectoryPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(OpenRootSubdirectory))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="OpenRootSubdirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenRootSubdirectoryProperty = OpenRootSubdirectoryPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenRootSubdirectory => (Commands.RelayCommand)GetValue(OpenRootSubdirectoryProperty);

        /// <summary>
        /// Called when the OpenRootSubdirectory event is raised by <see cref="OpenRootSubdirectory" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenRootSubdirectory" />.</param>
        protected void RaiseOpenRootSubdirectoryCommand(object parameter) // => OpenRootSubdirectoryCommand?.Invoke(this, new(parameter));
        {
            try { OnOpenRootSubdirectoryCommand(parameter); }
            finally { OpenRootSubdirectoryCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenRootSubdirectory">OpenRootSubdirectory Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenRootSubdirectory" />.</param>
        protected virtual void OnOpenRootSubdirectoryCommand(object parameter)
        {
            // TODO: Implement OnOpenRootSubdirectoryCommand Logic
        }

        #endregion
        #region StartCrawl Command Property Members

        /// <summary>
        /// Occurs when the <see cref="StartCrawl"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StartCrawlCommand;

        private static readonly DependencyPropertyKey StartCrawlPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(StartCrawl))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StartCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartCrawlProperty = StartCrawlPropertyKey.DependencyProperty;

        public Commands.RelayCommand StartCrawl => (Commands.RelayCommand)GetValue(StartCrawlProperty);

        /// <summary>
        /// Called when the StartCrawl event is raised by <see cref="StartCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        protected void RaiseStartCrawlCommand(object parameter) // => StartCrawlCommand?.Invoke(this, new(parameter));
        {
            try { OnStartCrawlCommand(parameter); }
            finally { StartCrawlCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="StartCrawl">StartCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StartCrawl" />.</param>
        protected virtual void OnStartCrawlCommand(object parameter)
        {
            // TODO: Implement OnStartCrawlCommand Logic
        }

        #endregion
        #region StopCrawl Command Property Members

        /// <summary>
        /// Occurs when the <see cref="StopCrawl"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> StopCrawlCommand;

        private static readonly DependencyPropertyKey StopCrawlPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(StopCrawl))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="StopCrawl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StopCrawlProperty = StopCrawlPropertyKey.DependencyProperty;

        public Commands.RelayCommand StopCrawl => (Commands.RelayCommand)GetValue(StopCrawlProperty);

        /// <summary>
        /// Called when the StopCrawl event is raised by <see cref="StopCrawl" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        protected void RaiseStopCrawlCommand(object parameter) // => StopCrawlCommand?.Invoke(this, new(parameter));
        {
            try { OnStopCrawlCommand(parameter); }
            finally { StopCrawlCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="StopCrawl">StopCrawl Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="StopCrawl" />.</param>
        protected virtual void OnStopCrawlCommand(object parameter)
        {
            // TODO: Implement OnStopCrawlCommand Logic
        }

        #endregion
        #region ShowCrawlActivityRecords Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowCrawlActivityRecords"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowCrawlActivityRecordsCommand;

        private static readonly DependencyPropertyKey ShowCrawlActivityRecordsPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowCrawlActivityRecords))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowCrawlActivityRecords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowCrawlActivityRecordsProperty = ShowCrawlActivityRecordsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowCrawlActivityRecords => (Commands.RelayCommand)GetValue(ShowCrawlActivityRecordsProperty);

        /// <summary>
        /// Called when the ShowCrawlActivityRecords event is raised by <see cref="ShowCrawlActivityRecords" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowCrawlActivityRecords" />.</param>
        protected void RaiseShowCrawlActivityRecordsCommand(object parameter) // => ShowCrawlActivityRecordsCommand?.Invoke(this, new(parameter));
        {
            try { OnShowCrawlActivityRecordsCommand(parameter); }
            finally { ShowCrawlActivityRecordsCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowCrawlActivityRecords">ShowCrawlActivityRecords Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowCrawlActivityRecords" />.</param>
        protected virtual void OnShowCrawlActivityRecordsCommand(object parameter)
        {
            // TODO: Implement OnShowCrawlActivityRecordsCommand Logic
        }

        #endregion
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the value of the primary key for the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="UpstreamId" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="UpstreamId" /> should not be <see langword="null" />, either.
        /// </remarks>
        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion

        public ListItemViewModel([DisallowNull] CrawlConfigListItem entity) : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(OpenRootSubdirectoryPropertyKey, new Commands.RelayCommand(RaiseOpenRootSubdirectoryCommand));
            SetValue(StartCrawlPropertyKey, new Commands.RelayCommand(RaiseStartCrawlCommand));
            SetValue(StopCrawlPropertyKey, new Commands.RelayCommand(RaiseStopCrawlCommand));
            SetValue(ShowCrawlActivityRecordsPropertyKey, new Commands.RelayCommand(RaiseShowCrawlActivityRecordsCommand));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
