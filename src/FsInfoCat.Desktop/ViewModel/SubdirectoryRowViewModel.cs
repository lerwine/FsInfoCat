using System.Diagnostics.CodeAnalysis;
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
        public static readonly DependencyProperty OptionsProperty = ColumnPropertyBuilder<DirectoryCrawlOptions, SubdirectoryRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryRow.Options))
            .DefaultValue(DirectoryCrawlOptions.None)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryRowViewModel<TEntity>)?.OnOptionsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DirectoryCrawlOptions Options { get => (DirectoryCrawlOptions)GetValue(OptionsProperty); set => SetValue(OptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Options"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Options"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Options"/> property.</param>
        protected virtual void OnOptionsPropertyChanged(DirectoryCrawlOptions oldValue, DirectoryCrawlOptions newValue) { }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<DirectoryStatus, SubdirectoryRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryRow.Status))
            .DefaultValue(DirectoryStatus.Incomplete)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DirectoryStatus Status { get => (DirectoryStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(DirectoryStatus oldValue, DirectoryStatus newValue) { }

        #endregion

        public SubdirectoryRowViewModel([DisallowNull] TEntity entity) : base(entity)
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
