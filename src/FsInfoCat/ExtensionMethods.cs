using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    public static class ExtensionMethods
    {
        public static readonly Regex BackslashEscapablePattern = new(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapableLBPattern = new(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?",
            RegexOptions.Compiled);

        class EnteredMethod : IDisposable
        {
            private bool disposedValue;
            private readonly string _format;
            private readonly object[] _args;
            private readonly ILogger _logger;

            internal EnteredMethod([DisallowNull] ILogger logger, string argsFormat, params object[] args)
            {
                (_logger = logger).LogDebug($"Enter #{{HashCode}}.{{Method}}({argsFormat})", args);
                _format = $"Exit #{{HashCode}}.{{Method}}({argsFormat})";
                _args = args;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                        _logger.LogDebug(_format, _args);

                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>([DisallowNull] this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}, {Arg3}, {Arg4}, {Arg5}, {Arg6}, {Arg7}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>([DisallowNull] this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}, {Arg3}, {Arg4}, {Arg5}, {Arg6}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2, arg3, arg4, arg5, arg6);

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5>([DisallowNull] this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}, {Arg3}, {Arg4}, {Arg5}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2, arg3, arg4, arg5);

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2, TArg3, TArg4>([DisallowNull] this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}, {Arg3}, {Arg4}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2, arg3, arg4);

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2, TArg3>([DisallowNull] this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, TArg3 arg3, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}, {Arg3}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2, arg3);

        public static IDisposable EnterMethod<TTarget, TArg1, TArg2>(this ILogger<TTarget> logger, TArg1 arg1, TArg2 arg2, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg1}, {Arg2}", RuntimeHelpers.GetHashCode(target), methodName, arg1, arg2);

        public static IDisposable EnterMethod<TTarget, TArg>([DisallowNull] this ILogger<TTarget> logger, TArg arg, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "{Arg}", RuntimeHelpers.GetHashCode(target), methodName, arg);

        public static IDisposable EnterMethod<TTarget>([DisallowNull] this ILogger<TTarget> logger, [DisallowNull] TTarget target, [CallerMemberName] string methodName = null) =>
            new EnteredMethod(logger, "", RuntimeHelpers.GetHashCode(target), methodName);

        public static DateTime CurrentHour(this DateTime dateTime) => (dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0) ? dateTime : new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, 0, dateTime.Kind);

        public static DateTime CurrentMinute(this DateTime dateTime) => (dateTime.Second == 0 && dateTime.Millisecond == 0) ? dateTime : new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0, dateTime.Kind);

        public static DateTime CurrentSecond(this DateTime dateTime) => (dateTime.Millisecond == 0) ? dateTime : new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0, dateTime.Kind);

        public static TimeSpan TruncateHour(this TimeSpan timeSpan) => (timeSpan.Minutes == 0 && timeSpan.Seconds == 0 && timeSpan.Milliseconds == 0) ? timeSpan : new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0, 0);

        public static TimeSpan TruncateMinute(this TimeSpan timeSpan) => (timeSpan.Seconds == 0 && timeSpan.Milliseconds == 0) ? timeSpan : new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0, 0);

        public static TimeSpan TruncateSecond(this TimeSpan timeSpan) => (timeSpan.Milliseconds == 0) ? timeSpan : new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, 0);

        public static long ToSeconds(this TimeSpan timeSpan) => Convert.ToInt64(timeSpan.TruncateSecond().TotalSeconds);

        public static string SplitPath(string path, out string leaf)
        {
            if (string.IsNullOrEmpty(path))
            {
                leaf = "";
                return "";
            }
            string directoryName = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directoryName))
            {
                leaf = "";
                return path;
            }
            leaf = Path.GetFileName(path);
            while (string.IsNullOrEmpty(leaf))
            {
                path = directoryName;
                leaf = Path.GetFileName(path);
                directoryName = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryName))
                {
                    leaf = "";
                    return path;
                }
            }
            return directoryName;
        }

        public static T DefaultIf<T>(T inputValue, [DisallowNull] Func<T, bool> predicate, T defaultValue)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            return predicate(inputValue) ? defaultValue : inputValue;
        }

        public static TResult DefaultIf<TInput, TResult>(TInput inputValue, [DisallowNull] PredicatedProduction<TInput, TResult> producer, TResult defaultValue)
        {
            if (producer is null)
                throw new ArgumentNullException(nameof(producer));
            return producer(inputValue, out TResult result) ? result : defaultValue;
        }

        public static T GetDefaultIf<T>(T inputValue, [DisallowNull] Func<T, bool> predicate, [DisallowNull] Func<T> defaultValueFunc)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            if (defaultValueFunc is null)
                throw new ArgumentNullException(nameof(defaultValueFunc));
            return predicate(inputValue) ? defaultValueFunc() : inputValue;
        }

        public static TResult GetDefaultIf<TInput, TResult>(TInput inputValue, [DisallowNull] PredicatedProduction<TInput, TResult> producer,
            [DisallowNull] Func<TInput, TResult> defaultValueFunc)
        {
            if (producer is null)
                throw new ArgumentNullException(nameof(producer));
            if (defaultValueFunc is null)
                throw new ArgumentNullException(nameof(defaultValueFunc));
            return producer(inputValue, out TResult result) ? result : defaultValueFunc(inputValue);
        }

        public static bool TryGetDescription(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
            if (attribute is not null && (result = attribute.GetDescription().NullIfWhiteSpace()) is not null)
                return true;
            return (result = memberInfo.GetCustomAttributes<DescriptionAttribute>(true).Select(a => a.Description.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static bool TryGetDescription(this MemberDescriptor descriptor, out string result)
        {
            if (descriptor is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (attribute is not null && (result = attribute.GetDescription().NullIfWhiteSpace()) is not null)
                return true;
            return (result = descriptor.Attributes.OfType<DescriptionAttribute>().Select(a => a.Description.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static bool TryGetDisplayName(this MemberDescriptor descriptor, out string result)
        {
            if (descriptor is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (attribute is not null && (result = attribute.GetName().NullIfWhiteSpace()) is not null)
                return true;
            return (result = descriptor.Attributes.OfType<DisplayNameAttribute>().Select(a => a.DisplayName.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static bool TryGetDisplayName(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is null)
            {
                result = null;
                return false;
            }
            DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
            if (attribute is not null && (result = attribute.GetName().NullIfWhiteSpace()) is not null)
                return true;
            return (result = memberInfo.GetCustomAttributes<DisplayNameAttribute>(true).Select(a => a.DisplayName.NullIfWhiteSpace()).FirstOrDefault(d => d is not null)) is not null;
        }

        public static bool TryGetShortName(this MemberDescriptor descriptor, out string result)
        {
            if (descriptor is not null)
            {
                DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (attribute is not null && (result = attribute.GetShortName().NullIfWhiteSpace()) is not null)
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGetShortName(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is not null)
            {
                DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
                if (attribute is not null && (result = attribute.GetShortName().NullIfWhiteSpace()) is not null)
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGetPrompt(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is not null)
            {
                DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
                if (attribute is not null && (result = attribute.GetPrompt().NullIfWhiteSpace()) is not null)
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGetPrompt(this MemberDescriptor descriptor, out string result)
        {
            if (descriptor is not null)
            {
                DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (attribute is not null && (result = attribute.GetPrompt().NullIfWhiteSpace()) is not null)
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGroupName(this MemberInfo memberInfo, out string result)
        {
            if (memberInfo is not null)
            {
                DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
                if (attribute is not null && (result = attribute.GetGroupName().NullIfWhiteSpace()) is not null)
                    return true;
                result = memberInfo.GetCustomAttributes<CategoryAttribute>(true).Select(c => c.Category).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(result))
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGroupName(this MemberDescriptor descriptor, out string result)
        {
            if (descriptor is not null)
            {
                DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (attribute is not null && (result = attribute.GetGroupName().NullIfWhiteSpace()) is not null)
                    return true;
                result = descriptor.Category;
                if (!string.IsNullOrWhiteSpace(result))
                    return true;
            }
            result = null;
            return false;
        }

        public static bool TryGetOrder(this MemberDescriptor descriptor, out int result)
        {
            if (descriptor is not null)
            {
                DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                int? order;
                if (attribute is not null && (order = attribute.GetOrder()).HasValue)
                {
                    result = order.Value;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryGetOrder(this MemberInfo memberInfo, out int result)
        {
            if (memberInfo is not null)
            {
                DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
                int? order;
                if (attribute is not null && (order = attribute.GetOrder()).HasValue)
                {
                    result = order.Value;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static string GetDisplay(this MemberInfo memberInfo, out string shortName, out string prompt, out string groupName, out string description, out int? order)
        {
            if (memberInfo is null)
            {
                shortName = prompt = groupName = description = null;
                order = null;
                return null;
            }
            string displayName;
            DisplayAttribute attribute = memberInfo.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault();
            if (attribute is null)
            {
                shortName = memberInfo.Name;
                groupName = memberInfo.GetCustomAttributes<CategoryAttribute>(true).Select(a => a.Category).Where(n => !string.IsNullOrWhiteSpace(n)).DefaultIfEmpty("").First();
                description = memberInfo.GetCustomAttributes<DescriptionAttribute>(true).Select(a => a.Description).Where(n => !string.IsNullOrWhiteSpace(n)).DefaultIfEmpty("").First();
                order = null;
                displayName = memberInfo.GetCustomAttributes<DisplayNameAttribute>(true).Select(a => a.DisplayName).Where(n => !string.IsNullOrWhiteSpace(n)).DefaultIfEmpty(shortName).First();
                prompt = $"{displayName}: ";
                return displayName;
            }

            order = attribute.GetOrder();
            displayName = attribute.GetName();
            shortName = attribute.GetShortName();
            description = attribute.GetDescription();
            prompt = attribute.GetPrompt();
            groupName = attribute.GetGroupName();
            if (string.IsNullOrWhiteSpace(displayName) || displayName == memberInfo.Name)
            {
                displayName = memberInfo.GetCustomAttributes<DisplayNameAttribute>(true).Select(a => a.DisplayName).Where(n => !string.IsNullOrWhiteSpace(n))
                    .DefaultIfEmpty(string.IsNullOrWhiteSpace(shortName) ? memberInfo.Name : shortName).First();
                if (string.IsNullOrWhiteSpace(shortName))
                    shortName = memberInfo.Name;
            }
            else if (string.IsNullOrWhiteSpace(shortName))
                shortName = displayName;
            if (string.IsNullOrWhiteSpace(prompt))
                prompt = $"{displayName}: ";
            if (string.IsNullOrWhiteSpace(groupName))
                groupName = memberInfo.GetCustomAttributes<CategoryAttribute>(true).Select(a => a.Category).Where(n => !string.IsNullOrWhiteSpace(n)).DefaultIfEmpty("").First();
            if (string.IsNullOrWhiteSpace(description))
                description = memberInfo.GetCustomAttributes<DescriptionAttribute>(true).Select(a => a.Description).Where(n => !string.IsNullOrWhiteSpace(n)).DefaultIfEmpty("").First();
            return displayName;
        }

        public static string GetDisplay(this MemberDescriptor descriptor, out string shortName, out string prompt, out string groupName, out string description, out int? order)
        {
            if (descriptor is null)
            {
                shortName = prompt = groupName = description = null;
                order = null;
                return null;
            }
            string displayName;
            DisplayAttribute attribute = descriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                shortName = descriptor.Name;
                groupName = descriptor.Category ?? "";
                description = descriptor.Description ?? "";
                order = null;
                displayName = descriptor.DisplayName;
                if (string.IsNullOrWhiteSpace(displayName))
                    displayName = shortName;
                prompt = $"{displayName}: ";
                return displayName;
            }

            order = attribute.GetOrder();
            displayName = attribute.GetName();
            shortName = attribute.GetShortName();
            description = attribute.GetDescription();
            prompt = attribute.GetPrompt();
            groupName = attribute.GetGroupName();
            if (string.IsNullOrWhiteSpace(displayName) || displayName == descriptor.Name)
            {
                displayName = string.IsNullOrWhiteSpace(descriptor.DisplayName) ? descriptor.Name : descriptor.DisplayName;
                if (string.IsNullOrWhiteSpace(shortName))
                    shortName = descriptor.Name;
            }
            else if (string.IsNullOrWhiteSpace(shortName))
                shortName = displayName;
            if (string.IsNullOrWhiteSpace(prompt))
                prompt = $"{displayName}: ";
            if (string.IsNullOrWhiteSpace(groupName))
                groupName = descriptor.Category ?? "";
            if (string.IsNullOrWhiteSpace(description))
                description = descriptor.Description ?? "";
            return displayName;
        }

        public static bool TryGetDefaultValue(this MemberDescriptor descriptor, out object defaultValue)
        {
            DefaultValueAttribute attribute = descriptor?.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                defaultValue = null;
                return false;
            }
            defaultValue = attribute.Value;
            return true;
        }

        public static bool TryGetDefaultValue(this MemberInfo memberInfo, out object defaultValue)
        {
            DefaultValueAttribute attribute = memberInfo?.GetCustomAttributes<DefaultValueAttribute>(true).FirstOrDefault();
            if (attribute is null)
            {
                defaultValue = null;
                return false;
            }
            defaultValue = attribute.Value;
            return true;
        }

        public static string GetDisplayName<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                FieldInfo fieldInfo = typeof(TEnum).GetField(name);
                if (fieldInfo.TryGetDisplayName(out string result))
                    return result;
            }
            return Enum.GetName(typeof(TEnum), value);
        }

        public static bool TryGetShortName<TEnum>(this TEnum value, out string result) where TEnum : struct, Enum
        {
            result = Enum.GetName(value);
            return result is not null && typeof(TEnum).GetField(result).TryGetShortName(out result);
        }

        public static bool TryGetDisplayName<TEnum>(this TEnum value, out string result) where TEnum : struct, Enum
        {
            result = Enum.GetName(value);
            return result is not null && typeof(TEnum).GetField(result).TryGetDisplayName(out result);
        }

        public static bool TryGetDescription<TEnum>(this TEnum value, out string result) where TEnum : struct, Enum
        {
            result = Enum.GetName(value);
            return result is not null && typeof(TEnum).GetField(result).TryGetDescription(out result);
        }

        public static bool TryGetAmbientValue<TEnum, TResult>(this TEnum value, out TResult result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                AmbientValueAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<AmbientValueAttribute>();
                if (attribute is not null && attribute.Value is TResult r)
                {
                    result = r;
                    return false;
                }
            }
            result = default;
            return false;
        }

        public static TResult GetAmbientValue<TEnum, TResult>(this TEnum value, TResult defaultValue = default)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                AmbientValueAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<AmbientValueAttribute>();
                if (attribute is not null && attribute.Value is TResult r)
                    return r;
            }
            return defaultValue;
        }

        public static IEnumerable<TEnum> GetFlagValues<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            Type type = typeof(TEnum);
            if (type.GetCustomAttribute<FlagsAttribute>() is null)
                return new[] { value };
#pragma warning disable CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
            return Enum.GetValues<TEnum>().Where(v => value.HasFlag(v));
#pragma warning restore CA2248 // Provide correct 'enum' argument to 'Enum.HasFlag'
        }

        public static ErrorCode ToErrorCode(this AccessErrorCode errorCode) => errorCode.GetAmbientValue(ErrorCode.Unexpected);

        public static AccessErrorCode ToAccessErrorCode(this ErrorCode errorCode) =>
            Enum.GetValues<AccessErrorCode>().Where(e => e.ToErrorCode() == errorCode).DefaultIfEmpty(AccessErrorCode.Unspecified).First();

        public static EventId ToEventId(this AccessErrorCode errorCode) => errorCode.ToErrorCode().ToEventId();

        public static EventId ToEventId(this ErrorCode errorCode) => new((byte)errorCode, errorCode.TryGetDescription(out string name) ? name : errorCode.GetDisplayName());

        public static bool IsNullableType(this Type type) => (type ?? throw new ArgumentNullException(nameof(type))).IsValueType && type.IsGenericType &&
            typeof(Nullable<>).Equals(type.GetGenericTypeDefinition());

        public static bool IsNullAssignable(this Type type) => !(type ?? throw new ArgumentNullException(nameof(type))).IsValueType || type.IsNullableType();

        public static string ToCsTypeName(this Type type, bool omitNamespaces = false)
        {
            if (type is null)
                return "null";
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsPointer)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}*";
            if (type.IsByRef)
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}&";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank < 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[]";
                if (rank == 2)
                    return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[,]";
                return $"{ToCsTypeName(type.GetElementType(), omitNamespaces)}[{new string(',', rank - 1)}]";
            }
            if (type.IsNullableType())
                return $"{ToCsTypeName(Nullable.GetUnderlyingType(type), omitNamespaces)}?";

            if (type.IsValueType)
            {
                if (type.Equals(typeof(void)))
                    return "void";
                if (type.Equals(typeof(char)))
                    return "char";
                if (type.Equals(typeof(bool)))
                    return "bool";
                if (type.Equals(typeof(byte)))
                    return "byte";
                if (type.Equals(typeof(sbyte)))
                    return "sbyte";
                if (type.Equals(typeof(short)))
                    return "short";
                if (type.Equals(typeof(ushort)))
                    return "ushort";
                if (type.Equals(typeof(int)))
                    return "int";
                if (type.Equals(typeof(uint)))
                    return "uint";
                if (type.Equals(typeof(long)))
                    return "long";
                if (type.Equals(typeof(ulong)))
                    return "ulong";
                if (type.Equals(typeof(float)))
                    return "float";
                if (type.Equals(typeof(double)))
                    return "double";
                if (type.Equals(typeof(decimal)))
                    return "decimal";
            }
            else
            {
                if (type.Equals(typeof(string)))
                    return "string";
                if (type.Equals(typeof(object)))
                    return "object";
            }
            string n = type.Name;
            string ns;
            if (type.IsNested)
                ns = ToCsTypeName(type.DeclaringType, omitNamespaces);
            else if (omitNamespaces || (ns = type.Namespace) is null || ns == "System")
                ns = "";

            if (type.IsGenericType)
            {
                int i = n.IndexOf("`");
                if (i > 0)
                    n = n.Substring(0, i);
                if (ns.Length > 0)
                    return $"{ns}.{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
                return $"{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName(omitNamespaces)))}>";
            }
            return (ns.Length > 0) ? $"{ns}.{n}" : n;
        }

        public static string ToPseudoCsText(object obj)
        {
            if (obj is null)
                return "null";
            if (obj is string s)
                return $"\"{EscapeCsString(s)}\"";
            if (obj is char c)
                return c switch
                {
                    '\'' => "'\\''",
                    '"' => "'\"'",
                    _ => $"'{EscapeCsString(new string(new char[] { c }))}'",
                };
            if (obj is bool bv)
                return bv ? "true" : "false";
            if (obj is byte bn)
                return bn.ToString("X2", CultureInfo.InvariantCulture);
            if (obj is sbyte sb)
                return $"(sbyte){sb.ToString("X2", CultureInfo.InvariantCulture)}";
            if (obj is short sv)
                return sv.ToString("X4", CultureInfo.InvariantCulture);
            if (obj is ushort us)
                return $"(ushort){us.ToString("X4", CultureInfo.InvariantCulture)}";
            if (obj is int i)
                return i.ToString("X8", CultureInfo.InvariantCulture);
            if (obj is uint ui)
                return $"{ui.ToString("X8", CultureInfo.InvariantCulture)}U";
            if (obj is long l)
                return l.ToString("X16", CultureInfo.InvariantCulture);
            if (obj is ulong ul)
                return $"{ul.ToString("X16", CultureInfo.InvariantCulture)}UL";
            if (obj is float fv)
                return $"{fv.ToString(CultureInfo.InvariantCulture)}f";
            if (obj is double d)
                return d.ToString(CultureInfo.InvariantCulture);
            if (obj is decimal m)
                return $"{m.ToString(CultureInfo.InvariantCulture)}m";
            if (obj is DateTime dt)
                return dt.ToString(CultureInfo.InvariantCulture);
            if (obj is DBNull)
                return "DBNull";
            if (obj is Type t)
                return t.ToCsTypeName();
            if (obj is IFormattable fm)
                return fm.ToString();
            if (obj is IConvertible cv)
            {
                switch (cv.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return ToPseudoCsText(cv.ToBoolean(CultureInfo.InvariantCulture));
                    case TypeCode.Byte:
                        return ToPseudoCsText(cv.ToByte(CultureInfo.InvariantCulture));
                    case TypeCode.Char:
                        return ToPseudoCsText(cv.ToChar(CultureInfo.InvariantCulture));
                    case TypeCode.DateTime:
                        return ToPseudoCsText(cv.ToDateTime(CultureInfo.InvariantCulture));
                    case TypeCode.DBNull:
                        return "DBNull";
                    case TypeCode.Decimal:
                        return ToPseudoCsText(cv.ToDecimal(CultureInfo.InvariantCulture));
                    case TypeCode.Double:
                        return ToPseudoCsText(cv.ToDouble(CultureInfo.InvariantCulture));
                    case TypeCode.Int16:
                        return ToPseudoCsText(cv.ToInt16(CultureInfo.InvariantCulture));
                    case TypeCode.Int32:
                        return ToPseudoCsText(cv.ToInt32(CultureInfo.InvariantCulture));
                    case TypeCode.Int64:
                        return ToPseudoCsText(cv.ToInt64(CultureInfo.InvariantCulture));
                    case TypeCode.SByte:
                        return ToPseudoCsText(cv.ToSByte(CultureInfo.InvariantCulture));
                    case TypeCode.Single:
                        return ToPseudoCsText(cv.ToSingle(CultureInfo.InvariantCulture));
                    case TypeCode.String:
                        return ToPseudoCsText(cv.ToString(CultureInfo.InvariantCulture));
                    case TypeCode.UInt16:
                        return ToPseudoCsText(cv.ToUInt16(CultureInfo.InvariantCulture));
                    case TypeCode.UInt32:
                        return ToPseudoCsText(cv.ToUInt32(CultureInfo.InvariantCulture));
                    case TypeCode.UInt64:
                        return ToPseudoCsText(cv.ToUInt64(CultureInfo.InvariantCulture));
                }
            }
            return obj.ToString();
        }

        public static string AsIndented(this string text, string indent = "    ")
        {
            if (text is null || string.IsNullOrEmpty(indent))
                return text;
            string[] lines = text.SplitLines();
            if (lines.Length == 1)
                return text;
            return lines.Take(1).Concat(lines.Skip(1).Select(s => (indent + s).TrimEnd())).JoinWithNewLines();
        }

        public static string EscapeCsString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BackslashEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"\\{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        return g.Value switch
                        {
                            "\r" => "\\r\r",
                            "\n" => "\\n\n",
                            _ => "\\r\\n\r\n",
                        };
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "\\0";
                        case '\a':
                            return "\\a";
                        case '\b':
                            return "\\b";
                        case '\f':
                            return "\\f";
                        case '\t':
                            return "\\t";
                        case '\v':
                            return "\\v";
                        default:
                            g = m.Groups["x"];
                            uint i = (uint)c;
                            if (g.Success)
                                return $"\\x{i:x4}{g.Value}";
                            return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                    }
                });
            return BackslashEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"\\{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "\\0";
                    case '\a':
                        return "\\a";
                    case '\b':
                        return "\\b";
                    case '\f':
                        return "\\f";
                    case '\n':
                        return "\\n";
                    case '\r':
                        return "\\r";
                    case '\t':
                        return "\\t";
                    case '\v':
                        return "\\v";
                    default:
                        Group g = m.Groups["x"];
                        uint i = (uint)c;
                        if (g.Success)
                            return $"\\x{i:x4}{g.Value}";
                        return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                }
            });
        }
    }

    public record IndexedValue<T>(int Index, T Value);
}
