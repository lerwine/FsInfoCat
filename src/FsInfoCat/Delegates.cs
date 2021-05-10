using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat
{
    public static class Delegates
    {
        public static bool NotNullOrWhiteSpace(string value) => !string.IsNullOrWhiteSpace(value);

        public static IEnumerable<string> NotNullOrWhiteSpace(this IEnumerable<string> source) =>
            (source is null) ? Array.Empty<string>() : source.Where(NotNullOrWhiteSpace);

        public static bool NotNull<T>(T value) where T : class => !(value is null);

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) where T : class =>
            (source is null) ? Array.Empty<T>() : source.Where(NotNull);

        public static bool NotNull<T>(T? value) where T : struct => value.HasValue;

        public static IEnumerable<T?> NotNull<T>(this IEnumerable<T?> source) where T : struct =>
            (source is null) ? Array.Empty<T?>() : source.Where(NotNull);
    }
}
