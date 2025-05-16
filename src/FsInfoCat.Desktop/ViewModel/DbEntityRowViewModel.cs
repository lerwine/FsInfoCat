using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class DbEntityRowViewModel<TEntity> : DependencyObject, IDbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity
    {
        private TEntity _entity;

        protected internal TEntity Entity
        {
            get => _entity;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (ReferenceEquals(_entity, value))
                    return;
                TEntity oldValue = _entity;
                _entity = value;
                Dispatcher.CheckInvoke(() => OnEntityObjectChanged(oldValue, value));
            }
        }

        TEntity IDbEntityRowViewModel<TEntity>.Entity { get => Entity; set => Entity = value; }

        Model.DbEntity IDbEntityRowViewModel.Entity => Entity;

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = ColumnPropertyBuilder<DateTime, DbEntityRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.DbEntity.CreatedOn))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.DbEntity.ModifiedOn))
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
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
        }

        [Obsolete("Not using change tracking")]
        protected virtual void RejectChanges()
        {
            CreatedOn = _entity.CreatedOn;
            ModifiedOn = _entity.ModifiedOn;
        }

        protected virtual void OnEntityObjectChanged([DisallowNull] TEntity oldValue, [DisallowNull] TEntity newValue)
        {
            CreatedOn = newValue.CreatedOn;
            ModifiedOn = newValue.ModifiedOn;
        }

        [Obsolete("Not using change tracking")]
        protected virtual void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args) => OnEntityPropertyChanged(args.PropertyName ?? "");

        [Obsolete("Not using change tracking")]
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
