using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SymbolicNameItemVM : DbEntityItemVM<SymbolicNameListItem>
    {
        #region IsInactive Property Members

        private static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInactive), typeof(bool), typeof(SymbolicNameItemVM),
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
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

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
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

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
        #region Priority Property Members

        private static readonly DependencyPropertyKey PriorityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Priority), typeof(int), typeof(SymbolicNameItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="Priority"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PriorityProperty = PriorityPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int Priority { get => (int)GetValue(PriorityProperty); private set => SetValue(PriorityPropertyKey, value); }

        #endregion
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string), typeof(SymbolicNameItemVM), new PropertyMetadata(""));

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

        public SymbolicNameItemVM([DisallowNull] SymbolicNameListItem model) : base(model)
        {
            Name = model.Name;
            IsInactive = model.IsInactive;
            Notes = model.Notes;
            Priority = model.Priority;
            FileSystemDisplayName = model.FileSystemDisplayName;
        }

        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(SymbolicNameListItem.FileSystemDisplayName):
                    Dispatcher.CheckInvoke(() => FileSystemDisplayName = Model?.FileSystemDisplayName ?? "");
                    break;
                case nameof(SymbolicNameListItem.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Model?.IsInactive ?? false);
                    break;
                case nameof(SymbolicNameListItem.Name):
                    Dispatcher.CheckInvoke(() => Name = Model?.Name);
                    break;
                case nameof(SymbolicNameListItem.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model?.Notes);
                    break;
                case nameof(SymbolicNameListItem.Priority):
                    Dispatcher.CheckInvoke(() => Priority = Model?.Priority ?? 0);
                    break;
            }
        }

        protected override DbSet<SymbolicNameListItem> GetDbSet(LocalDbContext dbContext) => dbContext.SymbolicNameListing;
    }
}
