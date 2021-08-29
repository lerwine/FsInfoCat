using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DocumentPropertiesListItemViewModel<TEntity> : DocumentPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IDocumentProperties
    {
        #region Contributor Property Members

        private static readonly DependencyPropertyKey ContributorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Contributor), typeof(string),
            typeof(DocumentPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Contributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContributorProperty = ContributorPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Contributor { get => GetValue(ContributorProperty) as string; private set => SetValue(ContributorPropertyKey, value); }

        #endregion

        public DocumentPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Contributor = Entity.Contributor.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IDocumentProperties.Contributor):
                    Dispatcher.CheckInvoke(() => Contributor = Entity.Contributor.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
