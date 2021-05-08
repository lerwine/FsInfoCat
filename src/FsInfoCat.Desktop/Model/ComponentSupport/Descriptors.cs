using FsInfoCat.Desktop.Model.Validation;
using System;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public static class Descriptors
    {
        public static IModelDescriptor CreateModelDescriptor(Type type, Func<PropertyDescriptor, bool> filter = null) =>
            (IModelDescriptor)typeof(ModelDescriptor<>).MakeGenericType(type).GetMethod(nameof(ModelDescriptor<object>.Create),
                new Type[] { typeof(Func<PropertyDescriptor, bool>), typeof(Func<,,>) }).Invoke(null, new object[] { filter, null });

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
