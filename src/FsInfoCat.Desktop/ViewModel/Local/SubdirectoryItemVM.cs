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
        public CrawlConfigItemVM CrawlConfiguration { get; private set; }
        public SubdirectoryItemVM Parent { get; private set; }
        public VolumeItemVM Volume { get; private set; }
        public string Name { get; }
        public DateTime CreationTime { get; }
        public DateTime LastAccessed { get; }
        public DateTime LastWriteTime { get; }
        public string Notes { get; }
        public DirectoryCrawlOptions Options { get; }
        public DirectoryStatus Status { get; }
        public int SharedTagCount { get; }
        public int PersonalTagCount { get; }
        public int AccessErrorCount { get; }
        public int SubDirectoryCount { get; }
        public int FileCount { get; }

        #endregion

        public SubdirectoryItemVM(Subdirectory entity, AsyncOps.AsyncBgModalVM bgOpMgr = null) : base(entity)
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
            (bgOpMgr ?? BgOps).FromAsync("", "", entity, LookupFullNameAsync).ContinueWith(task => CrawlConfiguration = entity.CrawlConfiguration.ToItemViewModel(bgOpMgr ?? BgOps))
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
            string fullName = await Subdirectory.LookupFullNameAsync(root, statusListener.CancellationToken);
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
            throw new NotImplementedException();
        }
    }
}
