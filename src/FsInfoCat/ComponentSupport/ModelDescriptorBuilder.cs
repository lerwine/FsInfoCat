using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Creates an instance of an <seealso cref="IModelTypeDescriptor{TModel}"/>, providing methods to customize the output.
    /// </summary>
    /// <typeparam name="TModel">The type of the target object model.</typeparam>

    public class ModelDescriptorBuilder<TModel> where TModel : class
    {
        /// <summary>
        /// The target object model type.
        /// </summary>
        public Type ModelType { get; }

        public ModelDescriptorBuilder()
        {
            ModelType = typeof(TModel);
        }

        /// <summary>
        /// Set options for the property of a model.
        /// </summary>
        /// <param name="propertyBuilder">The <see cref="IPropertyBuilder{TOwner, TValue}"/> that is about to be added.</param>
        protected virtual void SetPropertyOptions(IPropertyBuilder<TModel> propertyBuilder)
        {
            propertyBuilder.IgnoreProperty = propertyBuilder.Descriptor.DesignTimeOnly;
        }

        /// <summary>
        /// Create a new <see cref="IModelTypeDescriptor{TModel}"/> instance.
        /// </summary>
        /// <returns>The new <see cref="IModelTypeDescriptor{TModel}"/> instance.</returns>
        public IModelTypeDescriptor<TModel> Build() => new ModelDescriptor<TModel>(this);

        internal IList<ModelPropertyDescriptor<TModel>> BuildProperties(ModelDescriptor<TModel> owner)
        {
            Collection<ModelPropertyDescriptor<TModel>> properties = new Collection<ModelPropertyDescriptor<TModel>>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(ModelType))
            {
                PropertyBuilder<TModel> builder = (PropertyBuilder<TModel>)Activator.CreateInstance(typeof(PropertyBuilder<,>).MakeGenericType(ModelType, propertyDescriptor.PropertyType), new object[] { owner, propertyDescriptor });
                SetPropertyOptions(builder);
                if (!builder.IgnoreProperty)
                    properties.Add(builder.Build());
            }

            return properties;
        }
    }
}
