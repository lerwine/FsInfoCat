using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.FileSystems
{
    public class ListItemViewModel : FileSystemListItemViewModel<FileSystemListItem>, ILocalCrudEntityRowViewModel<FileSystemListItem>
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
        #region ShowVolumeListing Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowVolumeListing"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowVolumeListingCommand;

        private static readonly DependencyPropertyKey ShowVolumeListingPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowVolumeListing))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowVolumeListing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowVolumeListingProperty = ShowVolumeListingPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowVolumeListing => (Commands.RelayCommand)GetValue(ShowVolumeListingProperty);

        /// <summary>
        /// Called when the ShowVolumeListing event is raised by <see cref="ShowVolumeListing" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowVolumeListing" />.</param>
        protected void RaiseShowVolumeListingCommand(object parameter) // => ShowVolumeListingCommand?.Invoke(this, new(parameter));
        {
            try { OnShowVolumeListingCommand(parameter); }
            finally { ShowVolumeListingCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowVolumeListing">ShowVolumeListing Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowVolumeListing" />.</param>
        protected virtual void OnShowVolumeListingCommand(object parameter)
        {
            // TODO: Implement OnShowVolumeListingCommand Logic
        }

        #endregion
        #region ShowSymbolicNameListing Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ShowSymbolicNameListing"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ShowSymbolicNameListingCommand;

        private static readonly DependencyPropertyKey ShowSymbolicNameListingPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ShowSymbolicNameListing))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowSymbolicNameListing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowSymbolicNameListingProperty = ShowSymbolicNameListingPropertyKey.DependencyProperty;

        public Commands.RelayCommand ShowSymbolicNameListing => (Commands.RelayCommand)GetValue(ShowSymbolicNameListingProperty);

        /// <summary>
        /// Called when the ShowSymbolicNameListing event is raised by <see cref="ShowSymbolicNameListing" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowSymbolicNameListing" />.</param>
        protected void RaiseShowSymbolicNameListingCommand(object parameter) // => ShowSymbolicNameListingCommand?.Invoke(this, new(parameter));
        {
            try { OnShowSymbolicNameListingCommand(parameter); }
            finally { ShowSymbolicNameListingCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ShowSymbolicNameListing">ShowSymbolicNameListing Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ShowSymbolicNameListing" />.</param>
        protected virtual void OnShowSymbolicNameListingCommand(object parameter)
        {
            // TODO: Implement OnShowSymbolicNameListingCommand Logic
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

        public ListItemViewModel([DisallowNull] FileSystemListItem entity)
            : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(ShowVolumeListingPropertyKey, new Commands.RelayCommand(RaiseShowVolumeListingCommand));
            SetValue(ShowSymbolicNameListingPropertyKey, new Commands.RelayCommand(RaiseShowSymbolicNameListingCommand));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
