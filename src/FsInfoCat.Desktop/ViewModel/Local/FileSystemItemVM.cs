using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileSystemItemVM : DbEntityItemVM<FileSystem>
    {
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

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeCount), typeof(int), typeof(FileSystemItemVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemItemVM).OnVolumeCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int VolumeCount { get => (int)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeCount"/> property.</param>
        private void OnVolumeCountPropertyChanged(int oldValue, int newValue)
        {
            DeleteCommand.IsEnabled = newValue == 0 && SymbolicNameCount == 0;
        }

        #endregion
        #region SymbolicNameCount Property Members

        private static readonly DependencyPropertyKey SymbolicNameCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNameCount), typeof(int), typeof(FileSystemItemVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FileSystemItemVM).OnSymbolicNameCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SymbolicNameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNameCountProperty = SymbolicNameCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int SymbolicNameCount { get => (int)GetValue(SymbolicNameCountProperty); private set => SetValue(SymbolicNameCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SymbolicNameCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SymbolicNameCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SymbolicNameCount"/> property.</param>
        private void OnSymbolicNameCountPropertyChanged(int oldValue, int newValue)
        {
            DeleteCommand.IsEnabled = newValue == 0 && VolumeCount == 0;
        }

        #endregion

        internal FileSystemItemVM([DisallowNull] FileSystemsPageVM.EntityAndCounts result)
            : this(result.Entity)
        {
            SymbolicNameCount = result.SymbolicNameCount;
            VolumeCount = result.VolumeCount;
        }

        internal FileSystemItemVM([DisallowNull] FileSystem model)
            : base(model)
        {
            DefaultDriveType = model.DefaultDriveType;
            DisplayName = model.DisplayName;
            IsInactive = model.IsInactive;
            MaxNameLength = model.MaxNameLength;
            Notes = model.Notes;
            IsReadOnly = model.ReadOnly;
        }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property changes.
        /// </summary>
        /// <param name="propertyName">The previous value of the <see cref="Notes"/>.</param>
        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FileSystem.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(FileSystem.DefaultDriveType):
                    Dispatcher.CheckInvoke(() => DefaultDriveType = Model?.DefaultDriveType);
                    break;
                case nameof(FileSystem.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Model?.IsInactive ?? false);
                    break;
                case nameof(FileSystem.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Model?.MaxNameLength ?? DbConstants.DbColDefaultValue_MaxNameLength);
                    break;
                case nameof(FileSystem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(FileSystem.ReadOnly):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly ?? false);
                    break;
            }
        }

        protected override DbSet<FileSystem> GetDbSet(LocalDbContext dbContext) => dbContext.FileSystems;
    }
}
