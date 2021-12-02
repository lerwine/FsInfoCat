using System;
using System.Reflection;

namespace FsInfoCat
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ErrorCodeAttribute : Attribute
    {
        public ErrorCodeAttribute(ErrorCode code) => Code = code;

        public ErrorCode Code { get; }

        public static bool TryGetCode<TEnum>(TEnum value, out ErrorCode result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                ErrorCodeAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<ErrorCodeAttribute>();
                if (attribute is not null)
                {
                    result = attribute.Code;
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}

