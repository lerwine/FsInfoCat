using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileRowViewModel<TEntity> : FsItemRowViewModel<TEntity>
        where TEntity : DbEntity, IFileRow
    {
        #region Options Property Members

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = ColumnPropertyBuilder<FileCrawlOptions, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileRow.Options))
            .DefaultValue(FileCrawlOptions.None)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnOptionsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public FileCrawlOptions Options { get => (FileCrawlOptions)GetValue(OptionsProperty); set => SetValue(OptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Options"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Options"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Options"/> property.</param>
        protected virtual void OnOptionsPropertyChanged(FileCrawlOptions oldValue, FileCrawlOptions newValue) { }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<FileCorrelationStatus, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileRow.Status))
            .DefaultValue(FileCorrelationStatus.Dissociated)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public FileCorrelationStatus Status { get => (FileCorrelationStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(FileCorrelationStatus oldValue, FileCorrelationStatus newValue) { }

        #endregion
        #region LastHashCalculation Property Members

        /// <summary>
        /// Identifies the <see cref="LastHashCalculation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastHashCalculationProperty = ColumnPropertyBuilder<DateTime?, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IFileRow.LastHashCalculation))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnLastHashCalculationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastHashCalculation { get => (DateTime?)GetValue(LastHashCalculationProperty); set => SetValue(LastHashCalculationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastHashCalculation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastHashCalculation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastHashCalculation"/> property.</param>
        protected virtual void OnLastHashCalculationPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion

        public FileRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Options = entity.Options;
            Status = entity.Status;
            LastHashCalculation = entity.LastHashCalculation;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileRow.Options):
                    Dispatcher.CheckInvoke(() => Options = Entity.Options);
                    break;
                case nameof(IFileRow.Status):
                    Dispatcher.CheckInvoke(() => Status = Entity.Status);
                    break;
                case nameof(IFileRow.LastHashCalculation):
                    Dispatcher.CheckInvoke(() => LastHashCalculation = Entity.LastHashCalculation);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
