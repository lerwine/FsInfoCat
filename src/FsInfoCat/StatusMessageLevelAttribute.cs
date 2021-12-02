using System;
using System.Reflection;

namespace FsInfoCat
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class StatusMessageLevelAttribute : Attribute
    {
        public StatusMessageLevelAttribute(StatusMessageLevel code) => Level = code;

        public StatusMessageLevel Level { get; }

        public static bool TryGetLevel<TEnum>(TEnum value, out StatusMessageLevel result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                StatusMessageLevelAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<StatusMessageLevelAttribute>();
                if (attribute is not null)
                {
                    result = attribute.Level;
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}

