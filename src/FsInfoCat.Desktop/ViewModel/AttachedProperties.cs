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
    }
}
