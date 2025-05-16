using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlJobRowViewModel<TEntity>([DisallowNull] TEntity entity) : DbEntityRowViewModel<TEntity>(entity)
        where TEntity : Model.DbEntity, Model.ICrawlJobLogRow
    {
        #region RootPath Property Members

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.RootPath))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnRootPathPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string RootPath { get => GetValue(RootPathProperty) as string; set => SetValue(RootPathProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RootPath"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RootPath"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RootPath"/> property.</param>
        protected virtual void OnRootPathPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StatusCode Property Members

        /// <summary>
        /// Identifies the <see cref="StatusCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusCodeProperty = ColumnPropertyBuilder<Model.CrawlStatus, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.StatusCode))
            .DefaultValue(Model.CrawlStatus.NotRunning)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusCodePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.CrawlStatus StatusCode { get => (Model.CrawlStatus)GetValue(StatusCodeProperty); set => SetValue(StatusCodeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusCode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusCode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusCode"/> property.</param>
        protected virtual void OnStatusCodePropertyChanged(Model.CrawlStatus oldValue, Model.CrawlStatus newValue) { }

        #endregion
        #region MaxRecursionDepth Property Members

        /// <summary>
        /// Identifies the <see cref="MaxRecursionDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxRecursionDepthProperty = DependencyPropertyBuilder<CrawlJobRowViewModel<TEntity>, ushort>
            .Register(nameof(MaxRecursionDepth))
            .DefaultValue(DbConstants.DbColDefaultValue_MaxRecursionDepth)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnMaxRecursionDepthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort MaxRecursionDepth { get => (ushort)GetValue(MaxRecursionDepthProperty); set => SetValue(MaxRecursionDepthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxRecursionDepth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxRecursionDepth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxRecursionDepth"/> property.</param>
        protected virtual void OnMaxRecursionDepthPropertyChanged(ushort oldValue, ushort newValue) { }

        #endregion
        #region CrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlStartProperty = ColumnPropertyBuilder<DateTime, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.CrawlStart))
            .DefaultValue(default)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlStartPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime CrawlStart { get => (DateTime)GetValue(CrawlStartProperty); set => SetValue(CrawlStartProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CrawlStart"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CrawlStart"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CrawlStart"/> property.</param>
        protected virtual void OnCrawlStartPropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion
        #region CrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlEndProperty = ColumnPropertyBuilder<DateTime?, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.CrawlEnd))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnCrawlEndPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? CrawlEnd { get => (DateTime?)GetValue(CrawlEndProperty); set => SetValue(CrawlEndProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CrawlEnd"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CrawlEnd"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CrawlEnd"/> property.</param>
        protected virtual void OnCrawlEndPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region StatusMessage Property Members

        /// <summary>
        /// Identifies the <see cref="StatusMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusMessageProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.StatusMessage))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusMessagePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string StatusMessage { get => GetValue(StatusMessageProperty) as string; set => SetValue(StatusMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusMessage"/> property.</param>
        protected virtual void OnStatusMessagePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StatusDetail Property Members

        /// <summary>
        /// Identifies the <see cref="StatusDetail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusDetailProperty = ColumnPropertyBuilder<string, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.StatusDetail))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnStatusDetailPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string StatusDetail { get => GetValue(StatusDetailProperty) as string; set => SetValue(StatusDetailProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StatusDetail"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StatusDetail"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StatusDetail"/> property.</param>
        protected virtual void OnStatusDetailPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region FoldersProcessed Property Members

        /// <summary>
        /// Identifies the <see cref="FoldersProcessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FoldersProcessedProperty = ColumnPropertyBuilder<long, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.FoldersProcessed))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnFoldersProcessedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public long FoldersProcessed { get => (long)GetValue(FoldersProcessedProperty); set => SetValue(FoldersProcessedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FoldersProcessed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FoldersProcessed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FoldersProcessed"/> property.</param>
        protected virtual void OnFoldersProcessedPropertyChanged(long oldValue, long newValue) { }

        #endregion
        #region FilesProcessed Property Members

        /// <summary>
        /// Identifies the <see cref="FilesProcessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilesProcessedProperty = ColumnPropertyBuilder<long, CrawlJobRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ICrawlJobLogRow.FilesProcessed))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as CrawlJobRowViewModel<TEntity>)?.OnFilesProcessedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public long FilesProcessed { get => (long)GetValue(FilesProcessedProperty); set => SetValue(FilesProcessedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FilesProcessed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FilesProcessed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FilesProcessed"/> property.</param>
        protected virtual void OnFilesProcessedPropertyChanged(long oldValue, long newValue) { }

        #endregion
    }
}
