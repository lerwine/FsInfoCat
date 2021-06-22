using Microsoft.Extensions.Logging;

namespace FsInfoCat
{
    public interface ILoggingService
    {
        ILogger<T> CreateLogger<T>();
    }
}
