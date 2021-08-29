using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class GPSPropertiesListItemViewModel<TEntity> : GPSPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IGPSProperties
    {
        #region VersionID Property Members

        private static readonly DependencyPropertyKey VersionIDPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VersionID), typeof(string),
            typeof(GPSPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VersionID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionIDProperty = VersionIDPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VersionID { get => GetValue(VersionIDProperty) as string; private set => SetValue(VersionIDPropertyKey, value); }

        #endregion

        public GPSPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            VersionID = entity.VersionID.ToVersionString();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IGPSProperties.VersionID):
                    Dispatcher.CheckInvoke(() => VersionID = Entity.VersionID.ToVersionString());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
