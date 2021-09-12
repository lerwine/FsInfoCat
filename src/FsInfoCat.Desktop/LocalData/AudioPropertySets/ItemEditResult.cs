using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.AudioPropertySets
{
    public record ItemEditResult(AudioPropertiesListItem ItemEntity, EntityEditResultState State) : IEntityEditResult<AudioPropertiesListItem>;
}
