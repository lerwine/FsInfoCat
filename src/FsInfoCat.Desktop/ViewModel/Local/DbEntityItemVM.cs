using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class DbEntityItemVM<TDbEntity> : DependencyObject
        where TDbEntity : LocalDbEntity, new()
    {
        #region Command Members

        #region  Property Members

        /// <summary>
        /// Occurs when the <see cref="EditCommand">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> Edit;

        private static readonly DependencyPropertyKey EditCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditCommand),
            typeof(Commands.RelayCommand), typeof(DbEntityItemVM<TDbEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditCommandProperty = EditCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand EditCommand => (Commands.RelayCommand)GetValue(EditCommandProperty);

        /// <summary>
        /// Called when the <see cref="EditCommand">Edit Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="EditCommand" />.</param>
        protected virtual void OnEdit(object parameter) => Edit?.Invoke(this, new(parameter));

        #endregion
        #region  Property Members

        /// <summary>
        /// Occurs when the <see cref="DeleteCommand">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> Delete;

        private static readonly DependencyPropertyKey DeleteCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteCommand),
            typeof(Commands.RelayCommand), typeof(DbEntityItemVM<TDbEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteCommandProperty = DeleteCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand DeleteCommand => (Commands.RelayCommand)GetValue(DeleteCommandProperty);

        /// <summary>
        /// Called when the <see cref="DeleteCommand">Delete Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DeleteCommand" />.</param>
        protected virtual void OnDelete(object parameter) => Delete?.Invoke(this, new(parameter));

        #endregion

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(DbEntityItemVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion
        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(DbEntityItemVM<TDbEntity>),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion
        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(DbEntityItemVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion
        #region Model Property Members

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(TDbEntity), typeof(DbEntityItemVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public TDbEntity Model
        {
            get => (TDbEntity)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        #endregion

        protected DbEntityItemVM([DisallowNull] TDbEntity model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            model.PropertyChanged += Model_PropertyChanged;
            LastSynchronizedOn = model.LastSynchronizedOn;
            CreatedOn = model.CreatedOn;
            ModifiedOn = model.ModifiedOn;
            SetValue(EditCommandPropertyKey, new Commands.RelayCommand(OnEdit));
            SetValue(DeleteCommandPropertyKey, new Commands.RelayCommand(OnDelete));
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LocalDbEntity.LastSynchronizedOn):
                    Dispatcher.CheckInvoke(() => LastSynchronizedOn = Model?.LastSynchronizedOn);
                    break;
                case nameof(LocalDbEntity.CreatedOn):
                    Dispatcher.CheckInvoke(() => CreatedOn = Model?.CreatedOn ?? DateTime.Now);
                    break;
                case nameof(LocalDbEntity.ModifiedOn):
                    Dispatcher.CheckInvoke(() => ModifiedOn = Model?.ModifiedOn ?? DateTime.Now);
                    break;
                default:
                    OnModelPropertyChanged(e.PropertyName);
                    break;
            }
        }

        protected abstract void OnModelPropertyChanged(string propertyName);

        protected abstract DbSet<TDbEntity> GetDbSet(LocalDbContext dbContext);
    }
}
