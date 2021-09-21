using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class DbEntityRowViewModel<TEntity> : DependencyObject, IDbEntityRowViewModel<TEntity>
        where TEntity : DbEntity
    {
        private TEntity _entity;
        private readonly WeakPropertyChangedEventRelay _propertyChangedEventRelay;

        protected internal TEntity Entity
        {
            get => _entity;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                Dispatcher.CheckInvoke(() =>
                {
                    TEntity oldValue = _entity;
                    _entity = value;
                    if (ReferenceEquals(_entity, value))
                        return;
                    _propertyChangedEventRelay.Attach(value, true);
                    OnEntityObjectChanged(oldValue, value);
                });
            }
        }

        TEntity IDbEntityRowViewModel<TEntity>.Entity { get => Entity; set => Entity = value; }

        DbEntity IDbEntityRowViewModel.Entity => Entity;

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = ColumnPropertyBuilder<DateTime, DbEntityRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(DbEntity.CreatedOn))
            .DefaultValue(default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="CreatedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the database entity creation date/time.
        /// </summary>
        /// <value>The .</value>
        public DateTime CreatedOn { get => (DateTime)GetValue(CreatedOnProperty); private set => SetValue(CreatedOnPropertyKey, value); }

        #endregion
        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = ColumnPropertyBuilder<DateTime, DbEntityRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(DbEntity.ModifiedOn))
            .DefaultValue(default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ModifiedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the database entity modification date/time.
        /// </summary>
        /// <value>The .</value>
        public DateTime ModifiedOn { get => (DateTime)GetValue(ModifiedOnProperty); private set => SetValue(ModifiedOnPropertyKey, value); }

        #endregion

        protected DbEntityRowViewModel([DisallowNull] TEntity entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _propertyChangedEventRelay = WeakPropertyChangedEventRelay.Attach(entity, OnEntityPropertyChanged);
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
        }

        protected virtual void RejectChanges()
        {
            _entity.RejectChanges();
            CreatedOn = _entity.CreatedOn;
            ModifiedOn = _entity.ModifiedOn;
        }

        protected virtual void OnEntityObjectChanged([DisallowNull] TEntity oldValue, [DisallowNull] TEntity newValue)
        {
            foreach (string n in RevertibleChangeTracking.GetDifferences(oldValue, newValue).ToArray())
                OnEntityPropertyChanged(n);
        }

        protected virtual void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args) => OnEntityPropertyChanged(args.PropertyName ?? "");

        protected virtual void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(CreatedOn):
                    Dispatcher.CheckInvoke(() => CreatedOn = Entity.CreatedOn);
                    break;
                case nameof(ModifiedOn):
                    Dispatcher.CheckInvoke(() => ModifiedOn = Entity.ModifiedOn);
                    break;
            }
        }
    }
}
