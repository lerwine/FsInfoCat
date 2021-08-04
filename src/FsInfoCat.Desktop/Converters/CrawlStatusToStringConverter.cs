using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(CrawlStatus), typeof(string))]
    public sealed class CrawlStatusToStringConverter : SchemaEnumToStringComverter<CrawlStatus>
    {

    }
}
