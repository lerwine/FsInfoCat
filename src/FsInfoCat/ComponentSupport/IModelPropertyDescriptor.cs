using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    public interface IModelPropertyDescriptor : IModelProperty
    {
        /// <summary>
        /// Gets a value indicating whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if value change notifications may originate from outside the property descriptor; otherwise, <see langword="false"/>.
        /// </value>
        bool SupportsChangeEvents { get; }

        /// <summary>
        /// Gets the attributes for property validation.
        /// </summary>
        /// <value>
        /// The <see cref="ValidationAttribute"/> objects used for property validation.
        /// </value>
        IReadOnlyList<ValidationAttribute> ValidationAttributes { get; }

        new IModelTypeDescriptor Owner { get; }

        object GetValue(object model);
    }

    public interface IModelPropertyDescriptor<TModel> : IModelProperty<TModel>, IModelPropertyDescriptor where TModel : class
    {
        new IModelTypeDescriptor<TModel> Owner { get; }

        object GetValue(TModel model);
    }

    public interface IModelPropertyDescriptor<TModel, TValue> : IModelProperty<TModel, TValue>, ITypedModelPropertyDescriptor<TValue>, IModelPropertyDescriptor<TModel>
        where TModel : class
    {
        new TValue GetValue(TModel model);
    }
}
