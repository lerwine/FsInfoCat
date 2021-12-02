using Microsoft.Extensions.Hosting;

namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    interface IBgActivityService : IBgActivitySource, IHostedService
    {
    }
}
