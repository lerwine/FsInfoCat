using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.SymbolicNames
{
    public record ItemEditResult(SymbolicNameListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
