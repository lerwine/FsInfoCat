//using FsInfoCat.Desktop.Model.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public static class ComponentExtensions
    {
        /*
        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> ConnectionString(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(DbConnectionStringBuilder.ConnectionString)];

        public static PropertyValidationContext<OleDbConnectionStringBuilder, string> ConnectionString(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext<OleDbConnectionStringBuilder, string>)context[nameof(DbConnectionStringBuilder.ConnectionString)].Value;

        public static PropertyValidationContext<OdbcConnectionStringBuilder, string> ConnectionString(this ModelValidationContext<OdbcConnectionStringBuilder> context) =>
            (PropertyValidationContext<OdbcConnectionStringBuilder, string>)context[nameof(DbConnectionStringBuilder.ConnectionString)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> ApplicationIntent(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.ApplicationIntent)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> ApplicationName(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.ApplicationName)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> AsynchronousProcessing(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.AsynchronousProcessing)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> AttachDBFilename(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.AttachDBFilename)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, SqlAuthenticationMethod> Authentication(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext < SqlConnectionStringBuilder, SqlAuthenticationMethod>)context[nameof(SqlConnectionStringBuilder.Authentication)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, SqlConnectionColumnEncryptionSetting> ColumnEncryptionSetting(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, SqlConnectionColumnEncryptionSetting>)context[nameof(SqlConnectionStringBuilder.ColumnEncryptionSetting)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> ConnectRetryCount(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.ConnectRetryCount)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> ConnectRetryInterval(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.ConnectRetryInterval)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> ConnectTimeout(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.ConnectTimeout)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> ContextConnection(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.ContextConnection)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> CurrentLanguage(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.CurrentLanguage)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> DataSource(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.DataSource)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> EnclaveAttestationUrl(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.EnclaveAttestationUrl)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> Encrypt(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.Encrypt)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> Enlist(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.Enlist)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> FailoverPartner(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.FailoverPartner)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> InitialCatalog(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.InitialCatalog)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> IntegratedSecurity(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.IntegratedSecurity)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> LoadBalanceTimeout(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.LoadBalanceTimeout)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> MinPoolSize(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.MinPoolSize)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> MaxPoolSize(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.MaxPoolSize)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> MultipleActiveResultSets(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.MultipleActiveResultSets)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> MultiSubnetFailover(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.MultiSubnetFailover)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> NetworkLibrary(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.NetworkLibrary)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, int> PacketSize(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, int>)context[nameof(SqlConnectionStringBuilder.PacketSize)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> Password(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.Password)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> PersistSecurityInfo(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.PersistSecurityInfo)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, PoolBlockingPeriod> PoolBlockingPeriod(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, PoolBlockingPeriod>)context[nameof(SqlConnectionStringBuilder.PoolBlockingPeriod)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> Pooling(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.Pooling)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> Replication(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.Replication)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> TransactionBinding(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.TransactionBinding)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> TransparentNetworkIPResolution(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.TransparentNetworkIPResolution)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> TrustServerCertificate(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.TrustServerCertificate)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> TypeSystemVersion(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.TypeSystemVersion)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> UserID(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.UserID)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, bool> UserInstance(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, bool>)context[nameof(SqlConnectionStringBuilder.UserInstance)].Value;

        public static PropertyValidationContext<SqlConnectionStringBuilder, string> WorkstationID(this ModelValidationContext<SqlConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlConnectionStringBuilder, string>)context[nameof(SqlConnectionStringBuilder.WorkstationID)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> AutoshrinkThreshold(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.AutoshrinkThreshold)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, bool> CaseSensitive(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, bool>)context[nameof(SqlCeConnectionStringBuilder.CaseSensitive)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> DataSource(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(SqlCeConnectionStringBuilder.DataSource)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> DefaultLockEscalation(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.DefaultLockEscalation)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> DefaultLockTimeout(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.DefaultLockTimeout)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, bool> Encrypt(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, bool>)context[nameof(SqlCeConnectionStringBuilder.Encrypt)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> EncryptionMode(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(SqlCeConnectionStringBuilder.EncryptionMode)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, bool> Enlist(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, bool>)context[nameof(SqlCeConnectionStringBuilder.Enlist)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> FileAccessRetryTimeout(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.FileAccessRetryTimeout)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> FileMode(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(SqlCeConnectionStringBuilder.FileMode)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> FlushInterval(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.FlushInterval)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> MaxBufferSize(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.MaxBufferSize)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> InitialLcid(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.InitialLcid)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> MaxDatabaseSize(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.MaxDatabaseSize)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> Password(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(SqlCeConnectionStringBuilder.Password)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, bool> PersistSecurityInfo(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, bool>)context[nameof(SqlCeConnectionStringBuilder.PersistSecurityInfo)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, int> TempFileMaxSize(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, int>)context[nameof(SqlCeConnectionStringBuilder.TempFileMaxSize)].Value;

        public static PropertyValidationContext<SqlCeConnectionStringBuilder, string> TempFilePath(this ModelValidationContext<SqlCeConnectionStringBuilder> context) =>
            (PropertyValidationContext<SqlCeConnectionStringBuilder, string>)context[nameof(SqlCeConnectionStringBuilder.TempFilePath)].Value;

        public static PropertyValidationContext<OleDbConnectionStringBuilder, string> DataSource(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext<OleDbConnectionStringBuilder, string>)context[nameof(OleDbConnectionStringBuilder.DataSource)].Value;

        public static PropertyValidationContext<OleDbConnectionStringBuilder, string> FileName(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext<OleDbConnectionStringBuilder, string>)context[nameof(OleDbConnectionStringBuilder.FileName)].Value;

        public static PropertyValidationContext<OleDbConnectionStringBuilder, int> OleDbServices(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext < OleDbConnectionStringBuilder, int>)context[nameof(OleDbConnectionStringBuilder.OleDbServices)].Value;

        public static PropertyValidationContext<OleDbConnectionStringBuilder, bool> PersistSecurityInfo(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext < OleDbConnectionStringBuilder, bool>)context[nameof(OleDbConnectionStringBuilder.PersistSecurityInfo)].Value;

        public static PropertyValidationContext<OleDbConnectionStringBuilder, string> Provider(this ModelValidationContext<OleDbConnectionStringBuilder> context) =>
            (PropertyValidationContext<OleDbConnectionStringBuilder, string>)context[nameof(OleDbConnectionStringBuilder.Provider)].Value;

        public static PropertyValidationContext<OdbcConnectionStringBuilder, string> Driver(this ModelValidationContext<OdbcConnectionStringBuilder> context) =>
            (PropertyValidationContext<OdbcConnectionStringBuilder, string>)context[nameof(OdbcConnectionStringBuilder.Driver)].Value;

        public static PropertyValidationContext<OdbcConnectionStringBuilder, string> Dsn(this ModelValidationContext<OdbcConnectionStringBuilder> context) =>
            (PropertyValidationContext<OdbcConnectionStringBuilder, string>)context[nameof(OdbcConnectionStringBuilder.Dsn)].Value;

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

        */

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
