using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlSourceItemVM : DependencyObject
    {
        public event EventHandler Edit;

        public event EventHandler SetActive;

        public event EventHandler SetInactive;

        public event EventHandler ToggleActive;

        public event EventHandler Delete;

        private static readonly DependencyPropertyKey IdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Id), typeof(Guid), typeof(CrawlSourceItemVM),
            new PropertyMetadata(Guid.Empty));

        public static readonly DependencyProperty IdProperty = IdPropertyKey.DependencyProperty;

        public Guid Id
        {
            get { return (Guid)GetValue(IdProperty); }
            private set { SetValue(IdPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey DirectoryPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Directory), typeof(string),
            typeof(CrawlSourceItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty DirectoryProperty = DirectoryPropertyKey.DependencyProperty;

        public string Directory
        {
            get { return GetValue(DirectoryProperty) as string; }
            private set { SetValue(DirectoryPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(CrawlSourceItemVM),
            new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name
        {
            get { return GetValue(NameProperty) as string; }
            private set { SetValue(NamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MaxCrawlDepthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxCrawlDepth), typeof(int),
            typeof(CrawlSourceItemVM), new PropertyMetadata(32));

        public static readonly DependencyProperty MaxCrawlDepthProperty = MaxCrawlDepthPropertyKey.DependencyProperty;

        public int MaxCrawlDepth
        {
            get { return (int)GetValue(MaxCrawlDepthProperty); }
            private set { SetValue(MaxCrawlDepthPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MaxCrawlItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxCrawlItems), typeof(int),
            typeof(CrawlSourceItemVM), new PropertyMetadata(65535));

        public static readonly DependencyProperty MaxCrawlItemsProperty = MaxCrawlItemsPropertyKey.DependencyProperty;

        public int MaxCrawlItems
        {
            get { return (int)GetValue(MaxCrawlItemsProperty); }
            private set { SetValue(MaxCrawlItemsPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey InactivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Inactive), typeof(bool),
            typeof(CrawlSourceItemVM), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlSourceItemVM).OnInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty InactiveProperty = InactivePropertyKey.DependencyProperty;

        public bool Inactive
        {
            get { return (bool)GetValue(InactiveProperty); }
            private set { SetValue(InactivePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey LastStatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastStatusMessage), typeof(string),
            typeof(CrawlSourceItemVM), new PropertyMetadata(""));

        public static readonly DependencyProperty LastStatusMessageProperty = LastStatusMessagePropertyKey.DependencyProperty;

        public string LastStatusMessage
        {
            get { return GetValue(LastStatusMessageProperty) as string; }
            private set { SetValue(LastStatusMessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TotalFileCount), typeof(int),
            typeof(CrawlSourceItemVM), new PropertyMetadata(0));

        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        public int TotalFileCount
        {
            get { return (int)GetValue(TotalFileCountProperty); }
            private set { SetValue(TotalFileCountPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey PossibleDuplicateCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PossibleDuplicateCount),
            typeof(int), typeof(CrawlSourceItemVM), new PropertyMetadata(0));

        public static readonly DependencyProperty PossibleDuplicateCountProperty = PossibleDuplicateCountPropertyKey.DependencyProperty;

        public int PossibleDuplicateCount
        {
            get { return (int)GetValue(PossibleDuplicateCountProperty); }
            private set { SetValue(PossibleDuplicateCountPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ConfirmedDuplicateCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConfirmedDuplicateCount),
            typeof(int), typeof(CrawlSourceItemVM), new PropertyMetadata(0));

        public static readonly DependencyProperty ConfirmedDuplicateCountProperty = ConfirmedDuplicateCountPropertyKey.DependencyProperty;

        public int ConfirmedDuplicateCount
        {
            get { return (int)GetValue(ConfirmedDuplicateCountProperty); }
            private set { SetValue(ConfirmedDuplicateCountPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey EditCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EditCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourceItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty EditCommandProperty = EditCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand EditCommand => (Commands.RelayCommand)GetValue(EditCommandProperty);

        private static readonly DependencyPropertyKey SetActiveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SetActiveCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourceItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SetActiveCommandProperty = SetActiveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SetActiveCommand => (Commands.RelayCommand)GetValue(SetActiveCommandProperty);

        private static readonly DependencyPropertyKey SetInactiveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SetInactiveCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourceItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty SetInactiveCommandProperty = SetInactiveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SetInactiveCommand => (Commands.RelayCommand)GetValue(SetInactiveCommandProperty);

        private static readonly DependencyPropertyKey ToggleActiveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ToggleActiveCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourceItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty ToggleActiveCommandProperty = ToggleActiveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ToggleActiveCommand => (Commands.RelayCommand)GetValue(ToggleActiveCommandProperty);

        private static readonly DependencyPropertyKey DeleteCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DeleteCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourceItemVM), new PropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandProperty = DeleteCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand DeleteCommand => (Commands.RelayCommand)GetValue(DeleteCommandProperty);

        public CrawlSourceItemVM()
        {
            SetValue(SetActiveCommandPropertyKey, new Commands.RelayCommand(() => SetActive?.Invoke(this, EventArgs.Empty)));
            SetValue(SetInactiveCommandPropertyKey, new Commands.RelayCommand(() => SetInactive?.Invoke(this, EventArgs.Empty)));
            SetValue(ToggleActiveCommandPropertyKey, new Commands.RelayCommand(() => ToggleActive?.Invoke(this, EventArgs.Empty)));
            SetValue(EditCommandPropertyKey, new Commands.RelayCommand(() => Edit?.Invoke(this, EventArgs.Empty)));
            SetValue(DeleteCommandPropertyKey, new Commands.RelayCommand(() => Delete?.Invoke(this, EventArgs.Empty)));
            OnInactivePropertyChanged(!Inactive, Inactive);
        }

        protected virtual void OnInactivePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                SetActiveCommand.IsEnabled = true;
                SetInactiveCommand.IsEnabled = false;
            }
            else
            {
                SetActiveCommand.IsEnabled = false;
                SetInactiveCommand.IsEnabled = true;
            }
            // TODO: Implement OnInactivePropertyChanged Logic
        }
    }
}
