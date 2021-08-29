using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VideoPropertiesListItemViewModel<TEntity> : VideoPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IVideoProperties
    {
        #region Director Property Members

        private static readonly DependencyPropertyKey DirectorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Director), typeof(string),
            typeof(VideoPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Director"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectorProperty = DirectorPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Director { get => GetValue(DirectorProperty) as string; private set => SetValue(DirectorPropertyKey, value); }

        #endregion

        public VideoPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Director = entity.Director.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IVideoProperties.Director):
                    Dispatcher.CheckInvoke(() => Director = Entity.Director.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
