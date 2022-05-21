using System;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates the associated <see cref="Model.ErrorCode" />, typically for a <see cref="Model.MessageCode" /> enumerated field.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ErrorCodeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCodeAttribute"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        public ErrorCodeAttribute(Model.ErrorCode code) => Code = code;

        /// <summary>
        /// Gets the error code value.
        /// </summary>
        /// <value>The <see cref="Model.ErrorCode"/> value to be associated with the target field.</value>
        public Model.ErrorCode Code { get; }

        /// <summary>
        /// Gets the associated <see cref="Model.ErrorCode"/> value for an <see cref="Enum">enumerated</see> value if it is specified through a <c>ErrorCodeAttribute</c>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the <see cref="Enum">enumerated</see> value.</typeparam>
        /// <param name="value">The <see cref="Enum">enumerated</see> value.</param>
        /// <param name="result">The value of the <see cref="Code"/> if the <c>ErrorCodeAttribute</c> has been applied to the enumerated field; otherwise the default <see cref="Model.ErrorCode"/> value.</param>
        /// <returns><see langword="true"/> if an <c>ErrorCodeAttribute</c> was applied to the field of the provided <paramref name="value"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetCode<TEnum>(TEnum value, out Model.ErrorCode result)
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

