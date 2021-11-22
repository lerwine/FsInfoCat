using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace FsInfoCat.Services
{
    /// <summary>
    /// Defines a service for reporting the progress of background processes.
    /// </summary>
    public interface IBackgroundProgressService : IBackgroundProgressFactory, IHostedService, IObservable<IBackgroundProgressEvent>, IObservable<bool>, IReadOnlyCollection<IBackgroundOperation>
    {
        bool IsActive { get; }
    }
}
