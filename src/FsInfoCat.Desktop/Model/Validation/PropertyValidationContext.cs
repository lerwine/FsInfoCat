using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.Validation
{
    public sealed class PropertyValidationContext<TInstance, TValue> : PropertyContext<TInstance, TValue, ModelValidationContext<TInstance>>,
        IPropertyValidationContext<TInstance>
        where TInstance : class
    {
        private readonly ObservableCollection<ValidationResult> _errors = new ObservableCollection<ValidationResult>();
        private readonly ValidationMessageCollection _roErrors;
        private bool _invalidated = false;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event EventHandler HasErrorsChanged;

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        public bool HasErrors { get; private set; }

        public PropertyValidationContext(ModelValidationContext<TInstance> owner, PropertyDescriptor propertyDescriptor,
                IEqualityComparer<TValue> equalityComparer, IEnumerable<ValidationAttribute> validationAttributes)
            : base(owner, propertyDescriptor, equalityComparer)
        {
            _roErrors = new ValidationMessageCollection(_errors);
            _errors.CollectionChanged += Errors_CollectionChanged;
            ValidationAttributes = new ReadOnlyCollection<ValidationAttribute>(validationAttributes.ToArray());
            Revalidate(Value);
        }

        protected override void OnInstancePropertyValueChanged(TValue oldValue, TValue newValue)
        {
            try { base.OnInstancePropertyValueChanged(oldValue, newValue); }
            finally { Revalidate((TValue)newValue); }
        }

        private void Revalidate(TValue value)
        {
            if (ValidationAttributes.Count == 0)
            {
                _errors.Clear();
                return;
            }
            ValidationContext validationContext = new ValidationContext(Owner);
            Collection<ValidationResult> validationResults = new Collection<ValidationResult>();
            if (Validator.TryValidateValue(value, validationContext, validationResults, ValidationAttributes))
                _errors.Clear();
            else
            {
                if (validationResults.Count == 0)
                    validationResults.Add(new ValidationResult("Unspecified validation error", new string[] { nameof(Value) }));
                int end = (validationResults.Count < _errors.Count) ? validationResults.Count : _errors.Count;
                for (int i = 0; i < end; i++)
                {
                    if (!string.Equals(_errors[i].ErrorMessage, validationResults[i].ErrorMessage))
                        _errors[i] = validationResults[i];
                }
                if (end < validationResults.Count)
                    foreach (ValidationResult r in validationResults.Skip(end))
                        _errors.Add(r);
                else
                    while (_errors.Count > end)
                        _errors.RemoveAt(end);
            }
        }

        public ValidationResult Validate()
        {
            if (!CheckPropertyChange() && _invalidated)
            {
                _invalidated = false;
                return Revalidate();
            }
            switch (_errors.Count)
            {
                case 0:
                    return null;
                case 1:
                    return _errors.FirstOrDefault();
                default:
                    return new ValidationResult(string.Join(Environment.NewLine + Environment.NewLine, _roErrors), new string[] { nameof(Value) });
            }
        }

        public void Invalidate() => _invalidated = true;

        public ValidationResult Revalidate()
        {
            Revalidate(Value);
            return Validate();
        }

        private void Errors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (HasErrors)
            {
                if (_errors.Count == 0)
                    HasErrors = false;
                else
                {
                    RaiseErrorsChanged();
                    return;
                }
            }
            else
            {
                if (_errors.Count == 0)
                {
                    RaiseErrorsChanged();
                    return;
                }
                HasErrors = true;
            }
            try { RaisePropertyChanged(nameof(HasErrors)); }
            finally
            {
                try { HasErrorsChanged?.Invoke(this, EventArgs.Empty); }
                finally { RaiseErrorsChanged(); }
            }
        }

        private void RaiseErrorsChanged() => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(Name));

        //public IEnumerable<string> GetErrors() => (_roErrors.Count > 0) ? _roErrors : null;

        #region Explicit Members

        IModelValidationContext IPropertyValidationContext.Owner => Owner;

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            CheckPropertyChange();
            return (_errors.Count > 0) ? _errors.ToArray() : null;
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            CheckPropertyChange();
            return (string.Equals(propertyName, nameof(Value)) && _roErrors.Count > 0) ? _roErrors : null;
        }

        #endregion
    }
}
