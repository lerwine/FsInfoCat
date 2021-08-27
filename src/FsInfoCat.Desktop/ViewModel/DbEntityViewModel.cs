using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    public abstract class DbEntityViewModel<TEntity> : DependencyObject, IObserver<PropertyValueChangedEventArgs>
        where TEntity : DbEntity
    {
        protected internal TEntity Entity { get; }
        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(DbEntityViewModel<TEntity>),
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

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(DbEntityViewModel<TEntity>),
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

        protected DbEntityViewModel(TEntity entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
            entity.Subscribe(this);
        }

        public void OnCompleted()
        {
            // TODO: Implement push notification
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            // TODO: Implement push notification
            throw new NotImplementedException();
        }

        public void OnNext(PropertyValueChangedEventArgs value)
        {
            // TODO: Implement push notification
            throw new NotImplementedException();
        }
    }
}
