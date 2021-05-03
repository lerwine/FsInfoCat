using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Components
{
    public class PropertyTypeDescriptorContext<TInstance, TProperty> : IPropertyTypeDescriptorContext<TInstance>
        where TInstance : class
    {
        public TInstance Instance => ComponentContext.Instance;

        object ITypeDescriptorContext.Instance => Instance;

        public PropertyDescriptor PropertyDescriptor { get; }

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        public IContainer Container { get; }

        public ModelInstance<TInstance> ComponentContext { get; }

        IModelInstance IPropertyTypeDescriptorContext.ComponentContext => throw new NotImplementedException();

        public PropertyTypeDescriptorContext(ModelInstance<TInstance> componentContext, PropertyDefinition<TInstance, TProperty> definition, IContainer container = null)
        {
            ComponentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
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
