using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class DurationRange : DependencyObject, IFilter
    {
        private readonly DataErrorInfo _errorInfo;

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region Min Property Members

        /// <summary>
        /// Identifies the <see cref="Min"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinProperty = DependencyPropertyBuilder<DurationRange, Duration>
            .Register(nameof(Min))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DurationRange)?.OnMinPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Duration Min { get => (Duration)GetValue(MinProperty); set => SetValue(MinProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Min"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Min"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Min"/> property.</param>
        protected virtual void OnMinPropertyChanged(Duration oldValue, Duration newValue)
        {
            // TODO: Implement OnMinPropertyChanged Logic
        }

        #endregion
        #region Max Property Members

        /// <summary>
        /// Identifies the <see cref="Max"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty = DependencyPropertyBuilder<DurationRange, Duration>
            .Register(nameof(Max))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DurationRange)?.OnMaxPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Duration Max { get => (Duration)GetValue(MaxProperty); set => SetValue(MaxProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Max"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Max"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Max"/> property.</param>
        protected virtual void OnMaxPropertyChanged(Duration oldValue, Duration newValue)
        {
            // TODO: Implement OnMaxPropertyChanged Logic
        }

        #endregion
        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<DurationRange, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DurationRange)?.RaiseHasErrorsPropertyChanged(e))
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

        public bool IsInRange(long? seconds) => IsInRange(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);

        public bool IsInRange(TimeSpan? value)
        {
            Duration duration = Min;
            if (value.HasValue)
                return (duration is null || duration.CompareTo(value) <= 0) && ((duration = Max) is null || duration.CompareTo(value) >= 0);
            return (duration?.IncludeNull ?? true) && (Max?.IncludeNull ?? true);
        }

        public DurationRange()
        {
            _errorInfo = new();
            _errorInfo.ErrorsChanged += ErrorInfo_ErrorsChanged;
        }

        private void ErrorInfo_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public IEnumerable GetErrors(string propertyName) => _errorInfo.GetErrors(propertyName);

        internal static bool AreSame(DurationRange x, DurationRange y) => (x is null) ? (y is null || Duration.AreSame(x.Min, null)) :
            (ReferenceEquals(x, y) || Duration.AreSame(x.Min, y?.Min) && Duration.AreSame(x.Max, y?.Max));
    }
}
