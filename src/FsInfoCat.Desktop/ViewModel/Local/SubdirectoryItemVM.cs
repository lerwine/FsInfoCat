using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SubdirectoryItemVM : DbEntityItemVM<SubdirectoryListItemWithAncestorNames>
    {
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncBgModalVM), typeof(SubdirectoryItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AsyncOps.AsyncBgModalVM BgOps => (AsyncOps.AsyncBgModalVM)GetValue(BgOpsProperty);

        #endregion
        #region FullName Property Members

        private static readonly DependencyPropertyKey FullNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FullName), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="FullName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FullNameProperty = FullNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string FullName { get => GetValue(FullNameProperty) as string; private set => SetValue(FullNamePropertyKey, value); }

        #endregion
        #region Parent Property Members

        private static readonly DependencyPropertyKey ParentPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Parent), typeof(SubdirectoryItemVM), typeof(SubdirectoryItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Parent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentProperty = ParentPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public SubdirectoryItemVM Parent { get => (SubdirectoryItemVM)GetValue(ParentProperty); private set => SetValue(ParentPropertyKey, value); }

        #endregion
        #region Volume Property Members

        private static readonly DependencyPropertyKey VolumePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Volume), typeof(VolumeItemVM), typeof(SubdirectoryItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Volume"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeProperty = VolumePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeItemVM Volume { get => (VolumeItemVM)GetValue(VolumeProperty); private set => SetValue(VolumePropertyKey, value); }

        #endregion
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion
        #region CreationTime Property Members

        private static readonly DependencyPropertyKey CreationTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreationTime), typeof(DateTime), typeof(SubdirectoryItemVM),
                new PropertyMetadata(default(DateTime)  ));

        /// <summary>
        /// Identifies the <see cref="CreationTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreationTimeProperty = CreationTimePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime CreationTime { get => (DateTime)GetValue(CreationTimeProperty); private set => SetValue(CreationTimePropertyKey, value); }

        #endregion
        #region LastAccessed Property Members

        private static readonly DependencyPropertyKey LastAccessedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastAccessed), typeof(DateTime), typeof(SubdirectoryItemVM),
                new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// Identifies the <see cref="LastAccessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAccessedProperty = LastAccessedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastAccessed { get => (DateTime)GetValue(LastAccessedProperty); private set => SetValue(LastAccessedPropertyKey, value); }

        #endregion
        #region LastWriteTime Property Members

        private static readonly DependencyPropertyKey LastWriteTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastWriteTime), typeof(DateTime), typeof(SubdirectoryItemVM),
                new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// Identifies the <see cref="LastWriteTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastWriteTimeProperty = LastWriteTimePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastWriteTime { get => (DateTime)GetValue(LastWriteTimeProperty); private set => SetValue(LastWriteTimePropertyKey, value); }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

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
        #region Options Property Members

        private static readonly DependencyPropertyKey OptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Options), typeof(DirectoryCrawlOptions), typeof(SubdirectoryItemVM),
                new PropertyMetadata(DirectoryCrawlOptions.None));

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = OptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DirectoryCrawlOptions Options { get => (DirectoryCrawlOptions)GetValue(OptionsProperty); private set => SetValue(OptionsPropertyKey, value); }

        #endregion
        #region Status Property Members

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(DirectoryStatus), typeof(SubdirectoryItemVM),
                new PropertyMetadata(DirectoryStatus.Incomplete));

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DirectoryStatus Status { get => (DirectoryStatus)GetValue(StatusProperty); private set => SetValue(StatusPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long), typeof(SubdirectoryItemVM),
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
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long), typeof(SubdirectoryItemVM),
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
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long), typeof(SubdirectoryItemVM),
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
        #region SubdirectoryCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubdirectoryCount), typeof(long), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryCountProperty = SubdirectoryCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SubdirectoryCount { get => (long)GetValue(SubdirectoryCountProperty); private set => SetValue(SubdirectoryCountPropertyKey, value); }

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(long), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileCount { get => (long)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        #endregion
        #region VolumeDisplayName Property Members

        private static readonly DependencyPropertyKey VolumeDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeDisplayName), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeDisplayNameProperty = VolumeDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeDisplayName { get => GetValue(VolumeDisplayNameProperty) as string; private set => SetValue(VolumeDisplayNamePropertyKey, value); }

        #endregion
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

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
        #region VolumeIdentifier Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeIdentifier), typeof(VolumeIdentifier), typeof(SubdirectoryItemVM),
                new PropertyMetadata(VolumeIdentifier.Empty));

        /// <summary>
        /// Identifies the <see cref="VolumeIdentifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeIdentifierProperty = VolumeIdentifierPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeIdentifier VolumeIdentifier { get => (VolumeIdentifier)GetValue(VolumeIdentifierProperty); private set => SetValue(VolumeIdentifierPropertyKey, value); }

        #endregion
        #region CrawlConfigDisplayName Property Members

        private static readonly DependencyPropertyKey CrawlConfigDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfigDisplayName), typeof(string), typeof(SubdirectoryItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="CrawlConfigDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlConfigDisplayNameProperty = CrawlConfigDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CrawlConfigDisplayName { get => GetValue(CrawlConfigDisplayNameProperty) as string; private set => SetValue(CrawlConfigDisplayNamePropertyKey, value); }

        #endregion

        public SubdirectoryItemVM(SubdirectoryListItemWithAncestorNames entity) : base(entity)
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
            Name = entity.Name;
            CreationTime = entity.CreationTime;
            LastAccessed = entity.LastAccessed;
            LastWriteTime = entity.LastWriteTime;
            Notes = entity.Notes;
            Options = entity.Options;
            Status = entity.Status;
            SharedTagCount = entity.SharedTagCount;
            PersonalTagCount = entity.PersonalTagCount;
            AccessErrorCount = entity.AccessErrorCount;
            SubdirectoryCount = entity.SubdirectoryCount;
            FileCount = entity.FileCount;
            CrawlConfigDisplayName = entity.CrawlConfigDisplayName;
        }

        private async Task<string> LookupFullNameAsync(Subdirectory root, IWindowsStatusListener statusListener)
        {
            string fullName = await root.GetFullNameAsync(statusListener.CancellationToken);
            Dispatcher.Invoke(() => OnLookupFullNameComplete(fullName ?? ""));
            return fullName;
        }

        private void OnLookupFullNameComplete(string result)
        {
            if (FullName.Equals(result))
                return;
            FullName = result;
        }

        protected override DbSet<SubdirectoryListItemWithAncestorNames> GetDbSet(LocalDbContext dbContext) => dbContext.SubdirectoryListingWithAncestorNames;

        public static string FromAncestorNames(string ancestorNames)
        {
            if (string.IsNullOrEmpty(ancestorNames))
                return "";
            string[] segments = ancestorNames.Split("/").Where(n => n.Length > 0).Reverse().ToArray();
            if (segments.Length > 0)
            {
                if (segments[0].EndsWith("\"") || segments[0].EndsWith("/"))
                    segments[0] = segments[0][0..^1];
                return string.Join('\\', segments);
            }
            return segments[0];
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(SubdirectoryListItemWithAncestorNames.AccessErrorCount):
                    AccessErrorCount = Model?.AccessErrorCount ?? 0L;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.FileCount):
                    FileCount = Model?.FileCount ?? 0L;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.PersonalTagCount):
                    PersonalTagCount = Model?.PersonalTagCount ?? 0L;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.SharedTagCount):
                    SharedTagCount = Model?.SharedTagCount ?? 0L;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.SubdirectoryCount):
                    AccessErrorCount = Model?.AccessErrorCount ?? 0L;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.AncestorNames):
                    FullName = FromAncestorNames(Model?.AncestorNames);
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.CreationTime):
                    CreationTime = Model?.CreationTime ?? DateTime.Now;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.LastAccessed):
                    CreationTime = Model?.CreationTime ?? DateTime.Now;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.LastWriteTime):
                    CreationTime = Model?.CreationTime ?? DateTime.Now;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.Name):
                    Name = Model?.Name ?? "";
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.Notes):
                    Notes = Model?.Notes ?? "";
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.Options):
                    Options = Model?.Options ?? DirectoryCrawlOptions.None;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.Status):
                    Status = Model?.Status ?? DirectoryStatus.Incomplete;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.VolumeDisplayName):
                    VolumeDisplayName = Model?.VolumeDisplayName ?? "";
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.VolumeIdentifier):
                    VolumeIdentifier = Model?.VolumeIdentifier ?? VolumeIdentifier.Empty;
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.VolumeName):
                    VolumeName = Model?.VolumeName ?? "";
                    break;
                case nameof(SubdirectoryListItemWithAncestorNames.CrawlConfigDisplayName):
                    CrawlConfigDisplayName = Model?.CrawlConfigDisplayName ?? "";
                    break;
            }
        }
    }
}
