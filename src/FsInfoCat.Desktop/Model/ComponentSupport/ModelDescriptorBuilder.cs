using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ModelDescriptorBuilder<TModel>
        where TModel : class
    {
        /// <summary>
        /// Determines whether a property will be included in the <see cref="ModelDescriptor{TModel}"/>.
        /// </summary>
        /// <param name="modelDescriptor">The <see cref="ModelDescriptor{TModel}"/> being initalized.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <returns><see langword="true"/> to include the property; otherwise, <see langword="false"/>.</returns>
        public virtual bool ShouldIncludeProperty(ModelDescriptor<TModel> modelDescriptor, PropertyDescriptor propertyDescriptor)
            => !propertyDescriptor.DesignTimeOnly;

        internal IPropertyBuilder CreatePropertyBuilder(ModelDescriptor<TModel> modelDescriptor, PropertyDescriptor propertyDescriptor)
        {
            IPropertyBuilder builder = (IPropertyBuilder)Activator.CreateInstance(typeof(PropertyBuilder<>)
                .MakeGenericType(propertyDescriptor.PropertyType), new object[] { modelDescriptor, propertyDescriptor });
            ConfigurePropertyBuilder(builder);
            return builder;
        }

        /// <summary>
        /// Sets the validation attributes for a property.
        /// </summary>
        /// <param name="propertyDescriptor">The property descriptor being initalized.</param>
        /// <param name="attributes">An <see cref="IPropertyBuilder"/> that contains an attribute list,
        /// which is pre-populated with the property's declared validation attributes.</param>
        public virtual void ConfigurePropertyBuilder(IPropertyBuilder propertyBuilder) { }

        /// <summary>
        /// Creates the result <see cref="ModelDescriptor{TModel}"/>.
        /// </summary>
        /// <returns>The new <see cref="ModelDescriptor{TModel}"/>.</returns>
        public ModelDescriptor<TModel> Build() => new ModelDescriptor<TModel>(this);

        public interface IPropertyBuilder
        {
            ModelDescriptor<TModel> ModelDescriptor { get; }
            PropertyDescriptor PropertyDescriptor { get; }
            ValidationAttributeList ValidationAttributes { get; }
            IModelPropertyDescriptor<TModel> Build();
        }

        public class PropertyBuilder<TValue> : IPropertyBuilder
        {
            public IEqualityComparer<TValue> EqualityComparer { get; set; }

            public ModelDescriptor<TModel> ModelDescriptor { get; }

            public PropertyDescriptor PropertyDescriptor { get; }

            public ValidationAttributeList ValidationAttributes { get; } = new ValidationAttributeList();

            internal PropertyBuilder(ModelDescriptor<TModel> modelDescriptor, PropertyDescriptor propertyDescriptor)
            {
                ModelDescriptor = modelDescriptor ?? throw new ArgumentNullException(nameof(modelDescriptor));
                PropertyDescriptor = propertyDescriptor ?? throw new ArgumentNullException(nameof(propertyDescriptor));
                foreach (ValidationAttribute attr in propertyDescriptor.Attributes.OfType<ValidationAttribute>())
                    ValidationAttributes.Add(attr);
            }

            public ModelPropertyDescriptor<TModel, TValue> Build() => new ModelPropertyDescriptor<TModel, TValue>(this);

            IModelPropertyDescriptor<TModel> IPropertyBuilder.Build() => Build();
        }
    }
}
