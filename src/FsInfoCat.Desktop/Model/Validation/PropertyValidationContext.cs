using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
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
        private readonly ObservableCollection<string> _errors = new ObservableCollection<string>();
        private readonly ReadOnlyObservableCollection<string> _roErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event EventHandler HasErrorsChanged;

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        public bool HasErrors { get; private set; }

        public PropertyValidationContext(ModelValidationContext<TInstance> owner, PropertyDescriptor propertyDescriptor, IEnumerable<ValidationAttribute> validationAttributes)
            : base(owner, propertyDescriptor)
        {
            _roErrors = new ReadOnlyObservableCollection<string>(_errors);
            _errors.CollectionChanged += Errors_CollectionChanged;
            ValidationAttributes = new ReadOnlyCollection<ValidationAttribute>(validationAttributes.ToArray());
            Revalidate(Value);
        }

        protected override void OnInstancePropertyValueChanged(object oldValue, object newValue)
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
            ValidationContext validationContext = new ValidationContext(value);
            Collection<ValidationResult> validationResults = new Collection<ValidationResult>();
            if (Validator.TryValidateValue(value, validationContext, validationResults, ValidationAttributes))
                _errors.Clear();
            else
            {
                List<string> newMessages = validationResults.Select(v => v.ErrorMessage).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                if (newMessages.Count == 0)
                    newMessages.Add("Unspecified validation error");
                int index = -1;
                using (IEnumerator<string> enumerator = newMessages.GetEnumerator())
                {
                    while (++index < _errors.Count)
                    {
                        if (!enumerator.MoveNext())
                        {
                            while (_errors.Count > index)
                                _errors.RemoveAt(index);
                            return;
                        }
                        _errors[index] = enumerator.Current;
                    }
                    while (enumerator.MoveNext())
                        _errors.Add(enumerator.Current);
                }
            }
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

        public IEnumerable<string> GetErrors() => (_roErrors.Count > 0) ? _roErrors : null;

        #region Explicit Members

        IModelValidationContext IPropertyValidationContext.Owner => Owner;

        #endregion
    }
}
