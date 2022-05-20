using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundancyRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IRedundancy
    {
        #region Reference Property Members

        /// <summary>
        /// Identifies the <see cref="Reference"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReferenceProperty = ColumnPropertyBuilder<string, RedundancyRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundancy.Reference))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundancyRowViewModel<TEntity>)?.OnReferencePropertyChanged(oldValue, newValue))
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
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, RedundancyRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundancy.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundancyRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public RedundancyRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Reference = entity.Reference;
            Notes = entity.Notes;
        }
    }
}
