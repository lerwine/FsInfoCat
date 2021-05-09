using FsInfoCat.Desktop.Model.ComponentSupport;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class SqlConnectionStringModelDescriptionBuilder : DbConnectionStringModelDescriptorBuilder<SqlConnectionStringBuilder>
    {
        private static readonly ModelDescriptor<SqlConnectionStringBuilder> _descriptor = new SqlConnectionStringModelDescriptionBuilder().Build();

        public static bool ValidateInitialCatalog(string initialCatalog, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder)
            {
                if (string.IsNullOrWhiteSpace(connectionStringBuilder.AttachDBFilename))
                    result = string.IsNullOrWhiteSpace(initialCatalog) ?
                        new ValidationResult($"'{_descriptor.GetInitialCatalogDisplay()}' or '{_descriptor.GetAttachDBFilenameDisplay()}' is required.") : null;
                else
                    result = string.IsNullOrWhiteSpace(initialCatalog) ? null :
                        new ValidationResult($"Cannot use '{_descriptor.GetInitialCatalogDisplay()}' with '{_descriptor.GetAttachDBFilenameDisplay()}'.");
            }
            else
                result = null;
            return result is null;
        }

        public static bool ValidateAttachDBFilename(string attachDBFilename, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder)
            {
                if (string.IsNullOrWhiteSpace(connectionStringBuilder.InitialCatalog))
                    result = string.IsNullOrWhiteSpace(attachDBFilename) ?
                        new ValidationResult($"'{_descriptor.GetAttachDBFilenameDisplay()}' or '{_descriptor.GetInitialCatalogDisplay()}' is required.") : null;
                else
                    result = string.IsNullOrWhiteSpace(attachDBFilename) ? null :
                        new ValidationResult($"'{_descriptor.GetAttachDBFilenameDisplay()}' and '{_descriptor.GetInitialCatalogDisplay()}' cannot both be specifed.");
            }
            else
                result = null;
            return result is null;
        }

        public static bool ValidateUserID(string userID, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder)
            {
                switch (connectionStringBuilder.Authentication)
                {
                    case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                        if (string.IsNullOrWhiteSpace(userID))
                        {
                            if (connectionStringBuilder.IntegratedSecurity)
                            {
                                if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                    result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                                else
                                    result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetPasswordDisplay()}'.");
                            }
                            else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                result = null;
                            else
                                result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetPasswordDisplay()}'.");
                        }
                        else if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{_descriptor.GetIntegratedSecurityDisplay()}' or '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                            result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        else
                            result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetPasswordDisplay()}' or '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        break;
                    case SqlAuthenticationMethod.ActiveDirectoryInteractive:
                        if (string.IsNullOrWhiteSpace(userID))
                        {
                            if (connectionStringBuilder.IntegratedSecurity)
                                result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetIntegratedSecurityDisplay()}' or '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                            else
                                result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        }
                        else if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                        else
                            result = null;
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(userID))
                        {
                            if (connectionStringBuilder.IntegratedSecurity)
                                result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                            else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                result = null;
                            else
                                result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetPasswordDisplay()}'.");
                        }
                        else if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                            result = null;
                        else
                            result = new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required with '{_descriptor.GetPasswordDisplay()}'.");
                        break;
                }
            }
            else
                result = null;
            return result is null;
        }

        public static bool ValidatePassword(string password, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder && !string.IsNullOrWhiteSpace(password))
            {
                switch (connectionStringBuilder.Authentication)
                {
                    case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                        if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{_descriptor.GetIntegratedSecurityDisplay()}' or '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        else
                            result = new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}'.");
                        break;
                    default:
                        if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                        else
                            result = null;
                        break;
                }
            }
            else
                result = null;
            return result is null;
        }

        public static bool ValidateIntegratedSecurity(bool integratedSecurity, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder)
            {
                if (integratedSecurity)
                {
                    if (connectionStringBuilder.Authentication == SqlAuthenticationMethod.NotSpecified)
                    {
                        if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                        {
                            if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                result = null;
                            else
                                result = new ValidationResult($"Cannot use '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetPasswordDisplay()}'.");
                        }
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                            result = new ValidationResult($"Cannot use '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetUserIDDisplay()}'.");
                        else
                            result = new ValidationResult($"Cannot use '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetUserIDDisplay()}' or '{_descriptor.GetPasswordDisplay()}'.");
                    }
                    else if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                    {
                        if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                            result = new ValidationResult($"Cannot use  '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetAuthenticationDisplay()}'.");
                        else
                            result = new ValidationResult($"Cannot use  '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetAuthenticationDisplay()}' or '{_descriptor.GetPasswordDisplay()}'.");
                    }
                    else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                        result = new ValidationResult($"Cannot use  '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetAuthenticationDisplay()}' or '{_descriptor.GetUserIDDisplay()}'.");
                    else
                        result = new ValidationResult($"Cannot use  '{_descriptor.GetIntegratedSecurityDisplay()}' with '{_descriptor.GetAuthenticationDisplay()}', '{_descriptor.GetUserIDDisplay()}'or '{_descriptor.GetPasswordDisplay()}'.");
                }
                else if (connectionStringBuilder.Authentication == SqlAuthenticationMethod.NotSpecified && string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                    result = new ValidationResult($"Must use  '{_descriptor.GetIntegratedSecurityDisplay()}' without '{_descriptor.GetAuthenticationDisplay()}' or '{_descriptor.GetUserIDDisplay()}'.");
                else
                    result = null;
            }
            else
                result = null;
            return result is null;
        }

        public static bool ValidateAuthentication(SqlAuthenticationMethod authentication, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is SqlConnectionStringBuilder connectionStringBuilder)
            {
                switch (authentication)
                {
                    case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                        if (connectionStringBuilder.IntegratedSecurity)
                        {
                            if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                            {
                                if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                    result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with  '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                                else
                                    result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with  '{_descriptor.GetIntegratedSecurityDisplay()}' or '{_descriptor.GetPasswordDisplay()}'.");
                            }
                            else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetIntegratedSecurityDisplay()}' or '{_descriptor.GetUserIDDisplay()}'.");
                            else
                                result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetIntegratedSecurityDisplay()}', '{_descriptor.GetUserIDDisplay()}' or '{_descriptor.GetPasswordDisplay()}'.");
                        }
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                        {
                            if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                                result = null;
                            else
                                result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetPasswordDisplay()}'.");
                        }
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.Password))
                            result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetUserIDDisplay()}'.");
                        else
                            result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetUserIDDisplay()}' or '{_descriptor.GetPasswordDisplay()}'.");
                        break;
                    case SqlAuthenticationMethod.ActiveDirectoryInteractive:
                        if (connectionStringBuilder.IntegratedSecurity)
                        {
                            if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                                result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with  '{_descriptor.GetIntegratedSecurityDisplay()}' and without '{_descriptor.GetUserIDDisplay()}'.");
                            else
                                result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with  '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                        }
                        else if (string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                            result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' without '{_descriptor.GetUserIDDisplay()}'.");
                        else
                            result = null;
                        break;
                    case SqlAuthenticationMethod.NotSpecified:
                        result = null;
                        break;
                    default:
                        if (connectionStringBuilder.IntegratedSecurity)
                            result = new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(connectionStringBuilder.Authentication)}' with '{_descriptor.GetIntegratedSecurityDisplay()}'.");
                        else
                            result = null;
                        break;
                }
            }
            else
                result = null;
            return result is null;
        }

        public override void ConfigurePropertyBuilder(IPropertyBuilder<SqlConnectionStringBuilder> propertyBuilder)
        {
            switch (propertyBuilder.PropertyDescriptor.Name)
            {
                case nameof(SqlConnectionStringBuilder.DataSource):
                    propertyBuilder.ValidationAttributes.SetRequired(true);
                    break;
                case nameof(SqlConnectionStringBuilder.InitialCatalog):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidateInitialCatalog));
                    break;
                case nameof(SqlConnectionStringBuilder.AttachDBFilename):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidateAttachDBFilename));
                    break;
                case nameof(SqlConnectionStringBuilder.UserID):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidateUserID));
                    break;
                case nameof(SqlConnectionStringBuilder.Password):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidatePassword));
                    break;
                case nameof(SqlConnectionStringBuilder.IntegratedSecurity):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidateIntegratedSecurity));
                    break;
                case nameof(SqlConnectionStringBuilder.Authentication):
                    propertyBuilder.ValidationAttributes.EnsureCustomValidator<SqlConnectionStringModelDescriptionBuilder>(nameof(ValidateAuthentication));
                    break;
            }
            base.ConfigurePropertyBuilder(propertyBuilder);
        }
    }
}
