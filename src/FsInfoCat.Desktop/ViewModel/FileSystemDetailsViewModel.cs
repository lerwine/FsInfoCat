using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem> : FileSystemRowViewModel<TEntity>
        where TEntity : DbEntity, IFileSystem, IFileSystemRow
        where TVolumeEntity : DbEntity, IVolumeListItem
        where TVolumeItem : VolumeListItemViewModel<TVolumeEntity>
        where TSymbolicNameEntity : DbEntity, ISymbolicNameRow
        where TSymbolicNameItem : SymbolicNameRowViewModel<TSymbolicNameEntity>
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Volumes Property Members

        protected ObservableCollection<TVolumeItem> BackingVolumes { get; } = new();

        private static readonly DependencyPropertyKey VolumesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Volumes),
            typeof(ReadOnlyObservableCollection<TVolumeItem>),
            typeof(FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Volumes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumesProperty = VolumesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TVolumeItem> Volumes => (ReadOnlyObservableCollection<TVolumeItem>)GetValue(VolumesProperty);

        #endregion
        #region SymbolicNames Property Members

        protected ObservableCollection<TSymbolicNameItem> BackingSymbolicNames { get; } = new();

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames),
            typeof(ReadOnlyObservableCollection<TSymbolicNameItem>),
            typeof(FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TSymbolicNameItem> SymbolicNames => (ReadOnlyObservableCollection<TSymbolicNameItem>)GetValue(SymbolicNamesProperty);

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public FileSystemDetailsViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(VolumesPropertyKey, new ReadOnlyObservableCollection<TVolumeItem>(BackingVolumes));
            SetValue(SymbolicNamesPropertyKey, new ReadOnlyObservableCollection<TSymbolicNameItem>(BackingSymbolicNames));
        }
    }
}
