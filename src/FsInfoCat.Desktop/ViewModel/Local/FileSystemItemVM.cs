using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileSystemItemVM : DbEntityItemVM<FileSystemListItem>
    {
        #region ToggleCurrentItemActivation Property Members

        /// <summary>
        /// Occurs when the <see cref="ToggleCurrentItemActivation">ToggleCurrentItemActivation Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ToggleActivationRequest;

        private static readonly DependencyPropertyKey ToggleCurrentItemActivationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ToggleCurrentItemActivation),
            typeof(Commands.RelayCommand), typeof(FileSystemItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ToggleCurrentItemActivation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToggleCurrentItemActivationProperty = ToggleCurrentItemActivationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ToggleCurrentItemActivation => (Commands.RelayCommand)GetValue(ToggleCurrentItemActivationProperty);

        /// <summary>
        /// Called when the ToggleCurrentItemActivation event is raised by <see cref="ToggleCurrentItemActivation" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ToggleCurrentItemActivation" />.</param>
        private void RaiseToggleActivationRequest(object parameter)
        {
            try { OnToggleActivationRequest(parameter); }
            finally { ToggleActivationRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ToggleCurrentItemActivation">ToggleCurrentItemActivation Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ToggleCurrentItemActivation" />.</param>
        protected virtual void OnToggleActivationRequest(object parameter)
        {
            // TODO: Implement OnToggleActivation Logic
        }

        #endregion
        #region OpenVolumesWindow Command Members

        /// <summary>
        /// Occurs when the <see cref="OpenVolumesWindow">OpenVolumesWindow Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewVolumesRequest;

        private static readonly DependencyPropertyKey OpenVolumesWindowPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpenVolumesWindow),
            typeof(Commands.RelayCommand), typeof(FileSystemItemVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenVolumesWindowProperty = OpenVolumesWindowPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand OpenVolumesWindow => (Commands.RelayCommand)GetValue(OpenVolumesWindowProperty);

        /// <summary>
        /// Called when the OpenVolumesWindow event is raised by <see cref="OpenVolumesWindow" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenVolumesWindow" />.</param>
        private void RaiseViewVolumesRequest(object parameter)
        {
            try { OnViewVolumesRequest(parameter); }
            finally { ViewVolumesRequest?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="OpenVolumesWindow">OpenVolumesWindow Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="OpenVolumesWindow" />.</param>
        protected virtual void OnViewVolumesRequest(object parameter)
        {
            // TODO: Implement OnOpenVolumes Logic
        }

        #endregion
        #region DefaultDriveType Property Members

        private static readonly DependencyPropertyKey DefaultDriveTypePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DefaultDriveType), typeof(DriveType?), typeof(FileSystemItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DefaultDriveType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultDriveTypeProperty = DefaultDriveTypePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DriveType? DefaultDriveType { get => (DriveType?)GetValue(DefaultDriveTypeProperty); private set => SetValue(DefaultDriveTypePropertyKey, value); }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(FileSystemItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; private set => SetValue(DisplayNamePropertyKey, value); }

        #endregion
        #region IsInactive Property Members

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(FileSystemItemVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = IsInactivePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); private set => SetValue(IsInactivePropertyKey, value); }

        #endregion
        #region MaxNameLength Property Members

        private static readonly DependencyPropertyKey MaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLength), typeof(uint), typeof(FileSystemItemVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxNameLength));

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = MaxNameLengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public uint MaxNameLength { get => (uint)GetValue(MaxNameLengthProperty); private set => SetValue(MaxNameLengthPropertyKey, value); }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(FileSystemItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; private set => SetValue(NotesPropertyKey, value); }

        #endregion
        #region IsReadOnly Property Members

        private static readonly DependencyPropertyKey IsReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsReadOnly), typeof(bool), typeof(FileSystemItemVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = IsReadOnlyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsReadOnly { get => (bool)GetValue(IsReadOnlyProperty); private set => SetValue(IsReadOnlyPropertyKey, value); }

        #endregion
        #region VolumeCount Property Members

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeCount), typeof(long), typeof(FileSystemItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemItemVM).OnVolumeCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long VolumeCount { get => (long)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeCount"/> property.</param>
        private void OnVolumeCountPropertyChanged(long oldValue, long newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0L && SymbolicNameCount == 0L;
        }

        #endregion
        #region SymbolicNameCount Property Members

        private static readonly DependencyPropertyKey SymbolicNameCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNameCount), typeof(long), typeof(FileSystemItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemItemVM).OnSymbolicNameCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SymbolicNameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNameCountProperty = SymbolicNameCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SymbolicNameCount { get => (long)GetValue(SymbolicNameCountProperty); internal set => SetValue(SymbolicNameCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SymbolicNameCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SymbolicNameCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SymbolicNameCount"/> property.</param>
        private void OnSymbolicNameCountPropertyChanged(long oldValue, long newValue)
        {
            DeleteCurrentItem.IsEnabled = newValue == 0L && VolumeCount == 0L;
        }

        #endregion

        //internal FileSystemItemVM([DisallowNull] FileSystemsPageVM.EntityAndCounts result)
        //    : this(result.Entity)
        //{
        //    SymbolicNameCount = result.SymbolicNameCount;
        //    VolumeCount = result.VolumeCount;
        //}

        internal FileSystemItemVM([DisallowNull] FileSystemListItem model)
            : base(model)
        {
            SymbolicNameCount = model.SymbolicNameCount;
            VolumeCount = model.VolumeCount;
            DefaultDriveType = model.DefaultDriveType;
            DisplayName = model.DisplayName;
            IsInactive = model.IsInactive;
            MaxNameLength = model.MaxNameLength;
            Notes = model.Notes;
            IsReadOnly = model.ReadOnly;
            SetValue(OpenVolumesWindowPropertyKey, new Commands.RelayCommand(RaiseViewVolumesRequest));
            SetValue(ToggleCurrentItemActivationPropertyKey, new Commands.RelayCommand(RaiseToggleActivationRequest));
        }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property changes.
        /// </summary>
        /// <param name="propertyName">The previous value of the <see cref="Notes"/>.</param>
        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FileSystemListItem.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(FileSystemListItem.DefaultDriveType):
                    Dispatcher.CheckInvoke(() => DefaultDriveType = Model?.DefaultDriveType);
                    break;
                case nameof(FileSystemListItem.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Model?.IsInactive ?? false);
                    break;
                case nameof(FileSystemListItem.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Model?.MaxNameLength ?? DbConstants.DbColDefaultValue_MaxNameLength);
                    break;
                case nameof(FileSystemListItem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(FileSystemListItem.ReadOnly):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly ?? false);
                    break;
                case nameof(FileSystemListItem.PrimarySymbolicName):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly ?? false);
                    break;
                case nameof(FileSystemListItem.SymbolicNameCount):
                    Dispatcher.CheckInvoke(() => SymbolicNameCount = Model?.SymbolicNameCount ?? 0L);
                    break;
                case nameof(FileSystemListItem.VolumeCount):
                    Dispatcher.CheckInvoke(() => VolumeCount = Model?.VolumeCount ?? 0L);
                    break;
            }
        }

        protected override DbSet<FileSystemListItem> GetDbSet(LocalDbContext dbContext) => dbContext.FileSystemListing;
    }
}
