using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DbEntityRowViewModel<TEntity> : DependencyObject, IDbEntityRowViewModel<TEntity>
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

        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(DbEntityRowViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void RaiseEditCommand(object parameter) => EditCommand?.Invoke(this, new(parameter));

        #endregion
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(DbEntityRowViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
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

        protected DbEntityRowViewModel(TEntity entity)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
            _propertyChangedEventRelay = WeakPropertyChangedEventRelay.Attach(entity, OnEntityPropertyChanged);
        }

        protected virtual void OnEntityObjectChanged([DisallowNull] TEntity oldValue, [DisallowNull] TEntity newValue)
        {
            foreach (string n in RevertibleChangeTracking.GetDifferences(oldValue, newValue).ToArray())
                OnEntityPropertyChanged(n);
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
