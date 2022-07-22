using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.BinaryRedundantSets
{
    public record ItemEditResult(RedundantSetListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<RedundantSetListItem>;
}
