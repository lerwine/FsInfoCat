using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SubdirectoryRowViewModel<TEntity> : FsItemRowViewModel<TEntity>, ISubdirectoryRowViewModel
        where TEntity : Model.DbEntity, Model.ISubdirectoryRow
    {
        #region Options Property Members

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = ColumnPropertyBuilder<Model.DirectoryCrawlOptions, SubdirectoryRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryRow.Options))
            .DefaultValue(Model.DirectoryCrawlOptions.None)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryRowViewModel<TEntity>)?.OnOptionsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.DirectoryCrawlOptions Options { get => (Model.DirectoryCrawlOptions)GetValue(OptionsProperty); set => SetValue(OptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Options"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Options"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Options"/> property.</param>
        protected virtual void OnOptionsPropertyChanged(Model.DirectoryCrawlOptions oldValue, Model.DirectoryCrawlOptions newValue) { }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<Model.DirectoryStatus, SubdirectoryRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryRow.Status))
            .DefaultValue(Model.DirectoryStatus.Incomplete)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.DirectoryStatus Status { get => (Model.DirectoryStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(Model.DirectoryStatus oldValue, Model.DirectoryStatus newValue) { }

        #endregion

        Model.ISubdirectoryRow ISubdirectoryRowViewModel.Entity => Entity;

        public SubdirectoryRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Options = entity.Options;
            Status = entity.Status;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.ISubdirectoryRow.Options):
                    Dispatcher.CheckInvoke(() => Options = Entity.Options);
                    break;
                case nameof(Model.ISubdirectoryRow.Status):
                    Dispatcher.CheckInvoke(() => Status = Entity.Status);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
