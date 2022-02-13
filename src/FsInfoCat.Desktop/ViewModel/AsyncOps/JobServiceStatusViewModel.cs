using FsInfoCat.Activities;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    /// <summary>
    /// View model for representing activities being run by the <see cref="IAsyncActivityService"/>.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public partial class JobServiceStatusViewModel : DependencyObject
    {
        private readonly ILogger<JobServiceStatusViewModel> _logger;
        private readonly ActivityStartObserver _activityStartObserver;
        private readonly ActiveStateObserver _activeStateObserver;

        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<BackgroundJobVM>), typeof(JobServiceStatusViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets currently active background job view model items.
        /// </summary>
        /// <value>The collection <see cref="BackgroundJobVM"/> items respresenting actvities being run by the  <see cref="IAsyncActivityService"/>.</value>
        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion
        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool),
            typeof(JobServiceStatusViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        /// <summary>
        /// Indicates whether there is at least one background job to be displayed.
        /// </summary>
        /// <value><see langword="true"/> if there is at least one <see cref="BackgroundJobVM"/> in the <see cref="Items"/> collection; otherwise, <see langword="false"/>.</value>
        public bool IsBusy { get => (bool)GetValue(IsBusyProperty); private set => SetValue(IsBusyPropertyKey, value); }

        #endregion

        public JobServiceStatusViewModel()
        {
            _logger = App.GetLogger(this);
            _logger.LogDebug($"{nameof(JobServiceStatusViewModel)} constructor invoked");
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
            IAsyncActivityService backgroundService = Hosting.GetAsyncActivityService();
            _activityStartObserver = new(this, backgroundService);
            _activeStateObserver = new(this, backgroundService);
            _logger.LogDebug($"{nameof(JobServiceStatusViewModel)} Service instantiated");
        }
    }
}
