using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.Validation
{
    public sealed class PropertyValidationContext<TInstance, TValue> : PropertyContext<TInstance, TValue, ModelValidationContext<TInstance>>, IPropertyValidationContext<TInstance>
        where TInstance : class
    {
        public PropertyValidationContext(ModelValidationContext<TInstance> owner, PropertyDescriptor propertyDescriptor) : base(owner, propertyDescriptor)
        {
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => throw new NotImplementedException();

        IModelValidationContext IPropertyValidationContext.Owner => Owner;

        public IEnumerable<string> GetErrors()
        {
            throw new NotImplementedException();
        }
    }
}
