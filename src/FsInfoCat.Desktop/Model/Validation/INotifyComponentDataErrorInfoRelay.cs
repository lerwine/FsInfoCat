using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface INotifyComponentDataErrorInfoRelay : INotifyDataErrorInfo, IValidatableObject, INotifyComponentPropertyChangeRelay
    {
        new ReadOnlyCollection<IItemPropertyValidation> ItemProperties { get; }
        new IEnumerable<string> GetErrors(string propertyName);
    }

    public interface INotifyComponentDataErrorInfoRelay<TComponent> : INotifyComponentDataErrorInfoRelay, INotifyComponentPropertyChangeRelay<TComponent>
    {
    }
}
