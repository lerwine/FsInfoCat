using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Models
{
    public interface IModificationAccountAuditable : IModficationAuditable
    {
        [Editable(false)]
        [Display(Name = "Created By")]
        [ForeignKey(nameof(Account))]
        new Account CreatedBy { get; set; }

        [Editable(false)]
        [Display(Name = "Modified By")]
        [ForeignKey(nameof(Account))]
        new Account ModifiedBy { get; set; }
    }
}
