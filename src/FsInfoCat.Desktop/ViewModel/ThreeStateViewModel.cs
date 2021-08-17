using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ThreeStateViewModel : DependencyObject
    {
        #region Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ValuePropertyChanged;

        private static readonly DependencyPropertyKey ValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Value), typeof(bool?), typeof(ThreeStateViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnValuePropertyChanged(e)));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? Value { get => (bool?)GetValue(ValueProperty); private set => SetValue(ValuePropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="ValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="ValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnValuePropertyChanged((bool?)args.OldValue, (bool?)args.NewValue); }
            finally { ValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Value"/> property.</param>
        protected virtual void OnValuePropertyChanged(bool? oldValue, bool? newValue)
        {
            if (newValue.HasValue)
            {
                if (newValue.Value)
                    IsTrue = true;
                else
                    IsFalse = true;
            }
            else
                IsNull = true;
        }

        #endregion
        #region IsTrue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="IsTrue"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsTruePropertyChanged;

        private static readonly DependencyPropertyKey IsTruePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsTrue), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsTruePropertyChanged(e)));

        /// <summary>
        /// Identifies the <see cref="IsTrue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTrueProperty = IsTruePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsTrue { get => (bool)GetValue(IsTrueProperty); private set => SetValue(IsTruePropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsTrueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsTrueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsTruePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsTruePropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsTruePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsTrue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsTrue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsTrue"/> property.</param>
        protected virtual void OnIsTruePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                Value = true;
            else if (!(IsFalse || IsNull))
                Value = null;
        }

        #endregion
        #region IsFalse Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="IsFalse"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsFalsePropertyChanged;

        private static readonly DependencyPropertyKey IsFalsePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsFalse), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsFalsePropertyChanged(e)));

        /// <summary>
        /// Identifies the <see cref="IsFalse"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFalseProperty = IsFalsePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsFalse { get => (bool)GetValue(IsFalseProperty); private set => SetValue(IsFalsePropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsFalseProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsFalseProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsFalsePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsFalsePropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsFalsePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsFalse"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsFalse"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsFalse"/> property.</param>
        protected virtual void OnIsFalsePropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                Value = false;
            else if (!(IsTrue || IsNull))
                Value = null;
        }

        #endregion
        #region IsNull Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="IsNull"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsNullPropertyChanged;

        private static readonly DependencyPropertyKey IsNullPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNull), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsNullPropertyChanged(e)));

        /// <summary>
        /// Identifies the <see cref="IsNull"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNullProperty = IsNullPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsNull { get => (bool)GetValue(IsNullProperty); private set => SetValue(IsNullPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsNullProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsNullProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsNullPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsNullPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsNullPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsNull"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsNull"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsNull"/> property.</param>
        protected virtual void OnIsNullPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                Value = null;
            else if (!(IsTrue || IsFalse))
                Value = !(Value ?? false);
        }

        #endregion

        public ThreeStateViewModel() { }
        public ThreeStateViewModel(bool? initialValue) { Value = initialValue; }
    }
}
