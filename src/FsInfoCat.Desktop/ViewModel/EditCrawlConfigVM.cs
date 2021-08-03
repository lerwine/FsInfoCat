using FsInfoCat.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EditCrawlConfigVM : DependencyObject
    {
        private Collection<string> _changedProperties = new();
        private (Subdirectory Root, string Path)? _validatedPath;

        #region Properties

        private static readonly DependencyPropertyKey WindowTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(WindowTitle), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata("Edit New Crawl Configuration"));

        public static readonly DependencyProperty WindowTitleProperty = WindowTitlePropertyKey.DependencyProperty;

        public string WindowTitle
        {
            get { return GetValue(WindowTitleProperty) as string; }
            private set { SetValue(WindowTitlePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HasChangesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasChanges), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(false));

        public static readonly DependencyProperty HasChangesProperty = HasChangesPropertyKey.DependencyProperty;

        public bool HasChanges
        {
            get => (bool)GetValue(HasChangesProperty);
            private set => SetValue(HasChangesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Model), typeof(CrawlConfiguration), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public CrawlConfiguration Model
        {
            get => (CrawlConfiguration)GetValue(ModelProperty);
            private set => SetValue(ModelPropertyKey, value);
        }

        private static readonly DependencyPropertyKey RootPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Root), typeof(Subdirectory), typeof(EditCrawlConfigVM),
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

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(EditCrawlConfigVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path
        {
            get => GetValue(PathProperty) as string;
            private set => SetValue(PathPropertyKey, value);
        }

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
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this) || Model is null)
                return;
#else
            if (Model is null)
                return;
#endif
            TtlDays = newValue.Days;
            TtlHours = newValue.Hours;
            TtlMinutes = newValue.Minutes;
            TtlSeconds = newValue.Seconds;
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
            TimeSpan timeSpan = TTL;
            if (timeSpan.Days != newValue)
                TTL = new TimeSpan(newValue, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

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
            TimeSpan timeSpan = TTL;
            if (timeSpan.Hours != newValue)
                TTL = new TimeSpan(timeSpan.Days, newValue, timeSpan.Minutes, timeSpan.Seconds);
        }

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
            TimeSpan timeSpan = TTL;
            if (timeSpan.Minutes != newValue)
                TTL = new TimeSpan(timeSpan.Days, timeSpan.Hours, newValue, timeSpan.Seconds);
        }

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
            TimeSpan timeSpan = TTL;
            if (timeSpan.Minutes != newValue)
                TTL = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, newValue);
        }

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

        private static readonly DependencyPropertyKey EnabledStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnabledStatus), typeof(CrawlStatus), typeof(EditCrawlConfigVM),
                new PropertyMetadata(CrawlStatus.NotRunning));

        public static readonly DependencyProperty EnabledStatusProperty = EnabledStatusPropertyKey.DependencyProperty;

        public CrawlStatus EnabledStatus
        {
            get => (CrawlStatus)GetValue(EnabledStatusProperty);
            private set => SetValue(EnabledStatusPropertyKey, value);
        }

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(EditCrawlConfigVM),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as EditCrawlConfigVM).OnIsEnabledPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        protected virtual void OnIsEnabledPropertyChanged(bool oldValue, bool newValue) => StatusValue = newValue ? EnabledStatus : CrawlStatus.Disabled;

        private static readonly DependencyPropertyKey LastCrawlEndPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlEnd), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlEndProperty = LastCrawlEndPropertyKey.DependencyProperty;

        public DateTime? LastCrawlEnd
        {
            get => (DateTime?)GetValue(LastCrawlEndProperty);
            private set => SetValue(LastCrawlEndPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastCrawlStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastCrawlStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastCrawlStartProperty = LastCrawlStartPropertyKey.DependencyProperty;

        public DateTime? LastCrawlStart
        {
            get => (DateTime?)GetValue(LastCrawlStartProperty);
            private set => SetValue(LastCrawlStartPropertyKey, value);
        }

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NextScheduledStart), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart
        {
            get => (DateTime?)GetValue(NextScheduledStartProperty);
            private set => SetValue(NextScheduledStartPropertyKey, value);
        }

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

        private static readonly DependencyPropertyKey CreatedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreatedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty CreatedOnProperty = CreatedOnPropertyKey.DependencyProperty;

        public DateTime CreatedOn
        {
            get => (DateTime)GetValue(CreatedOnProperty);
            private set => SetValue(CreatedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ModifiedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModifiedOn), typeof(DateTime), typeof(EditCrawlConfigVM),
                new PropertyMetadata(default));

        public static readonly DependencyProperty ModifiedOnProperty = ModifiedOnPropertyKey.DependencyProperty;

        public DateTime ModifiedOn
        {
            get => (DateTime)GetValue(ModifiedOnProperty);
            private set => SetValue(ModifiedOnPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(EditCrawlConfigVM),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn
        {
            get => (DateTime?)GetValue(LastSynchronizedOnProperty);
            private set => SetValue(LastSynchronizedOnPropertyKey, value);
        }

#endregion

        internal static CrawlConfiguration Edit(DirectoryInfo crawlRoot)
        {
            View.EditCrawlConfigWindow window = new();
            EditCrawlConfigVM vm = (EditCrawlConfigVM)window.DataContext;
            if (vm is null)
            {
                vm = new();
                window.DataContext = vm;
            }
            vm.Initialize(crawlRoot);
            if (window.ShowDialog() ?? false)
            {
                CrawlConfiguration model = vm.Model;
                if (model is null)
                {
                    // TODO: Initialize new model object
                }
                return model;
            }
            return null;
        }

        internal static void UpsertItem(CrawlConfiguration item, ReadOnlyObservableCollection<CrawlConfigurationVM> crawlConfigurations, List<CrawlConfigurationVM> allCrawlConfigurations, bool showActive, bool showInactive)
        {
            // TODO: Add or update view model item
            throw new NotImplementedException();
        }

        internal void Initialize([DisallowNull] DirectoryInfo directoryInfo)
        {
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            Subdirectory.ImportBranchAsync(directoryInfo, dbContext).ContinueWith(async t =>
            {
                if (!t.IsCanceled)
                {
                    Subdirectory subdirectory = t.Result;
                    CrawlConfiguration crawlConfiguration = await dbContext.Entry(subdirectory).GetRelatedReferenceAsync(d => d.CrawlConfiguration, CancellationToken.None);
                    Dispatcher.Invoke(() =>
                    {
                        _validatedPath = (subdirectory, directoryInfo.FullName);
                        if (crawlConfiguration is not null)
                            Initialize(crawlConfiguration);
                        else
                        {
                            Root = subdirectory;
                            CreatedOn = ModifiedOn = DateTime.Now;
                            MaxTotalItems = int.MaxValue;
                            TTL = TimeSpan.FromDays(1.0);
                            RescheduleInterval = TimeSpan.FromDays(1.0);
                            HasChanges = true;
                        }
                    });
                }
            }).ContinueWith(t =>
            {
                try { dbContext.Dispose(); }
                finally { serviceScope.Dispose(); }
            });
        }

        internal void Initialize([DisallowNull] CrawlConfiguration crawlConfiguration)
        {
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
    }
}
