using Microsoft.EntityFrameworkCore;
using FsInfoCat.Local;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainVM : DependencyObject
    {
        private ILogger<MainVM> _logger;

        #region NavigatedContent Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="NavigatedContent"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler NavigatedContentPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="NavigatedContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NavigatedContentProperty = DependencyProperty.Register(nameof(NavigatedContent), typeof(object), typeof(MainVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as MainVM)?.OnNavigatedContentPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public object NavigatedContent { get => (object)GetValue(NavigatedContentProperty); set => SetValue(NavigatedContentProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="NavigatedContentProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="NavigatedContentProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnNavigatedContentPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnNavigatedContentPropertyChanged((object)args.OldValue, (object)args.NewValue); }
            finally { NavigatedContentPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="NavigatedContent"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="NavigatedContent"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="NavigatedContent"/> property.</param>
        protected virtual void OnNavigatedContentPropertyChanged(object oldValue, object newValue)
        {
            if (oldValue is Page oldPage && oldPage.DataContext is INotifyNavigationContentChanged oldNavigated)
                oldNavigated.OnNavigatedFrom(this);
            if (newValue is Page newPage && newPage.DataContext is INotifyNavigationContentChanged newNavigated)
                newNavigated.OnNavigatedTo(this);
        }

        #endregion

        public MainVM()
        {
        }
    }
}
