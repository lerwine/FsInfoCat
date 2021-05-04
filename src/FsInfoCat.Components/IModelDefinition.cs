using System.Collections.Generic;

namespace FsInfoCat.Components
{
    public interface IModelDefinition
    {
        IReadOnlyList<IPropertyDefinition> Properties { get; }
    }
}
