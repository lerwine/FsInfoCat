using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundantSetRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IRedundantSetRow
    {
        #region Reference Property Members

        /// <summary>
        /// Identifies the <see cref="Reference"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReferenceProperty = ColumnPropertyBuilder<string, RedundantSetRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSetRow.Reference))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundantSetRowViewModel<TEntity>)?.OnReferencePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Reference { get => GetValue(ReferenceProperty) as string; set => SetValue(ReferenceProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Reference"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Reference"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Reference"/> property.</param>
        protected virtual void OnReferencePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, RedundantSetRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSetRow.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundantSetRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public RedundantSetRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Reference = entity.Reference;
            Notes = entity.Notes;
        }
    }
}
