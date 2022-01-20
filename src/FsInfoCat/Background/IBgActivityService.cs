using Microsoft.Extensions.Hosting;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    interface IBgActivityService : IBgActivitySource, IHostedService
    {
    }
}
