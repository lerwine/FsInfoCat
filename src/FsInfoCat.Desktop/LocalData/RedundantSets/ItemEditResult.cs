using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public record ItemEditResult(RedundantSetListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<RedundantSetListItem>;
}
