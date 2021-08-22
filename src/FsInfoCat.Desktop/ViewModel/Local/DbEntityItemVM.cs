using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class DbEntityItemVM<TDbEntity> : DependencyObject
        where TDbEntity : LocalDbEntity, new()
    {
        #region Command Members

        #region EditCurrentItem Property Members

        /// <summary>
        /// Occurs when the <see cref="EditCurrentItem"/> command is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditRequest;

        private static readonly DependencyPropertyKey EditCurrentItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditCurrentItem),
            typeof(Commands.RelayCommand), typeof(DbEntityItemVM<TDbEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="EditCurrentItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditCurrentItemProperty = EditCurrentItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the EditCurrentItem command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand EditCurrentItem => (Commands.RelayCommand)GetValue(EditCurrentItemProperty);

        /// <summary>
        /// Called when the <see cref="EditCurrentItem"/> command is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="EditCurrentItem" />.</param>
        protected virtual void RaiseEditRequest(object parameter) => EditRequest?.Invoke(this, new(parameter));

        #endregion
        #region DeleteCurrentItem Property Members

        /// <summary>
        /// Occurs when the <see cref="DeleteCurrentItem"/> command is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteRequest;

        private static readonly DependencyPropertyKey DeleteCurrentItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteCurrentItem),
            typeof(Commands.RelayCommand), typeof(DbEntityItemVM<TDbEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref=""/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteCurrentItemProperty = DeleteCurrentItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the DeleteCurrentItem command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand DeleteCurrentItem => (Commands.RelayCommand)GetValue(DeleteCurrentItemProperty);

        /// <summary>
        /// Called when the <see cref="DeleteCurrentItem"/> command is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DeleteCurrentItem" />.</param>
        protected virtual void RaiseDeleteRequest(object parameter) => DeleteRequest?.Invoke(this, new(parameter));

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
            SetValue(EditCurrentItemPropertyKey, new Commands.RelayCommand(RaiseEditRequest));
            SetValue(DeleteCurrentItemPropertyKey, new Commands.RelayCommand(RaiseDeleteRequest));
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CheckAccess())
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
            else
                Dispatcher.Invoke(() => Model_PropertyChanged(sender, e));
        }

        protected abstract void OnModelPropertyChanged(string propertyName);

        protected abstract DbSet<TDbEntity> GetDbSet(LocalDbContext dbContext);
    }
}
