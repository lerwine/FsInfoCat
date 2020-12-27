using System;
using System.Windows;
using FsInfoCat.Models;

namespace FsInfoCat.Desktop.ViewModels
{
    public static class ViewModelHelper
    {
        public static object CoerceAsString(DependencyObject d, object baseValue) => ModelHelper.CoerceAsString(baseValue);
        public static object CoerceAsTrimmedString(DependencyObject d, object baseValue) => ModelHelper.CoerceAsTrimmedString(baseValue);
        public static object CoerceAsWsNormalizedString(DependencyObject d, object baseValue) => ModelHelper.CoerceAsWsNormalizedString(baseValue);
        public static object CoerceAsGuid(DependencyObject d, object baseValue) => ModelHelper.CoerceAsGuid(baseValue);
        public static object CoerceAsBoolean(DependencyObject d, object baseValue) => ModelHelper.CoerceAsBoolean(baseValue);
    }
}
