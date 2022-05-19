using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileRowViewModel<TEntity> : FsItemRowViewModel<TEntity>, IFileRowViewModel
        where TEntity : Model.DbEntity, Model.IFileRow
    {
        #region Options Property Members

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = ColumnPropertyBuilder<Model.FileCrawlOptions, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileRow.Options))
            .DefaultValue(Model.FileCrawlOptions.None)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnOptionsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.FileCrawlOptions Options { get => (Model.FileCrawlOptions)GetValue(OptionsProperty); set => SetValue(OptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Options"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Options"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Options"/> property.</param>
        protected virtual void OnOptionsPropertyChanged(Model.FileCrawlOptions oldValue, Model.FileCrawlOptions newValue) { }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<Model.FileCorrelationStatus, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileRow.Status))
            .DefaultValue(Model.FileCorrelationStatus.Dissociated)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.FileCorrelationStatus Status { get => (Model.FileCorrelationStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(Model.FileCorrelationStatus oldValue, Model.FileCorrelationStatus newValue) { }

        #endregion
        #region LastHashCalculation Property Members

        /// <summary>
        /// Identifies the <see cref="LastHashCalculation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastHashCalculationProperty = ColumnPropertyBuilder<DateTime?, FileRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileRow.LastHashCalculation))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as FileRowViewModel<TEntity>)?.OnLastHashCalculationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? LastHashCalculation { get => (DateTime?)GetValue(LastHashCalculationProperty); set => SetValue(LastHashCalculationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastHashCalculation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastHashCalculation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastHashCalculation"/> property.</param>
        protected virtual void OnLastHashCalculationPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion

        Model.IFileRow IFileRowViewModel.Entity => Entity;

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
                case nameof(Model.IFileRow.Options):
                    Dispatcher.CheckInvoke(() => Options = Entity.Options);
                    break;
                case nameof(Model.IFileRow.Status):
                    Dispatcher.CheckInvoke(() => Status = Entity.Status);
                    break;
                case nameof(Model.IFileRow.LastHashCalculation):
                    Dispatcher.CheckInvoke(() => LastHashCalculation = Entity.LastHashCalculation);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
