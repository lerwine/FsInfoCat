using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class TimeReference : DependencyObject, ITimeReference
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected DataErrorInfo ErrorInfo { get; }

        #region IsExclusive Property Members

        /// <summary>
        /// Identifies the <see cref="IsExclusive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExclusiveProperty = DependencyPropertyBuilder<TimeReference, bool>
            .Register(nameof(IsExclusive))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsExclusive { get => (bool)GetValue(IsExclusiveProperty); set => SetValue(IsExclusiveProperty, value); }

        #endregion
        #region IncludeNull Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeNull"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeNullProperty = DependencyPropertyBuilder<TimeReference, bool>
            .Register(nameof(IncludeNull))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeNull { get => (bool)GetValue(IncludeNullProperty); set => SetValue(IncludeNullProperty, value); }

        #endregion
        #region HasErrors Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<TimeReference, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as TimeReference)?.RaiseHasErrorsPropertyChanged(e))
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

        protected TimeReference()
        {
            (ErrorInfo = new()).ErrorsChanged += TimeReference_ErrorsChanged;
        }

        protected abstract int CompareTo(DateTime other);

        public int CompareTo(DateTime? other) => other.HasValue ? CompareTo(other.Value) : IncludeNull ? 0 : 1;

        private void TimeReference_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            HasErrors = ErrorInfo.HasErrors;
            ErrorsChanged?.Invoke(this, e);
        }

        public IEnumerable GetErrors(string propertyName) => ErrorInfo.GetErrors(propertyName);

        public abstract DateTime ToDateTime();
    }
}
