using FsInfoCat.Desktop.Converters;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    public class ListItemViewModel : VolumeListItemWithFileSystemViewModel<VolumeListItemWithFileSystem>, ILocalCrudEntityRowViewModel<VolumeListItemWithFileSystem>
    {
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
        #region IdentifierDisplayText Property Members

        private static readonly DependencyPropertyKey IdentifierDisplayTextPropertyKey = DependencyPropertyBuilder<ListItemViewModel, string>
            .Register(nameof(IdentifierDisplayText))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IdentifierDisplayText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierDisplayTextProperty = IdentifierDisplayTextPropertyKey.DependencyProperty;

        public string IdentifierDisplayText { get => GetValue(IdentifierDisplayTextProperty) as string; private set => SetValue(IdentifierDisplayTextPropertyKey, value); }

        private void UpdateIdentifierDisplayText(string volumeName, VolumeIdentifier identifier)
        {
            if (string.IsNullOrWhiteSpace(volumeName) || !(identifier.SerialNumber.HasValue  || identifier.UUID.HasValue))
                IdentifierDisplayText = VolumeIdentifierToStringConverter.Convert(identifier);
            else
                IdentifierDisplayText = identifier.IsEmpty() ? volumeName : $"v{volumeName} ({VolumeIdentifierToStringConverter.Convert(identifier)})";
        }

        #endregion
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
        #region BrowseVolume Command Property Members
        /// <summary>
        /// Occurs when the <see cref="BrowseVolume"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> BrowseVolumeCommand;

        private static readonly DependencyPropertyKey BrowseVolumePropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(BrowseVolume))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="BrowseVolume"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrowseVolumeProperty = BrowseVolumePropertyKey.DependencyProperty;

        public Commands.RelayCommand BrowseVolume => (Commands.RelayCommand)GetValue(BrowseVolumeProperty);

        /// <summary>
        /// Called when the BrowseVolume event is raised by <see cref="BrowseVolume" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="BrowseVolume" />.</param>
        protected void RaiseBrowseVolumeCommand(object parameter) => BrowseVolumeCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnBrowseVolumeCommand(parameter); }
        //   finally { BrowseVolumeCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="BrowseVolume">BrowseVolume Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="BrowseVolume" />.</param>
        protected virtual void OnBrowseVolumeCommand(object parameter)
        {
            // TODO: Implement OnBrowseVolumeCommand Logic
        }

        #endregion
        #region OpenFileSystem Command Property Members

        /// <summary>
        /// Occurs when the <see cref="OpenFileSystem"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenFileSystemCommand;

        private static readonly DependencyPropertyKey OpenFileSystemPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(OpenFileSystem))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="OpenFileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenFileSystemProperty = OpenFileSystemPropertyKey.DependencyProperty;

        public Commands.RelayCommand OpenFileSystem => (Commands.RelayCommand)GetValue(OpenFileSystemProperty);

        /// <summary>
        /// Called when the OpenFileSystem event is raised by <see cref="OpenFileSystem" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenFileSystem" />.</param>
        protected void RaiseOpenFileSystemCommand(object parameter) => OpenFileSystemCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnOpenFileSystemCommand(parameter); }
        //   finally { OpenFileSystemCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="OpenFileSystem">OpenFileSystem Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenFileSystem" />.</param>
        protected virtual void OnOpenFileSystemCommand(object parameter)
        {
            // TODO: Implement OnOpenFileSystemCommand Logic
        }

        #endregion
        #region ViewErrorListing Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ViewErrorListing"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewErrorListingCommand;

        private static readonly DependencyPropertyKey ViewErrorListingPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ViewErrorListing))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ViewErrorListing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewErrorListingProperty = ViewErrorListingPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewErrorListing => (Commands.RelayCommand)GetValue(ViewErrorListingProperty);

        /// <summary>
        /// Called when the ViewErrorListing event is raised by <see cref="ViewErrorListing" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewErrorListing" />.</param>
        protected void RaiseViewErrorListingCommand(object parameter) => ViewErrorListingCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnViewErrorListingCommand(parameter); }
        //   finally { ViewErrorListingCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ViewErrorListing">ViewErrorListing Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewErrorListing" />.</param>
        protected virtual void OnViewErrorListingCommand(object parameter)
        {
            // TODO: Implement OnViewErrorListingCommand Logic
        }

        #endregion
        #region ViewPersonalTags Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ViewPersonalTags"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewPersonalTagsCommand;

        private static readonly DependencyPropertyKey ViewPersonalTagsPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ViewPersonalTags))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ViewPersonalTags"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewPersonalTagsProperty = ViewPersonalTagsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewPersonalTags => (Commands.RelayCommand)GetValue(ViewPersonalTagsProperty);

        /// <summary>
        /// Called when the ViewPersonalTags event is raised by <see cref="ViewPersonalTags" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewPersonalTags" />.</param>
        protected void RaiseViewPersonalTagsCommand(object parameter) => ViewPersonalTagsCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnViewPersonalTagsCommand(parameter); }
        //   finally { ViewPersonalTagsCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ViewPersonalTags">ViewPersonalTags Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewPersonalTags" />.</param>
        protected virtual void OnViewPersonalTagsCommand(object parameter)
        {
            // TODO: Implement OnViewPersonalTagsCommand Logic
        }

        #endregion
        #region ViewSharedTags Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ViewSharedTags"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewSharedTagsCommand;

        private static readonly DependencyPropertyKey ViewSharedTagsPropertyKey = DependencyPropertyBuilder<ListItemViewModel, Commands.RelayCommand>
            .Register(nameof(ViewSharedTags))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ViewSharedTags"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewSharedTagsProperty = ViewSharedTagsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewSharedTags => (Commands.RelayCommand)GetValue(ViewSharedTagsProperty);

        /// <summary>
        /// Called when the ViewSharedTags event is raised by <see cref="ViewSharedTags" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewSharedTags" />.</param>
        protected void RaiseViewSharedTagsCommand(object parameter) => ViewSharedTagsCommand?.Invoke(this, new(parameter));
        // {
        //   try { OnViewSharedTagsCommand(parameter); }
        //   finally { ViewSharedTagsCommand?.Invoke(this, new(parameter)); }
        // }

        /// <summary>
        /// Called when the <see cref="ViewSharedTags">ViewSharedTags Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewSharedTags" />.</param>
        protected virtual void OnViewSharedTagsCommand(object parameter)
        {
            // TODO: Implement OnViewSharedTagsCommand Logic 
        }

        #endregion

        protected override void OnVolumeNamePropertyChanged(string oldValue, string newValue)
        {
            UpdateIdentifierDisplayText(newValue, Identifier);
            base.OnVolumeNamePropertyChanged(oldValue, newValue);
        }

        protected override void OnIdentifierPropertyChanged(VolumeIdentifier oldValue, VolumeIdentifier newValue)
        {
            UpdateIdentifierDisplayText(VolumeName, newValue);
            base.OnIdentifierPropertyChanged(oldValue, newValue);
        }

        public ListItemViewModel([DisallowNull] VolumeListItemWithFileSystem entity) : base(entity)
        {
            SetValue(SynchronizeNowPropertyKey, new Commands.RelayCommand(RaiseSynchronizeNowCommand));
            SetValue(BrowseVolumePropertyKey, new Commands.RelayCommand(RaiseBrowseVolumeCommand));
            SetValue(OpenFileSystemPropertyKey, new Commands.RelayCommand(RaiseOpenFileSystemCommand));
            SetValue(ViewErrorListingPropertyKey, new Commands.RelayCommand(RaiseViewErrorListingCommand));
            SetValue(ViewPersonalTagsPropertyKey, new Commands.RelayCommand(RaiseViewPersonalTagsCommand));
            SetValue(ViewSharedTagsPropertyKey, new Commands.RelayCommand(RaiseViewSharedTagsCommand));
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            UpdateIdentifierDisplayText(VolumeName, Identifier);
        }
    }
}
