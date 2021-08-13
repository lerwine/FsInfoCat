using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class SaveCrawlConfigAsyncOpManager : AsyncOpResultManagerViewModel<(CrawlConfiguration, EditCrawlConfigVM),
        AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)>,
        AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)>.StatusListenerImpl,
        (CrawlConfiguration Configuration, Subdirectory Root)>
    {
        internal async Task<(CrawlConfiguration Configuration, Subdirectory Root)> SaveChangesAsync((CrawlConfiguration Entity, EditCrawlConfigVM ViewModel) state,
            AsyncFuncOpViewModel<(CrawlConfiguration, EditCrawlConfigVM), (CrawlConfiguration Configuration, Subdirectory Root)>.StatusListenerImpl statusListener)
        {
            EditCrawlConfigVM vm = state.ViewModel ?? throw new ArgumentNullException(nameof(state.ViewModel));
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            EntityEntry<CrawlConfiguration> entry;
            if (state.Entity is null)
            {
                CrawlConfiguration model = vm.Dispatcher.Invoke(() => new CrawlConfiguration()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    DisplayName = vm.DisplayName,
                    MaxRecursionDepth = vm.MaxRecursionDepth,
                    Notes = vm.Notes,
                    Root = vm.Root,
                    StatusValue = vm.IsEnabled ? CrawlStatus.NotRunning : CrawlStatus.Disabled
                });
                model.ModifiedOn = model.CreatedOn;
                entry = dbContext.CrawlConfigurations.Add(model);
            }
            else
                entry = dbContext.Entry(state.Entity);
            vm.Dispatcher.Invoke(() =>
            {
                CrawlConfiguration model = entry.Entity;
                if (entry.State != EntityState.Added)
                {
                    model.ModifiedOn = DateTime.Now;
                    model.DisplayName = vm.DisplayName;
                    model.MaxRecursionDepth = vm.MaxRecursionDepth;
                    model.Notes = vm.Notes;
                    model.Root = vm.Root;
                    if (!vm.IsEnabled)
                        model.StatusValue = CrawlStatus.Disabled;
                }
                if (vm.LimitTotalItems)
                    model.MaxTotalItems = vm.MaxTotalItems;
                else
                    model.MaxTotalItems = null;
                if (vm.LimitTTL)
                    model.TTL = ((((vm.TtlDays.Value * 24L) + vm.TtlHours.Value) * 60L) + vm.TtlMinutes.Value) * 60L;
                else
                    model.TTL = null;
                if (vm.AutoReschedule)
                {
                    model.RescheduleAfterFail = vm.RescheduleAfterFail;
                    model.RescheduleFromJobEnd = vm.RescheduleFromJobEnd;
                    model.RescheduleInterval = ((((vm.RescheduleDays.Value * 24L) + vm.RescheduleHours.Value) * 60L) + vm.RescheduleMinutes.Value) * 60L;
                }
                else
                {
                    model.RescheduleAfterFail = false;
                    model.RescheduleFromJobEnd = false;
                    model.RescheduleInterval = null;
                }
                if (vm.NoReschedule)
                    model.NextScheduledStart = null;
                else
                    model.NextScheduledStart = new DateTime(vm.NextScheduledStartDate.Value.Year, vm.NextScheduledStartDate.Value.Month, vm.NextScheduledStartDate.Value.Day,
                        (vm.NextScheduledStartHour.Value == 12) ? (vm.NextScheduledStartIsPm ? 12 : 0) : (vm.NextScheduledStartIsPm ? vm.NextScheduledStartHour.Value + 12 : vm.NextScheduledStartHour.Value),
                        vm.NextScheduledStartMinute.Value, 0, DateTimeKind.Local);
            });
            try
            {
                await dbContext.SaveChangesAsync(true, statusListener.CancellationToken);
                if (entry.State != EntityState.Unchanged)
                    throw new InvalidOperationException("Failed to save changes to the database.");
            }
            catch
            {
                if (state.Entity is not null)
                    await entry.ReloadAsync(statusListener.CancellationToken);
                throw;
            }
            return (entry.Entity, await entry.GetRelatedReferenceAsync(e => e.Root, statusListener.CancellationToken));
        }
    }
}
