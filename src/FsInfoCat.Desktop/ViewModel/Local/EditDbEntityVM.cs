using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class EditDbEntityVM<TDbEntity> : DependencyObject, INotifyDataErrorInfo
        where TDbEntity : LocalDbEntity, new()
    {
        /// <summary>
        /// Occurs when the window should be closed by setting <see cref="Window.DialogResult"/> to <see langword="true"/>.
        /// </summary>
        public event EventHandler CloseSuccess;

        /// <summary>
        /// Occurs when the window should be closed by setting <see cref="Window.DialogResult"/> to <see langword="false"/>.
        /// </summary>
        public event EventHandler CloseCancel;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region SaveCommand Property Members

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(EditDbEntityVM<TDbEntity>), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the bindable "Save" command.
        /// </summary>
        /// <value>The bindable command for saving changes and closing the edit window.</value>
        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private void OnSaveExecute(object parameter)
        {
            if (OnBeforeSave())
            {
                ModifiedOn = Model.ModifiedOn = DateTime.Now;
                if (IsNew)
                    CreatedOn = Model.CreatedOn = ModifiedOn;
                SaveChangesAsync();
            }
        }

        private static async Task<bool> SaveChangesAsync(ModelViewModel state, AsyncOps.IStatusListener statusListener)
        {
            EditDbEntityVM<TDbEntity> vm = state.ViewModel ?? throw new ArgumentException($"{nameof(state.ViewModel)} cannot be null.", nameof(state));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry<TDbEntity> entry = dbContext.Entry(state.Entity);
            return await vm.OnSaveChangesAsync(entry, dbContext, statusListener, state.ForceSave);
        }

        #endregion
        #region CancelCommand Property Members

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(EditDbEntityVM<TDbEntity>), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the bindable "Cancel" command.
        /// </summary>
        /// <value>The bindable command for discarding changes and closing the edit window.</value>
        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void OnCancelExecute(object parameter) => CloseCancel?.Invoke(this, EventArgs.Empty);

        #endregion
        #region OpAggregate Property Members

        private static readonly DependencyPropertyKey OpAggregatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpAggregate),
            typeof(AsyncOps.AsyncBgModalVM), typeof(EditDbEntityVM<TDbEntity>), new PropertyMetadata(null));

        public static readonly DependencyProperty OpAggregateProperty = OpAggregatePropertyKey.DependencyProperty;

        public AsyncOps.AsyncBgModalVM OpAggregate => (AsyncOps.AsyncBgModalVM)GetValue(OpAggregateProperty);

        #endregion
        #region Change Tracking / Validation Members

        internal bool HasErrors => Validation.HasErrors;

        bool INotifyDataErrorInfo.HasErrors => Validation.HasErrors;

        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNew), typeof(bool), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew
        {
            get => (bool)GetValue(IsNewProperty);
            private set => SetValue(IsNewPropertyKey, value);
        }

        #endregion
        #region Model Property Members

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(TDbEntity), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditDbEntityVM<TDbEntity>).RaiseModelPropertyChanged((TDbEntity)e.OldValue, (TDbEntity)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public TDbEntity Model { get => (TDbEntity)GetValue(ModelProperty); private set => SetValue(ModelPropertyKey, value); }

        private void RaiseModelPropertyChanged(TDbEntity oldValue, TDbEntity newValue)
        {
            SaveCommand.IsEnabled = newValue is not null && ChangeTracker.AnyInvalid && !Validation.AnyInvalid;
            OnModelPropertyChanged(oldValue, newValue);
        }

        /// <summary>
        /// Called when the value of the <see cref="Model"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Model"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Model"/> property.</param>
        protected abstract void OnModelPropertyChanged(TDbEntity oldValue, TDbEntity newValue);

        #endregion
        #region ChangeTracker Members

        private static readonly DependencyPropertyKey ChangeTrackerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ChangeTracker), typeof(ChangeStateTracker), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeTrackerProperty = ChangeTrackerPropertyKey.DependencyProperty;

        public ChangeStateTracker ChangeTracker => (ChangeStateTracker)GetValue(ChangeTrackerProperty);

        #endregion
        #region Validation Property Members

        private static readonly DependencyPropertyKey ValidationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Validation), typeof(ValidationMessageTracker), typeof(EditDbEntityVM<TDbEntity>),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ValidationProperty = ValidationPropertyKey.DependencyProperty;

        public ValidationMessageTracker Validation => (ValidationMessageTracker)GetValue(ValidationProperty);

        private void Validation_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        #endregion

        protected virtual void OnValidationStateChanged(object sender, EventArgs e)
        {
            SaveCommand.IsEnabled = Model is not null && ChangeTracker.AnyInvalid && !Validation.AnyInvalid;
        }

        public IEnumerable GetErrors(string propertyName) => Validation.GetErrors(propertyName);

        #endregion
        #region WindowTitle Property Members

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string),
            typeof(EditDbEntityVM<TDbEntity>), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        #endregion
        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion
        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion

        protected EditDbEntityVM()
        {
            SetValue(SaveCommandPropertyKey, new Commands.RelayCommand(OnSaveExecute));
            SetValue(CancelCommandPropertyKey, new Commands.RelayCommand(OnCancelExecute));
            ValidationMessageTracker validation = new();
            SetValue(ValidationPropertyKey, validation);
            ChangeStateTracker changeTracker = new();
            SetValue(ChangeTrackerPropertyKey, changeTracker);
            SetValue(OpAggregatePropertyKey, new AsyncOps.AsyncBgModalVM());
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            validation.ErrorsChanged += Validation_ErrorsChanged;
            validation.AnyInvalidPropertyChanged += OnValidationStateChanged;
            changeTracker.AnyInvalidPropertyChanged += OnValidationStateChanged;
        }

        internal bool? ShowDialog([DisallowNull] Window window, [DisallowNull] TDbEntity entity, bool isNew)
        {
            if (window is null)
                throw new ArgumentNullException(nameof(window));
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
            IsNew = isNew;
            Model = entity;
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            window.DataContext = this;
            // DEFERRED: Need to put something in here that is going to catch it if the user closes the window during a save operation
            EventHandler closeCancel = new((sender, e) => window.DialogResult = false);
            EventHandler closeSuccess = new((sender, e) => window.DialogResult = true);
            CloseCancel += closeCancel;
            CloseSuccess += closeSuccess;
            if (window.Owner is null)
                window.Owner = Application.Current.MainWindow;
            try { return window.ShowDialog(); }
            finally
            {
                CloseCancel -= closeCancel;
                CloseSuccess -= closeSuccess;
            }
        }

        protected abstract DbSet<TDbEntity> GetDbSet([DisallowNull] LocalDbContext dbContext);

        /// <summary>
        /// Called from the <see cref="System.Windows.Threading.DispatcherObject.Dispatcher"/> thread when the <see cref="SaveCommand"/> is invoked,
        /// immediately before the background process is started.
        /// </summary>
        /// <returns><see langword="true"/> to proceed with saving changes; otherwise <see langword="false"/> to cancel the <see cref="SaveCommand"/> command.</returns>
        protected abstract bool OnBeforeSave();

        /// <summary>
        /// Starts a <see cref="Task{bool}"/> that asynchronously saves <see cref="Model"/> changes to the database.
        /// </summary>
        /// <param name="forceSave"><see langword="true"/> to save changes, even when <see cref="EntityEntry.State"/> is <see cref="EntityState.Unchanged"/>;
        /// otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Task{bool}"/> that <see langword="true"/> if changes were successfully saved to the database; otherwise, <see langword="false"/>.</returns>
        protected virtual Task<bool> SaveChangesAsync(bool forceSave = false)
        {
            Task<bool> task = OpAggregate.FromAsync("Saving Changes", "Connecting to database", new ModelViewModel(Model, this, forceSave), SaveChangesAsync);
            task.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    Dispatcher.Invoke(() => CloseSuccess?.Invoke(this, EventArgs.Empty));
            });
            return task;
        }

        /// <summary>
        /// Aynchonrously called to save changes to the database.
        /// </summary>
        /// <param name="entry">The entry to be saved.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="statusListener">The background operation status listener.</param>
        /// <param name="force"><see langword="true"/> to save changes, even when <see cref="EntityEntry.State"/> is <see cref="EntityState.Unchanged"/>;
        /// otherwise, <see langword="false"/></param>
        /// <returns><see langword="true"/> if changes were successfully saved to the database; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">Failed to save changes to the database.</exception>
        protected virtual async Task<bool> OnSaveChangesAsync([DisallowNull] EntityEntry<TDbEntity> entry, [DisallowNull] LocalDbContext dbContext, [DisallowNull] AsyncOps.IStatusListener statusListener, bool force = false)
        {
            if (!(force || IsNew) && entry.State == EntityState.Unchanged)
                return false;
            try
            {
                if (IsNew && entry.State == EntityState.Detached)
                    entry = GetDbSet(dbContext).Add(entry.Entity);
                await dbContext.SaveChangesAsync(true, statusListener.CancellationToken);
                if (entry.State != EntityState.Unchanged)
                    throw new InvalidOperationException("Failed to save changes to the database.");
            }
            catch
            {
                if (!IsNew)
                    await entry.ReloadAsync(statusListener.CancellationToken);
                throw;
            }
            return true;
        }

        //internal static bool EditObsolete<TWindow, TVm>([DisallowNull] CrawlConfiguration model)
        //    where TWindow : Window, new()
        //    where TVm : EditDbEntityVM<TDbEntity>, new()
        //{
        //    View.Local.EditCrawlConfigWindow window = new();
        //    EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
        //    if (vm is null)
        //    {
        //        vm = new();
        //        window.DataContext = vm;
        //    }
        //    vm.Initialize(model);
        //    vm.CloseCancel += new EventHandler((sender, e) => window.DialogResult = false);
        //    vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
        //    return window.ShowDialog() ?? false;
        //}

        //public static bool EditObsolete<TWindow, TVm, TLoadResult>(IEntityLoader<TDbEntity, TVm, TLoadResult> entityLoader, out TDbEntity model, out bool isNew)
        //    where TWindow : Window, new()
        //    where TVm : EditDbEntityVM<TDbEntity>, new()
        //{
        //    if (entityLoader is null)
        //        throw new ArgumentNullException(nameof(entityLoader));
        //    TWindow window = new();
        //    TVm vm = (TVm)window.DataContext;
        //    if (vm is null)
        //    {
        //        vm = new();
        //        window.DataContext = vm;
        //    }

        //    EventHandler closeCancelHandler = new((sender, e) =>
        //    {
        //        window.DialogResult = false;
        //    });

        //    vm.CloseCancel += closeCancelHandler;
        //    vm.OpAggregate.OperationFaulted += closeCancelHandler;
        //    vm.OpAggregate.OperationCancelRequested += closeCancelHandler;
        //    vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
        //    window.Loaded += new RoutedEventHandler((sender, e) =>
        //    {
        //        Task<(TLoadResult loadResult, EntityState State)> task = vm.OpAggregate.FromAsync(entityLoader.LoadingTitle,
        //            entityLoader.InitialLoadingMessage, async statusListener =>
        //            {
        //                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
        //                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
        //                TLoadResult loadResult = await entityLoader.LoadAsync(dbContext, statusListener);
        //                EntityEntry<TDbEntity> entry;
        //                TDbEntity entity = entityLoader.GetEntity(loadResult);
        //                if (entity is null)
        //                {
        //                    //entity = vm.InitializeNewModel();
        //                    entry = vm.GetDbSet(dbContext).Add(entity);
        //                }
        //                else if ((entry = dbContext.Entry(entity)).State == EntityState.Detached)
        //                    entry = vm.GetDbSet(dbContext).Add(entity);
        //                return (loadResult, entry.State);
        //            });
        //        task.ContinueWith(task =>
        //        {
        //            if (task.IsCompletedSuccessfully)
        //                vm.Dispatcher.Invoke(() =>
        //                {
        //                    vm.OpAggregate.OperationCancelRequested -= closeCancelHandler;
        //                    vm.OpAggregate.OperationFaulted -= closeCancelHandler;
        //                    //vm.Initialize(entityLoader.GetEntity(task.Result.Item1), task.Result.Item2);
        //                    entityLoader.InitializeViewModel(vm, task.Result.Item1, task.Result.Item2);
        //                });
        //        });
        //    });
        //    if (window.ShowDialog() ?? false)
        //    {
        //        isNew = vm.IsNew;
        //        model = vm.Model;
        //        return true;
        //    }
        //    model = null;
        //    isNew = false;
        //    return false;
        //}

        //public static bool EditObsolete<TWindow, TVm>([DisallowNull] string loadingTitle, [DisallowNull] string initialLoadingMessage,
        //    [DisallowNull] Func<LocalDbContext, AsyncOps.IStatusListener, Task<TDbEntity>> getDbEntityOpAsync, out TDbEntity model, out bool isNew)
        //    where TWindow : Window, new()
        //    where TVm : EditDbEntityVM<TDbEntity>, new()
        //{
        //    return EditObsolete<TWindow, TVm, TDbEntity>(new EntityLoader<TDbEntity, TVm>(loadingTitle, initialLoadingMessage, getDbEntityOpAsync),
        //        out model, out isNew);
        //}

        public record ModelViewModel(TDbEntity Entity, EditDbEntityVM<TDbEntity> ViewModel, bool ForceSave);
    }
}
