using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    public class ModelDefinition<TComponent> : IModelDefinition
        where TComponent : class
    {
        public ReadOnlyCollection<IPropertyDefinition<TComponent>> Properties { get; }

        IReadOnlyList<IPropertyDefinition> IModelDefinition.Properties => Properties;
    }
}
