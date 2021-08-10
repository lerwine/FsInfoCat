using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EditCrawlConfigVM : DependencyObject
    {
        private readonly Collection<string> _changedProperties = new();
        private (Subdirectory Root, string Path)? _validatedPath;

        public event EventHandler CloseSuccess;
        public event EventHandler CloseCancel;
        public event EventHandler PopupButtonClick;

        #region Properties

        #region SelectRootCommand Property

        private static readonly DependencyPropertyKey SelectRootCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectRootCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectRootCommandProperty = SelectRootCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SelectRootCommand => (Commands.RelayCommand)GetValue(SelectRootCommandProperty);

        private void OnSelectRootExecute(object parameter)
        {
            // TODO: Implement OnSelectRootExecute Logic
        }

        #endregion

        #region SaveCommand Property

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private void OnSaveExecute(object parameter)
        {
            // TODO: Implement OnSaveExecute Logic
        }

        #endregion

        #region CancelCommand Property

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(EditCrawlConfigVM), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void OnCancelExecute(object parameter) => CloseCancel?.Invoke(this, EventArgs.Empty);

        #endregion

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

        #region IsNew Property

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNew), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew
        {
            get => (bool)GetValue(IsNewProperty);
            private set => SetValue(IsNewPropertyKey, value);
        }

        #endregion

        #region WindowTitle Property

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        #endregion

        #region HasChanges Property

        private static readonly DependencyPropertyKey HasChangesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasChanges), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty HasChangesProperty = HasChangesPropertyKey.DependencyProperty;

        public bool HasChanges
        {
            get => (bool)GetValue(HasChangesProperty);
            private set => SetValue(HasChangesPropertyKey, value);
        }

        #endregion

        #region Model Property

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(CrawlConfiguration), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public CrawlConfiguration Model
        {
            get => (CrawlConfiguration)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        #endregion

        #region Root Property

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
                if (!(Model is null || _changedProperties.Contains(nameof(Root))))
                {
                    _changedProperties.Add(nameof(Root));
                    HasChanges = true;
                }
                Path = "";
            }
            else
            {
                if (Model is not null)
                {
                    if (Model.Id == newValue.Id)
                    {
                        if (_changedProperties.Remove(nameof(Root)) && _changedProperties.Count == 0)
                            HasChanges = false;
                    }
                    else if (!_changedProperties.Contains(nameof(Root)))
                    {
                        _changedProperties.Add(nameof(Root));
                        HasChanges = true;
                    }
                }
                if (_validatedPath.HasValue && ReferenceEquals(_validatedPath.Value.Root, newValue))
                    Path = _validatedPath.Value.Path;
                else
                    Subdirectory.LookupFullNameAsync(newValue).ContinueWith(r =>
                    {
                        if (!r.IsCanceled)
                        {
                            string path = r.Result;
                            _validatedPath = (newValue, path);
                            Dispatcher.Invoke(() => Path = path);
                        }
                    });
            }
        }

        #endregion

        #region Path Property

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

        #endregion

        #region DisplayName Property

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
            if ((newValue = newValue.AsWsNormalizedOrEmpty()) == Model.DisplayName)
            {
                if (_changedProperties.Remove(nameof(DisplayName)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(DisplayName)))
            {
                _changedProperties.Add(nameof(DisplayName));
                HasChanges = true;
            }
        }

        #endregion

        #region MaxRecursionDepth Property

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
            if (newValue == Model.MaxRecursionDepth)
            {
                if (_changedProperties.Remove(nameof(MaxRecursionDepth)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(MaxRecursionDepth)))
            {
                _changedProperties.Add(nameof(MaxRecursionDepth));
                HasChanges = true;
            }
        }

        #endregion

        #region LimitTotalItems Property

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
            if (Model.MaxTotalItems.HasValue ? newValue && MaxTotalItems == Model.MaxTotalItems.Value : !newValue)
            {
                if (_changedProperties.Remove(nameof(MaxTotalItems)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(MaxTotalItems)))
            {
                _changedProperties.Add(nameof(MaxTotalItems));
                HasChanges = true;
            }
        }

        #endregion

        #region MaxTotalItems Property

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
            if (Model.MaxTotalItems.HasValue ? LimitTotalItems && MaxTotalItems == Model.MaxTotalItems.Value : !LimitTotalItems)
            {
                if (_changedProperties.Remove(nameof(MaxTotalItems)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(MaxTotalItems)))
            {
                _changedProperties.Add(nameof(MaxTotalItems));
                HasChanges = true;
            }
        }

        #endregion

        #region LimitTTL Property

        public static readonly DependencyProperty LimitTTLProperty = DependencyProperty.Register(nameof(LimitTTL), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnLimitTTLPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

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
            if (Model.TTL.HasValue ? newValue && TTL == TimeSpan.FromSeconds(Model.TTL.Value) : !newValue)
            {
                if (_changedProperties.Remove(nameof(TTL)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(TTL)))
            {
                _changedProperties.Add(nameof(TTL));
                HasChanges = true;
            }
        }

        #endregion

        #region TTL Property

        public static readonly DependencyProperty TTLProperty = DependencyProperty.Register(nameof(TTL), typeof(TimeSpan), typeof(EditCrawlConfigVM),
                new PropertyMetadata(TimeSpan.Zero, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnTTLPropertyChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue)));

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
            if (Model.TTL.HasValue ? LimitTTL && newValue == TimeSpan.FromSeconds(Model.TTL.Value) : !LimitTTL)
            {
                if (_changedProperties.Remove(nameof(TTL)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(TTL)))
            {
                _changedProperties.Add(nameof(TTL));
                HasChanges = true;
            }
        }

        #endregion

        #region TtlDays Property

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

        #region TtlHours Property

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

        #region TtlMinutes Property

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

        #region TtlSeconds Property

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

        #region StatusValue Property

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
            if (newValue == Model.StatusValue)
            {
                if (_changedProperties.Remove(nameof(StatusValue)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(StatusValue)))
            {
                _changedProperties.Add(nameof(StatusValue));
                HasChanges = true;
            }
        }

        #endregion

        #region EnabledStatus Property

        private static readonly DependencyPropertyKey EnabledStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnabledStatus), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning));

        public static readonly DependencyProperty EnabledStatusProperty = EnabledStatusPropertyKey.DependencyProperty;

        public CrawlStatus EnabledStatus
        {
            get => (CrawlStatus)GetValue(EnabledStatusProperty);
            private set => SetValue(EnabledStatusPropertyKey, value);
        }

        #endregion

        #region IsEnabled Property

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

        #region LastCrawlStart Property

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        #endregion

        #region LastCrawlEnd Property

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        #endregion

        #region NextScheduledStart Property

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

        #endregion

        #region Notes Property

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
            if ((newValue ?? "") == Model.Notes)
            {
                if (_changedProperties.Remove(nameof(Notes)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(Notes)))
            {
                _changedProperties.Add(nameof(Notes));
                HasChanges = true;
            }
        }

        #endregion

        #region AutoReschedule Property

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
            if (Model.RescheduleInterval.HasValue ? newValue && RescheduleInterval == TimeSpan.FromSeconds(Model.RescheduleInterval.Value) : !newValue)
            {
                if (_changedProperties.Remove(nameof(RescheduleInterval)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(RescheduleInterval)))
            {
                _changedProperties.Add(nameof(RescheduleInterval));
                HasChanges = true;
            }
        }

        #endregion

        #region RescheduleInterval Property

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
            if (Model.RescheduleInterval.HasValue ? AutoReschedule && newValue == TimeSpan.FromSeconds(Model.RescheduleInterval.Value) : !AutoReschedule)
            {
                if (_changedProperties.Remove(nameof(RescheduleInterval)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(RescheduleInterval)))
            {
                _changedProperties.Add(nameof(RescheduleInterval));
                HasChanges = true;
            }
        }

        #endregion

        #region RescheduleDays Property

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

        #region RescheduleHours Property

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

        #region RescheduleMinutes Property

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

        #region RescheduleSeconds Property

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

        #region RescheduleAfterFail Property

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
            if (newValue == Model.RescheduleAfterFail)
            {
                if (_changedProperties.Remove(nameof(RescheduleAfterFail)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(RescheduleAfterFail)))
            {
                _changedProperties.Add(nameof(RescheduleAfterFail));
                HasChanges = true;
            }
        }

        #endregion

        #region RescheduleFromJobEnd Property

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
            if (newValue == Model.RescheduleFromJobEnd)
            {
                if (_changedProperties.Remove(nameof(RescheduleFromJobEnd)) && _changedProperties.Count == 0)
                    HasChanges = false;
            }
            else if (!_changedProperties.Contains(nameof(RescheduleFromJobEnd)))
            {
                _changedProperties.Add(nameof(RescheduleFromJobEnd));
                HasChanges = true;
            }
        }

        #endregion

        #region CreatedOn Property

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        #endregion

        #region ModifiedOn Property

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        #endregion

        #region LastSynchronizedOn Property

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

        #endregion

        #region BgOpStatus Property

        private static readonly DependencyPropertyKey BgOpStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatus), typeof(AsyncOpStatusCode), typeof(EditCrawlConfigVM),
                new PropertyMetadata(AsyncOpStatusCode.NotStarted));

        public static readonly DependencyProperty BgOpStatusProperty = BgOpStatusPropertyKey.DependencyProperty;

        /// <summary>Controls visiblity of modal elements:</summary>
        /// <remarks>
        /// Values affect the UI as follows:
        /// <list type="table">
        /// <item>
        ///     <description>Values</description>
        ///     <description>Visibility</description>
        ///     <description>Button Text</description>
        ///     <description>State Name</description>
        /// </item>
        /// <item>
        ///     <description><see cref="AsyncOpStatusCode.NotStarted"/>, <see cref="AsyncOpStatusCode.RanToCompletion"/></description>
        ///     <description><see cref="Visibility.Collapsed"/></description>
        ///     <description>(n/a)</description>
        ///     <description>NotActive</description>
        /// </item>
        /// <item>
        ///     <description><see cref="AsyncOpStatusCode.Running"/>, <see cref="AsyncOpStatusCode.CancellationPending"/></description>
        ///     <description><see cref="Visibility.Visible"/></description>
        ///     <description>Cancel</description>
        ///     <description>Active</description>
        /// </item>
        /// <item>
        ///     <description><see cref="AsyncOpStatusCode.Canceled"/>, <see cref="AsyncOpStatusCode.Faulted"/></description>
        ///     <description><see cref="Visibility.Visible"/></description>
        ///     <description>OK</description>
        ///     <description>Aborted</description>
        /// </item>
        /// </list></remarks>
        public AsyncOpStatusCode BgOpStatus
        {
            get => (AsyncOpStatusCode)GetValue(BgOpStatusProperty);
            private set => SetValue(BgOpStatusPropertyKey, value);
        }

        #endregion

        #region BgOpMessageLevel Property

        private static readonly DependencyPropertyKey BgOpMessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpMessageLevel), typeof(StatusMessageLevel), typeof(EditCrawlConfigVM),
                new PropertyMetadata(StatusMessageLevel.Information));

        public static readonly DependencyProperty BgOpMessageLevelProperty = BgOpMessageLevelPropertyKey.DependencyProperty;

        public StatusMessageLevel BgOpMessageLevel
        {
            get => (StatusMessageLevel)GetValue(BgOpMessageLevelProperty);
            private set => SetValue(BgOpMessageLevelPropertyKey, value);
        }

        #endregion

        #region BgOpStatusMessage Property

        private static readonly DependencyPropertyKey BgOpStatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOpStatusMessage), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty BgOpStatusMessageProperty = BgOpStatusMessagePropertyKey.DependencyProperty;

        public string BgOpStatusMessage
        {
            get => GetValue(BgOpStatusMessageProperty) as string;
            private set => SetValue(BgOpStatusMessagePropertyKey, value);
        }

        #endregion

        #region BgOpDuration Property

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

        #endregion

        #endregion

        public EditCrawlConfigVM()
        {
            System.Windows.Controls.Border b;
            SetValue(SelectRootCommandPropertyKey, new Commands.RelayCommand(OnSelectRootExecute));
            SetValue(SaveCommandPropertyKey, new Commands.RelayCommand(OnSaveExecute));
            SetValue(CancelCommandPropertyKey, new Commands.RelayCommand(OnCancelExecute));
            SetValue(PopupButtonClickCommandPropertyKey, new Commands.RelayCommand(OnPopupButtonClickExecute));
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
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

            EventHandler closeCancelHandler = new EventHandler((sender, e) => window.DialogResult = false);
            vm.CloseCancel += closeCancelHandler;
            vm.PopupButtonClick += closeCancelHandler;
            vm.CloseSuccess += new EventHandler((sender, e) => window.DialogResult = true);
            window.Loaded += new RoutedEventHandler((sender, e) =>
            {
                // Task<(CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)>
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

        private void LookupCrawlConfig_DurationPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BgOpDuration = (e.NewValue as TimeSpan?) ?? TimeSpan.Zero;
        }

        private void LookupCrawlConfig_MessageLevelPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BgOpMessageLevel = (StatusMessageLevel)e.NewValue;
        }

        private void LookupCrawlConfig_StatusMessagePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BgOpStatusMessage = (e.NewValue as string) ?? "";
        }

        private void LookupCrawlConfig_AsyncOpStatusPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BgOpStatus = (AsyncOpStatusCode)e.NewValue;
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
                HasChanges = true;
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
            HasChanges = false;
            WindowTitle = "Edit Crawl Configuration";
        }

        private AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)> LookupCrawlConfigAsync(string path) =>
            AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.FromAsync(path, LookupCrawlConfigOpMgr, LookupCrawlConfigOpMgr.LookupCrawlConfig);
    }

    public class LookupCrawlConfigAsyncOpManager : AsyncOpResultManagerViewModel<string, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>
    {
        internal async Task<(CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)> LookupCrawlConfig(string path, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            statusListener.SetMessage("Checking for existing directory information", StatusMessageLevel.Information);
            DirectoryInfo crawlRoot;
            try { crawlRoot = new DirectoryInfo(path); }
            catch (System.Security.SecurityException securityException)
            {
                throw new AsyncOperationFailureException(securityException.Message, ErrorCode.SecurityException,
                    FsInfoCat.Properties.Resources.ErrorMessage_SecurityException, securityException);
            }
            catch (PathTooLongException pathTooLongException)
            {
                throw new AsyncOperationFailureException(pathTooLongException.Message, ErrorCode.PathTooLong,
                    FsInfoCat.Properties.Resources.ErrorMessage_PathTooLongError, pathTooLongException);
            }
            catch (Exception exception)
            {
                throw new AsyncOperationFailureException(exception.Message, ErrorCode.InvalidPath, FsInfoCat.Properties.Resources.ErrorMessage_InvalidPathError,
                    exception);
            }
            if (!crawlRoot.Exists)
                throw new AsyncOperationFailureException(FsInfoCat.Properties.Resources.ErrorMessage_DirectoryNotFound, ErrorCode.DirectoryNotFound);
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<Subdirectory> subdirectory = await Subdirectory.ImportBranchAsync(crawlRoot, dbContext, statusListener.CancellationToken);
            CrawlConfiguration crawlConfiguration;
            if (subdirectory.State == EntityState.Added)
            {
                statusListener.SetMessage("Importing new path information");
                await dbContext.SaveChangesAsync(statusListener.CancellationToken);
                crawlConfiguration = null;
            }
            else
            {
                statusListener.SetMessage("Checking for existing configuration");
                crawlConfiguration = await subdirectory.GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
            }
            return (crawlConfiguration, subdirectory.Entity, crawlRoot.FullName);
            //Dispatcher.Invoke(() => vm.Initialize(crawlConfiguration, subdirectory.Entity, crawlRoot.FullName));
        }
    }
}
