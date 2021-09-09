using FsInfoCat.Local;

namespace FsInfoCat.Desktop.LocalData.SummaryPropertySets
{
    public record ItemEditResult(SummaryPropertiesListItem ItemEntity, Microsoft.EntityFrameworkCore.EntityState Result);
}
