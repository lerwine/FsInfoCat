using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel> : SymbolicNameRowViewModel<TEntity>
        where TEntity : DbEntity, ISymbolicName
        where TFileSystemEntity : DbEntity, IFileSystemRow
        where TFileSystemModel : FileSystemRowViewModel<TFileSystemEntity>
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region FileSystem Property Members

        /// <summary>
        /// Identifies the <see cref="FileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemProperty = DependencyProperty.Register(nameof(FileSystem), typeof(TFileSystemModel),
            typeof(SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel>), new PropertyMetadata(null,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel>)?.OnFileSystemPropertyChanged((TFileSystemModel)e.OldValue,
                    (TFileSystemModel)e.NewValue)));

        public TFileSystemModel FileSystem { get => (TFileSystemModel)GetValue(FileSystemProperty); set => SetValue(FileSystemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystem"/> property.</param>
        private void OnFileSystemPropertyChanged(TFileSystemModel oldValue, TFileSystemModel newValue) { }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public SymbolicNameDetailsViewModel([DisallowNull] TEntity entity, TFileSystemModel fileSystem) : base(entity)
        {
            FileSystem = fileSystem;
        }
    }
}
