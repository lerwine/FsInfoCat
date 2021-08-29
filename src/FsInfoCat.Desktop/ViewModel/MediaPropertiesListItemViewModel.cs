using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MediaPropertiesListItemViewModel<TEntity> : MediaPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IMediaProperties
    {
        #region Producer Property Members

        private static readonly DependencyPropertyKey ProducerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Producer), typeof(string), typeof(MediaPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Producer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProducerProperty = ProducerPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Producer { get => GetValue(ProducerProperty) as string; private set => SetValue(ProducerPropertyKey, value); }

        #endregion
        #region Writer Property Members

        private static readonly DependencyPropertyKey WriterPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Writer), typeof(string), typeof(MediaPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Writer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WriterProperty = WriterPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Writer { get => GetValue(WriterProperty) as string; private set => SetValue(WriterPropertyKey, value); }

        #endregion

        public MediaPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Writer = entity.Writer.ToNormalizedDelimitedText();
            Producer = entity.Producer.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMediaProperties.Producer):
                    Dispatcher.CheckInvoke(() => Producer = Entity.Producer.ToNormalizedDelimitedText());
                    break;
                case nameof(IMediaProperties.Writer):
                    Dispatcher.CheckInvoke(() => Writer = Entity.Writer.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
