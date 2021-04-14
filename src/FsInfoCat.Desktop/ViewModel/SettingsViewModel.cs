using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SettingsViewModel : DependencyObject
    {
        public event DependencyPropertyChangedEventHandler DbConnectionSettingsPropertyChanged;

        public static readonly DependencyProperty DbConnectionSettingsProperty =
            DependencyProperty.Register(nameof(DbConnectionSettings), typeof(DbConnectionSettingsViewModel), typeof(SettingsViewModel),
                new PropertyMetadata(null,
                    (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnDbConnectionSettingsPropertyChanged(e),
                    (DependencyObject d, object baseValue) => baseValue ?? new DbConnectionSettingsViewModel()));

        public DbConnectionSettingsViewModel DbConnectionSettings
        {
            get { return (DbConnectionSettingsViewModel)GetValue(DbConnectionSettingsProperty); }
            set { SetValue(DbConnectionSettingsProperty, value); }
        }

        protected virtual void OnDbConnectionSettingsPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnDbConnectionSettingsPropertyChanged((DbConnectionSettingsViewModel)args.OldValue, (DbConnectionSettingsViewModel)args.NewValue); }
            finally { DbConnectionSettingsPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnDbConnectionSettingsPropertyChanged(DbConnectionSettingsViewModel oldValue, DbConnectionSettingsViewModel newValue)
        {
            // TODO: Implement OnDbConnectionSettingsPropertyChanged Logic
        }

    }
}
