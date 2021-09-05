using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobFactoryServiceViewModel : DependencyObject
    {
        [ServiceBuilderHandler]
#pragma warning disable IDE0051 // Remove unused private members
        private static void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0051 // Remove unused private members
        {
            services.AddSingleton<IWindowsAsyncJobFactoryService, AsyncJobService>()
                .AddSingleton<IAsyncJobFactoryService>(services => services.GetRequiredService<IWindowsAsyncJobFactoryService>());
        }

        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<BackgroundJobVM>), typeof(JobFactoryServiceViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion
        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool),
            typeof(JobFactoryServiceViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        public bool IsBusy { get => (bool)GetValue(IsBusyProperty); private set => SetValue(IsBusyPropertyKey, value); }

        #endregion

        public JobFactoryServiceViewModel()
        {
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
        }

        private void AddJob(BackgroundJobVM item, IAsyncJob job) => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
            {
                _backingItems.Add(item);
                job.Task.ContinueWith(task => RemoveJob(item));
            }
            IsBusy = _backingItems.Count > 0;
            return item;
        });

        private void RemoveJob(BackgroundJobVM item) => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
                _backingItems.Remove(item);
            IsBusy = _backingItems.Count > 0;
        });

        internal void CancelAll()
        {
            // TODO: Implement CancelAll()
            throw new NotImplementedException();
        }
    }
}
