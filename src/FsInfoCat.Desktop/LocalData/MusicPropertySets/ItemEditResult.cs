using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.MusicPropertySets
{
    public record ItemEditResult(MusicPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<MusicPropertiesListItem>;
}
