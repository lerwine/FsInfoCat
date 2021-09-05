using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SymbolicNameRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ISymbolicNameRow
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = ColumnPropertyBuilder<string, SymbolicNameRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISymbolicNameRow.Name))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SymbolicNameRowViewModel<TEntity>)?.OnNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, SymbolicNameRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISymbolicNameRow.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SymbolicNameRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Priority Property Members

        /// <summary>
        /// Identifies the <see cref="Priority"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PriorityProperty = ColumnPropertyBuilder<int, SymbolicNameRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISymbolicNameRow.Priority))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as SymbolicNameRowViewModel<TEntity>)?.OnPriorityPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int Priority { get => (int)GetValue(PriorityProperty); set => SetValue(PriorityProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Priority"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Priority"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Priority"/> property.</param>
        protected virtual void OnPriorityPropertyChanged(int oldValue, int newValue) { }

        #endregion
        #region IsInactive Property Members

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = ColumnPropertyBuilder<bool, SymbolicNameRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISymbolicNameRow.IsInactive))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as SymbolicNameRowViewModel<TEntity>)?.OnIsInactivePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); set => SetValue(IsInactiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsInactive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsInactive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsInactive"/> property.</param>
        protected virtual void OnIsInactivePropertyChanged(bool oldValue, bool newValue) { }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public SymbolicNameRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Name = entity.Name;
            Notes = entity.Notes;
            Priority = entity.Priority;
            IsInactive = entity.IsInactive;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISymbolicNameRow.Name):
                    Dispatcher.CheckInvoke(() => Name = Entity.Name);
                    break;
                case nameof(ISymbolicNameRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(ISymbolicNameRow.Priority):
                    Dispatcher.CheckInvoke(() => Priority = Entity.Priority);
                    break;
                case nameof(ISymbolicNameRow.IsInactive):
                    Dispatcher.CheckInvoke(() => IsInactive = Entity.IsInactive);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
