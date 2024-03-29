using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FsItemRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>, IFsItemRowViewModel
        where TEntity : Model.DbEntity, Model.IDbFsItemRow
    {
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = ColumnPropertyBuilder<string, FsItemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDbFsItem.Name))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) =>(d as FsItemRowViewModel<TEntity>)?.OnNamePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Name"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Name"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Name"/> property.</param>
        protected virtual void OnNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region LastAccessed Property Members

        /// <summary>
        /// Identifies the <see cref="LastAccessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAccessedProperty = ColumnPropertyBuilder<DateTime, FsItemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDbFsItemRow.LastAccessed))
            .DefaultValue(default)
            .OnChanged((d, oldValue, newValue) => (d as FsItemRowViewModel<TEntity>)?.OnLastAccessedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime LastAccessed { get => (DateTime)GetValue(LastAccessedProperty); set => SetValue(LastAccessedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastAccessed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastAccessed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastAccessed"/> property.</param>
        protected virtual void OnLastAccessedPropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, FsItemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDbFsItem.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as FsItemRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CreationTime Property Members

        /// <summary>
        /// Identifies the <see cref="CreationTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreationTimeProperty = ColumnPropertyBuilder<DateTime, FsItemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDbFsItemRow.CreationTime))
            .DefaultValue(default)
            .OnChanged((d, oldValue, newValue) => (d as FsItemRowViewModel<TEntity>)?.OnCreationTimePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime CreationTime { get => (DateTime)GetValue(CreationTimeProperty); set => SetValue(CreationTimeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreationTime"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreationTime"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreationTime"/> property.</param>
        protected virtual void OnCreationTimePropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion
        #region LastWriteTime Property Members

        /// <summary>
        /// Identifies the <see cref="LastWriteTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastWriteTimeProperty = ColumnPropertyBuilder<DateTime, FsItemRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDbFsItemRow.LastWriteTime))
            .DefaultValue(default)
            .OnChanged((d, oldValue, newValue) => (d as FsItemRowViewModel<TEntity>)?.OnLastWriteTimePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime LastWriteTime { get => (DateTime)GetValue(LastWriteTimeProperty); set => SetValue(LastWriteTimeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastWriteTime"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastWriteTime"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastWriteTime"/> property.</param>
        protected virtual void OnLastWriteTimePropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion

        Model.IDbFsItemRow IFsItemRowViewModel.Entity => Entity;

        public FsItemRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Name = entity.Name;
            LastAccessed = entity.LastAccessed;
            Notes = entity.Notes;
            CreationTime = entity.CreationTime;
            LastWriteTime = entity.LastWriteTime;
        }
    }
}
