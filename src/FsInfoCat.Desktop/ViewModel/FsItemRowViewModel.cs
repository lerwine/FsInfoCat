using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FsItemRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IDbFsItemRow
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(FsItemRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FsItemRowViewModel<TEntity>)?.OnNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Name"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Name"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Name"/> property.</param>
        protected void OnNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNamePropertyChanged Logic
        }

        #endregion
        #region LastAccessed Property Members

        /// <summary>
        /// Identifies the <see cref="LastAccessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAccessedProperty = DependencyProperty.Register(nameof(LastAccessed), typeof(DateTime),
            typeof(FsItemRowViewModel<TEntity>), new PropertyMetadata(default(DateTime), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FsItemRowViewModel<TEntity>)?.OnLastAccessedPropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastAccessed { get => (DateTime)GetValue(LastAccessedProperty); set => SetValue(LastAccessedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastAccessed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastAccessed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastAccessed"/> property.</param>
        protected void OnLastAccessedPropertyChanged(DateTime oldValue, DateTime newValue)
        {
            // TODO: Implement OnLastAccessedPropertyChanged Logic
        }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(FsItemRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FsItemRowViewModel<TEntity>)?.OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        #region CreationTime Property Members

        /// <summary>
        /// Identifies the <see cref="CreationTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreationTimeProperty = DependencyProperty.Register(nameof(CreationTime), typeof(DateTime),
            typeof(FsItemRowViewModel<TEntity>), new PropertyMetadata(default(DateTime), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FsItemRowViewModel<TEntity>)?.OnCreationTimePropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime CreationTime { get => (DateTime)GetValue(CreationTimeProperty); set => SetValue(CreationTimeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreationTime"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreationTime"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreationTime"/> property.</param>
        protected void OnCreationTimePropertyChanged(DateTime oldValue, DateTime newValue)
        {
            // TODO: Implement OnCreationTimePropertyChanged Logic
        }

        #endregion
        #region LastWriteTime Property Members

        /// <summary>
        /// Identifies the <see cref="LastWriteTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastWriteTimeProperty = DependencyProperty.Register(nameof(LastWriteTime), typeof(DateTime),
            typeof(FsItemRowViewModel<TEntity>), new PropertyMetadata(default(DateTime), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as FsItemRowViewModel<TEntity>)?.OnLastWriteTimePropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastWriteTime { get => (DateTime)GetValue(LastWriteTimeProperty); set => SetValue(LastWriteTimeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastWriteTime"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastWriteTime"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastWriteTime"/> property.</param>
        protected void OnLastWriteTimePropertyChanged(DateTime oldValue, DateTime newValue)
        {
            // TODO: Implement OnLastWriteTimePropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public FsItemRowViewModel(TEntity entity) : base(entity)
        {
            Name = entity.Name;
            LastAccessed = entity.LastAccessed;
            Notes = entity.Notes;
            CreationTime = entity.CreationTime;
            LastWriteTime = entity.LastWriteTime;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IDbFsItemRow.Name):
                    Dispatcher.CheckInvoke(() => Name = Entity.Name);
                    break;
                case nameof(IDbFsItemRow.LastAccessed):
                    Dispatcher.CheckInvoke(() => LastAccessed = Entity.LastAccessed);
                    break;
                case nameof(IDbFsItemRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(IDbFsItemRow.CreationTime):
                    Dispatcher.CheckInvoke(() => CreationTime = Entity.CreationTime);
                    break;
                case nameof(IDbFsItemRow.LastWriteTime):
                    Dispatcher.CheckInvoke(() => LastWriteTime = Entity.LastWriteTime);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
