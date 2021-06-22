using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    public interface ITypeInstanceModel : ITypeDescriptorContext
    {
        IReadOnlyList<IPropertyInstanceModel> Properties { get; }
    }
}
