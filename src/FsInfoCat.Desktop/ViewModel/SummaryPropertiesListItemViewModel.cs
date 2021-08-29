using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertiesListItemViewModel<TEntity> : SummaryPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, ISummaryProperties
    {
        #region Author Property Members

        private static readonly DependencyPropertyKey AuthorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Author), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Author"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AuthorProperty = AuthorPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Author { get => GetValue(AuthorProperty) as string; private set => SetValue(AuthorPropertyKey, value); }

        #endregion
        #region Keywords Property Members

        private static readonly DependencyPropertyKey KeywordsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Keywords), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Keywords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeywordsProperty = KeywordsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Keywords { get => GetValue(KeywordsProperty) as string; private set => SetValue(KeywordsPropertyKey, value); }

        #endregion
        #region ItemAuthors Property Members

        private static readonly DependencyPropertyKey ItemAuthorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemAuthors), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="ItemAuthors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAuthorsProperty = ItemAuthorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ItemAuthors { get => GetValue(ItemAuthorsProperty) as string; private set => SetValue(ItemAuthorsPropertyKey, value); }

        #endregion
        #region Kind Property Members

        private static readonly DependencyPropertyKey KindPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Kind), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Kind"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = KindPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Kind { get => GetValue(KindProperty) as string; private set => SetValue(KindPropertyKey, value); }

        #endregion

        public SummaryPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Author = entity.Author.ToNormalizedDelimitedText();
            Keywords = entity.Keywords.ToNormalizedDelimitedText();
            ItemAuthors = entity.ItemAuthors.ToNormalizedDelimitedText();
            Kind = entity.Kind.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISummaryProperties.Author):
                    Dispatcher.CheckInvoke(() => Author = Entity.Author.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.Keywords):
                    Dispatcher.CheckInvoke(() => Keywords = Entity.Keywords.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.ItemAuthors):
                    Dispatcher.CheckInvoke(() => ItemAuthors = Entity.ItemAuthors.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.Kind):
                    Dispatcher.CheckInvoke(() => Kind = Entity.Kind.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
