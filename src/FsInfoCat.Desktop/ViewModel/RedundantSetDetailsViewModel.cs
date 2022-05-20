using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundantSetDetailsViewModel<TEntity, TBinaryPropertySetEntity, TBinaryPropertySetItem, TRedundancyEntity, TRedundancyItem>
        : RedundantSetRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IRedundantSet
        where TBinaryPropertySetEntity : Model.DbEntity, Model.IBinaryPropertySet
        where TBinaryPropertySetItem : BinaryPropertySetRowViewModel<TBinaryPropertySetEntity>
        where TRedundancyEntity : Model.DbEntity, Model.IRedundancy
        where TRedundancyItem : RedundancyRowViewModel<TRedundancyEntity>
    {
        #region BinaryProperties Property Members

        protected ObservableCollection<TBinaryPropertySetItem> BackingBinaryProperties { get; } = new();

        private static readonly DependencyPropertyKey BinaryPropertiesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TBinaryPropertySetItem>, RedundantSetDetailsViewModel<TEntity, TBinaryPropertySetEntity, TBinaryPropertySetItem, TRedundancyEntity, TRedundancyItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSet.BinaryProperties))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="BinaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryPropertiesProperty = BinaryPropertiesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TBinaryPropertySetItem> BinaryProperties => (ReadOnlyObservableCollection<TBinaryPropertySetItem>)GetValue(BinaryPropertiesProperty);

        #endregion
        #region Redundancies Property Members

        protected ObservableCollection<TRedundancyItem> BackingRedundancies { get; } = new();

        private static readonly DependencyPropertyKey RedundanciesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TRedundancyItem>, RedundantSetDetailsViewModel<TEntity, TBinaryPropertySetEntity, TBinaryPropertySetItem, TRedundancyEntity, TRedundancyItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IRedundantSet.Redundancies))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Redundancies"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundanciesProperty = RedundanciesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TRedundancyItem> Redundancies => (ReadOnlyObservableCollection<TRedundancyItem>)GetValue(RedundanciesProperty);

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyPropertyBuilder<RedundantSetDetailsViewModel<TEntity, TBinaryPropertySetEntity, TBinaryPropertySetItem, TRedundancyEntity, TRedundancyItem>, RedundancyRemediationStatus>
            .Register(nameof(Status))
            .DefaultValue(RedundancyRemediationStatus.Unconfirmed)
            .AsReadWrite();

        public RedundancyRemediationStatus Status { get => (RedundancyRemediationStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        #endregion
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemInsertedResult([DisallowNull] Model.DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public RedundantSetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(BinaryPropertiesPropertyKey, new ReadOnlyObservableCollection<TBinaryPropertySetItem>(BackingBinaryProperties));
            SetValue(RedundanciesPropertyKey, new ReadOnlyObservableCollection<TRedundancyItem>(BackingRedundancies));
        }
    }
}
