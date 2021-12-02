using System;
using System.Reflection;

namespace FsInfoCat
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessageCodeAttribute : Attribute
    {
        public MessageCodeAttribute(MessageCode code) => Code = code;

        public MessageCode Code { get; }

        public static bool TryGetCode<TEnum>(TEnum value, out MessageCode result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                MessageCodeAttribute attribute = typeof(TEnum).GetField(name)?.GetCustomAttribute<MessageCodeAttribute>();
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

