using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IItemPropertyInfo
    {
        string Name { get; }
        Type PropertyType { get; }
        string Description { get; }
        string Category { get; }
        string DisplayName { get; }
        bool IsReadOnly { get; }
    }
}
