using System;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates the associated <see cref="Model.MessageCode" />, typically for a <see cref="Model.ErrorCode" /> enumerated field.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MessageCodeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCodeAttribute"/> class.
        /// </summary>
        /// <param name="code">The message code.</param>
        public MessageCodeAttribute(Model.MessageCode code) => Code = code;

        /// <summary>
        /// Gets the message code value.
        /// </summary>
        /// <value>The <see cref="Model.MessageCode"/> value to be associated with the target field.</value>
        public Model.MessageCode Code { get; }

        /// <summary>
        /// Gets the associated <see cref="Model.MessageCode"/> value for an <see cref="Enum">enumerated</see> value if it is specified through a <c>MessageCodeAttribute</c>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the <see cref="Enum">enumerated</see> value.</typeparam>
        /// <param name="value">The <see cref="Enum">enumerated</see> value.</param>
        /// <param name="result">The value of the <see cref="Code"/> if the <c>MessageCodeAttribute</c> has been applied to the enumerated field; otherwise the
        /// default <see cref="Model.MessageCode"/> value.</param>
        /// <returns><see langword="true"/> if a <c>MessageCodeAttribute</c> was applied to the field of the provided <paramref name="value"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool TryGetCode<TEnum>(TEnum value, out Model.MessageCode result)
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

