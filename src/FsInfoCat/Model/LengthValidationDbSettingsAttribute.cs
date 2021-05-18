using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class LengthValidationDbSettingsAttribute : DbSettingsAttribute
    {
        /// <summary>
        /// Gets name of the application setting within <see cref="DBSettings"/> to reference as the minimum valid length.
        /// </summary>
        /// <value>
        /// The name of the <see cref="Int32"/> application setting from <see cref="DBSettings"/> to use for the inclusive minimum length or <see langword="null"/>
        /// if there is no minimum length.
        /// </value>
        public string MinLengthSettingsName { get; }

        /// <summary>
        /// Gets name of the application setting within <see cref="DBSettings"/> to reference as the minimum valid length.
        /// </summary>
        /// <value>
        /// The name of the <see cref="Int32"/> application setting from <see cref="DBSettings"/> to use for the inclusive maximum length or <see langword="null"/>
        /// if there is no minimum maximum.
        /// </value>
        public string MaxLengthSettingsName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LengthValidationDbSettingsAttribute"/> trims the value before assessing length.
        /// </summary>
        /// <value>
        /// If <see langword="true"/>, leading and trailing whitespace is trimmed from strings, or for other <see cref="IEnumerable"/> types,
        /// leading and trailing <see langword="null"/> elements are not counted; otherwise, <see langword="false"/> to use the actual countl
        /// </value>
        public bool Trim { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="null"/> values as though they are zero-length.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to treat <see langword="null"/> values as having zero-length; otherwise, <see langword="false"/> to consider <see langword="null"/>
        /// values to be valid from the aspect of this validation attribute.
        /// </value>
        public bool TreatNullAsZeroLength { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthValidationDbSettingsAttribute"/> class for maximum length validation.
        /// </summary>
        /// <param name="settingsName">The name of an <see cref="Int32"/> application setting from <see cref="DBSettings"/> that contains the inclusive maximum
        /// valid length value.</param>
        public LengthValidationDbSettingsAttribute(string settingsName) : this(settingsName, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthValidationDbSettingsAttribute"/> class for miminum or maximum length validation.
        /// </summary>
        /// <param name="settingsName">The name of an <see cref="Int32"/> application setting from <see cref="DBSettings"/>.</param>
        /// <param name="isMinLength">If set to <see langword="true"/>, the minimum length is validated according to the <see cref="DBSettings"/> value;
        /// otherwise, <see langword="false"/> to validate the maximum length.</param>
        public LengthValidationDbSettingsAttribute(string settingsName, bool isMinLength)
        {
            if (isMinLength)
                MinLengthSettingsName = string.IsNullOrWhiteSpace(settingsName) ? "" : settingsName;
            else
                MaxLengthSettingsName = string.IsNullOrWhiteSpace(settingsName) ? "" : settingsName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthValidationDbSettingsAttribute"/> class for minimum and maximum length validation.
        /// </summary>
        /// <param name="minLengthSettingsName">The name of an <see cref="Int32"/> application setting from <see cref="DBSettings"/> that contains the inclusive minimum
        /// valid length value.</param>
        /// <param name="maxLengthSettingsName">The name of an <see cref="Int32"/> application setting from <see cref="DBSettings"/> that contains the inclusive maximum
        /// valid length value.</param>
        public LengthValidationDbSettingsAttribute(string minLengthSettingsName, string maxLengthSettingsName)
        {
            MinLengthSettingsName = string.IsNullOrWhiteSpace(minLengthSettingsName) ? "" : minLengthSettingsName;
            MaxLengthSettingsName = string.IsNullOrWhiteSpace(maxLengthSettingsName) ? "" : maxLengthSettingsName;
        }

        public int GetMinLength()
        {
            if (string.IsNullOrWhiteSpace(MinLengthSettingsName))
                return -1;
            return GetSettingsValue<int>(MinLengthSettingsName);
        }

        public int GetMaxLength()
        {
            if (string.IsNullOrWhiteSpace(MaxLengthSettingsName))
                return -1;
            return GetSettingsValue<int>(MaxLengthSettingsName);
        }

        public override bool IsValid(object value)
        {
            int minLength, maxLength;
            try { minLength = GetMinLength(); }
            catch (Exception exc)
            {
                throw new ValidationException($"Error getting minimum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", exc);
            }
            try { maxLength = GetMaxLength(); }
            catch (Exception exc)
            {
                throw new ValidationException($"Error getting maximum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", exc);
            }
            if (value is null)
                return !(TreatNullAsZeroLength && minLength > 0);
            int length;
            if (value is string text)
                length = (Trim ? text.Trim() : text).Length;
            else if (value is IEnumerable enumerable)
            {
                if (Trim)
                    length = enumerable.Cast<object>().SkipWhile(o => o is null).Reverse().SkipWhile(o => o is null).Count();
                else if (enumerable is ICollection collection)
                    length = collection.Count;
                else
                    try
                    {
                        IEnumerable<Type> interfaces = enumerable.GetType().GetInterfaces().Where(i => i.IsGenericType);
                        Type t = typeof(ICollection<>);
                        PropertyInfo property;
                        if ((t = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition().Equals(t))) is null)
                        {
                            t = typeof(IDictionary<,>);
                            if ((t = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition().Equals(t))) is null)
                                property = null;
                            else
                                property = t.GetProperty(nameof(IDictionary<object, object>.Count));
                        }
                        else
                            property = t.GetProperty(nameof(ICollection<object>.Count));
                        if (property is null)
                            length = enumerable.Cast<object>().Count();
                        else
                            length = (int)property.GetValue(enumerable);
                    }
                    catch { length = enumerable.Cast<object>().Count(); }
            }
            else
                throw new ValidationException($"Input value {value.GetType().FullName} is not an enumerable type and cannot be validated for length.", this, value);
            if (maxLength < 0)
                return minLength < 0 || length >= minLength;
            if (minLength > maxLength)
                throw new ValidationException($"Input value {value.GetType().FullName} cannot be validated by this {nameof(LengthValidationDbSettingsAttribute)}. The value contained by {MinLengthSettingsName} ({minLength}) is greater than the value contained by {MaxLengthSettingsName} ({maxLength}).", this, value);

            return length <= maxLength && (minLength < 0 || length >= minLength);
        }

        private static IEnumerable<string> GetMemberNames(ValidationContext validationContext)
        {
            if (!(validationContext is null || string.IsNullOrWhiteSpace(validationContext.MemberName)))
                yield return validationContext.MemberName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int minLength, maxLength;
            try { minLength = GetMinLength(); }
            catch (Exception exc)
            {
                return new ValidationResult($"Error getting minimum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", GetMemberNames(validationContext));
            }
            try { maxLength = GetMaxLength(); }
            catch (Exception exc)
            {
                return new ValidationResult($"Error getting maximum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", GetMemberNames(validationContext));
            }
            if (value is null)
            {
                if (TreatNullAsZeroLength && minLength > 0)
                    return new ValidationResult(ErrorMessageString, GetMemberNames(validationContext));
                return ValidationResult.Success;
            }
            int length;
            if (value is string text)
                length = (Trim ? text.Trim() : text).Length;
            else if (value is IEnumerable enumerable)
            {
                if (Trim)
                    length = enumerable.Cast<object>().SkipWhile(o => o is null).Reverse().SkipWhile(o => o is null).Count();
                else if (enumerable is ICollection collection)
                    length = collection.Count;
                else
                    try
                    {
                        IEnumerable<Type> interfaces = enumerable.GetType().GetInterfaces().Where(i => i.IsGenericType);
                        Type t = typeof(ICollection<>);
                        PropertyInfo property;
                        if ((t = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition().Equals(t))) is null)
                        {
                            t = typeof(IDictionary<,>);
                            if ((t = interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition().Equals(t))) is null)
                                property = null;
                            else
                                property = t.GetProperty(nameof(IDictionary<object, object>.Count));
                        }
                        else
                            property = t.GetProperty(nameof(ICollection<object>.Count));
                        if (property is null)
                            length = enumerable.Cast<object>().Count();
                        else
                            length = (int)property.GetValue(enumerable);
                    }
                    catch { length = enumerable.Cast<object>().Count(); }
            }
            else
                return new ValidationResult($"Input value {value.GetType().FullName} is not an enumerable type and cannot be validated for length.",
                    GetMemberNames(validationContext));
            if (maxLength < 0)
            {
                if (minLength < 0 || length >= minLength)
                    return ValidationResult.Success;
            }
            else
            {
                if (minLength > maxLength)
                    return new ValidationResult($"Input value {value.GetType().FullName} cannot be validated by this {nameof(LengthValidationDbSettingsAttribute)}. The value contained by {MinLengthSettingsName} ({minLength}) is greater than the value contained by {MaxLengthSettingsName} ({maxLength}).",
                        GetMemberNames(validationContext));

                if (length <= maxLength && (minLength < 0 || length >= minLength))
                    return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessageString, GetMemberNames(validationContext));
        }
    }
}
