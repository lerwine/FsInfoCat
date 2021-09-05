using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundancyRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IRedundancy
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Reference Property Members

        /// <summary>
        /// Identifies the <see cref="Reference"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReferenceProperty = ColumnPropertyBuilder<string, RedundancyRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRedundancy.Reference))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundancyRowViewModel<TEntity>)?.OnReferencePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
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
            .RegisterEntityMapped<TEntity>(nameof(IRedundancy.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RedundancyRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
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
#pragma warning restore IDE0060 // Remove unused parameter

        public RedundancyRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Reference = entity.Reference;
            Notes = entity.Notes;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IRedundancy.Reference):
                    Dispatcher.CheckInvoke(() => Reference = Entity.Reference);
                    break;
                case nameof(IRedundancy.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
