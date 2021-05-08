using FsInfoCat.Desktop.Model.ComponentSupport;
using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.Model.Validation
{
    public sealed class ModelValidationContext<TInstance> : ModelContextBase<TInstance, IPropertyValidationContext<TInstance>>,
        IModelContext<TInstance>, IModelValidationContext
        where TInstance : class
    {
        public event EventHandler HasErrorsChanged;
        public event EventHandler<ModelErrorsChangedEventArgs> ModelErrorsChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly LinkedComponentList<IPropertyValidationContext<TInstance>> _invalidProperties = new LinkedComponentList<IPropertyValidationContext<TInstance>>();

        IPropertyContext<TInstance> IReadOnlyDictionary<string, IPropertyContext<TInstance>>.this[string key] => this[key];

        IPropertyContext IReadOnlyDictionary<string, IPropertyContext>.this[string key] => this[key];

        IModelDescriptor IModelContext.ModelDescriptor => ModelDescriptor;

        IReadOnlyList<IPropertyContext<TInstance>> IModelContext<TInstance>.Properties => Properties;

        IReadOnlyList<IPropertyValidationContext> IModelValidationContext.Properties => Properties;

        IReadOnlyList<IPropertyContext> IModelContext.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        IContainer ITypeDescriptorContext.Container => null;

        public bool HasErrors { get; private set; }

        object ITypeDescriptorContext.Instance => Instance;

        int IReadOnlyCollection<KeyValuePair<string, IPropertyContext>>.Count => throw new NotImplementedException();

        int IReadOnlyCollection<KeyValuePair<string, IPropertyContext<TInstance>>>.Count => throw new NotImplementedException();

        IEnumerable<IPropertyContext<TInstance>> IReadOnlyDictionary<string, IPropertyContext<TInstance>>.Values => throw new NotImplementedException();

        IEnumerable<IPropertyContext> IReadOnlyDictionary<string, IPropertyContext>.Values => throw new NotImplementedException();

        public ModelValidationContext(ModelDescriptor<TInstance> modelDescriptor, TInstance instance) :
            base(modelDescriptor, instance, (owner, pd) => Descriptors.CreatePropertyValidationContext((ModelValidationContext<TInstance>)owner, instance, pd))
        {
            _invalidProperties.EmptyChanged += InvalidProperties_EmptyChanged;
            foreach (IPropertyValidationContext<TInstance> property in Properties)
            {
                if (property.HasErrors)
                    _invalidProperties.Add(property);
                WeakEventManager<IPropertyValidationContext<TInstance>, EventArgs>.AddHandler(property,
                    nameof(IPropertyValidationContext<TInstance>.HasErrorsChanged),
                    OnHasErrorsChangedChanged);
            }
        }

        private void InvalidProperties_EmptyChanged(object sender, EventArgs e)
        {
            HasErrors = !_invalidProperties.IsEmpty;
            OnHasErrorsChanged(HasErrors);
        }

        private void OnHasErrorsChanged(bool hasErrors)
        {
            HasErrorsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnHasErrorsChangedChanged(object sender, EventArgs e)
        {
            if (sender is IPropertyValidationContext<TInstance> property)
            {
                if (property.HasErrors)
                {
                    if (!_invalidProperties.Contains(property))
                        _invalidProperties.Add(property);
                }
                else if (_invalidProperties.Contains(property))
                    _invalidProperties.Remove(property);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        void ITypeDescriptorContext.OnComponentChanged() { }

        public IEnumerable<ValidationResult> Revalidate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        bool IReadOnlyDictionary<string, IPropertyContext>.TryGetValue(string key, out IPropertyContext value)
        {
            if (TryGetValue(key, out IPropertyValidationContext<TInstance> result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        bool IReadOnlyDictionary<string, IPropertyContext<TInstance>>.TryGetValue(string key, out IPropertyContext<TInstance> value)
        {
            if (TryGetValue(key, out IPropertyValidationContext<TInstance> result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        IEnumerator<KeyValuePair<string, IPropertyContext<TInstance>>> IEnumerable<KeyValuePair<string, IPropertyContext<TInstance>>>.GetEnumerator()
        {
            return Properties.Select(p => new KeyValuePair<string, IPropertyContext<TInstance>>(p.Name, p)).GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, IPropertyContext>> IEnumerable<KeyValuePair<string, IPropertyContext>>.GetEnumerator()
        {
            return Properties.Select(p => new KeyValuePair<string, IPropertyContext>(p.Name, p)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Properties.Select(p => new KeyValuePair<string, IPropertyContext<TInstance>>(p.Name, p))).GetEnumerator();
    }
}
