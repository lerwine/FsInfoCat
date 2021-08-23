using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumeItemVM : DbEntityItemVM<VolumeListItem>
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

        internal VolumeItemVM([DisallowNull] VolumeListItem model)
            : base(model)
        {
            // TODO: Use bgOpMgr to initialize or remove it
            DisplayName = model.DisplayName;
            Identifier = model.Identifier;
            MaxNameLength = model.MaxNameLength;
            Notes = model.Notes;
            IsReadOnly = model.ReadOnly;
            RootPath = model.RootPath;
            AccessErrorCount = model.AccessErrorCount;
            PersonalTagCount = model.PersonalTagCount;
            SharedTagCount = model.SharedTagCount;
            Status = model.Status;
            Type = model.Type;
            VolumeName = model.VolumeName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VolumeListItem.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Model?.DisplayName);
                    break;
                case nameof(VolumeListItem.Identifier):
                    Dispatcher.CheckInvoke(() => Identifier = Model?.Identifier ?? VolumeIdentifier.Empty);
                    break;
                case nameof(VolumeListItem.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Model?.MaxNameLength);
                    break;
                case nameof(VolumeListItem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(VolumeListItem.ReadOnly):
                    Dispatcher.CheckInvoke(() => IsReadOnly = Model?.ReadOnly);
                    break;
                case nameof(VolumeListItem.RootPath):
                    Dispatcher.CheckInvoke(() => RootPath = Model?.RootPath ?? "");
                    break;
                case nameof(VolumeListItem.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Model?.AccessErrorCount ?? 0L);
                    break;
                case nameof(VolumeListItem.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Model?.PersonalTagCount ?? 0L);
                    break;
                case nameof(VolumeListItem.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Model?.SharedTagCount ?? 0L);
                    break;
                case nameof(VolumeListItem.Status):
                    Dispatcher.CheckInvoke(() => Status = Model?.Status ?? VolumeStatus.Unknown);
                    break;
                case nameof(VolumeListItem.Type):
                    Dispatcher.CheckInvoke(() => Type = Model?.Type ?? DriveType.Unknown);
                    break;
                case nameof(VolumeListItem.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Model?.VolumeName);
                    break;
            }
        }

        protected override DbSet<VolumeListItem> GetDbSet(LocalDbContext dbContext) => dbContext.VolumeListing;
    }
}
