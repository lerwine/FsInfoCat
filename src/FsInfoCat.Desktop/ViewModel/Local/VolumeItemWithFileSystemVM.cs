using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="DbEntityListingPageVM{TDbEntity, TItemVM}.Items"/> in the <see cref="VolumesPageVM"/> view model.
    /// </summary>
    public sealed class VolumeItemWithFileSystemVM : VolumeItemVM<VolumeListItemWithFileSystem>
    {
        #region RootPath Property Members

        private static readonly DependencyPropertyKey RootPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootPath), typeof(string), typeof(VolumeItemWithFileSystemVM), new PropertyMetadata(""));

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
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string), typeof(VolumeItemWithFileSystemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        #endregion
        #region EffectiveReadOnly Property Members

        private static readonly DependencyPropertyKey EffectiveReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EffectiveReadOnly), typeof(bool), typeof(VolumeItemWithFileSystemVM),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="EffectiveReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveReadOnlyProperty = EffectiveReadOnlyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool EffectiveReadOnly { get => (bool)GetValue(EffectiveReadOnlyProperty); private set => SetValue(EffectiveReadOnlyPropertyKey, value); }

        #endregion
        #region EffectiveMaxNameLength Property Members

        private static readonly DependencyPropertyKey EffectiveMaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EffectiveMaxNameLength), typeof(uint), typeof(VolumeItemWithFileSystemVM),
                new PropertyMetadata(0u));

        /// <summary>
        /// Identifies the <see cref="EffectiveMaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveMaxNameLengthProperty = EffectiveMaxNameLengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public uint EffectiveMaxNameLength { get => (uint)GetValue(EffectiveMaxNameLengthProperty); private set => SetValue(EffectiveMaxNameLengthPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long), typeof(VolumeItemWithFileSystemVM),
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

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long), typeof(VolumeItemWithFileSystemVM),
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

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long), typeof(VolumeItemWithFileSystemVM),
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

        internal VolumeItemWithFileSystemVM([DisallowNull] VolumeListItemWithFileSystem model)
            : base(model)
        {
            RootPath = model.RootPath;
            AccessErrorCount = model.AccessErrorCount;
            PersonalTagCount = model.PersonalTagCount;
            SharedTagCount = model.SharedTagCount;
            FileSystemDisplayName = model.FileSystemDisplayName;
            EffectiveReadOnly = model.EffectiveReadOnly;
            EffectiveMaxNameLength = model.EffectiveMaxNameLength;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VolumeListItemWithFileSystem.FileSystemDisplayName):
                    Dispatcher.CheckInvoke(() => FileSystemDisplayName = Model?.FileSystemDisplayName ?? "");
                    break;
                case nameof(VolumeListItemWithFileSystem.EffectiveReadOnly):
                    Dispatcher.CheckInvoke(() => EffectiveReadOnly = Model?.EffectiveReadOnly ?? false);
                    break;
                case nameof(VolumeListItemWithFileSystem.EffectiveMaxNameLength):
                    Dispatcher.CheckInvoke(() => EffectiveMaxNameLength = Model?.EffectiveMaxNameLength ?? 0u);
                    break;
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

        protected override DbSet<VolumeListItemWithFileSystem> GetDbSet(LocalDbContext dbContext) => dbContext.VolumeListingWithFileSystem;
    }
}
