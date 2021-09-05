using System;
using System.Diagnostics.CodeAnalysis;
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
        public static readonly DependencyProperty RootPathProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.RootPath))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnRootPathPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        private void OnRootPathPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StatusCode Property Members

        /// <summary>
        /// Identifies the <see cref="StatusCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusCodeProperty = ColumnPropertyBuilder<CrawlStatus, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.StatusCode))
            .DefaultValue(CrawlStatus.NotRunning)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusCodePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        private void OnStatusCodePropertyChanged(CrawlStatus oldValue, CrawlStatus newValue) { }

        #endregion
        #region CrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlStartProperty = ColumnPropertyBuilder<DateTime, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.CrawlStart))
            .DefaultValue(default(DateTime))
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlStartPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        private void OnCrawlStartPropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion
        #region CrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlEndProperty = ColumnPropertyBuilder<DateTime?, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.CrawlEnd))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlEndPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        private void OnCrawlEndPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region StatusMessage Property Members

        /// <summary>
        /// Identifies the <see cref="StatusMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusMessageProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.StatusMessage))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusMessagePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        private void OnStatusMessagePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StatusDetail Property Members

        /// <summary>
        /// Identifies the <see cref="StatusDetail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusDetailProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlJobLogRow.StatusDetail))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusDetailPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        private void OnStatusDetailPropertyChanged(string oldValue, string newValue) { }

        #endregion

        protected CrawlJobRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            // TODO: Initialize properties
        }
    }
}
