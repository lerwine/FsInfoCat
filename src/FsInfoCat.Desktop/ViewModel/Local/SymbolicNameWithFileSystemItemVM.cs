using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View model for the <see cref="FileSystemsPageVM.SymbolicNames"/> collection in the <see cref="FileSystemsPageVM"/> view model.
    /// </summary>
    public class SymbolicNameItemVM : SymbolicNameRowItemVM<SymbolicName>
    {
        public SymbolicNameItemVM(SymbolicName model) : base(model) { }

        protected override DbSet<SymbolicName> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNames;
    }

    /// <summary>
    /// View model for <see cref="DbEntityListingPageVM{TDbEntity}.Items"/> in the <see cref="SymbolicNamesPageVM"/> view model.
    /// </summary>
    public class SymbolicNameWithFileSystemItemVM : SymbolicNameRowItemVM<SymbolicNameListItem>
    {
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string), typeof(SymbolicNameWithFileSystemItemVM), new PropertyMetadata(""));

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

        public SymbolicNameWithFileSystemItemVM([DisallowNull] SymbolicNameListItem model) : base(model)
        {
            FileSystemDisplayName = model.FileSystemDisplayName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(FileSystemDisplayName))
                Dispatcher.CheckInvoke(() => FileSystemDisplayName = Model?.FileSystemDisplayName ?? "");
            else
                base.OnNestedModelPropertyChanged(propertyName);
        }

        protected override DbSet<SymbolicNameListItem> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNameListing;
    }
}
