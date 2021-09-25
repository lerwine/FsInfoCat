using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class OptionalValueViewModel<T> : NotifyDataErrorInfoViewModel
        where T : struct
    {
        #region InputValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="InputValue"/> dependency property is being validated.
        /// </summary>
        public event EventHandler<PropertyValidatingEventArgs<T>> ValidateInputValue;

        /// <summary>
        /// Identifies the <see cref="InputValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InputValueProperty = DependencyPropertyBuilder<OptionalValueViewModel<T>, T?>
            .Register(nameof(InputValue))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnInputValuePropertyChanged(newValue))
            .AsReadWrite();

        public T? InputValue { get => (T?)GetValue(InputValueProperty); set => SetValue(InputValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="InputValue"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="InputValue"/> property.</param>
        protected virtual void OnInputValuePropertyChanged(T? newValue)
        {
            using IDisposable scope = _logger.EnterMethod(newValue, this);
            if (IsEnabled && !ForceNullResult && Revalidate(newValue, IsMandatory()))
                ResultValue = newValue;
        }

        #endregion
        #region ForceNullResult Property Members

        /// <summary>
        /// Identifies the <see cref="ForceNullResult"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ForceNullResultProperty = DependencyPropertyBuilder<OptionalValueViewModel<T>, bool>
            .Register(nameof(ForceNullResult))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnForceNullResultPropertyChanged(newValue))
            .AsReadWrite();

        public bool ForceNullResult { get => (bool)GetValue(ForceNullResultProperty); set => SetValue(ForceNullResultProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ForceNullResult"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="ForceNullResult"/> property.</param>
        protected virtual void OnForceNullResultPropertyChanged(bool newValue)
        {
            using IDisposable scope = _logger.EnterMethod(newValue, this);
            if (IsEnabled && InputValue.HasValue)
            {
                T? value = newValue ? null : InputValue;
                if (Revalidate(value, IsMandatory()))
                    ResultValue = value;
            }
        }

        #endregion
        #region IsRequired Property Members

        /// <summary>
        /// Identifies the <see cref="IsRequired"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty = DependencyPropertyBuilder<OptionalValueViewModel<T>, bool>
            .Register(nameof(IsRequired))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnIsRequiredPropertyChanged(newValue))
            .AsReadWrite();

        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsRequired"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="IsRequired"/> property.</param>
        protected virtual void OnIsRequiredPropertyChanged(bool newValue)
        {
            using IDisposable scope = _logger.EnterMethod(newValue, this);
            if (!OverrideRequirement.HasValue)
                OnMandatoryStateChanged(newValue);
        }

        #endregion
        #region OverrideRequirement Property Members

        private static readonly DependencyPropertyKey OverrideRequirementPropertyKey = DependencyPropertyBuilder<OptionalValueViewModel<T>, bool?>
            .Register(nameof(OverrideRequirement))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnOverrideRequirementPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="OverrideRequirement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OverrideRequirementProperty = OverrideRequirementPropertyKey.DependencyProperty;

        public bool? OverrideRequirement { get => (bool?)GetValue(OverrideRequirementProperty); protected set => SetValue(OverrideRequirementPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="OverrideRequirement"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="OverrideRequirement"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="OverrideRequirement"/> property.</param>
        protected virtual void OnOverrideRequirementPropertyChanged(bool? oldValue, bool? newValue)
        {
            using IDisposable scope = _logger.EnterMethod(oldValue, newValue, this);
            bool isMandatory = newValue ?? IsRequired;
            if (isMandatory != (oldValue ?? IsRequired))
                OnMandatoryStateChanged(isMandatory);
        }

        #endregion
        #region IsEnabled Property Members

        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyPropertyBuilder<OptionalValueViewModel<T>, bool>
            .Register(nameof(IsEnabled))
            .DefaultValue(true)
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnIsEnabledPropertyChanged(newValue))
            .AsReadWrite();

        public bool IsEnabled { get => (bool)GetValue(IsEnabledProperty); set => SetValue(IsEnabledProperty, value); }

        protected virtual void OnIsEnabledPropertyChanged(bool newValue)
        {
            using IDisposable scope = _logger.EnterMethod(newValue, this);
            if (newValue)
            {
                T? value = ForceNullResult ? null : InputValue;
                if (Revalidate(value, IsMandatory()))
                    ResultValue = value;
            }
            else
                ErrorInfo.ClearErrors(nameof(InputValue));
        }

        #endregion
        #region ResultValue Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="ResultValue"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ResultValuePropertyChanged;

        private static readonly DependencyPropertyKey ResultValuePropertyKey = DependencyPropertyBuilder<OptionalValueViewModel<T>, T?>
            .Register(nameof(ResultValue))
            .DefaultValue(null)
            .OnChanged((d, e) => (d as OptionalValueViewModel<T>)?.OnResultValuePropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ResultValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResultValueProperty = ResultValuePropertyKey.DependencyProperty;

        public T? ResultValue { get => (T?)GetValue(ResultValueProperty); private set => SetValue(ResultValuePropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="ResultValueProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="ResultValueProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnResultValuePropertyChanged(DependencyPropertyChangedEventArgs args) => ResultValuePropertyChanged?.Invoke(this, args);

        #endregion
        #region RequirementErrorMessage Property Members

        /// <summary>
        /// Identifies the <see cref="RequirementErrorMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RequirementErrorMessageProperty = DependencyPropertyBuilder<OptionalValueViewModel<T>, string>
            .Register(nameof(RequirementErrorMessage))
            .OnChanged((d, oldValue, newValue) => (d as OptionalValueViewModel<T>)?.OnRequirementErrorMessagePropertyChanged(oldValue, newValue))
            .CoerseWith(NullIfWhiteSpaceOrNormalizedStringCoersion.Default)
            .AsReadWrite();
        private readonly ILogger<OptionalValueViewModel<T>> _logger;

        public string RequirementErrorMessage { get => GetValue(RequirementErrorMessageProperty) as string; set => SetValue(RequirementErrorMessageProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RequirementErrorMessage"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RequirementErrorMessage"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RequirementErrorMessage"/> property.</param>
        protected virtual void OnRequirementErrorMessagePropertyChanged(string oldValue, string newValue)
        {
            if (ErrorInfo.GetErrors(InputValueProperty.Name) is string validationMessage && validationMessage == (oldValue ?? FsInfoCat.Properties.Resources.DisplayName_Required))
                ErrorInfo.SetErrors(InputValueProperty.Name, newValue ?? FsInfoCat.Properties.Resources.DisplayName_Required);
        }

        #endregion

        public T? GetValue() => ForceNullResult ? null : InputValue;

        public void SetValue(T? value)
        {
            ForceNullResult = !value.HasValue;
            InputValue = value;
        }

        public bool IsMandatory() => OverrideRequirement ?? IsRequired;

        protected virtual void OnMandatoryStateChanged(bool isMandatory)
        {
            using IDisposable scope = _logger.EnterMethod(isMandatory, this);
            if (IsEnabled && (ForceNullResult || !InputValue.HasValue))
            {
                if (isMandatory)
                    ErrorInfo.SetErrors(InputValueProperty.Name, RequirementErrorMessage ?? FsInfoCat.Properties.Resources.DisplayName_Required);
                else
                {
                    ErrorInfo.ClearErrors(InputValueProperty.Name);
                    ResultValue = null;
                }
            }
        }
        public OptionalValueViewModel()
        {
            _logger = App.GetLogger(this);
        }

        private bool RaiseValidateResultValue(T value)
        {
            PropertyValidatingEventArgs<T> args = new(value, InputValueProperty.Name);
            OnValidateInputValue(args);
            ErrorInfo.SetErrors(InputValueProperty.Name, args.ValidationMessage);
            return !ErrorInfo.HasErrors;
        }

        protected virtual void OnValidateInputValue(PropertyValidatingEventArgs<T> args) => ValidateInputValue?.Invoke(this, args);

        public bool Revalidate()
        {
            if (IsEnabled)
                return Revalidate(ForceNullResult ? null : InputValue, IsMandatory());
            ErrorInfo.ClearErrors(InputValueProperty.Name);
            return true;
        }

        private bool Revalidate(T? inputValue, bool isMandatory)
        {
            if (inputValue.HasValue)
                return RaiseValidateResultValue(inputValue.Value);
            if (isMandatory)
            {
                ErrorInfo.SetErrors(InputValueProperty.Name, RequirementErrorMessage ?? FsInfoCat.Properties.Resources.DisplayName_Required);
                return false;
            }
            return true;
        }
    }
}
