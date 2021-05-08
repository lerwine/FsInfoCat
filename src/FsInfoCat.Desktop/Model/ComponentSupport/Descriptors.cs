using FsInfoCat.Desktop.Model.Validation;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public static class Descriptors
    {
        public static IPropertyContext<TInstance> CreatePropertyContext<TInstance>(ModelContext<TInstance> owner, TInstance instance,
            IModelPropertyDescriptor<TInstance> propertyDescriptor)
            where TInstance : class
        {
            throw new NotImplementedException();
        }

        internal static IPropertyValidationContext<TInstance> CreatePropertyValidationContext<TInstance>(ModelValidationContext<TInstance> owner,
            TInstance instance, IModelPropertyDescriptor<TInstance> pd) where TInstance : class
        {
            throw new NotImplementedException();
        }
    }
}
