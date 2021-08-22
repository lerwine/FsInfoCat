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
    public class SubdirectoryItemVM : DbEntityItemVM<Subdirectory>
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
        #region CrawlConfig Property Members

        private static readonly DependencyPropertyKey CrawlConfigPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfig), typeof(CrawlConfigItemVM), typeof(SubdirectoryItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CrawlConfig"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlConfigProperty = CrawlConfigPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public CrawlConfigItemVM CrawlConfig { get => (CrawlConfigItemVM)GetValue(CrawlConfigProperty); private set => SetValue(CrawlConfigPropertyKey, value); }

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

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(int), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int SharedTagCount { get => (int)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(int), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int PersonalTagCount { get => (int)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(int), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int AccessErrorCount { get => (int)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region SubDirectoryCount Property Members

        private static readonly DependencyPropertyKey SubDirectoryCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubDirectoryCount), typeof(int), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="SubDirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubDirectoryCountProperty = SubDirectoryCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int SubDirectoryCount { get => (int)GetValue(SubDirectoryCountProperty); private set => SetValue(SubDirectoryCountPropertyKey, value); }

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(int), typeof(SubdirectoryItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int FileCount { get => (int)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        #endregion

        public SubdirectoryItemVM(Subdirectory entity, AsyncOps.AsyncBgModalVM bgOpMgr = null) : base(entity)
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
            (bgOpMgr ?? BgOps).FromAsync("", "", entity, LookupFullNameAsync).ContinueWith(task => CrawlConfig = entity.CrawlConfiguration.ToItemViewModel(bgOpMgr ?? BgOps))
                .ContinueWith(task =>
                {
                    Subdirectory parent = entity.Parent;
                    if (parent is null)
                        Volume = entity.Volume?.ToItemViewModel(bgOpMgr ?? BgOps);
                    else
                        Parent = entity.Parent.ToItemViewModel(bgOpMgr ?? BgOps);
                });
            Name = entity.Name;
            CreationTime = entity.CreationTime;
            LastAccessed = entity.LastAccessed;
            LastWriteTime = entity.LastWriteTime;
            Notes = entity.Notes;
            Options = entity.Options;
            Status = entity.Status;
            SharedTagCount = entity.SharedTags.Count();
            PersonalTagCount = entity.PersonalTags.Count();
            AccessErrorCount = entity.AccessErrors.Count();
            SubDirectoryCount = entity.SubDirectories.Count();
            FileCount = entity.Files.Count();
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

        protected override DbSet<Subdirectory> GetDbSet(LocalDbContext dbContext) => dbContext.Subdirectories;

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISubdirectory.CrawlConfiguration):
                    CrawlConfig = Model?.CrawlConfiguration.ToItemViewModel(BgOps);
                    break;
                case nameof(ISubdirectory.Parent):
                    if (Model?.Parent is not null)
                    {
                        Parent = Model?.Parent?.ToItemViewModel(BgOps);
                        Volume = null;
                    }
                    break;
                case nameof(ISubdirectory.Volume):
                    if (Model?.Volume is not null)
                    {
                        Volume = Model?.Volume?.ToItemViewModel(BgOps);
                        Parent = null;
                    }
                    break;

            }    
            //(bgOpMgr ?? BgOps).FromAsync("", "", entity, LookupFullNameAsync).ContinueWith(task => CrawlConfig = entity.CrawlConfiguration.ToItemViewModel(bgOpMgr ?? BgOps))
            //    .ContinueWith(task =>
            //    {
            //        Subdirectory parent = entity.Parent;
            //        if (parent is null)
            //            Volume = entity.Volume?.ToItemViewModel(bgOpMgr ?? BgOps);
            //        else
            //            Parent = entity.Parent.ToItemViewModel(bgOpMgr ?? BgOps);
            //    });
            //Name = entity.Name;
            //CreationTime = entity.CreationTime;
            //LastAccessed = entity.LastAccessed;
            //LastWriteTime = entity.LastWriteTime;
            //Notes = entity.Notes;
            //Options = entity.Options;
            //Status = entity.Status;
            //SharedTagCount = entity.SharedTags.Count();
            //PersonalTagCount = entity.PersonalTags.Count();
            //AccessErrorCount = entity.AccessErrors.Count();
            //SubDirectoryCount = entity.SubDirectories.Count();
            //FileCount = entity.Files.Count();
            throw new NotImplementedException();
        }
    }
}
