using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class GetSubdirectoryFullNameAsyncOpManager : AsyncOpResultManagerViewModel<Subdirectory,
        AsyncFuncOpViewModel<Subdirectory, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>,
        AsyncFuncOpViewModel<Subdirectory, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl,
        (CrawlConfiguration Configuration, Subdirectory Root, string Path)>
    {
        internal async Task<(CrawlConfiguration Configuration, Subdirectory Root, string Path)> LookupFullNameAsync(Subdirectory subdirectory,
            AsyncFuncOpViewModel<Subdirectory, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl statusListener)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry<Subdirectory> entry = dbContext.Entry(subdirectory);
            Guid id = subdirectory.Id;
            CrawlConfiguration crawlConfiguration = await (from c in dbContext.CrawlConfigurations where c.RootId == id select c).FirstOrDefaultAsync(statusListener.CancellationToken);
            Subdirectory parent = subdirectory.ParentId.HasValue ? await entry.GetRelatedReferenceAsync(d => d.Parent, statusListener.CancellationToken) : null;
            string path = subdirectory.Name;
            if (parent is null)
                return (crawlConfiguration, subdirectory, path);
            StringBuilder stringBuilder = new(path);
            do
            {
                stringBuilder.Append('/').Append(parent.Name);
                entry = dbContext.Entry(subdirectory);
            }
            while (parent.ParentId.HasValue && (parent = await entry.GetRelatedReferenceAsync(d => d.Parent, statusListener.CancellationToken)) is not null);
            return (crawlConfiguration, subdirectory, stringBuilder.ToString());
        }
    }
}
