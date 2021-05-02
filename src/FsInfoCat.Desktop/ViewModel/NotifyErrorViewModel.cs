using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    /// <summary>
    /// A base class for a <seealso cref="DependencyObject"/> that implements <seealso cref="INotifyDataErrorInfo" />.
    /// </summary>
    public abstract class NotifyErrorViewModel : DependencyObject, INotifyDataErrorInfo
    {
        private Dictionary<string, Tuple<object, ReadOnlyCollection<string>>> _invalidValues = new Dictionary<string, Tuple<object, ReadOnlyCollection<string>>>();
        private Dictionary<string, object> _validValues = new Dictionary<string, object>();

        /// <summary>
        /// Occurs when the <see cref="HasErrors"/> property value changes.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasErrors), typeof(bool),
            typeof(NotifyErrorViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as NotifyErrorViewModel).OnHasErrorsPropertyChanged(e)));

        /// <summary>
        /// <seealso cref="DependencyProperty"/> which returns a value that indicates whether the entity has validation errors.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            private set { SetValue(HasErrorsPropertyKey, value); }
        }

        private void OnHasErrorsPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnHasErrorsPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { HasErrorsPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the <see cref="HasErrors"/> property value changes.
        /// </summary>
        /// <param name="oldValue"><see langword="true"/> the current <see cref="NotifyErrorViewModel"/> previously had errors; otherwise, <see langword="false"/>.</param>
        /// <param name="newValue"><see langword="true"/> the current <see cref="NotifyErrorViewModel"/> has errors; otherwise, <see langword="false"/>.</param>
        protected virtual void OnHasErrorsPropertyChanged(bool oldValue, bool newValue) { }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or <see langword="null" /> or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>
        /// The validation errors for the property or entity.
        /// </returns>
        public IEnumerable GetErrors(string propertyName)
        {
            Monitor.Enter(_invalidValues);
            try
            {
                if (_invalidValues.ContainsKey(propertyName))
                    return _invalidValues[propertyName].Item2;
            }
            finally { Monitor.Exit(_invalidValues); }
            return null;
        }

        private void RaiseErrorsChanged(string propertyName, bool isValid)
        {
            try { OnErrorsChanged(propertyName, isValid); }
            finally { ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName)); }
        }

        /// <summary>
        /// Called when errors have changed for a property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="isValid"><see langword="true"/> if the property is valid; otherwise, <see langword="false"/>.</param>
        protected virtual void OnErrorsChanged(string propertyName, bool isValid) { }

        /// <summary>
        /// Gets the last validation result for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if the property is valid; <see langword="false"/> if it is invalid; otherwise <see langword="null"/>
        /// if the property has never been validated.</returns>
        protected bool? IsValid([CallerMemberName] string propertyName = null)
        {
            Monitor.Enter(_invalidValues);
            try
            {
                if (_validValues.ContainsKey(propertyName))
                    return true;
                if (_invalidValues.ContainsKey(propertyName))
                    return false;
            }
            finally { Monitor.Exit(_invalidValues); }
            return null;
        }

        /// <summary>
        /// Determines whether the specified property has already been validated.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if the property had previously been validated; otherwise, <see langword="false"/>.</returns>
        protected bool HasPreviousValidation([CallerMemberName] string propertyName = null)
        {
            Monitor.Enter(_invalidValues);
            try { return _invalidValues.ContainsKey(propertyName) || _validValues.ContainsKey(propertyName); }
            finally { Monitor.Exit(_invalidValues); }
        }

        /// <summary>
        /// Gets the previously validated value.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The validated value or the default value if it had never been validated.</returns>
        protected T GetPreviousValidatedValue<T>([CallerMemberName] string propertyName = null)
        {
            Monitor.Enter(_invalidValues);
            try
            {
                if (_invalidValues.ContainsKey(propertyName))
                    return (T)_invalidValues[propertyName].Item1;
                if (_validValues.ContainsKey(propertyName))
                    return (T)_validValues[propertyName];
            }
            finally { Monitor.Exit(_invalidValues); }
            return default;
        }

        /// <summary>
        /// Clears the errors for a property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The valid value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if validation state for the property changed (invalid to valid); otherwise, <see langword="false"/>.</returns>
        protected bool ClearErrors<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Monitor.Enter(_invalidValues);
            try
            {
                if (_invalidValues.ContainsKey(propertyName))
                {
                    _invalidValues.Remove(propertyName);
                    _validValues.Add(propertyName, value);
                }
                else
                {
                    if (_validValues.ContainsKey(propertyName))
                        _validValues[propertyName] = value;
                    else
                        _validValues.Add(propertyName, value);
                    return false;
                }
            }
            finally { Monitor.Exit(_invalidValues); }
            try
            {
                if (_invalidValues.Count == 0)
                    HasErrors = false;
            }
            finally { RaiseErrorsChanged(propertyName, true); }
            return true;
        }

        /// <summary>
        /// Sets the validation message for a property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The validated value.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if validation state for the property changed from invalid to valid; <see langword="false"/>
        /// if it changed from valid to invalid; otherwise, <see langword="null"/> if the validation state for the property was unchanged.</returns>
        protected bool? SetValidationMessage<T>(T value, string errorMessage, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                if (ClearErrors(value, propertyName))
                    return true;
            }
            else if (SetError(value, errorMessage, propertyName))
                return false;
            return null;
        }

        /// <summary>
        /// Sets the validation messages for a property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The validated value.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if validation state for the property changed from invalid to valid; <see langword="false"/>
        /// if it changed from valid to invalid; otherwise, <see langword="null"/> if the validation state for the property was unchanged.</returns>
        protected bool? SetValidationMessages<T>(T value, IEnumerable<string> errorMessages, [CallerMemberName] string propertyName = null)
        {
            if (errorMessages is null || !errorMessages.Any(m => !string.IsNullOrWhiteSpace(m)))
            {
                if (ClearErrors(value, propertyName))
                    return true;
            }
            else if (SetErrors(value, errorMessages, propertyName))
                return false;
            return null;
        }

        /// <summary>
        /// Sets the errors for a property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The invalid value.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if validation state for the property changed (valid to invalid); otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessages"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="errorMessages"/> is empty or all elements are <see langword="null"/>,
        /// empty or contain only whitespace.</exception>
        protected bool SetErrors<T>(T value, IEnumerable<string> errorMessages, [CallerMemberName] string propertyName = null)
        {
            if (errorMessages is null)
                throw new ArgumentNullException(nameof(errorMessages));
            string[] newErrors = errorMessages.Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();
            if (newErrors.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(errorMessages));
            Tuple<object, ReadOnlyCollection<string>> item = new Tuple<object, ReadOnlyCollection<string>>(value, new ReadOnlyCollection<string>(newErrors));
            bool hadErrors;
            Monitor.Enter(_invalidValues);
            try
            {
                hadErrors = _invalidValues.ContainsKey(propertyName);
                if (hadErrors)
                {
                    ReadOnlyCollection<string> oldErrors = _invalidValues[propertyName].Item2;
                    if (oldErrors.Count == newErrors.Length && oldErrors.SequenceEqual(newErrors))
                    {
                        _invalidValues[propertyName] = item;
                        return false;
                    }
                    _invalidValues[propertyName] = item;
                }
                else
                {
                    if (_validValues.ContainsKey(propertyName))
                        _validValues.Remove(propertyName);
                    _invalidValues.Add(propertyName, item);
                }
            }
            finally { Monitor.Exit(_invalidValues); }
            if (hadErrors)
            {
                RaiseErrorsChanged(propertyName, false);
                return false;
            }
            try { HasErrors = true; }
            finally { RaiseErrorsChanged(propertyName, false); }
            return true;
        }

        /// <summary>
        /// Sets the error for a property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="value">The invalid value.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see langword="true"/> if validation state for the property changed (valid to invalid); otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessage"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="errorMessage"/> is empty or contains only whitespace.</exception>
        protected bool SetError<T>(T value, string errorMessage, [CallerMemberName] string propertyName = null)
        {
            if (errorMessage is null)
                throw new ArgumentNullException(nameof(errorMessage));
            if (errorMessage.Trim().Length == 0)
                throw new ArgumentOutOfRangeException(nameof(errorMessage));
            return SetErrors(value, new string[] { errorMessage }, propertyName);
        }

        private bool _modelUpdating = false;
        protected bool IsModelUpdating() => _modelUpdating;

        protected void UpdateModelProperty<TModel, TProperty>(TModel model, string propertyName, TProperty value, Action<TModel, TProperty> valueSetter,
            Func<TProperty, string> validator = null) => UpdateModelProperty(model, propertyName, value, valueSetter, null, validator);

        protected void UpdateModelProperty<TModel, TProperty>(TModel model, string propertyName, TProperty value, Action<TModel, TProperty> valueSetter,
            Action<TModel> onModelPropertyUpdated, Func<TProperty, string> validator = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                if (propertyName is null)
                    throw new ArgumentNullException(nameof(propertyName));
                throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
            if (valueSetter is null)
                throw new ArgumentNullException(nameof(valueSetter));
            bool modelUpdating = _modelUpdating;
            _modelUpdating = true;
            try
            {
                valueSetter(model, value);
                onModelPropertyUpdated?.Invoke(model);
            }
            catch (Exception exception)
            {
                LogException(exception, "{ExceptionType} occurred while setting model property {PropertyName} to \"{Value}\": {Message}",
                    exception.GetType().Name, propertyName, value, exception.Message);
                SetError(value, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, propertyName);
                return;
            }
            finally { _modelUpdating = modelUpdating; }
            if (validator is null)
                ClearErrors(value, propertyName);
            else
                SetValidationMessage(value, validator(value), propertyName);
        }

        /// <summary>
        /// Attempts to coerce a value from one type to another.
        /// </summary>
        /// <typeparam name="TInput">The type of the input value.</typeparam>
        /// <typeparam name="TOutput">The type of the output value.</typeparam>
        /// <param name="value">The input value.</param>
        /// <param name="result">The result value.</param>
        /// <returns>A <see langword="null"/> or whitespace-only string if the <paramref name="value"/> was coerced as a <typeparamref name="TOutput"/> value;
        /// otherwise, the error message to indicate a parse failure.</returns>
        protected delegate string TryCoerceValueHandler<TInput, TOutput>(TInput value, out TOutput result);

        protected void UpdateParsedIntModelProperty<TModel>(TModel model, string inputPropertyName, string inputValue, Action<TModel, int?> valueSetter,
            Func<int?, string> validator = null) where TModel : class => UpdateParsedIntModelProperty(model, inputPropertyName, inputValue, valueSetter, null, validator);

        protected void UpdateParsedIntModelProperty<TModel>(TModel model, string inputPropertyName, string inputValue, Action<TModel, int?> valueSetter,
            Action<TModel> onModelPropertyUpdated, Func<int?, string> validator = null) where TModel : class =>
            UpdateCoercedModelProperty(model, inputPropertyName, inputValue, (string s, out int? i) =>
            {
                if (string.IsNullOrWhiteSpace(s))
                    i = null;
                else if (double.TryParse(s, out double d) && d >= int.MinValue && d <= int.MaxValue && Math.Floor(d) == d)
                    i = (int)d;
                else
                {
                    i = null;
                    return "Invalid integer value";
                }
                return null;
            }, valueSetter, onModelPropertyUpdated, validator);

        protected void UpdateCoercedModelProperty<TModel, TInput, TProperty>(TModel model, string inputPropertyName, TInput inputValue,
             TryCoerceValueHandler<TInput, TProperty> tryCoerceValue, Action<TModel, TProperty> valueSetter, Func<TProperty, string> validator = null)
            where TModel : class => UpdateCoercedModelProperty(model, inputPropertyName, inputValue, tryCoerceValue, valueSetter, null, validator);

        protected void UpdateCoercedModelProperty<TModel, TInput, TProperty>(TModel model, string inputPropertyName, TInput inputValue,
             TryCoerceValueHandler<TInput, TProperty> tryCoerceValue, Action<TModel, TProperty> valueSetter, Action<TModel> onModelPropertyUpdated,
             Func<TProperty, string> validator = null)
            where TModel : class
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(inputPropertyName))
            {
                if (inputPropertyName is null)
                    throw new ArgumentNullException(nameof(inputPropertyName));
                throw new ArgumentOutOfRangeException(nameof(inputPropertyName));
            }
            if (tryCoerceValue is null)
                throw new ArgumentNullException(nameof(tryCoerceValue));
            if (valueSetter is null)
                throw new ArgumentNullException(nameof(valueSetter));
            string errorMessage = tryCoerceValue(inputValue, out TProperty value);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                bool modelUpdating = _modelUpdating;
                _modelUpdating = true;
                try
                {
                    valueSetter(model, value);
                    onModelPropertyUpdated?.Invoke(model);
                }
                catch (Exception exception)
                {
                    LogException(exception, "{ExceptionType} occurred while setting model property {PropertyName} to \"{Value}\": {Message}",
                        exception.GetType().Name, inputPropertyName, inputValue, exception.Message);
                    SetError(inputValue, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, inputPropertyName);
                    return;
                }
                finally { _modelUpdating = modelUpdating; }
                if (validator is null)
                    ClearErrors(inputValue, inputPropertyName);
                else
                    SetValidationMessage(inputValue, validator(value), inputPropertyName);
            }
            else
                SetError(inputValue, errorMessage, inputPropertyName);
        }

        protected abstract void LogException(Exception exception, string message);
        protected abstract void LogException(Exception exception, string message, params object[] args);
    }
}
