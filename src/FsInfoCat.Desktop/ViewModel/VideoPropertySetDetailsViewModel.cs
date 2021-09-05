using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VideoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : VideoPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IVideoPropertySet, IVideoProperties
        where TFileEntity : DbEntity, IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, VideoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVideoPropertySet.Files))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Files"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilesProperty = FilesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<TFileItem> Files => (ReadOnlyObservableCollection<TFileItem>)GetValue(FilesProperty);

        #endregion

        public VideoPropertySetDetailsViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
        }
    }
}
