using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class RedundantSetsPageVM : DbEntityListingPageVM<RedundantSet, RedundantSetItemVM>
    {
    }
    public class RedundantSetItemVM : DbEntityItemVM<RedundantSet>
    {
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(RedundantSetItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            private set => SetValue(DisplayNamePropertyKey, value);
        }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(RedundantSetItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            private set => SetValue(NotesPropertyKey, value);
        }

        #endregion

        internal RedundantSetItemVM([DisallowNull] RedundantSet model)
            : base(model)
        {
            Notes = model.Notes;
            // TODO: Initialize properties
        }

        protected override void OnModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(RedundantSet.Notes):
                    Dispatcher.Invoke(() => Notes = Model.Notes);
                    break;
                    // TODO: Check for remainder of properties
            }
        }
    }
    public class EditRedundantSetVM : EditDbEntityVM<Volume>
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

        #region Command Members

        #region SaveCommand Property Members

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(EditRedundantSetVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the bindable "Save" command.
        /// </summary>
        /// <value>The bindable command for saving changes and closing the edit window.</value>
        // TODO: Make sure save command is disabled when there are validation errors or no changes.
        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private void OnSaveExecute(object parameter)
        {
            RedundantSet model = Model;
            if (model is null)
            {
                model = new()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Notes = Notes
                    // TODO: Initialize model
                };
                model.ModifiedOn = model.CreatedOn;
            }
            else
            {
                model.ModifiedOn = DateTime.Now;
                model.Notes = Notes;
                // TODO: Initialize model
            }
            // TODO: Initialize model
            AsyncOps.AsyncFuncOpViewModel<ModelViewModel, RedundantSet> asyncOp = SaveChangesAsync(model);
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
            typeof(Commands.RelayCommand), typeof(EditRedundantSetVM), new PropertyMetadata(null));

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

        private AsyncOps.AsyncFuncOpViewModel<ModelViewModel, RedundantSet> SaveChangesAsync(RedundantSet redundantSet) =>
            OpAggregate.FromAsync("Saving Changes", "Connecting to database", new(redundantSet, this), SaveChangesOpMgr, SaveChangesAsync);

        #region OpAggregate Property Members

        private static readonly DependencyPropertyKey OpAggregatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(OpAggregate),
            typeof(AsyncOps.AsyncOpAggregate), typeof(EditRedundantSetVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OpAggregateProperty = OpAggregatePropertyKey.DependencyProperty;

        public AsyncOps.AsyncOpAggregate OpAggregate => (AsyncOps.AsyncOpAggregate)GetValue(OpAggregateProperty);

        #endregion
        #region SaveChangesOpMgr Property Members

        private static readonly DependencyPropertyKey SaveChangesOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveChangesOpMgr), typeof(AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, RedundantSet>), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SaveChangesOpMgrProperty = SaveChangesOpMgrPropertyKey.DependencyProperty;

        public AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, RedundantSet> SaveChangesOpMgr => (AsyncOps.AsyncOpResultManagerViewModel<ModelViewModel, RedundantSet>)GetValue(SaveChangesOpMgrProperty);

        private static async Task<RedundantSet> SaveChangesAsync(ModelViewModel state,
            AsyncOps.AsyncFuncOpViewModel<ModelViewModel, RedundantSet>.StatusListenerImpl statusListener)
        {
            EditRedundantSetVM vm = state.ViewModel ?? throw new ArgumentException($"{nameof(state.ViewModel)} cannot be null.", nameof(state));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<RedundantSet> entry;
            if (state.Entity is null)
            {
                RedundantSet model = vm.Dispatcher.Invoke(() => new RedundantSet()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Notes = vm.Notes
                    // TODO: Initialize model
                });
                model.ModifiedOn = model.CreatedOn;
                entry = dbContext.RedundantSets.Add(model);
            }
            else
                entry = dbContext.Entry(state.Entity);
            vm.Dispatcher.Invoke(() =>
            {
                RedundantSet model = entry.Entity;
                if (entry.State != EntityState.Added)
                {
                    model.ModifiedOn = DateTime.Now;
                    model.Notes = vm.Notes;
                    // TODO: Update model
                }
                // TODO: Update model
            });
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

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNew), typeof(bool), typeof(EditRedundantSetVM),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew
        {
            get => (bool)GetValue(IsNewProperty);
            private set => SetValue(IsNewPropertyKey, value);
        }

        #endregion

        #region Model Property Members

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(RedundantSet), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public RedundantSet Model
        {
            get => (RedundantSet)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        #endregion

        #region Validation ChangeTracker Members

        private static readonly DependencyPropertyKey ChangeTrackerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ChangeTracker), typeof(ChangeStateTracker), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeTrackerProperty = ChangeTrackerPropertyKey.DependencyProperty;

        public ChangeStateTracker ChangeTracker => (ChangeStateTracker)GetValue(ChangeTrackerProperty);

        #endregion

        #region Validation Property Members

        private static readonly DependencyPropertyKey ValidationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Validation), typeof(ValidationMessageTracker), typeof(EditRedundantSetVM),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ValidationProperty = ValidationPropertyKey.DependencyProperty;

        public ValidationMessageTracker Validation => (ValidationMessageTracker)GetValue(ValidationProperty);

        private void Validation_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        #endregion

        private void OnValidationStateChanged(object sender, EventArgs e)
        {

        }

        public IEnumerable GetErrors(string propertyName) => Validation.GetErrors(propertyName);

        #endregion

        #region Other Property Members

        #region WindowTitle Property Members

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string),
            typeof(EditRedundantSetVM), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        #endregion

        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditRedundantSetVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditRedundantSetVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue);
        }

        #endregion

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditRedundantSetVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion

        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditRedundantSetVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion

        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditRedundantSetVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion

        #endregion

        public record ModelViewModel(RedundantSet Entity, EditRedundantSetVM ViewModel);

    }
}
