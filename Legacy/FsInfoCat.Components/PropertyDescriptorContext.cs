using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Components
{
    public class PropertyDescriptorContext<TInstance, TProperty> : IPropertyDescriptorContext<TInstance>
        where TInstance : class
    {
        public TInstance Instance => OwnerContext.Instance;

        object ITypeDescriptorContext.Instance => Instance;

        public PropertyDescriptor PropertyDescriptor { get; }

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        public IContainer Container { get; }

        public TypeInstanceModel<TInstance> OwnerContext { get; }

        ITypeInstanceModel IPropertyDescriptorContext.OwnerContext => throw new NotImplementedException();

        public PropertyDescriptorContext(TypeInstanceModel<TInstance> componentContext, PropertyDefinitionModel<TInstance, TProperty> definition, IContainer container = null)
        {
            OwnerContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
            PropertyDescriptor = (definition ?? throw new ArgumentNullException(nameof(definition))).GetDescriptor();
            ValidationAttributes = definition.ValidationAttributes;
            Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.Equals(typeof(PropertyDescriptor)))
                return PropertyDescriptor;
            if (serviceType.Equals(typeof(IContainer)))
                return Container;
            return null;
        }

        public void OnComponentChanged() { }

        public bool OnComponentChanging() => !PropertyDescriptor.IsReadOnly;
    }
}
