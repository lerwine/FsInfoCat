using FsInfoCat.Collections;
using FsInfoCat.DeferredDelegation;
using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EditCrawlConfigVM : DependencyObject, INotifyDataErrorInfo
    {
        private (Subdirectory Root, string Path)? _validatedPath;

        public event EventHandler CloseSuccess;
        public event EventHandler CloseCancel;
        public event EventHandler PopupButtonClick;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Initialization Members

        public EditCrawlConfigVM()
        {
            SetValue(SelectRootCommandPropertyKey, new Commands.RelayCommand(OnSelectRootExecute));
            SetValue(SaveCommandPropertyKey, new Commands.RelayCommand(OnSaveExecute));
            SetValue(CancelCommandPropertyKey, new Commands.RelayCommand(OnCancelExecute));
            SetValue(PopupButtonClickCommandPropertyKey, new Commands.RelayCommand(OnPopupButtonClickExecute));
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
            SetValue(LookupCrawlConfigOpMgrPropertyKey, new LookupCrawlConfigAsyncOpManager());
        }

        public void OnWindowClosing()
        {
            LookupCrawlConfigOpMgr.CancelAll();
        }

        internal static bool Edit([DisallowNull] CrawlConfiguration model)
        {
            View.EditCrawlConfigWindow window = new();
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

        internal static bool Edit(string crawlRoot, out CrawlConfiguration model, out bool isNew)
        {
            View.EditCrawlConfigWindow window = new();
            EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }

            EventHandler closeCancelHandler = new((sender, e) => window.DialogResult = false);
            vm.CloseCancel += closeCancelHandler;
            vm.PopupButtonClick += closeCancelHandler;
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            window.Loaded += new RoutedEventHandler((sender, e) =>
            {
                AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)> lookupCrawlConfig = vm.LookupCrawlConfigAsync(crawlRoot);
                lookupCrawlConfig.AsyncOpStatusPropertyChanged += vm.LookupCrawlConfig_AsyncOpStatusPropertyChanged;
                lookupCrawlConfig.StatusMessagePropertyChanged += vm.LookupCrawlConfig_StatusMessagePropertyChanged;
                lookupCrawlConfig.MessageLevelPropertyChanged += vm.LookupCrawlConfig_MessageLevelPropertyChanged;
                lookupCrawlConfig.DurationPropertyChanged += vm.LookupCrawlConfig_DurationPropertyChanged;
                vm.BgOpStatusMessage = lookupCrawlConfig.StatusMessage;
                vm.BgOpMessageLevel = lookupCrawlConfig.MessageLevel;
                vm.BgOpStatus = lookupCrawlConfig.AsyncOpStatus;

                lookupCrawlConfig.GetTask().ContinueWith(task =>
                {
                    lookupCrawlConfig.AsyncOpStatusPropertyChanged -= vm.LookupCrawlConfig_AsyncOpStatusPropertyChanged;
                    lookupCrawlConfig.StatusMessagePropertyChanged -= vm.LookupCrawlConfig_StatusMessagePropertyChanged;
                    lookupCrawlConfig.MessageLevelPropertyChanged -= vm.LookupCrawlConfig_MessageLevelPropertyChanged;
                    lookupCrawlConfig.DurationPropertyChanged -= vm.LookupCrawlConfig_DurationPropertyChanged;
                    vm.Dispatcher.Invoke(() =>
                    {
                        vm.BgOpStatusMessage = lookupCrawlConfig.StatusMessage;
                        vm.BgOpMessageLevel = lookupCrawlConfig.MessageLevel;
                        vm.BgOpStatus = lookupCrawlConfig.AsyncOpStatus;
                        if (task.IsCanceled)
                        {
                            // TODO: Log cancellation.
                        }
                        else if (task.IsFaulted)
                        {
                            // TODO: Log failure.
                        }
                        else
                        {
                            vm.PopupButtonClick -= closeCancelHandler;
                            vm.Initialize(task.Result.Configuration, task.Result.Root, task.Result.ValidatedPath);
                            vm.IsBgOperationActive = false;
                            vm.LookupCrawlConfigOpMgr.RemoveOperation(lookupCrawlConfig);
                        }
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

        private void Initialize(CrawlConfiguration crawlConfiguration, Subdirectory subdirectory, string fullPathName)
        {
            _validatedPath = (subdirectory, fullPathName);
            if (crawlConfiguration is not null)
                Initialize(crawlConfiguration);
            else
            {
                IsNew = true;
                Root = subdirectory;
                CreatedOn = ModifiedOn = DateTime.Now;
                MaxTotalItems = int.MaxValue;
                TTL = TimeSpan.FromDays(1.0);
                RescheduleInterval = TimeSpan.FromDays(1.0);
            }
        }

        internal void Initialize([DisallowNull] CrawlConfiguration crawlConfiguration)
        {
            IsNew = false;
            Model = crawlConfiguration;
            Root = crawlConfiguration.Root;
            CreatedOn = crawlConfiguration.CreatedOn;
            ModifiedOn = crawlConfiguration.ModifiedOn;
            DisplayName = crawlConfiguration.DisplayName;
            MaxRecursionDepth = crawlConfiguration.MaxRecursionDepth;
            ulong? mti = crawlConfiguration.MaxTotalItems;
            LimitTotalItems = mti.HasValue;
            MaxTotalItems = mti ?? int.MaxValue;
            long? seconds = crawlConfiguration.TTL;
            LimitTTL = seconds.HasValue;
            TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : TimeSpan.FromDays(1.0);
            StatusValue = crawlConfiguration.StatusValue;
            LastCrawlEnd = crawlConfiguration.LastCrawlEnd;
            LastCrawlStart = crawlConfiguration.LastCrawlStart;
            NextScheduledStart = crawlConfiguration.NextScheduledStart;
            Notes = crawlConfiguration.Notes;
            RescheduleAfterFail = crawlConfiguration.RescheduleAfterFail;
            RescheduleFromJobEnd = crawlConfiguration.RescheduleFromJobEnd;
            seconds = crawlConfiguration.RescheduleInterval;
            AutoReschedule = seconds.HasValue;
            RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : TimeSpan.FromDays(1.0);
            LastSynchronizedOn = crawlConfiguration.LastSynchronizedOn;
            WindowTitle = "Edit Crawl Configuration";
        }

        #endregion

        #region Command Members

        #region SelectRootCommand Property Members

        private static readonly DependencyPropertyKey SelectRootCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectRootCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectRootCommandProperty = SelectRootCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SelectRootCommand => (Commands.RelayCommand)GetValue(SelectRootCommandProperty);

        private void OnSelectRootExecute(object parameter)
        {
            // TODO: Implement OnSelectRootExecute Logic
        }

        #endregion

        #region SaveCommand Property Members

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private void OnSaveExecute(object parameter)
        {
            // TODO: Implement OnSaveExecute Logic
        }

        #endregion

        #region CancelCommand Property Members

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void OnCancelExecute(object parameter) => CloseCancel?.Invoke(this, EventArgs.Empty);

        #endregion

        #region PopupButtonClickCommand Property Members

        private static readonly DependencyPropertyKey PopupButtonClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PopupButtonClickCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty PopupButtonClickCommandProperty = PopupButtonClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand PopupButtonClickCommand => (Commands.RelayCommand)GetValue(PopupButtonClickCommandProperty);

        private void OnPopupButtonClickExecute(object parameter)
        {
            switch (BgOpStatus)
            {
                case AsyncOpStatusCode.RanToCompletion:
                    LookupCrawlConfigOpMgr.CancelAll();
                    break;
                case AsyncOpStatusCode.Faulted:
                case AsyncOpStatusCode.Canceled:
                    BgOpStatus = AsyncOpStatusCode.NotStarted;
                    PopupButtonClick?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        #endregion

        #endregion

        #region Background Operation Properties

        #region BgOpStatus Property Members

        private static readonly DependencyPropertyKey BgOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatus), typeof(AsyncOpStatusCode), typeof(EditCrawlConfigVM),
                new PropertyMetadata(AsyncOpStatusCode.NotStarted));

        public static readonly DependencyProperty BgOpStatusProperty = BgOpStatusPropertyKey.DependencyProperty;

        public AsyncOpStatusCode BgOpStatus
        {
            get => (AsyncOpStatusCode)GetValue(BgOpStatusProperty);
            private set => SetValue(BgOpStatusPropertyKey, value);
        }

        #endregion

        #region BgOpMessageLevel Property Members

        private static readonly DependencyPropertyKey BgOpMessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpMessageLevel), typeof(StatusMessageLevel), typeof(EditCrawlConfigVM),
                new PropertyMetadata(StatusMessageLevel.Information));

        public static readonly DependencyProperty BgOpMessageLevelProperty = BgOpMessageLevelPropertyKey.DependencyProperty;

        public StatusMessageLevel BgOpMessageLevel
        {
            get => (StatusMessageLevel)GetValue(BgOpMessageLevelProperty);
            private set => SetValue(BgOpMessageLevelPropertyKey, value);
        }

        #endregion

        #region BgOpStatusMessage Property Members

        private static readonly DependencyPropertyKey BgOpStatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatusMessage), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty BgOpStatusMessageProperty = BgOpStatusMessagePropertyKey.DependencyProperty;

        public string BgOpStatusMessage
        {
            get => GetValue(BgOpStatusMessageProperty) as string;
            private set => SetValue(BgOpStatusMessagePropertyKey, value);
        }

        #endregion

        #region BgOpDuration Property Members

        private static readonly DependencyPropertyKey BgOpDurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpDuration), typeof(TimeSpan), typeof(EditCrawlConfigVM),
                new PropertyMetadata(TimeSpan.Zero));

        public static readonly DependencyProperty BgOpDurationProperty = BgOpDurationPropertyKey.DependencyProperty;

        public TimeSpan BgOpDuration
        {
            get => (TimeSpan)GetValue(BgOpDurationProperty);
            private set => SetValue(BgOpDurationPropertyKey, value);
        }

        #endregion

        #region IsBgOperationActive Property

        private static readonly DependencyPropertyKey IsBgOperationActivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBgOperationActive), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsBgOperationActiveProperty = IsBgOperationActivePropertyKey.DependencyProperty;

        public bool IsBgOperationActive
        {
            get => (bool)GetValue(IsBgOperationActiveProperty);
            private set => SetValue(IsBgOperationActivePropertyKey, value);
        }

        #endregion

        #region LookupCrawlConfigOpMgr Property

        private static readonly DependencyPropertyKey LookupCrawlConfigOpMgrPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LookupCrawlConfigOpMgr), typeof(LookupCrawlConfigAsyncOpManager), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LookupCrawlConfigOpMgrProperty = LookupCrawlConfigOpMgrPropertyKey.DependencyProperty;

        public LookupCrawlConfigAsyncOpManager LookupCrawlConfigOpMgr => (LookupCrawlConfigAsyncOpManager)GetValue(LookupCrawlConfigOpMgrProperty);

        private void LookupCrawlConfig_DurationPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpDuration = (e.NewValue as TimeSpan?) ?? TimeSpan.Zero;

        private void LookupCrawlConfig_MessageLevelPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpMessageLevel = (StatusMessageLevel)e.NewValue;

        private void LookupCrawlConfig_StatusMessagePropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpStatusMessage = (e.NewValue as string) ?? "";

        private void LookupCrawlConfig_AsyncOpStatusPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) => BgOpStatus = (AsyncOpStatusCode)e.NewValue;

        #endregion

        #endregion

        #region Change Tracking / Validation Members

        internal bool HasErrors => Validation.HasErrors;

        bool INotifyDataErrorInfo.HasErrors => Validation.HasErrors;

        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNew), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew
        {
            get => (bool)GetValue(IsNewProperty);
            private set => SetValue(IsNewPropertyKey, value);
        }

        #endregion

        #region Model Property Members

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(CrawlConfiguration), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public CrawlConfiguration Model
        {
            get => (CrawlConfiguration)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        #endregion

        #region Validation ChangeTracker Members

        private static readonly DependencyPropertyKey ChangeTrackerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ChangeTracker), typeof(ChangeStateTracker), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeTrackerProperty = ChangeTrackerPropertyKey.DependencyProperty;

        public ChangeStateTracker ChangeTracker => (ChangeStateTracker)GetValue(ChangeTrackerProperty);

        #endregion

        #region Validation Property Members

        private static readonly DependencyPropertyKey ValidationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Validation), typeof(ValidationMessageTracker), typeof(EditCrawlConfigVM),
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

        #region DisplayName Property Members

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(EditCrawlConfigVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DisplayName
        {
            get => GetValue(DisplayNameProperty) as string;
            set => SetValue(DisplayNameProperty, value);
        }

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(DisplayName), (newValue = newValue.AsWsNormalizedOrEmpty()) == Model?.DisplayName);
            if (newValue.Length == 0)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired);
            else if (newValue.Length > DbConstants.DbColMaxLen_LongName)
                Validation.SetErrorMessage(nameof(DisplayName), FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength);
            else
                Validation.ClearErrorMessages(nameof(DisplayName));
        }

        #endregion

        #region MaxTotalItems Members

        #region LimitTotalItems Property Members

        public static readonly DependencyProperty LimitTotalItemsProperty = DependencyProperty.Register(nameof(LimitTotalItems), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnLimitTotalItemsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool LimitTotalItems
        {
            get => (bool)GetValue(LimitTotalItemsProperty);
            set => SetValue(LimitTotalItemsProperty, value);
        }

        protected virtual void OnLimitTotalItemsPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnMaxTotalItemsChanged(MaxTotalItems, newValue);
        }

        #endregion

        #region MaxTotalItems Property Members

        public static readonly DependencyProperty MaxTotalItemsProperty = DependencyProperty.Register(nameof(MaxTotalItems), typeof(ulong), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0UL, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnMaxTotalItemsPropertyChanged((ulong)e.OldValue, (ulong)e.NewValue)));

        public ulong MaxTotalItems
        {
            get => (ulong)GetValue(MaxTotalItemsProperty);
            set => SetValue(MaxTotalItemsProperty, value);
        }

        protected virtual void OnMaxTotalItemsPropertyChanged(ulong oldValue, ulong newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnMaxTotalItemsChanged(newValue, LimitTotalItems);
        }

        private void OnMaxTotalItemsChanged(ulong value, bool isEnabled)
        {
            if (isEnabled)
            {
                ChangeTracker.SetChangeState(nameof(MaxTotalItems), Model?.MaxTotalItems != value);
                if (value < DbConstants.DbColMinValue_TTL)
                    Validation.SetErrorMessage(nameof(MaxTotalItems), FsInfoCat.Properties.Resources.ErrorMessage_TTLInvalid);
                else
                    Validation.ClearErrorMessages(nameof(MaxTotalItems));
            }
            else
            {
                Validation.ClearErrorMessages(nameof(MaxTotalItems));
                ChangeTracker.SetChangeState(nameof(MaxTotalItems), Model?.MaxTotalItems is not null);
            }
        }

        #endregion

        #endregion

        #region Root Directory Members

        #region Root Property Members

        private static readonly DependencyPropertyKey RootPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Root), typeof(Subdirectory), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRootPropertyChanged((Subdirectory)e.OldValue, (Subdirectory)e.NewValue)));

        public static readonly DependencyProperty RootProperty = RootPropertyKey.DependencyProperty;

        public Subdirectory Root
        {
            get => (Subdirectory)GetValue(RootProperty);
            private set => SetValue(RootPropertyKey, value);
        }

        protected virtual void OnRootPropertyChanged(Subdirectory oldValue, Subdirectory newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            if (newValue is null)
            {
                Path = "";
                Validation.SetErrorMessage(nameof(Path), FsInfoCat.Properties.Resources.ErrorMessage_RootPathRequired);
                ChangeTracker.SetChangeState(nameof(Path), (Model?.RootId ?? Guid.Empty) != Guid.Empty);
            }
            else
            {
                ReadOnlyCollection<string> errors = Validation.GetErrors(nameof(Path));
                Validation.ClearErrorMessages(nameof(Path));
                ChangeTracker.SetChangeState(nameof(Path), Model?.RootId != newValue.Id);
                if (_validatedPath.HasValue && ReferenceEquals(_validatedPath.Value.Root, newValue))
                    Path = _validatedPath.Value.Path;
                else
                    // TODO: Replace with async view model
                    Subdirectory.LookupFullNameAsync(newValue).ContinueWith(r =>
                    {
                        if (r.IsCanceled)
                        {
                            if (errors is not null)
                                Dispatcher.Invoke(() => Validation.SetErrorMessage(nameof(Path), errors.First(), errors.Skip(1).ToArray()));
                        }
                        else
                        {
                            string path = r.Result;
                            _validatedPath = (newValue, path);
                            Dispatcher.Invoke(() => Path = path);
                        }
                    });
            }
        }

        #endregion

        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        #endregion

        private AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)> LookupCrawlConfigAsync(string path) =>
            AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.FromAsync(path, LookupCrawlConfigOpMgr, LookupCrawlConfigAsyncOpManager.LookupCrawlConfig);

        #endregion

        #region WindowTitle Property Members

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        #endregion

        #region MaxRecursionDepth Property Members

        public static readonly DependencyProperty MaxRecursionDepthProperty = DependencyProperty.Register(nameof(MaxRecursionDepth), typeof(ushort), typeof(EditCrawlConfigVM),
                new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnMaxRecursionDepthPropertyChanged((ushort)e.OldValue, (ushort)e.NewValue)));

        public ushort MaxRecursionDepth
        {
            get => (ushort)GetValue(MaxRecursionDepthProperty);
            set => SetValue(MaxRecursionDepthProperty, value);
        }

        protected virtual void OnMaxRecursionDepthPropertyChanged(ushort oldValue, ushort newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(MaxRecursionDepth), Model?.MaxRecursionDepth != newValue);
        }

        #endregion

        #region TTL Members

        #region LimitTTL Property Members

        public static readonly DependencyProperty LimitTTLProperty = DependencyProperty.Register(nameof(LimitTTL), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnLimitTTLPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool LimitTTL
        {
            get => (bool)GetValue(LimitTTLProperty);
            set => SetValue(LimitTTLProperty, value);
        }

        protected virtual void OnLimitTTLPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnTTLChanged(TTL, newValue);
        }

        #endregion

        #region TTL Property Members

        public static readonly DependencyProperty TTLProperty = DependencyProperty.Register(nameof(TTL), typeof(TimeSpan), typeof(EditCrawlConfigVM), new PropertyMetadata(TimeSpan.Zero,
            (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditCrawlConfigVM).OnTTLPropertyChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue)));

        public TimeSpan TTL
        {
            get => (TimeSpan)GetValue(TTLProperty);
            set => SetValue(TTLProperty, value);
        }

        protected virtual void OnTTLPropertyChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            TtlDays = newValue.Days;
            TtlHours = newValue.Hours;
            TtlMinutes = newValue.Minutes;
            TtlSeconds = newValue.Seconds;
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnTTLChanged(newValue, LimitTTL);
        }

        private void OnTTLChanged(TimeSpan value, bool isEnabled)
        {
            long? originalValue = Model?.TTL;
            if (isEnabled)
            {
                ChangeTracker.SetChangeState(nameof(TTL), !originalValue.HasValue || TimeSpan.FromSeconds(originalValue.Value) != value);
                if (value < TimeSpan.FromSeconds(DbConstants.DbColMinValue_TTL))
                    Validation.SetErrorMessage(nameof(TTL), FsInfoCat.Properties.Resources.ErrorMessage_TTLInvalid);
                else
                    Validation.ClearErrorMessages(nameof(TTL));
            }
            else
            {
                ChangeTracker.SetChangeState(nameof(TTL), originalValue.HasValue);
                Validation.ClearErrorMessages(nameof(TTL));
            }
        }

        #endregion

        #region TtlDays Property Members

        public static readonly DependencyProperty TtlDaysProperty = DependencyProperty.Register(nameof(TtlDays), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlDaysPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int TtlDays
        {
            get => (int)GetValue(TtlDaysProperty);
            set => SetValue(TtlDaysProperty, value);
        }

        protected virtual void OnTtlDaysPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = TTL;
            if (timeSpan.Days != newValue)
                TTL = new TimeSpan(newValue, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        #endregion

        #region TtlHours Property Members

        public static readonly DependencyProperty TtlHoursProperty = DependencyProperty.Register(nameof(TtlHours), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlHoursPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int TtlHours
        {
            get => (int)GetValue(TtlHoursProperty);
            set => SetValue(TtlHoursProperty, value);
        }

        protected virtual void OnTtlHoursPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = TTL;
            if (timeSpan.Hours != newValue)
                TTL = new TimeSpan(timeSpan.Days, newValue, timeSpan.Minutes, timeSpan.Seconds);
        }

        #endregion

        #region TtlMinutes Property Members

        public static readonly DependencyProperty TtlMinutesProperty = DependencyProperty.Register(nameof(TtlMinutes), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlMinutesPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int TtlMinutes
        {
            get => (int)GetValue(TtlMinutesProperty);
            set => SetValue(TtlMinutesProperty, value);
        }

        protected virtual void OnTtlMinutesPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = TTL;
            if (timeSpan.Minutes != newValue)
                TTL = new TimeSpan(timeSpan.Days, timeSpan.Hours, newValue, timeSpan.Seconds);
        }

        #endregion

        #region TtlSeconds Property Members

        public static readonly DependencyProperty TtlSecondsProperty = DependencyProperty.Register(nameof(TtlSeconds), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTtlSecondsPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int TtlSeconds
        {
            get => (int)GetValue(TtlSecondsProperty);
            set => SetValue(TtlSecondsProperty, value);
        }

        protected virtual void OnTtlSecondsPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = TTL;
            if (timeSpan.Minutes != newValue)
                TTL = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, newValue);
        }

        #endregion

        #endregion

        #region Status Members

        #region StatusValue Property Members

        private static readonly DependencyPropertyKey StatusValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusValue), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnStatusValuePropertyChanged((CrawlStatus)e.OldValue, (CrawlStatus)e.NewValue)));

        public static readonly DependencyProperty StatusValueProperty = StatusValuePropertyKey.DependencyProperty;

        public CrawlStatus StatusValue
        {
            get => (CrawlStatus)GetValue(StatusValueProperty);
            private set => SetValue(StatusValuePropertyKey, value);
        }

        protected virtual void OnStatusValuePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue)
        {
            IsEnabled = newValue != CrawlStatus.Disabled;
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(StatusValue), Model?.StatusValue != newValue);
        }

        #endregion

        #region EnabledStatus Property Members

        private static readonly DependencyPropertyKey EnabledStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnabledStatus), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning));

        public static readonly DependencyProperty EnabledStatusProperty = EnabledStatusPropertyKey.DependencyProperty;

        public CrawlStatus EnabledStatus
        {
            get => (CrawlStatus)GetValue(EnabledStatusProperty);
            private set => SetValue(EnabledStatusPropertyKey, value);
        }

        #endregion

        #region IsEnabled Property Members

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnIsEnabledPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        protected virtual void OnIsEnabledPropertyChanged(bool oldValue, bool newValue) => StatusValue = newValue ? EnabledStatus : CrawlStatus.Disabled;

        #endregion

        #endregion

        #region LastCrawlStart Property Members

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        #endregion

        #region LastCrawlEnd Property Members

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        #endregion

        #region NextScheduledStart Property Members

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

        #endregion

        #region Notes Property Members

        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(EditCrawlConfigVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Notes
        {
            get => GetValue(NotesProperty) as string;
            set => SetValue(NotesProperty, value);
        }

        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(Notes), Model?.Notes != newValue);
        }

        #endregion

        #region RescheduleInterval Members

        #region AutoReschedule Property Members

        public static readonly DependencyProperty AutoRescheduleProperty = DependencyProperty.Register(nameof(AutoReschedule), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnAutoReschedulePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AutoReschedule
        {
            get => (bool)GetValue(AutoRescheduleProperty);
            set => SetValue(AutoRescheduleProperty, value);
        }

        private void OnAutoReschedulePropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnRescheduleIntervalChanged(RescheduleInterval, newValue);
            if (newValue)
            {
                ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), Model?.RescheduleAfterFail != RescheduleAfterFail);
                ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), Model?.RescheduleFromJobEnd != RescheduleFromJobEnd);
            }
            else
            {
                ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), false);
                ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), false);
            }
        }

        #endregion

        #region RescheduleInterval Property Members

        public static readonly DependencyProperty RescheduleIntervalProperty = DependencyProperty.Register(nameof(RescheduleInterval), typeof(TimeSpan), typeof(EditCrawlConfigVM),
                new PropertyMetadata(TimeSpan.Zero, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleIntervalPropertyChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue)));

        public TimeSpan RescheduleInterval
        {
            get => (TimeSpan)GetValue(RescheduleIntervalProperty);
            set => SetValue(RescheduleIntervalProperty, value);
        }

        private void OnRescheduleIntervalPropertyChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            RescheduleDays = newValue.Days;
            RescheduleHours = newValue.Hours;
            RescheduleMinutes = newValue.Minutes;
            RescheduleSeconds = newValue.Seconds;
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            OnRescheduleIntervalChanged(newValue, AutoReschedule);
        }

        private void OnRescheduleIntervalChanged(TimeSpan value, bool isEnabled)
        {
            long? originalValue = Model?.RescheduleInterval;
            if (isEnabled)
            {
                ChangeTracker.SetChangeState(nameof(RescheduleInterval), !originalValue.HasValue || TimeSpan.FromSeconds(originalValue.Value) != value);
                if (value < TimeSpan.FromSeconds(DbConstants.DbColMinValue_RescheduleInterval))
                    Validation.SetErrorMessage(nameof(RescheduleInterval), FsInfoCat.Properties.Resources.ErrorMessage_RescheduleIntervalInvalid);
                else
                    Validation.ClearErrorMessages(nameof(RescheduleInterval));
            }
            else
            {
                ChangeTracker.SetChangeState(nameof(RescheduleInterval), originalValue.HasValue);
                Validation.ClearErrorMessages(nameof(RescheduleInterval));
            }
        }

        #endregion

        #region RescheduleDays Property Members

        public static readonly DependencyProperty RescheduleDaysProperty = DependencyProperty.Register(nameof(RescheduleDays), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleDaysPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int RescheduleDays
        {
            get => (int)GetValue(RescheduleDaysProperty);
            set => SetValue(RescheduleDaysProperty, value);
        }

        protected virtual void OnRescheduleDaysPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = RescheduleInterval;
            if (timeSpan.Days != newValue)
                RescheduleInterval = new TimeSpan(newValue, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        #endregion

        #region RescheduleHours Property Members

        public static readonly DependencyProperty RescheduleHoursProperty = DependencyProperty.Register(nameof(RescheduleHours), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleHoursPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int RescheduleHours
        {
            get => (int)GetValue(RescheduleHoursProperty);
            set => SetValue(RescheduleHoursProperty, value);
        }

        protected virtual void OnRescheduleHoursPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = RescheduleInterval;
            if (timeSpan.Hours != newValue)
                RescheduleInterval = new TimeSpan(timeSpan.Days, newValue, timeSpan.Minutes, timeSpan.Seconds);
        }

        #endregion

        #region RescheduleMinutes Property Members

        public static readonly DependencyProperty RescheduleMinutesProperty = DependencyProperty.Register(nameof(RescheduleMinutes), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleMinutesPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int RescheduleMinutes
        {
            get => (int)GetValue(RescheduleMinutesProperty);
            set => SetValue(RescheduleMinutesProperty, value);
        }

        protected virtual void OnRescheduleMinutesPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = RescheduleInterval;
            if (timeSpan.Minutes != newValue)
                RescheduleInterval = new TimeSpan(timeSpan.Days, timeSpan.Hours, newValue, timeSpan.Seconds);
        }

        #endregion

        #region RescheduleSeconds Property Members

        public static readonly DependencyProperty RescheduleSecondsProperty = DependencyProperty.Register(nameof(RescheduleSeconds), typeof(int), typeof(EditCrawlConfigVM),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleSecondsPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int RescheduleSeconds
        {
            get => (int)GetValue(RescheduleSecondsProperty);
            set => SetValue(RescheduleSecondsProperty, value);
        }

        protected virtual void OnRescheduleSecondsPropertyChanged(int oldValue, int newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            TimeSpan timeSpan = RescheduleInterval;
            if (timeSpan.Seconds != newValue)
                RescheduleInterval = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, newValue);
        }

        #endregion

        #region RescheduleAfterFail Property Members

        public static readonly DependencyProperty RescheduleAfterFailProperty = DependencyProperty.Register(nameof(RescheduleAfterFail), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleAfterFailPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool RescheduleAfterFail
        {
            get => (bool)GetValue(RescheduleAfterFailProperty);
            set => SetValue(RescheduleAfterFailProperty, value);
        }

        private void OnRescheduleAfterFailPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(RescheduleAfterFail), Model?.RescheduleAfterFail != newValue);
        }

        #endregion

        #region RescheduleFromJobEnd Property Members

        public static readonly DependencyProperty RescheduleFromJobEndProperty = DependencyProperty.Register(nameof(RescheduleFromJobEnd), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnRescheduleFromJobEndPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool RescheduleFromJobEnd
        {
            get => (bool)GetValue(RescheduleFromJobEndProperty);
            set => SetValue(RescheduleFromJobEndProperty, value);
        }

        private void OnRescheduleFromJobEndPropertyChanged(bool oldValue, bool newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            ChangeTracker.SetChangeState(nameof(RescheduleFromJobEnd), Model?.RescheduleFromJobEnd != newValue);
        }

        #endregion

        #endregion

        #region CreatedOn Property Members

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion

        #region ModifiedOn Property Members

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion

        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion

        #endregion
    }
}
