using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.VideoPropertySets
{
    public class ListItemViewModel : VideoPropertiesListItemViewModel<VideoPropertiesListItem>, ILocalCrudEntityRowViewModel<VideoPropertiesListItem>
    {
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(ListItemViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion

        public ListItemViewModel([DisallowNull] VideoPropertiesListItem entity) : base(entity)
        {
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
