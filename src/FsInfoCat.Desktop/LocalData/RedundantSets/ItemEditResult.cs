using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public record ItemEditResult(RedundantSetListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
