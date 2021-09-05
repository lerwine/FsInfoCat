using FsInfoCat.Numerics;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DenominatedLengthViewModel : DependencyObject
    {
        #region DenominatedValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="DenominatedValue"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler DenominatedValuePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="DenominatedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DenominatedValueProperty = DependencyProperty.Register(nameof(DenominatedValue), typeof(BinaryDenominatedInt64?), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DenominatedLengthViewModel)?.OnDenominatedValuePropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public BinaryDenominatedInt64? DenominatedValue { get => (BinaryDenominatedInt64?)GetValue(DenominatedValueProperty); set => SetValue(DenominatedValueProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="DenominatedValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="DenominatedValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnDenominatedValuePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnDenominatedValuePropertyChanged((BinaryDenominatedInt64?)args.OldValue, (BinaryDenominatedInt64?)args.NewValue); }
            finally { DenominatedValuePropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="DenominatedValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DenominatedValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DenominatedValue"/> property.</param>
        protected virtual void OnDenominatedValuePropertyChanged(BinaryDenominatedInt64? oldValue, BinaryDenominatedInt64? newValue) { }

        #endregion
        #region HasValue Property Members

        /// <summary>
        /// Identifies the <see cref="HasValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasValueProperty = DependencyProperty.Register(nameof(HasValue), typeof(bool), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DenominatedLengthViewModel)?.OnHasValuePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool HasValue { get => (bool)GetValue(HasValueProperty); set => SetValue(HasValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasValue"/> property.</param>
        private void OnHasValuePropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region Denomination Property Members

        private static readonly DependencyPropertyKey DenominationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Denomination), typeof(EnumValuePickerVM<RangeDenomination>), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Denomination"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DenominationProperty = DenominationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<RangeDenomination> Denomination => (EnumValuePickerVM<RangeDenomination>)GetValue(DenominationProperty);

        #endregion
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double?), typeof(DenominatedLengthViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DenominatedLengthViewModel)?.OnValuePropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? Value { get => (double?)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Value"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Value"/> property.</param>
        private void OnValuePropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        public DenominatedLengthViewModel()
        {
            EnumValuePickerVM<RangeDenomination> denomination = new() { SelectedValue = RangeDenomination.Megabytes };
            SetValue(DenominationPropertyKey, denomination);
            denomination.SelectedItemPropertyChanged += Denomination_SelectedItemPropertyChanged;
        }

        private void Denomination_SelectedItemPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Implement Denomination_SelectedItemPropertyChanged(object, e)
            throw new NotImplementedException();
        }
    }
}
