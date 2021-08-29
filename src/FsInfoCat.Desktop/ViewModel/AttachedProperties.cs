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
    }
}
