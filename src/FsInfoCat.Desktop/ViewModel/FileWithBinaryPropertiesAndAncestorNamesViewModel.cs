using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity> : FileWithAncestorNamesViewModel<TEntity>, IFileWithBinaryPropertiesAndAncestorNamesViewModel
        where TEntity : Model.DbEntity, Model.IFileListItemWithBinaryPropertiesAndAncestorNames
    {
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = ColumnPropertyBuilder<long, FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileListItemWithBinaryPropertiesAndAncestorNames.Length))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        public long Length { get => (long)GetValue(LengthProperty); private set => SetValue(LengthPropertyKey, value); }

        #endregion
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = ColumnPropertyBuilder<Model.MD5Hash?, FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileListItemWithBinaryPropertiesAndAncestorNames.Hash))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = HashPropertyKey.DependencyProperty;

        public Model.MD5Hash? Hash { get => (Model.MD5Hash?)GetValue(HashProperty); private set => SetValue(HashPropertyKey, value); }

        #endregion

        Model.IFileListItemWithBinaryPropertiesAndAncestorNames IFileWithBinaryPropertiesAndAncestorNamesViewModel.Entity => Entity;

        public FileWithBinaryPropertiesAndAncestorNamesViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Length = entity.Length;
            Hash = entity.Hash;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.IFileListItemWithBinaryPropertiesAndAncestorNames.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                case nameof(Model.IFileListItemWithBinaryPropertiesAndAncestorNames.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
