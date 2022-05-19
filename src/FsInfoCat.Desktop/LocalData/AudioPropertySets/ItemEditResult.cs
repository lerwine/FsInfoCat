using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;

namespace FsInfoCat.Desktop.LocalData.AudioPropertySets
{
    public record ItemEditResult(AudioPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<AudioPropertiesListItem>;
}
