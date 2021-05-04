using System.Collections.Generic;

namespace FsInfoCat.Components
{
    public interface ITypeDefinitionModel
    {
        IReadOnlyList<IPropertyDefinitionModel> Properties { get; }
    }
}
