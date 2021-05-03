using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface INotifyComponentPropertyChangeRelay : IComponentRelay, INotifyPropertyChanged, IRevertibleChangeTracking, IItemProperties
    {
        new ReadOnlyCollection<IItemPropertyDefinition> ItemProperties { get; }
    }

    public interface INotifyComponentPropertyChangeRelay<TComponent> : IComponentRelay<TComponent>, INotifyComponentPropertyChangeRelay
    {
    }
}
