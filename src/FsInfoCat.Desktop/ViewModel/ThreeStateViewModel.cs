using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    /// <summary>
    /// View model that represents a three-state value.
    /// </summary>
    /// <seealso cref="DependencyObject" />
    public class ThreeStateViewModel : DependencyObject
    {
        private readonly ILogger<ThreeStateViewModel> _logger;

        #region Value Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ValuePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(bool?), typeof(ThreeStateViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnValuePropertyChanged(e)));

        /// <summary>
        /// Gets or sets a 3-state value.
        /// </summary>
        /// <value>The 3-state value that can be <see langword="true"/>, <see langword="false"/> or <see langword="null"/>.</value>
        public bool? Value { get => (bool?)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="ValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="ValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogDebug("Enter <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnValuePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
            try { OnValuePropertyChanged((bool?)args.OldValue, (bool?)args.NewValue); }
            finally { ValuePropertyChanged?.Invoke(this, args); }
            _logger.LogDebug("Exit <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnValuePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
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

        /// <summary>
        /// Identifies the <see cref="IsTrue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTrueProperty = DependencyProperty.Register(nameof(IsTrue), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsTruePropertyChanged(e)));

        /// <summary>
        /// Gets or sets indicating whether <see cref="Value"/> is <see langword="true"/>.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="Value"/> is <see langword="true"/>;
        /// otherwise <see langword="false"/> if <see cref="Value"/> is <see langword="null"/> or <see langword="false"/>.</value>
        public bool IsTrue { get => (bool)GetValue(IsTrueProperty); set => SetValue(IsTrueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsTrueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsTrueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsTruePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogDebug("Enter <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsTruePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
            try { OnIsTruePropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsTruePropertyChanged?.Invoke(this, args); }
            _logger.LogDebug("Exit <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsTruePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
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

        /// <summary>
        /// Identifies the <see cref="IsFalse"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFalseProperty = DependencyProperty.Register(nameof(IsFalse), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsFalsePropertyChanged(e)));

        /// <summary>
        /// Gets or sets indicating whether <see cref="Value"/> is <see langword="false"/>.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="Value"/> is <see langword="false"/>;
        /// otherwise <see langword="false"/> if <see cref="Value"/> is <see langword="null"/> or <see langword="true"/>.</value>
        public bool IsFalse { get => (bool)GetValue(IsFalseProperty); set => SetValue(IsFalseProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsFalseProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsFalseProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsFalsePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogDebug("Enter <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsFalsePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
            try { OnIsFalsePropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsFalsePropertyChanged?.Invoke(this, args); }
            _logger.LogDebug("Exit <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsFalsePropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
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

        /// <summary>
        /// Identifies the <see cref="IsNull"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNullProperty = DependencyProperty.Register(nameof(IsNull), typeof(bool), typeof(ThreeStateViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ThreeStateViewModel)?.OnIsNullPropertyChanged(e)));

        /// <summary>
        /// Gets or sets indicating whether <see cref="Value"/> is <see langword="null"/>.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="Value"/> is <see langword="null"/>;
        /// otherwise <see langword="false"/> if <see cref="Value"/> is <see langword="true"/> or <see langword="false"/>.</value>
        public bool IsNull { get => (bool)GetValue(IsNullProperty); set => SetValue(IsNullProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsNullProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsNullProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsNullPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            _logger.LogDebug("Enter <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsNullPropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
            try { OnIsNullPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsNullPropertyChanged?.Invoke(this, args); }
            _logger.LogDebug("Exit <#{HashCode}>.{MethodName}({{ Property.Name = \"{Name}\", OldValue = {OldValue}, NewValue = {NewValue} }})", RuntimeHelpers.GetHashCode(this),
                nameof(OnIsNullPropertyChanged), args.Property.Name, args.OldValue, args.NewValue);
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

        public ThreeStateViewModel()
        {
            _logger = App.GetLogger(this);
        }

        public ThreeStateViewModel(bool? initialValue) : this() { Value = initialValue; }
    }
}
