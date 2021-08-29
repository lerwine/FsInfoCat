using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class PhotoPropertiesListItemViewModel<TEntity> : PhotoPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IPhotoProperties
    {
        #region Event Property Members

        private static readonly DependencyPropertyKey EventPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Event), typeof(string),
            typeof(PhotoPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Event"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventProperty = EventPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Event { get => GetValue(EventProperty) as string; private set => SetValue(EventPropertyKey, value); }

        #endregion
        #region PeopleNames Property Members

        private static readonly DependencyPropertyKey PeopleNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PeopleNames), typeof(string),
            typeof(PhotoPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="PeopleNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeopleNamesProperty = PeopleNamesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string PeopleNames { get => GetValue(PeopleNamesProperty) as string; private set => SetValue(PeopleNamesPropertyKey, value); }

        #endregion

        public PhotoPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Event = entity.Event.ToNormalizedDelimitedText();
            PeopleNames = entity.PeopleNames.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IPhotoProperties.Event):
                    Dispatcher.CheckInvoke(() => Event = Entity.Event.ToNormalizedDelimitedText());
                    break;
                case nameof(IPhotoProperties.PeopleNames):
                    Dispatcher.CheckInvoke(() => PeopleNames = Entity.PeopleNames.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
