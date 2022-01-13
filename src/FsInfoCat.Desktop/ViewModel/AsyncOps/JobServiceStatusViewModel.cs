using FsInfoCat.AsyncOps;
using FsInfoCat.Activities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobServiceStatusViewModel : DependencyObject
    {
        private readonly ILogger<JobServiceStatusViewModel> _logger;
        private readonly EventObserver _eventObserver;
        private readonly ActiveStateObserver _activeStateObserver;

        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<BackgroundJobVM>), typeof(JobServiceStatusViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion
        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool),
            typeof(JobServiceStatusViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        public bool IsBusy { get => (bool)GetValue(IsBusyProperty); private set => SetValue(IsBusyPropertyKey, value); }

        #endregion

        private void OnOperationStarted([DisallowNull] ICancellableOperation progressInfo, MessageCode? code, [DisallowNull] IObservable<IBackgroundProgressEvent> observable) =>
            BackgroundJobVM.OnOperationStarted(Dispatcher, _backingItems, progressInfo, code, observable);

        public JobServiceStatusViewModel()
        {
            _logger = App.GetLogger(this);
            _logger.LogDebug($"{nameof(JobServiceStatusViewModel)} constructor invoked");
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
            IAsyncActivityService backgroundService = Hosting.GetAsyncActivityService();
            _eventObserver = new(this, backgroundService);
            _activeStateObserver = new(this, backgroundService);
            _logger.LogDebug($"{nameof(JobServiceStatusViewModel)} Service instantiated");
        }
    }
}
