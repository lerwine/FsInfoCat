using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    public interface IModelPropertyContext : IModelProperty, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// set of values.
        /// </returns>
        object Value { get; }

        /// <summary>
        /// Returns a collection of standard values from the default context for the property.
        /// </summary>
        /// <value>
        /// A <see cref="ICollection" /> containing a standard set of valid values.
        /// </value>
        ICollection StandardValues { get; }

        new IModelContext Owner { get; }

        IModelPropertyDescriptor Descriptor { get; }
    }

    public interface IModelPropertyContext<TModel> : IModelProperty<TModel>, IModelPropertyContext
        where TModel : class
    {
        new IModelContext<TModel> Owner { get; }

        new IModelPropertyDescriptor<TModel> Descriptor { get; }
    }

    public interface IModelPropertyContext<TModel, TValue> : IModelProperty<TModel, TValue>, ITypedModelPropertyContext<TValue>, IModelPropertyContext<TModel>
        where TModel : class
    {

        new ReadOnlyObservableCollection<TValue> StandardValues { get; }

        new IModelPropertyDescriptor<TModel, TValue> Descriptor { get; }
    }
}
