using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.DRMPropertySets
{
    public class ListItemViewModel : DRMPropertiesListItemViewModel<DRMPropertiesListItem>, ILocalCrudEntityRowViewModel<DRMPropertiesListItem>
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

        public ListItemViewModel([DisallowNull] DRMPropertiesListItem entity) : base(entity)
        {
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }
    }
}