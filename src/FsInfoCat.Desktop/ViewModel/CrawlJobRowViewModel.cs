using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlJobRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlJobLogRow
    {
        #region RootPath Property Members

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = DependencyProperty.Register(nameof(RootPath), typeof(string), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CrawlJobRowViewModel<TEntity>)?.OnRootPathPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string RootPath { get => GetValue(RootPathProperty) as string; set => SetValue(RootPathProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RootPath"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RootPath"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RootPath"/> property.</param>
        private void OnRootPathPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnRootPathPropertyChanged Logic
        }

        #endregion
        #region StatusCode Property Members

        /// <summary>
        /// Identifies the <see cref="StatusCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusCodeProperty = DependencyProperty.Register(nameof(StatusCode), typeof(CrawlStatus), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata(CrawlStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlJobRowViewModel<TEntity>)?.OnStatusCodePropertyChanged((CrawlStatus)e.OldValue, (CrawlStatus)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public CrawlStatus StatusCode { get => (CrawlStatus)GetValue(StatusCodeProperty); set => SetValue(StatusCodeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusCode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusCode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusCode"/> property.</param>
        private void OnStatusCodePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue)
        {
            // TODO: Implement OnStatusCodePropertyChanged Logic
        }

        #endregion
        #region CrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlStartProperty = DependencyProperty.Register(nameof(CrawlStart), typeof(DateTime), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata(default(DateTime), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlStartPropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime CrawlStart { get => (DateTime)GetValue(CrawlStartProperty); set => SetValue(CrawlStartProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CrawlStart"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CrawlStart"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CrawlStart"/> property.</param>
        private void OnCrawlStartPropertyChanged(DateTime oldValue, DateTime newValue)
        {
            // TODO: Implement OnCrawlStartPropertyChanged Logic
        }

        #endregion
        #region CrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlEndProperty = DependencyProperty.Register(nameof(CrawlEnd), typeof(DateTime?), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlEndPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? CrawlEnd { get => (DateTime?)GetValue(CrawlEndProperty); set => SetValue(CrawlEndProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CrawlEnd"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CrawlEnd"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CrawlEnd"/> property.</param>
        private void OnCrawlEndPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnCrawlEndPropertyChanged Logic
        }

        #endregion
        #region StatusMessage Property Members

        /// <summary>
        /// Identifies the <see cref="StatusMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusMessageProperty = DependencyProperty.Register(nameof(StatusMessage), typeof(string), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlJobRowViewModel<TEntity>)?.OnStatusMessagePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string StatusMessage { get => GetValue(StatusMessageProperty) as string; set => SetValue(StatusMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusMessage"/> property.</param>
        private void OnStatusMessagePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStatusMessagePropertyChanged Logic
        }

        #endregion
        #region StatusDetail Property Members

        /// <summary>
        /// Identifies the <see cref="StatusDetail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusDetailProperty = DependencyProperty.Register(nameof(StatusDetail), typeof(string), typeof(CrawlJobRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlJobRowViewModel<TEntity>)?.OnStatusDetailPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string StatusDetail { get => GetValue(StatusDetailProperty) as string; set => SetValue(StatusDetailProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusDetail"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusDetail"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusDetail"/> property.</param>
        private void OnStatusDetailPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStatusDetailPropertyChanged Logic
        }

        #endregion

        protected CrawlJobRowViewModel(TEntity entity) : base(entity)
        {

        }
    }
}
