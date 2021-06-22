using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    public class TypeDefinitionModel<TOwner> : ITypeDefinitionModel
        where TOwner : class
    {
        public ReadOnlyCollection<IPropertyDefinitionModel<TOwner>> Properties { get; }

        IReadOnlyList<IPropertyDefinitionModel> ITypeDefinitionModel.Properties => Properties;
    }
}
