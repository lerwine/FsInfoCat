using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class ModelValidationContext<TInstance> : ModelContextBase<TInstance, IPropertyValidationContext<TInstance>>, IModelContext<TInstance>, IModelValidationContext
        where TInstance : class
    {
        public event EventHandler<ModelErrorsChangedEventArgs> ModelErrorsChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IModelDescriptor IModelContext.ModelDescriptor => ModelDescriptor;

        IReadOnlyList<IPropertyContext<TInstance>> IModelContext<TInstance>.Properties => Properties;

        IReadOnlyList<IPropertyValidationContext> IModelValidationContext.Properties => Properties;

        IReadOnlyList<IPropertyContext> IModelContext.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        protected PropertyDescriptor PropertyDescriptor => null;

        IContainer ITypeDescriptorContext.Container => throw new NotImplementedException();

        public bool HasErrors => throw new NotImplementedException();

        object ITypeDescriptorContext.Instance => Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        public ModelValidationContext(ModelDescriptor<TInstance> modelDescriptor, TInstance instance) : base(modelDescriptor, instance)
        {
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
