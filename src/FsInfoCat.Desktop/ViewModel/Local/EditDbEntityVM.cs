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

        protected abstract TDbEntity InitializeNewModel();

        protected abstract DbSet<TDbEntity> GetDbSet(LocalDbContext dbContext);

        protected virtual void OnSavingModel(EntityEntry<TDbEntity> entityEntry, LocalDbContext dbContext, AsyncOps.IStatusListener<ModelViewModel> statusListener) { }

        protected abstract void UpdateModelForSave(TDbEntity model, bool isNew);

        #region Command Members

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
            AsyncOps.AsyncFuncOpViewModel<ModelViewModel, TDbEntity> asyncOp = OpAggregate.FromAsync("Saving Changes", "Connecting to database", new(Model, this), SaveChangesOpMgr, SaveChangesAsync);
            asyncOp.GetTask().ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (task.IsCompletedSuccessfully)
                        CloseSuccess?.Invoke(this, EventArgs.Empty);
                });
            });
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

        #endregion

        #region Background Operation Properties

        #region OpAggregate Property Members

        private static readonly DependencyPropertyKey OpAggregatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpAggregate),
            typeof(AsyncOps.AsyncBgModalVM), typeof(EditDbEntityVM<TDbEntity>), new PropertyMetadata(null));

        public static readonly DependencyProperty OpAggregateProperty = OpAggregatePropertyKey.DependencyProperty;

        public AsyncOps.AsyncBgModalVM OpAggregate => (AsyncOps.AsyncBgModalVM)GetValue(OpAggregateProperty);

        #endregion
        
        #region SaveChangesOpMgr Property Members

        private static readonly DependencyPropertyKey SaveChangesOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveChangesOpMgr), typeof(AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, TDbEntity>), typeof(EditDbEntityVM<TDbEntity>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SaveChangesOpMgrProperty = SaveChangesOpMgrPropertyKey.DependencyProperty;

        public AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, TDbEntity> SaveChangesOpMgr => (AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, TDbEntity>)GetValue(SaveChangesOpMgrProperty);

        private static async Task<TDbEntity> SaveChangesAsync(ModelViewModel state, AsyncOps.IStatusListener<ModelViewModel> statusListener)
        {
            EditDbEntityVM<TDbEntity> vm = state.ViewModel ?? throw new ArgumentException($"{nameof(state.ViewModel)} cannot be null.", nameof(state));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<TDbEntity> entry;
            if (state.Entity is null)
            {
                TDbEntity model = vm.Dispatcher.Invoke(() => vm.InitializeNewModel());
                model.ModifiedOn = model.CreatedOn;
                entry = vm.GetDbSet(dbContext).Add(model);
            }
            else
                entry = dbContext.Entry(state.Entity);
            vm.Dispatcher.Invoke(() =>
            {
                TDbEntity model = entry.Entity;
                if (entry.State != EntityState.Added)
                    model.ModifiedOn = DateTime.Now;
                // TODO: Update model
                vm.UpdateModelForSave(model, entry.State == EntityState.Added);
            });
            vm.OnSavingModel(entry, dbContext, statusListener);
            try
            {
                await dbContext.SaveChangesAsync(true, statusListener.CancellationToken);
                if (entry.State != EntityState.Unchanged)
                    throw new InvalidOperationException("Failed to save changes to the database.");
            }
            catch
            {
                if (state.Entity is not null)
                    await entry.ReloadAsync(statusListener.CancellationToken);
                throw;
            }
            return entry.Entity;
        }

        #endregion

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
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public TDbEntity Model
        {
            get => (TDbEntity)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

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
            SaveCommand.IsEnabled = ChangeTracker.AnyInvalid && !Validation.AnyInvalid;
        }

        public IEnumerable GetErrors(string propertyName) => Validation.GetErrors(propertyName);

        #endregion

        #region Other Property Members

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

        #endregion

        protected EditDbEntityVM()
        {
            SetValue(SaveCommandPropertyKey, new Commands.RelayCommand(OnSaveExecute));
            SetValue(CancelCommandPropertyKey, new Commands.RelayCommand(OnCancelExecute));
            ValidationMessageTracker validation = new();
            SetValue(ValidationPropertyKey, validation);
            ChangeStateTracker changeTracker = new();
            SetValue(ChangeTrackerPropertyKey, changeTracker);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            validation.ErrorsChanged += Validation_ErrorsChanged;
            validation.AnyInvalidPropertyChanged += OnValidationStateChanged;
            changeTracker.AnyInvalidPropertyChanged += OnValidationStateChanged;
            // DEFERRED: Figure out why this crashes designer
            SetValue(SaveChangesOpMgrPropertyKey, new AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, TDbEntity>());
            SetValue(OpAggregatePropertyKey, new AsyncOps.AsyncBgModalVM());
        }

        /// <summary>
        /// Instantiates a new <see cref="View.EditCrawlConfigWindow"/> to edit the properties of an existing <see cref="CrawlConfiguration"/>.
        /// </summary>
        /// <typeparam name="TWindow">The type of window to use.</typeparam>
        /// <typeparam name="TVm">The view model used by the window.</typeparam>
        /// <param name="model">The <see cref="LocalDbEntity"/> of type <typeparamref name="TDbEntity"/> to be modified.</param>
        /// <returns><see langword="true"/> if modifications were successfully saved to the databaase; otherwise <see langword="false"/> to indicate the user cancelled or there was
        /// an error that prohibited successful initialization.</returns>
        internal static bool EditObsolete<TWindow, TVm>([DisallowNull] CrawlConfiguration model)
            where TWindow : Window, new()
            where TVm : EditDbEntityVM<TDbEntity>, new()
        {
            View.Local.EditCrawlConfigWindow window = new();
            EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }
            vm.Initialize(model);
            vm.CloseCancel += new EventHandler((sender, e) => window.DialogResult = false);
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            return window.ShowDialog() ?? false;
        }

        public static bool EditObsolete<TWindow, TVm, TLoadResult>(IEntityLoader<TDbEntity, TVm, TLoadResult> entityLoader, out TDbEntity model, out bool isNew)
            where TWindow : Window, new()
            where TVm : EditDbEntityVM<TDbEntity>, new()
        {
            if (entityLoader is null)
                throw new ArgumentNullException(nameof(entityLoader));
            TWindow window = new();
            TVm vm = (TVm)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }

            EventHandler closeCancelHandler = new((sender, e) =>
            {
                window.DialogResult = false;
            });

            vm.CloseCancel += closeCancelHandler;
            vm.OpAggregate.OperationFaulted += closeCancelHandler;
            vm.OpAggregate.OperationCancelRequested += closeCancelHandler;
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            window.Loaded += new RoutedEventHandler((sender, e) =>
            {
                AsyncOps.AsyncOpResultManagerViewModel<(TLoadResult, EntityState)> loader = new();
                AsyncOps.AsyncFuncOpViewModel<(TLoadResult, EntityState)> op = vm.OpAggregate.FromAsync(entityLoader.LoadingTitle,
                    entityLoader.InitialLoadingMessage, loader, async statusListener =>
                {
                    using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
                    TLoadResult loadResult = await entityLoader.LoadAsync(dbContext, statusListener);
                    EntityEntry<TDbEntity> entry;
                    TDbEntity entity = entityLoader.GetEntity(loadResult);
                    if (entity is null)
                    {
                        entity = vm.InitializeNewModel();
                        entry = vm.GetDbSet(dbContext).Add(entity);
                    }
                    else if ((entry = dbContext.Entry(entity)).State == EntityState.Detached)
                        entry = vm.GetDbSet(dbContext).Add(entity);
                    return (loadResult, entry.State);
                });
                op.GetTask().ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                        vm.Dispatcher.Invoke(() =>
                        {
                            vm.OpAggregate.OperationCancelRequested -= closeCancelHandler;
                            vm.OpAggregate.OperationFaulted -= closeCancelHandler;
                            vm.Initialize(entityLoader.GetEntity(task.Result.Item1), task.Result.Item2);
                            entityLoader.InitializeViewModel(vm, task.Result.Item1, task.Result.Item2);
                        });
                });
            });
            if (window.ShowDialog() ?? false)
            {
                isNew = vm.IsNew;
                model = vm.Model;
                return true;
            }
            model = null;
            isNew = false;
            return false;
        }

        /// <summary>
        /// Instantiates a new <see cref="Window"/> to edit the properties of a <see cref="LocalDbEntity"/>.
        /// </summary>
        /// <typeparam name="TWindow">The type of window to use.</typeparam>
        /// <typeparam name="TVm">The view model used by the window.</typeparam>
        /// <param name="loadingTitle">The title to display in the <see cref="View.AsyncBgModalControl"/> while the entity is being loaded.</param>
        /// <param name="initialLoadingMessage">The status message to initially display within the <see cref="View.AsyncBgModalControl"/>.</param>
        /// <param name="getDbEntityOpAsync">A factory method that returns a <see cref="Task{TDbEntity}"/> that is presumably the return value
        /// of an asynchronous method.</param>
        /// <param name="model">Returns the <see cref="LocalDbEntity"/> that was edited or inserted into the database.</param>
        /// <param name="isNew"><see langword="true"/> if a new entity was inserted into the database; otherwise, <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if any changes were saved to the database or if a new entity was inserted into the database;
        /// otherwise <see langword="false"/> to indicate the user cancelled or there was an error that prohibited successful initialization.</returns>
        public static bool EditObsolete<TWindow, TVm>([DisallowNull] string loadingTitle, [DisallowNull] string initialLoadingMessage,
            [DisallowNull] Func<LocalDbContext, AsyncOps.IStatusListener, Task<TDbEntity>> getDbEntityOpAsync, out TDbEntity model, out bool isNew)
            where TWindow : Window, new()
            where TVm : EditDbEntityVM<TDbEntity>, new()
        {
            return EditObsolete<TWindow, TVm, TDbEntity>(new EntityLoader<TDbEntity, TVm>(loadingTitle, initialLoadingMessage, getDbEntityOpAsync),
                out model, out isNew);
        }

        protected virtual void Initialize(TDbEntity model, EntityState state)
        {
            IsNew = state == EntityState.Added;
            Model = model;
            CreatedOn = model.CreatedOn;
            ModifiedOn = model.ModifiedOn;
            LastSynchronizedOn = model.LastSynchronizedOn;
        }

        public record ModelViewModel(TDbEntity Entity, EditDbEntityVM<TDbEntity> ViewModel);
    }
}
