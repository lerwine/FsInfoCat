using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public interface ITypedProperty<TValue> : IModelProperty
    {
        /// <summary>
        /// Returns a collection of standard values from the default context for the property.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> containing a standard set of valid values, or <see langword="null" /> if the property does not support a standard
        /// set of values.
        /// </returns>
        new IEnumerable<TValue> GetStandardValues();
    }
}
