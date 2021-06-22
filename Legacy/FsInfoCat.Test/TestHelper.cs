using Microsoft.Extensions.Logging;

namespace FsInfoCat.Test
{
    public static class TestHelper
    {
        public static readonly ILoggerFactory LoggerFactory;

        static TestHelper()
        {
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
                builder.AddConsole();
            });
        }
    }
}
