using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Represents a property of a model object from the context of an instantiated model.
    /// </summary>
    public interface IModelPropertyContext : IModelProperty, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        event EventHandler ValueChanged;

        /// <summary>
        /// Gets the current property value.
        /// </summary>
        /// <returns>
        /// The property value.
        /// </returns>
        object RawValue { get; }

        /// <summary>
        /// Gets and sets a textual representation of a property value.
        /// </summary>
        string TextValue { get; set; }

        /// <summary>
        /// Returns a collection of standard values from the default context for the property.
        /// </summary>
        /// <value>
        /// A <see cref="ICollection" /> containing a standard set of valid values.
        /// </value>
        IReadOnlyList<IDisplayValue> StandardValues { get; }

        /// <summary>
        /// Describes the model instance that owns this property.
        /// </summary>
        new IModelContext Owner { get; }

        /// <summary>
        /// Represents the corresponding property object of the <see cref="IModelPropertyDescriptor"/> that was used to create the owning context object.
        /// </summary>
        IModelPropertyDescriptor Descriptor { get; }

        ValidationResult[] Validate();

        ValidationResult[] Revalidate();
    }

    /// <summary>
    /// Represents a property of a model object from the context of an instantiated model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IModelPropertyContext<TModel> : IModelProperty<TModel>, IModelPropertyContext
        where TModel : class
    {
        /// <summary>
        /// Describes the model instance that owns this property.
        /// </summary>
        new IModelContext<TModel> Owner { get; }

        /// <summary>
        /// Represents the corresponding property object of the <see cref="IModelPropertyDescriptor{TModel}"/> that was used to create the owning context object.
        /// </summary>
        new IModelPropertyDescriptor<TModel> Descriptor { get; }
    }

    /// <summary>
    /// Represents a property of a model object from the context of an instantiated model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IModelPropertyContext<TModel, TValue> : IModelProperty<TModel, TValue>, ITypedModelPropertyContext<TValue>, IModelPropertyContext<TModel>
        where TModel : class
    {
        /// <summary>
        /// Returns a collection of standard values from the default context for the property.
        /// </summary>
        /// <value>
        /// A <see cref="ICollection" /> containing a standard set of valid values.
        /// </value>
        new ReadOnlyObservableCollection<IDisplayValue<TValue>> StandardValues { get; }

        /// <summary>
        /// Represents the corresponding property object of the <see cref="IModelPropertyDescriptor{TModel, TValue}"/> that was used to create the owning context object.
        /// </summary>
        new IModelPropertyDescriptor<TModel, TValue> Descriptor { get; }
    }
}
