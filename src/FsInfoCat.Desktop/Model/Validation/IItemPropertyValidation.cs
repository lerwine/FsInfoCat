using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IItemPropertyValidation : IItemPropertyDefinition, INotifyDataErrorInfo, IValidatableObject
    {
        ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }
    }
}
