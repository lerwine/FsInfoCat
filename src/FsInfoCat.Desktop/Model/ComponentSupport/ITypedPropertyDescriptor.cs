using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public interface ITypedPropertyDescriptor<TValue> : ITypedProperty<TValue>, IModelPropertyDescriptor,
        IEquatable<ITypedPropertyDescriptor<TValue>>, IEqualityComparer<TValue>
    {
        /// <summary>
        /// Gets the value of this property on the specified object.
        /// </summary>
        /// <param name="component">The component that contains the current property.</param>
        /// <returns>The value of this property on the specified <paramref name="component"/> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        /// <exception cref="InvalidCastException"><paramref name="component"/> type does not match the underlying owner type.</exception>
        new TValue GetValue(object component);
    }
}
