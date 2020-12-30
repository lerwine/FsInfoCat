using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface ILogin : IValidatableModel
    {
        /// <summary>
        /// Gets the user's login name.
        /// </summary>
        string LoginName { get; set; }
    }
}
