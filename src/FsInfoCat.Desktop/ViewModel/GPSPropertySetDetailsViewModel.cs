using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class GPSPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : GPSPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IGPSPropertySet, IGPSProperties
        where TFileEntity : DbEntity, IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Files),
            typeof(ReadOnlyObservableCollection<TFileItem>),
            typeof(GPSPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>), new PropertyMetadata(null));

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

        public GPSPropertySetDetailsViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
        }
    }
}
