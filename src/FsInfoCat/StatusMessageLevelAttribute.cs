using System;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates the associated <see cref="Model.StatusMessageLevel" />, typically for a <see cref="Model.MessageCode" /> enumerated field.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class StatusMessageLevelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMessageLevelAttribute"/> class.
        /// </summary>
        /// <param name="code">The status message severity level.</param>
        public StatusMessageLevelAttribute(Model.StatusMessageLevel code) => Level = code;

        /// <summary>
        /// Gets the status message severity level.
        /// </summary>
        /// <value>The <see cref="Model.StatusMessageLevel"/> which indicates the severity of the status message associated with the target field.</value>
        public Model.StatusMessageLevel Level { get; }

        /// <summary>
        /// Gets the associated <see cref="Model.StatusMessageLevel"/> value for an <see cref="Enum">enumerated</see> value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the <see cref="Enum">enumerated</see> value.</typeparam>
        /// <param name="value">The <see cref="Enum">enumerated</see> value.</param>
        /// <param name="result">The <see cref="Model.StatusMessageLevel"/> that is associated with the specified <see cref="Enum">enumerated</see> value.</param>
        /// <returns><see langword="true"/> if a <c>StatusMessageLevelAttribute</c> was found on the field for the provided <paramref name="value"/>;
        /// otherwise, <see langword="false"/> to indicate that <paramref name="result"/> contains a default calculated value.</returns>
        /// <remarks>If the field for the specified <paramref name="value"/> does not have a <c>StatusMessageLevelAttribute</c>, then <paramref name="result"/>
        /// will be set to <see cref="Model.StatusMessageLevel.Information"/> if the field for the specfied <paramref name="value"/>
        /// does not also have an <see cref="ErrorCodeAttribute"/> or <see cref="Model.StatusMessageLevel.Error"/> if it does.</remarks>
        public static bool TryGetLevel<TEnum>(TEnum value, out Model.StatusMessageLevel result)
            where TEnum : struct, Enum
        {
            string name = Enum.GetName(value);
            if (name is not null)
            {
                FieldInfo fieldInfo = typeof(TEnum).GetField(name);
                if (fieldInfo is not null)
                {
                    StatusMessageLevelAttribute attribute = fieldInfo.GetCustomAttribute<StatusMessageLevelAttribute>();
                    if (attribute is not null)
                    {
                        result = attribute.Level;
                        return true;
                    }
                    if (fieldInfo.GetCustomAttribute<ErrorCodeAttribute>() is not null)
                    {
                        result = Model.StatusMessageLevel.Error;
                        return false;
                    }
                }
            }
            result = Model.StatusMessageLevel.Information;
            return false;
        }
    }
}

