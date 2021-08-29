using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SubdirectoryRowViewModel<TEntity> : FsItemRowViewModel<TEntity>
        where TEntity : DbEntity, ISubdirectoryRow
    {
        #region Options Property Members

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(DirectoryCrawlOptions),
            typeof(SubdirectoryRowViewModel<TEntity>), new PropertyMetadata(DirectoryCrawlOptions.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SubdirectoryRowViewModel<TEntity>)?.OnOptionsPropertyChanged((DirectoryCrawlOptions)e.OldValue, (DirectoryCrawlOptions)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DirectoryCrawlOptions Options { get => (DirectoryCrawlOptions)GetValue(OptionsProperty); set => SetValue(OptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Options"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Options"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Options"/> property.</param>
        private void OnOptionsPropertyChanged(DirectoryCrawlOptions oldValue, DirectoryCrawlOptions newValue)
        {
            // TODO: Implement OnOptionsPropertyChanged Logic
        }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(DirectoryStatus),
            typeof(SubdirectoryRowViewModel<TEntity>), new PropertyMetadata(DirectoryStatus.Incomplete, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SubdirectoryRowViewModel<TEntity>)?.OnStatusPropertyChanged((DirectoryStatus)e.OldValue, (DirectoryStatus)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DirectoryStatus Status { get => (DirectoryStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        private void OnStatusPropertyChanged(DirectoryStatus oldValue, DirectoryStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        #endregion

        public SubdirectoryRowViewModel(TEntity entity) : base(entity)
        {
            Options = entity.Options;
            Status = entity.Status;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISubdirectoryRow.Options):
                    Dispatcher.CheckInvoke(() => Options = Entity.Options);
                    break;
                case nameof(ISubdirectoryRow.Status):
                    Dispatcher.CheckInvoke(() => Status = Entity.Status);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
