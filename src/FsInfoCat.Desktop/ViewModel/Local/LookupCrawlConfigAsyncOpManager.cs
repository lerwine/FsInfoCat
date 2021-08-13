using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class LookupCrawlConfigAsyncOpManager : AsyncOpResultManagerViewModel<string, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>
    {
        internal static async Task<(CrawlConfiguration Configuration, Subdirectory Root, string ValidatedPath)> LookupCrawlConfig(string path, AsyncFuncOpViewModel<string, (CrawlConfiguration Configuration, Subdirectory Root, string Path)>.StatusListenerImpl statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            statusListener.SetMessage("Checking for existing directory information", StatusMessageLevel.Information);
            DirectoryInfo crawlRoot;
            try { crawlRoot = new DirectoryInfo(path); }
            catch (System.Security.SecurityException securityException)
            {
                throw new AsyncOperationFailureException(securityException.Message, ErrorCode.SecurityException,
                    FsInfoCat.Properties.Resources.ErrorMessage_SecurityException, securityException);
            }
            catch (PathTooLongException pathTooLongException)
            {
                throw new AsyncOperationFailureException(pathTooLongException.Message, ErrorCode.PathTooLong,
                    FsInfoCat.Properties.Resources.ErrorMessage_PathTooLongError, pathTooLongException);
            }
            catch (Exception exception)
            {
                throw new AsyncOperationFailureException(exception.Message, ErrorCode.InvalidPath, FsInfoCat.Properties.Resources.ErrorMessage_InvalidPathError,
                    exception);
            }
            if (!crawlRoot.Exists)
                throw new AsyncOperationFailureException(FsInfoCat.Properties.Resources.ErrorMessage_DirectoryNotFound, ErrorCode.DirectoryNotFound);
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<Subdirectory> subdirectory = await Subdirectory.ImportBranchAsync(crawlRoot, dbContext, statusListener.CancellationToken);
            CrawlConfiguration crawlConfiguration;
            if (subdirectory.State == EntityState.Added)
            {
                statusListener.SetMessage("Importing new path information");
                await dbContext.SaveChangesAsync(statusListener.CancellationToken);
                crawlConfiguration = null;
            }
            else
            {
                statusListener.SetMessage("Checking for existing configuration");
                crawlConfiguration = await subdirectory.GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
            }
            return (crawlConfiguration, subdirectory.Entity, crawlRoot.FullName);
            //Dispatcher.Invoke(() => vm.Initialize(crawlConfiguration, subdirectory.Entity, crawlRoot.FullName));
        }
    }
}
