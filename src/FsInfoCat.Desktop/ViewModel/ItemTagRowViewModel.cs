﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemTagRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IItemTagRow
    {
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyPropertyBuilder<ItemTagRowViewModel<TEntity>, string>
            .Register(nameof(Notes))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        public string Notes { get => GetValue(NotesProperty) as string; private set => SetValue(NotesPropertyKey, value); }

        #endregion

        public ItemTagRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Notes = entity.Notes;
        }

        protected override void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(Notes))
                Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
            else
                base.OnEntityPropertyChanged(sender, args);
        }
    }
}