using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.SymbolicNames
{
    public record ItemEditResult(SymbolicNameListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<SymbolicNameListItem>;
}
