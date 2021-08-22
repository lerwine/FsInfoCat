using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public static class AttachedProperties
    {
        public static string GetFullName(DependencyObject obj)
        {
            return (string)obj.GetValue(FullNameProperty);
        }

        public static void SetFullName(DependencyObject obj, string value)
        {
            obj.SetValue(FullNameProperty, value);
        }

        public static readonly DependencyProperty FullNameProperty = DependencyProperty.RegisterAttached(nameof(FullName), typeof(string), typeof(AttachedProperties), new PropertyMetadata(null));

        public static Guid? GetFullNameId(DependencyObject obj)
        {
            if (obj is null)
                return null;
            return (Guid?)obj.GetValue(FullNameIdProperty);
        }

        private static void SetFullNameId(DependencyObject obj, Guid? value)
        {
            obj.SetValue(FullNameIdPropertyKey, value);
        }

        private static readonly DependencyPropertyKey FullNameIdPropertyKey = DependencyProperty.RegisterAttachedReadOnly("FullNameId", typeof(Guid?), typeof(AttachedProperties), new PropertyMetadata(null));

        public static readonly DependencyProperty FullNameIdProperty = FullNameIdPropertyKey.DependencyProperty;

        public static string FullName(this Local.CrawlConfigItemVM crawlConfig) => GetFullName(crawlConfig);

        public static string FullName(this Local.EditCrawlConfigVM crawlConfig) => GetFullName(crawlConfig);

        private static Task<string> PGetFullNameAsync(FsInfoCat.Local.CrawlConfiguration crawlConfiguration, IWindowsStatusListener statusListener) =>
            PGetFullNameAsync(crawlConfiguration, null, statusListener);

        private static Task<string> PGetFullNameAsync(FsInfoCat.Local.Subdirectory subdirectory, IWindowsStatusListener statusListener) =>
            PGetFullNameAsync(subdirectory, null, statusListener);

        private static async Task<string> PGetFullNameAsync(FsInfoCat.Local.Subdirectory subdirectory, FsInfoCat.Local.LocalDbContext dbContext, IWindowsStatusListener statusListener)
        {
            if (dbContext is null)
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using FsInfoCat.Local.LocalDbContext context = serviceScope.ServiceProvider.GetRequiredService<FsInfoCat.Local.LocalDbContext>();
                return await PGetFullNameAsync(subdirectory, context, statusListener);
            }
            return await subdirectory.GetFullNameAsync(dbContext, statusListener.CancellationToken);
        }

        private static async Task<string> PGetFullNameAsync(FsInfoCat.Local.CrawlConfiguration crawlConfiguration, FsInfoCat.Local.LocalDbContext dbContext, IWindowsStatusListener statusListener)
        {
            if (dbContext is null)
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using FsInfoCat.Local.LocalDbContext context = serviceScope.ServiceProvider.GetRequiredService<FsInfoCat.Local.LocalDbContext>();
                return await PGetFullNameAsync(crawlConfiguration, context, statusListener);
            }
            FsInfoCat.Local.Subdirectory root = crawlConfiguration.Root;
            if (root is null)
            {
                root = await dbContext.Entry(crawlConfiguration).GetRelatedReferenceAsync(c => c.Root, statusListener.CancellationToken);
                if (root is null)
                    return null;
            }
            return await root.GetFullNameAsync(dbContext, statusListener.CancellationToken);
        }

        private static string OnGetFullName(DependencyObject item, Guid rootId, string path, Action<string> onCompleted)
        {
            if (string.IsNullOrEmpty(path))
            {
                item.Dispatcher.Invoke(() =>
                {
                    item.ClearValue(FullNameProperty);
                    item.ClearValue(FullNameIdPropertyKey);
                    onCompleted?.Invoke(path);
                });
                return null;
            }
            item.Dispatcher.Invoke(() =>
            {
                SetFullName(item, path);
                SetFullNameId(item, rootId);
                onCompleted?.Invoke(path);
            });
            return path;
        }

        public static async Task<string> GetFullNameAsync(this Local.CrawlConfigItemVM item, IWindowsStatusListener statusListener, FsInfoCat.Local.LocalDbContext dbContext = null)
        {
            FsInfoCat.Local.CrawlConfiguration model;
            if (item is null || (model = await item.Dispatcher.InvokeAsync(() => item.Model, DispatcherPriority.Background, statusListener.CancellationToken)) is null)
                return null;
            string fullName = await item.Dispatcher.InvokeAsync(() =>
            {
                string n = GetFullName(item);
                if (!string.IsNullOrEmpty(n) && GetFullNameId(item) == model.RootId)
                    return n;
                return null;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            if (!string.IsNullOrEmpty(fullName))
                return fullName;
            fullName = await PGetFullNameAsync(model, dbContext, statusListener);
            if (string.IsNullOrEmpty(fullName))
            {
                item.Dispatcher.Invoke(() =>
                {
                    item.ClearValue(FullNameProperty);
                    item.ClearValue(FullNameIdPropertyKey);
                });
                return null;
            }
            item.Dispatcher.Invoke(() =>
            {
                SetFullName(item, fullName);
                SetFullNameId(item, model.RootId);
            });
            return OnGetFullName(item, model.RootId, fullName, null);
        }

        public static async Task<string> GetFullNameAsync(this Local.EditCrawlConfigVM item, IWindowsStatusListener statusListener, FsInfoCat.Local.LocalDbContext dbContext = null)
        {
            ISimpleIdentityReference<FsInfoCat.Local.Subdirectory> root;
            if (item is null || (root = await item.Dispatcher.InvokeAsync(() => item.Root, DispatcherPriority.Background, statusListener.CancellationToken)) is null)
                return null;
            if (root is not FsInfoCat.Local.Subdirectory model)
            {
                FsInfoCat.Local.CrawlConfiguration source = item.Dispatcher.Invoke(() => item.Model);
                if (dbContext is null)
                {
                    using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                    using FsInfoCat.Local.LocalDbContext context = serviceScope.ServiceProvider.GetRequiredService<FsInfoCat.Local.LocalDbContext>();
                    model = await context.Entry(source).GetRelatedReferenceAsync(c => c.Root, statusListener.CancellationToken);
                }
                else
                    model = await dbContext.Entry(source).GetRelatedReferenceAsync(c => c.Root, statusListener.CancellationToken);
                if (model is null)
                    return null;
            }
            string fullName = await item.Dispatcher.InvokeAsync(() =>
            {
                string n = GetFullName(item);
                if (!string.IsNullOrEmpty(n) && GetFullNameId(item) == model.Id)
                    return n;
                return null;
            }, DispatcherPriority.Background, statusListener.CancellationToken);
            if (!string.IsNullOrEmpty(fullName))
                return fullName;
            return OnGetFullName(item, model.Id, await PGetFullNameAsync(model, dbContext, statusListener), null);
        }

        public static void GetFullName(this Local.CrawlConfigItemVM item, AsyncOps.AsyncBgModalVM bgOpManager, Action<string> onCompleted)
        {
            FsInfoCat.Local.CrawlConfiguration model = item?.Model;
            if (model is null)
                onCompleted(null);
            else
            {
                string fullName = GetFullName(item);
                if (!string.IsNullOrEmpty(fullName) && GetFullNameId(item) == model.RootId)
                    onCompleted(fullName);
                else
                    bgOpManager.FromAsync("", "", model, PGetFullNameAsync).ContinueWith(task => OnGetFullName(item, model.RootId, task.Result, onCompleted));
            }
        }

        public static void GetFullName(this Local.EditCrawlConfigVM item, AsyncOps.AsyncBgModalVM bgOpManager, Action<string> onCompleted)
        {
            if (item is null)
                onCompleted(null);
            else
            {
                FsInfoCat.Local.Subdirectory root = item.Root as FsInfoCat.Local.Subdirectory;
                string fullName = GetFullName(item);
                if (root is null)
                    onCompleted(null);
                else if (!string.IsNullOrEmpty(fullName) && root.Id == GetFullNameId(item))
                    onCompleted(fullName);
                else
                    bgOpManager.FromAsync("", "", root, PGetFullNameAsync).ContinueWith(task => OnGetFullName(item, root.Id, task.Result, onCompleted));
            }
        }

        public static bool TrySyncFullName(this Local.CrawlConfigItemVM source, Local.EditCrawlConfigVM target)
        {
            string fullName = source.FullName();
            if (!string.IsNullOrEmpty(fullName) && source.Model?.RootId == target.Root?.Id)
            {
                SetFullName(target, fullName);
                SetFullNameId(target, GetFullNameId(source));
                return true;
            }
            return false;
        }

        public static bool TrySyncFullName(this Local.EditCrawlConfigVM source, Local.CrawlConfigItemVM target)
        {
            string fullName = source.FullName();
            if (!string.IsNullOrEmpty(fullName) && source.Root?.Id == target.Model?.RootId)
            {
                SetFullName(target, fullName);
                SetFullNameId(target, GetFullNameId(source));
                return true;
            }
            return false;
        }
    }
}
