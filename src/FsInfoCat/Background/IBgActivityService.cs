using Microsoft.Extensions.Hosting;

namespace FsInfoCat.Background
{
    interface IBgActivityService : IBgActivitySource, IHostedService
    {
    }
}
