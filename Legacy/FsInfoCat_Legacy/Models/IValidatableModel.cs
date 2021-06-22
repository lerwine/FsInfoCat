using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IValidatableModel : INormalizable, IValidatableObject
    {
        IList<ValidationResult> ValidateAll();
    }
}
