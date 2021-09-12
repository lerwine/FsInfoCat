using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class NotifyDataErrorInfoViewModel : DependencyObject, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected DataErrorInfo ErrorInfo { get; }

        #region HasErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<NotifyDataErrorInfoViewModel, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as NotifyDataErrorInfoViewModel)?.RaiseHasErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasErrorsPropertyChanged?.Invoke(this, args);

        #endregion

        protected NotifyDataErrorInfoViewModel()
        {
            (ErrorInfo = new()).ErrorsChanged += (object sender, DataErrorsChangedEventArgs e) => Dispatcher.CheckInvoke(() => OnErrorsChanged(e));
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            HasErrors = ErrorInfo.HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        public IEnumerable GetErrors(string propertyName) => ErrorInfo.GetErrors(propertyName);
    }
}
