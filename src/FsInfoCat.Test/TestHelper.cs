using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
