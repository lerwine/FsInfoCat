using FsInfoCat.Desktop.Model.ComponentSupport;
using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class ModelValidationContext<TInstance> : ModelContextBase<TInstance, IPropertyValidationContext<TInstance>>, IModelContext<TInstance>,
        IModelValidationContext
        where TInstance : class
    {
        public event EventHandler HasErrorsChanged;
        public event EventHandler<ModelErrorsChangedEventArgs> ModelErrorsChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly LinkedComponentList<IPropertyValidationContext<TInstance>> _invalidProperties = new LinkedComponentList<IPropertyValidationContext<TInstance>>();

        IModelDescriptor IModelContext.ModelDescriptor => ModelDescriptor;

        IReadOnlyList<IPropertyContext<TInstance>> IModelContext<TInstance>.Properties => Properties;

        IReadOnlyList<IPropertyValidationContext> IModelValidationContext.Properties => Properties;

        IReadOnlyList<IPropertyContext> IModelContext.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        protected PropertyDescriptor PropertyDescriptor => null;

        IContainer ITypeDescriptorContext.Container => throw new NotImplementedException();

        public bool HasErrors { get; private set; }

        object ITypeDescriptorContext.Instance => Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

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

        protected virtual void OnHasErrorsChanged(bool hasErrors)
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
    }
}
