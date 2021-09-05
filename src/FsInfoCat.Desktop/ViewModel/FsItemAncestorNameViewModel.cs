using System;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FsItemAncestorNameViewModel<TEntity> : DependencyObject
        where TEntity : DbEntity, IDbFsItemAncestorName
    {
        protected internal TEntity Entity { get; }

#pragma warning disable IDE0060 // Remove unused parameter
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = ColumnPropertyBuilder<string, FsItemAncestorNameViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IDbFsItemAncestorName.Name))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as FsItemAncestorNameViewModel<TEntity>)?.OnNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = ColumnPropertyBuilder<string, FsItemAncestorNameViewModel<TEntity>>
            .Register(nameof(Path))
            .DefaultValue("")
            .AsReadOnly();

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
#pragma warning restore IDE0060 // Remove unused parameter

        public FsItemAncestorNameViewModel(TEntity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            WeakPropertyChangedEventRelay.Attach(entity, OnEntityPropertyChanged);
            Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames);
            Name = Entity.Name;
        }

        protected virtual void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args) => OnEntityPropertyChanged(args.PropertyName ?? "");

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
