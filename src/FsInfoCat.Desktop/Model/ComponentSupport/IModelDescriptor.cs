using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Generic representation of the characteristics of a model class and its properties.
    /// </summary>
    /// <remarks>This is functionally similar to a <see cref="System.ComponentModel.TypeDescriptor"/>.</remarks>
    public interface IModelDescriptor : IEquatable<IModelDescriptor>, IModelInfo, IReadOnlyDictionary<string, IModelPropertyDescriptor>,
        IEqualityComparer
    {
        Type ComponentType { get; }

        new IReadOnlyList<IModelPropertyDescriptor> Properties { get; }
    }
}
