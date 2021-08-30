using FsInfoCat.Collections;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DbEntityRowViewModel<TEntity> : DependencyObject
        where TEntity : DbEntity
    {
        private TEntity _entity;
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

        protected virtual void OnEntityObjectChanged([DisallowNull] TEntity oldValue, [DisallowNull] TEntity newValue)
        {
            foreach (string n in RevertibleChangeTracking.GetDifferences(oldValue, newValue).ToArray())
                OnEntityPropertyChanged(n);
        }

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(DbEntityRowViewModel<TEntity>),
                new PropertyMetadata(default(DateTime)));

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

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(DbEntityRowViewModel<TEntity>),
                new PropertyMetadata(default(DateTime)));

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

        private readonly WeakPropertyChangedEventRelay _propertyChangedEventRelay;
        protected DbEntityRowViewModel(TEntity entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
            _propertyChangedEventRelay = WeakPropertyChangedEventRelay.Attach(entity, OnEntityPropertyChanged);
        }

        protected void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args) => OnEntityPropertyChanged(args.PropertyName ?? "");

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
