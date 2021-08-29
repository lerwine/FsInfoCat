using System;
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
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(FileCrawlOptions),
            typeof(FileRowViewModel<TEntity>), new PropertyMetadata(FileCrawlOptions.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileRowViewModel<TEntity>)?.OnOptionsPropertyChanged((FileCrawlOptions)e.OldValue, (FileCrawlOptions)e.NewValue)));

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
        private void OnOptionsPropertyChanged(FileCrawlOptions oldValue, FileCrawlOptions newValue)
        {
            // TODO: Implement OnOptionsPropertyChanged Logic
        }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(FileCorrelationStatus),
            typeof(FileRowViewModel<TEntity>), new PropertyMetadata(FileCorrelationStatus.Dissociated, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileRowViewModel<TEntity>)?.OnStatusPropertyChanged((FileCorrelationStatus)e.OldValue, (FileCorrelationStatus)e.NewValue)));

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
        private void OnStatusPropertyChanged(FileCorrelationStatus oldValue, FileCorrelationStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        #endregion
        #region LastHashCalculation Property Members

        /// <summary>
        /// Identifies the <see cref="LastHashCalculation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastHashCalculationProperty = DependencyProperty.Register(nameof(LastHashCalculation), typeof(DateTime?),
            typeof(FileRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FileRowViewModel<TEntity>)?.OnLastHashCalculationPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

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
        private void OnLastHashCalculationPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnLastHashCalculationPropertyChanged Logic
        }

        #endregion

        public FileRowViewModel(TEntity entity) : base(entity)
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
