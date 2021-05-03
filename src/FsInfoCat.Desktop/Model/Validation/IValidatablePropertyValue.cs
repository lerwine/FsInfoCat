using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IValidatablePropertyValue : IInstanceItemPropertyValue, IValidatableObject, INotifyDataErrorInfo
    {
    }

    public interface IValidatablePropertyValue<TValue> : IInstanceItemPropertyValue<TValue>, IValidatablePropertyValue
    {
    }
}
