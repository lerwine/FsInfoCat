using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.ComponentSupport
{
    internal abstract class PropertyBuilder<TModel> : IPropertyBuilder<TModel>
        where TModel : class
    {
        public bool IgnoreProperty { get; set; }

        public bool UseInvariantStringConversion { get; set; }

        internal PropertyDescriptor Descriptor { get; }

        PropertyDescriptor IPropertyBuilder<TModel>.Descriptor => Descriptor;

        internal ModelDescriptor<TModel> Owner { get; }

        IModelDescriptor<TModel> IPropertyBuilder<TModel>.Owner => Owner;

        internal ValidationAttributeCollection ValidationAttributes { get; } = new ValidationAttributeCollection();

        IList<ValidationAttribute> IPropertyBuilder<TModel>.ValidationAttributes => ValidationAttributes;

        protected PropertyBuilder(ModelDescriptor<TModel> owner, PropertyDescriptor propertyDescriptor)
        {
            Owner = owner;
            Descriptor = propertyDescriptor;
            foreach (ValidationAttribute attribute in propertyDescriptor.Attributes.OfType<ValidationAttribute>())
                ValidationAttributes.Add(attribute);
        }

        internal abstract ModelPropertyDescriptor<TModel> Build();
    }

    internal class PropertyBuilder<TModel, TValue> : PropertyBuilder<TModel>, IPropertyBuilder<TModel, TValue>
        where TModel : class
    {
        public IEqualityComparer<TValue> Comparer { get; set; } = EqualityComparer<TValue>.Default;

        public PropertyBuilder(ModelDescriptor<TModel> owner, PropertyDescriptor propertyDescriptor) : base(owner, propertyDescriptor) { }

        internal override ModelPropertyDescriptor<TModel> Build() => new ModelPropertyDescriptor<TModel, TValue>(this);
    }
}
