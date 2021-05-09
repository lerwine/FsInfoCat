using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public static class ComponentExtensions
    {
        public static string GetInitialCatalogDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.InitialCatalog)].DisplayName;

        public static string GetInitialCatalogDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.InitialCatalog)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetPasswordDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.Password)].DisplayName;

        public static string GetPasswordDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.Password)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetDataSourceDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.DataSource)].DisplayName;

        public static string GetDataSourceDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.DataSource)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetUserIDDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.UserID)].DisplayName;

        public static string GetUserIDDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.UserID)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetAuthenticationDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.Authentication)].DisplayName;

        public static string GetAuthenticationDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, SqlAuthenticationMethod value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.Authentication)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetIntegratedSecurityDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.IntegratedSecurity)].DisplayName;

        public static string GetIntegratedSecurityDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.IntegratedSecurity)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static string GetAttachDBFilenameDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor) =>
            descriptor[nameof(SqlConnectionStringBuilder.AttachDBFilename)].DisplayName;

        public static string GetAttachDBFilenameDisplay(this ModelDescriptor<SqlConnectionStringBuilder> descriptor, string value)
        {
            IModelPropertyDescriptor<SqlConnectionStringBuilder> pd = descriptor[nameof(SqlConnectionStringBuilder.AttachDBFilename)];
            return $"{pd.DisplayName}={pd.ConvertToString(value)}";
        }

        public static int? GetMaxLength(this IEnumerable<ValidationAttribute> source, out string errorMessage)
        {
            MaxLengthAttribute attribute = source?.OfType<MaxLengthAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                errorMessage = null;
                return null;
            }
            errorMessage = attribute.ErrorMessage;
            return attribute.Length;
        }

        public static int? GetMinLength(this IEnumerable<ValidationAttribute> source, out string errorMessage)
        {
            MinLengthAttribute attribute = source?.OfType<MinLengthAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                errorMessage = null;
                return null;
            }
            errorMessage = attribute.ErrorMessage;
            return attribute.Length;
        }

        public static bool GetRange(this IEnumerable<ValidationAttribute> source, out int minimum, out int maximum, out string errorMessage)
        {
            RangeAttribute attribute = source?.OfType<RangeAttribute>().FirstOrDefault();
            if (!(attribute is null) && attribute.Minimum is int min && attribute.Maximum is int max)
            {
                minimum = min;
                maximum = max;
                errorMessage = attribute.ErrorMessage;
                return true;
            }
            maximum = minimum = default;
            errorMessage = null;
            return false;
        }

        public static bool GetRange(this IEnumerable<ValidationAttribute> source, out double minimum, out double maximum, out string errorMessage)
        {
            RangeAttribute attribute = source?.OfType<RangeAttribute>().FirstOrDefault();
            if (!(attribute is null) && attribute.Minimum is double min && attribute.Maximum is double max)
            {
                minimum = min;
                maximum = max;
                errorMessage = attribute.ErrorMessage;
                return true;
            }
            maximum = minimum = default;
            errorMessage = null;
            return false;
        }

        public static bool GetRange<T>(this IEnumerable<ValidationAttribute> source, out string minimum, out string maximum, out string errorMessage)
        {
            RangeAttribute attribute = source?.OfType<RangeAttribute>().FirstOrDefault();
            if (!(attribute is null) && attribute.OperandType.Equals(typeof(T)) && attribute.Minimum is string min && attribute.Maximum is string max)
            {
                minimum = min;
                maximum = max;
                errorMessage = attribute.ErrorMessage;
                return true;
            }
            maximum = minimum = errorMessage = null;
            return false;
        }

        public static bool GetStringLength(this IEnumerable<ValidationAttribute> source, out int maximumLength, out int minimumLength, out string errorMessage)
        {
            StringLengthAttribute attribute = source?.OfType<StringLengthAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                maximumLength = minimumLength = default;
                errorMessage = null;
                return false;
            }
            maximumLength = attribute.MaximumLength;
            minimumLength = attribute.MinimumLength;
            errorMessage = attribute.ErrorMessage;
            return true;
        }

        public static bool GetRegularExpression(this IEnumerable<ValidationAttribute> source, out string pattern, out int matchTimeoutInMilliseconds, out string errorMessage)
        {
            RegularExpressionAttribute attribute = source?.OfType<RegularExpressionAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                matchTimeoutInMilliseconds = default;
                pattern = errorMessage = null;
                return false;
            }
            matchTimeoutInMilliseconds = attribute.MatchTimeoutInMilliseconds;
            pattern = attribute.Pattern;
            errorMessage = attribute.ErrorMessage;
            return true;
        }

        public static bool GetCompare(this IEnumerable<ValidationAttribute> source, out string otherProperty, out string errorMessage)
        {
            CompareAttribute attribute = source?.OfType<CompareAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                otherProperty = errorMessage = null;
                return false;
            }
            otherProperty = attribute.OtherProperty;
            errorMessage = attribute.ErrorMessage;
            return true;
        }

        public static bool IsRequired(this IEnumerable<ValidationAttribute> source) => !(source is null) && source.OfType<RequiredAttribute>().Any();

        public static bool IsRequired(this IEnumerable<ValidationAttribute> source, out bool allowEmptyStrings, out string errorMessage)
        {
            RequiredAttribute attribute = source?.OfType<RequiredAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                allowEmptyStrings = false;
                errorMessage = null;
                return false;
            }
            allowEmptyStrings = attribute.AllowEmptyStrings;
            errorMessage = attribute.ErrorMessage;
            return true;
        }

    }
}
