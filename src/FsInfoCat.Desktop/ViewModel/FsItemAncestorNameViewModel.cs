using System;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FsItemAncestorNameViewModel<TEntity> : DependencyObject
        where TEntity : DbEntity, IDbFsItemAncestorName
    {
        protected internal TEntity Entity { get; }

        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(FsItemAncestorNameViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FsItemAncestorNameViewModel<TEntity>)?.OnNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Name"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Name"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Name"/> property.</param>
        private void OnNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNamePropertyChanged Logic
        }

        #endregion
        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(FsItemAncestorNameViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Path"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Path { get => GetValue(PathProperty) as string; private set => SetValue(PathPropertyKey, value); }

        #endregion

        public FsItemAncestorNameViewModel(TEntity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            WeakPropertyChangedEventRelay.Attach(entity, OnEntityPropertyChanged);
            Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames);
            Name = Entity.Name;
        }

        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args) => OnEntityPropertyChanged(args.PropertyName ?? "");

        protected virtual void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IDbFsItemAncestorName.Name):
                    Dispatcher.CheckInvoke(() => Name = Entity.Name);
                    break;
                case nameof(IDbFsItemAncestorName.AncestorNames):
                    Dispatcher.CheckInvoke(() => Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames));
                    break;
            }
        }
    }
}
