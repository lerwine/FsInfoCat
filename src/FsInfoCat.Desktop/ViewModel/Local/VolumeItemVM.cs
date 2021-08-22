using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumeItemVM : DbEntityItemVM<Volume>, IHasSubdirectoryEntity
    {
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

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
        #region FileSystem Property Members

        private static readonly DependencyPropertyKey FileSystemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystem), typeof(FileSystem), typeof(VolumeItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="FileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemProperty = FileSystemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public FileSystem FileSystem { get => (FileSystem)GetValue(FileSystemProperty); private set => SetValue(FileSystemPropertyKey, value); }

        #endregion
        #region Identifier Property Members

        private static readonly DependencyPropertyKey IdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Identifier), typeof(VolumeIdentifier), typeof(VolumeItemVM),
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
        #region MaxNameLength Property Members

        private static readonly DependencyPropertyKey MaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxNameLength), typeof(uint?), typeof(VolumeItemVM),
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

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

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

        private static readonly DependencyPropertyKey IsReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsReadOnly), typeof(bool?), typeof(VolumeItemVM),
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
        #region RootDirectory Property Members

        private static readonly DependencyPropertyKey RootDirectoryPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootDirectory), typeof(SubdirectoryItemVM), typeof(VolumeItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RootDirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootDirectoryProperty = RootDirectoryPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public SubdirectoryItemVM RootDirectory { get => (SubdirectoryItemVM)GetValue(RootDirectoryProperty); private set => SetValue(RootDirectoryPropertyKey, value); }

        #endregion
        #region Status Property Members

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(VolumeStatus), typeof(VolumeItemVM),
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
        #region Type Property Members

        private static readonly DependencyPropertyKey TypePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Type), typeof(DriveType), typeof(VolumeItemVM),
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
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(VolumeItemVM), new PropertyMetadata(""));

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

        internal VolumeItemVM([DisallowNull] Volume model)
            : base(model)
        {
            DisplayName = model.DisplayName;
            FileSystem = model.FileSystem;
            Identifier = model.Identifier;
            MaxNameLength = model.MaxNameLength;
            Notes = model.Notes;
            IsReadOnly = model.ReadOnly;
            RootDirectory = model.RootDirectory.ToItemViewModel();
            Status = model.Status;
            Type = model.Type;
            VolumeName = model.VolumeName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Volume.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(Volume.FileSystem):
                    Dispatcher.CheckInvoke(() => FileSystem = Model?.FileSystem);
                    break;
                case nameof(Volume.Identifier):
                    Dispatcher.CheckInvoke(() => Identifier = Model?.Identifier ?? VolumeIdentifier.Empty);
                    break;
                case nameof(Volume.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Model?.MaxNameLength);
                    break;
                case nameof(Volume.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(Volume.ReadOnly):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly);
                    break;
                case nameof(Volume.RootDirectory):
                    Dispatcher.CheckInvoke(() => RootDirectory = Model?.RootDirectory?.ToItemViewModel());
                    break;
                case nameof(Volume.Status):
                    Dispatcher.CheckInvoke(() => Status = Model?.Status ?? VolumeStatus.Unknown);
                    break;
                case nameof(Volume.Type):
                    Dispatcher.CheckInvoke(() => Type = Model?.Type ?? DriveType.Unknown);
                    break;
                case nameof(Volume.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Model?.VolumeName);
                    break;
            }
        }

        ISimpleIdentityReference<Subdirectory> IHasSubdirectoryEntity.GetSubdirectoryEntity() => CheckAccess() ? Model?.RootDirectory : Dispatcher.Invoke(() => Model?.RootDirectory);

        protected override DbSet<Volume> GetDbSet(LocalDbContext dbContext) => dbContext.Volumes;

        public async Task<ISimpleIdentityReference<Subdirectory>> GetSubdirectoryEntityAsync([DisallowNull] IWindowsStatusListener statusListener)
        {
            return (await Dispatcher.InvokeAsync(() => Model))?.RootDirectory;
        }
    }
}
