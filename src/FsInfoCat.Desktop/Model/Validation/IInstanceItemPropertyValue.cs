using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IInstanceItemPropertyValue : IItemPropertyInfo, IRevertibleChangeTracking
    {
        object Value { get; }
    }


    public interface IInstanceItemPropertyValue<TValue> : IInstanceItemPropertyValue
    {
        new TValue Value { get; }
    }
}
