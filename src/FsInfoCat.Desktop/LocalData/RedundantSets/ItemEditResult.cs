using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public record ItemEditResult(RedundantSetListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<RedundantSetListItem>;
}
