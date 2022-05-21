using FsInfoCat.Numerics;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class DenominatedLengthViewModel : DependencyObject
    {
        private int _changeLevel;

        #region DenominatedValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="DenominatedValue"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler DenominatedValuePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="DenominatedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DenominatedValueProperty = DependencyProperty.Register(nameof(DenominatedValue), typeof(BinaryDenominatedInt64F?), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DenominatedLengthViewModel)?.OnDenominatedValuePropertyChanged(e)));

        public BinaryDenominatedInt64F? DenominatedValue { get => (BinaryDenominatedInt64F?)GetValue(DenominatedValueProperty); set => SetValue(DenominatedValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="DenominatedValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="DenominatedValueProperty"/> that tracks changes to its effective value.</param>
        private void OnDenominatedValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnDenominatedValuePropertyChanged((BinaryDenominatedInt64F?)args.OldValue, (BinaryDenominatedInt64F?)args.NewValue); }
            finally { DenominatedValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="DenominatedValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DenominatedValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DenominatedValue"/> property.</param>
        private void OnDenominatedValuePropertyChanged(BinaryDenominatedInt64F? oldValue, BinaryDenominatedInt64F? newValue)
        {
            bool isChange = Interlocked.Increment(ref _changeLevel) == 1;
            try
            {
                if (isChange)
                {
                    BinaryValue = newValue?.BinaryValue;
                    Numerator = newValue?.Numerator;
                    if (newValue.HasValue)
                    {
                        HasValue = true;
                        Denomination.SelectedValue = newValue.Value.Denominator;
                    }
                    else
                        HasValue = false;
                }
            }
            finally { _ = Interlocked.Decrement(ref _changeLevel); }
        }

        #endregion
        #region HasValue Property Members

        /// <summary>
        /// Identifies the <see cref="HasValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasValueProperty = DependencyProperty.Register(nameof(HasValue), typeof(bool), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DenominatedLengthViewModel)?.OnHasValuePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool HasValue { get => (bool)GetValue(HasValueProperty); set => SetValue(HasValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasValue"/> property.</param>
        private void OnHasValuePropertyChanged(bool oldValue, bool newValue)
        {
            bool isChange = Interlocked.Increment(ref _changeLevel) == 1;
            try
            {
                if (isChange)
                    BinaryValue = (DenominatedValue = newValue ? ToDenominatedValue(Numerator, Denomination.SelectedValue) : null)?.BinaryValue;
            }
            finally { _ = Interlocked.Decrement(ref _changeLevel); }
        }

        #endregion
        #region Denomination Property Members

        private static readonly DependencyPropertyKey DenominationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Denomination), typeof(EnumValuePickerVM<BinaryDenomination>), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Denomination"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DenominationProperty = DenominationPropertyKey.DependencyProperty;

        public EnumValuePickerVM<BinaryDenomination> Denomination => (EnumValuePickerVM<BinaryDenomination>)GetValue(DenominationProperty);

        private void Denomination_SelectedItemPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isChange = Interlocked.Increment(ref _changeLevel) == 1;
            try
            {
                if (isChange)
                    BinaryValue = (DenominatedValue = HasValue ? ToDenominatedValue(Numerator, (e.NewValue as EnumChoiceItem<BinaryDenomination>)?.Value) : null)?.BinaryValue;
            }
            finally { _ = Interlocked.Decrement(ref _changeLevel); }
        }

        #endregion
        #region Numerator Property Members

        /// <summary>
        /// Identifies the <see cref="Numerator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NumeratorProperty = DependencyProperty.Register(nameof(Numerator), typeof(double?), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DenominatedLengthViewModel)?.OnNumeratorPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        public double? Numerator { get => (double?)GetValue(NumeratorProperty); set => SetValue(NumeratorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Numerator"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Numerator"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Numerator"/> property.</param>
        private void OnNumeratorPropertyChanged(double? oldValue, double? newValue)
        {
            bool isChange = Interlocked.Increment(ref _changeLevel) == 1;
            try
            {
                if (isChange)
                    BinaryValue = (DenominatedValue = HasValue ? ToDenominatedValue(newValue, Denomination.SelectedValue) : null)?.BinaryValue;
            }
            finally { _ = Interlocked.Decrement(ref _changeLevel); }
        }

        public static BinaryDenominatedInt64F? ToDenominatedValue(double? numeratedValue, BinaryDenomination? denomination) =>
            (numeratedValue.HasValue && denomination.HasValue) ? new(numeratedValue.Value, denomination.Value) : null;

        #endregion
        #region BinaryValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="BinaryValue"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler BinaryValuePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="BinaryValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryValueProperty = DependencyPropertyBuilder<DenominatedLengthViewModel, long?>
            .Register(nameof(BinaryValue))
            .DefaultValue(null)
            .OnChanged((d, e) => (d as DenominatedLengthViewModel)?.RaiseBinaryValuePropertyChanged(e))
            .AsReadWrite();

        public long? BinaryValue { get => (long?)GetValue(BinaryValueProperty); set => SetValue(BinaryValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="BinaryValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="BinaryValueProperty"/> that tracks changes to its effective value.</param>
        private void RaiseBinaryValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnBinaryValuePropertyChanged((long?)args.OldValue, (long?)args.NewValue); }
            finally { BinaryValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="BinaryValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="BinaryValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="BinaryValue"/> property.</param>
        private void OnBinaryValuePropertyChanged(long? oldValue, long? newValue)
        {
            bool isChange = Interlocked.Increment(ref _changeLevel) == 1;
            try
            {
                if (isChange && DenominatedValue?.BinaryValue != newValue)
                {
                    BinaryDenominatedInt64F? dv = DenominatedValue = newValue.HasValue ? new(newValue.Value) : null;
                    if (dv.HasValue)
                    {
                        Numerator = dv.Value.Numerator;
                        Denomination.SelectedValue = dv.Value.Denominator;
                        HasValue = true;
                    }
                    else
                    {
                        HasValue = false;
                        Numerator = null;
                    }
                }
            }
            finally { _ = Interlocked.Decrement(ref _changeLevel); }
        }

        #endregion
        public DenominatedLengthViewModel()
        {
            EnumValuePickerVM<BinaryDenomination> denomination = new() { SelectedValue = BinaryDenomination.Megabytes };
            SetValue(DenominationPropertyKey, denomination);
            denomination.SelectedItemPropertyChanged += Denomination_SelectedItemPropertyChanged;
        }
    }
}
