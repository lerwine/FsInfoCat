using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public static class CommonAttached
    {
        #region DisplayText Attached Property Members

        public const string PropertyName_DisplayText = "DisplayText";

        public static string GetDisplayText([DisallowNull] DependencyObject obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            return (string)obj.GetValue(DisplayTextProperty);
        }

        internal static void SetDisplayText([DisallowNull] DependencyObject obj, string value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            obj.SetValue(DisplayTextProperty, value);
        }

        internal static readonly DependencyPropertyKey DisplayTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly(PropertyName_DisplayText, typeof(string),
            typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayTextProperty = DisplayTextPropertyKey.DependencyProperty;

        #endregion

        #region ListItemTitle Attached Property Members

        public const string PropertyName_ListItemTitle = "ListItemTitle";

        public static string GetListItemTitle([DisallowNull] DependencyObject obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            return (string)obj.GetValue(ListItemTitleProperty);
        }

        internal static void SetListItemTitle([DisallowNull] DependencyObject obj, string value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            obj.SetValue(ListItemTitleProperty, value);
        }

        internal static readonly DependencyPropertyKey ListItemTitlePropertyKey = DependencyProperty.RegisterAttachedReadOnly(PropertyName_ListItemTitle, typeof(string),
            typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty ListItemTitleProperty = ListItemTitlePropertyKey.DependencyProperty;

        #endregion

        #region BooleanOptional Attached Property Members

        public const string PropertyName_BooleanOptional = "BooleanOptional";

        public static bool? GetBooleanOptional([DisallowNull] DependencyObject obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            return (bool?)obj.GetValue(BooleanOptionalProperty);
        }

        internal static void SetBooleanOptional([DisallowNull] DependencyObject obj, bool? value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            obj.SetValue(BooleanOptionalProperty, value);
        }

        internal static readonly DependencyPropertyKey BooleanOptionalPropertyKey = DependencyProperty.RegisterAttachedReadOnly(PropertyName_BooleanOptional, typeof(bool?),
            typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty BooleanOptionalProperty = BooleanOptionalPropertyKey.DependencyProperty;

        #endregion
    }
}
