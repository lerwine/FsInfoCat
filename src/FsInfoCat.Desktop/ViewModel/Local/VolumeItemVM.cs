using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class VolumeItemVM<TDbEntity> : DbEntityItemVM<TDbEntity>
        where TDbEntity : VolumeRow, new()
    {
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(VolumeItemVM<TDbEntity>), new PropertyMetadata(""));

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
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(VolumeItemVM<TDbEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeName { get => GetValue(VolumeNameProperty) as string; private set => SetValue(VolumeNamePropertyKey, value); }

        #endregion
        #region Identifier Property Members

        private static readonly DependencyPropertyKey IdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Identifier), typeof(VolumeIdentifier), typeof(VolumeItemVM<TDbEntity>),
                new PropertyMetadata(VolumeIdentifier.Empty));

        /// <summary>
        /// Identifies the <see cref="Identifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = IdentifierPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeIdentifier Identifier { get => (VolumeIdentifier)GetValue(IdentifierProperty); private set => SetValue(IdentifierPropertyKey, value); }

        #endregion
        #region Type Property Members

        private static readonly DependencyPropertyKey TypePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Type), typeof(DriveType), typeof(VolumeItemVM<TDbEntity>),
                new PropertyMetadata(DriveType.Unknown));

        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = TypePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DriveType Type { get => (DriveType)GetValue(TypeProperty); private set => SetValue(TypePropertyKey, value); }

        #endregion
        #region Status Property Members

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(VolumeStatus), typeof(VolumeItemVM<TDbEntity>),
                new PropertyMetadata(VolumeStatus.Unknown));

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeStatus Status { get => (VolumeStatus)GetValue(StatusProperty); private set => SetValue(StatusPropertyKey, value); }

        #endregion
        #region IsReadOnly Property Members

        private static readonly DependencyPropertyKey IsReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsReadOnly), typeof(bool?), typeof(VolumeItemVM<TDbEntity>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IsReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = IsReadOnlyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool? IsReadOnly { get => (bool?)GetValue(IsReadOnlyProperty); private set => SetValue(IsReadOnlyPropertyKey, value); }

        #endregion
        #region MaxNameLength Property Members

        private static readonly DependencyPropertyKey MaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLength), typeof(uint?), typeof(VolumeItemVM<TDbEntity>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = MaxNameLengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public uint? MaxNameLength { get => (uint?)GetValue(MaxNameLengthProperty); private set => SetValue(MaxNameLengthPropertyKey, value); }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(VolumeItemVM<TDbEntity>), new PropertyMetadata(""));

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

        protected VolumeItemVM([DisallowNull] TDbEntity model)
            : base(model)
        {
            DisplayName = model.DisplayName;
            Identifier = model.Identifier;
            MaxNameLength = model.MaxNameLength;
            Notes = model.Notes;
            IsReadOnly = model.ReadOnly;
            Status = model.Status;
            Type = model.Type;
            VolumeName = model.VolumeName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VolumeListItemWithFileSystem.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName ?? "");
                    break;
                case nameof(VolumeListItemWithFileSystem.Identifier):
                    Dispatcher.CheckInvoke(() => Identifier = Model?.Identifier ?? VolumeIdentifier.Empty);
                    break;
                case nameof(VolumeListItemWithFileSystem.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Model?.MaxNameLength);
                    break;
                case nameof(VolumeListItemWithFileSystem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes ?? "");
                    break;
                case nameof(VolumeListItemWithFileSystem.ReadOnly):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly);
                    break;
                case nameof(VolumeListItemWithFileSystem.Status):
                    Dispatcher.CheckInvoke(() => Status = Model?.Status ?? VolumeStatus.Unknown);
                    break;
                case nameof(VolumeListItemWithFileSystem.Type):
                    Dispatcher.CheckInvoke(() => Type = Model?.Type ?? DriveType.Unknown);
                    break;
                case nameof(VolumeListItemWithFileSystem.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Model?.VolumeName ?? "");
                    break;
            }
        }
    }

    /// <summary>
    /// View Model for the <see cref="FileSystemsPageVM.Volumes"/> listing in the <see cref="FileSystemsPageVM"/> view model.
    /// </summary>
    public sealed class VolumeItemVM : VolumeItemVM<VolumeListItem>
    {
        #region RootPath Property Members

        private static readonly DependencyPropertyKey RootPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootPath), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = RootPathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string RootPath { get => GetValue(RootPathProperty) as string; private set => SetValue(RootPathPropertyKey, value); }

        #endregion
        #region RootSubdirectoryCount Property Members

        private static readonly DependencyPropertyKey RootSubdirectoryCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootSubdirectoryCount), typeof(long), typeof(VolumeItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeItemVM).OnRootSubdirectoryCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="RootSubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootSubdirectoryCountProperty = RootSubdirectoryCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long RootSubdirectoryCount { get => (long)GetValue(RootSubdirectoryCountProperty); private set => SetValue(RootSubdirectoryCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="RootSubdirectoryCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RootSubdirectoryCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RootSubdirectoryCount"/> property.</param>
        private void OnRootSubdirectoryCountPropertyChanged(long oldValue, long newValue) => CanDelete = newValue == 0L && RootFileCount == 0L;

        #endregion
        #region RootFileCount Property Members

        private static readonly DependencyPropertyKey RootFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootFileCount), typeof(long), typeof(VolumeItemVM),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeItemVM).OnRootFileCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="RootFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootFileCountProperty = RootFileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long RootFileCount { get => (long)GetValue(RootFileCountProperty); private set => SetValue(RootFileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="RootFileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RootFileCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RootFileCount"/> property.</param>
        private void OnRootFileCountPropertyChanged(long oldValue, long newValue) => CanDelete = RootSubdirectoryCount == 0L && newValue == 0L;

        #endregion
        #region CanDelete Property Members

        private static readonly DependencyPropertyKey CanDeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanDelete), typeof(bool), typeof(VolumeItemVM),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="CanDelete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanDeleteProperty = CanDeletePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool CanDelete { get => (bool)GetValue(CanDeleteProperty); private set => SetValue(CanDeletePropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long), typeof(VolumeItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long), typeof(VolumeItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long), typeof(VolumeItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion

        public VolumeItemVM(VolumeListItem model) : base(model)
        {
            RootPath = model.RootPath;
            AccessErrorCount = model.AccessErrorCount;
            PersonalTagCount = model.PersonalTagCount;
            SharedTagCount = model.SharedTagCount;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VolumeListItemWithFileSystem.RootPath):
                    Dispatcher.CheckInvoke(() => RootPath = Model?.RootPath ?? "");
                    break;
                case nameof(VolumeListItemWithFileSystem.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Model?.AccessErrorCount ?? 0L);
                    break;
                case nameof(VolumeListItemWithFileSystem.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Model?.PersonalTagCount ?? 0L);
                    break;
                case nameof(VolumeListItemWithFileSystem.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Model?.SharedTagCount ?? 0L);
                    break;
                default:
                    base.OnNestedModelPropertyChanged(propertyName);
                    break;
            }
        }

        protected override DbSet<VolumeListItem> GetDbSet(LocalDbContext dbContext) => dbContext.VolumeListing;
    }
}
