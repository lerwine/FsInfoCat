using FsInfoCat.Desktop.Model.ComponentSupport;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class SqlConnectionStringModelDescriptionBuilder : DbConnectionStringModelDescriptorBuilder<SqlConnectionStringBuilder>
    {
        private static readonly ModelDescriptor<SqlConnectionStringBuilder> _descriptor = new SqlConnectionStringModelDescriptionBuilder().Build();

        public static ValidationResult ValidateInitialCatalog(string initialCatalog, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext)
            {
                PropertyValidationContext<SqlConnectionStringBuilder, string> attachDbFilename = modelContext.AttachDBFilename();
                attachDbFilename.CheckPropertyChange();
                if (string.IsNullOrWhiteSpace(attachDbFilename.Value))
                {
                    if (string.IsNullOrWhiteSpace(initialCatalog))
                        return new ValidationResult($"'{_descriptor.GetInitialCatalogDisplay()}' or '{attachDbFilename.DisplayName}' is required.");
                    if (attachDbFilename.HasErrors)
                        attachDbFilename.Invalidate();
                    return null;
                }
                return string.IsNullOrWhiteSpace(initialCatalog) ? null :
                    new ValidationResult($"Cannot use '{_descriptor.GetInitialCatalogDisplay()}' with '{attachDbFilename.DisplayName}'.");
            }
            return null;
        }

        public static ValidationResult ValidateAttachDBFilename(string attachDBFilename, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext)
            {   
                PropertyValidationContext<SqlConnectionStringBuilder, string> initialCatalog = modelContext.InitialCatalog();
                initialCatalog.CheckPropertyChange();
                if (string.IsNullOrWhiteSpace(attachDBFilename))
                {
                    PropertyValidationContext<SqlConnectionStringBuilder, string> userID = modelContext.UserID();
                    PropertyValidationContext<SqlConnectionStringBuilder, bool> integratedSecurity = modelContext.IntegratedSecurity();
                    PropertyValidationContext<SqlConnectionStringBuilder, SqlAuthenticationMethod> authentication = modelContext.Authentication();
                    userID.CheckPropertyChange();
                    integratedSecurity.CheckPropertyChange();
                    authentication.CheckPropertyChange();
                    if (authentication.Value == SqlAuthenticationMethod.NotSpecified && !integratedSecurity.Value && string.IsNullOrWhiteSpace(userID.Value) && userID.HasErrors)
                        userID.Invalidate();
                    if (string.IsNullOrWhiteSpace(initialCatalog.Value))
                        return new ValidationResult($"'{_descriptor.GetAttachDBFilenameDisplay()}' or '{initialCatalog.DisplayName}' is required.");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(initialCatalog.Value))
                {
                    if (initialCatalog.HasErrors)
                        initialCatalog.Invalidate();
                    return null;
                }
                return new ValidationResult($"Cannot use '{_descriptor.GetAttachDBFilenameDisplay()}' with '{initialCatalog.DisplayName}'.");
            }
            return null;
        }

        public static ValidationResult ValidateUserID(string userID, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext)
            {
                PropertyValidationContext<SqlConnectionStringBuilder, SqlAuthenticationMethod> authentication = modelContext.Authentication();
                PropertyValidationContext<SqlConnectionStringBuilder, bool> integratedSecurity = modelContext.IntegratedSecurity();
                PropertyValidationContext<SqlConnectionStringBuilder, string> password = modelContext.Password();
                PropertyValidationContext<SqlConnectionStringBuilder, string> attachDBFilename = modelContext.AttachDBFilename();
                authentication.CheckPropertyChange();
                integratedSecurity.CheckPropertyChange();
                password.CheckPropertyChange();
                attachDBFilename.CheckPropertyChange();

                if (string.IsNullOrWhiteSpace(userID))
                {
                    if (integratedSecurity.Value)
                        return null;
                    switch (authentication.Value)
                    {
                        case SqlAuthenticationMethod.ActiveDirectoryInteractive:
                        case SqlAuthenticationMethod.SqlPassword:
                            return new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required.");
                        case SqlAuthenticationMethod.NotSpecified:
                            if (string.IsNullOrWhiteSpace(attachDBFilename.Value))
                                return new ValidationResult($"'{_descriptor.GetUserIDDisplay()}' required.");
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(password.Value) && !password.HasErrors)
                        password.Invalidate();
                    return null;
                }

                if (authentication.Value == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
                {
                    if (integratedSecurity.Value)
                        return new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{integratedSecurity.DisplayName}' or '{_descriptor.GetAuthenticationDisplay(authentication.Value)}'.");
                    return new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{_descriptor.GetAuthenticationDisplay(authentication.Value)}'.");
                }
                if (integratedSecurity.Value)
                    return new ValidationResult($"Cannot use '{_descriptor.GetUserIDDisplay()}' with '{integratedSecurity.DisplayName}'.");
                if (password.HasErrors)
                    password.Invalidate();
            }
            return null;
        }

        public static ValidationResult ValidatePassword(string password, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext && !string.IsNullOrWhiteSpace(password))
            {
                PropertyValidationContext<SqlConnectionStringBuilder, SqlAuthenticationMethod> authentication = modelContext.Authentication();
                PropertyValidationContext<SqlConnectionStringBuilder, bool> integratedSecurity = modelContext.IntegratedSecurity();
                PropertyValidationContext<SqlConnectionStringBuilder, string> userID = modelContext.UserID();
                authentication.CheckPropertyChange();
                integratedSecurity.CheckPropertyChange();
                userID.CheckPropertyChange();
                if (authentication.Value == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
                {
                    if (integratedSecurity.Value)
                        return new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{integratedSecurity.DisplayName}' or '{_descriptor.GetAuthenticationDisplay(authentication.Value)}'.");
                    return new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{_descriptor.GetAuthenticationDisplay(authentication.Value)}'.");
                }
                if (integratedSecurity.Value)
                    return new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' with '{integratedSecurity.DisplayName}'.");
                if (string.IsNullOrWhiteSpace(userID.Value))
                    return new ValidationResult($"Cannot use '{_descriptor.GetPasswordDisplay()}' without '{userID.DisplayName}'.");
            }
            return null;
        }

        public static ValidationResult ValidateIntegratedSecurity(bool integratedSecurity, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext)
            {
                PropertyValidationContext<SqlConnectionStringBuilder, SqlAuthenticationMethod> authentication = modelContext.Authentication();
                PropertyValidationContext<SqlConnectionStringBuilder, string> password = modelContext.Password();
                PropertyValidationContext<SqlConnectionStringBuilder, string> userID = modelContext.UserID();
                authentication.CheckPropertyChange();
                password.CheckPropertyChange();
                userID.CheckPropertyChange();
                if (integratedSecurity)
                {
                    switch (authentication.Value)
                    {
                        case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                            authentication.Invalidate();
                            break;
                        case SqlAuthenticationMethod.NotSpecified:
                            if (!string.IsNullOrWhiteSpace(userID.Value))
                                userID.Invalidate();
                            if (!string.IsNullOrWhiteSpace(password.Value))
                                password.Invalidate();
                            return null;
                        default:
                            authentication.Invalidate();
                            if (!string.IsNullOrWhiteSpace(userID.Value))
                                userID.Invalidate();
                            if (!string.IsNullOrWhiteSpace(password.Value))
                                password.Invalidate();
                            break;
                    }
                    return new ValidationResult($"Cannot use  '{_descriptor.GetIntegratedSecurityDisplay()}' with '{authentication.DisplayName}'.");
                }

                switch (authentication.Value)
                {
                    case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                        if (authentication.HasErrors)
                            authentication.Invalidate();
                        break;
                    case SqlAuthenticationMethod.NotSpecified:
                        if (userID.HasErrors)
                            userID.Invalidate();
                        if (!string.IsNullOrWhiteSpace(password.Value) && password.HasErrors)
                            password.Invalidate();
                        break;
                    default:
                        if (authentication.HasErrors)
                            authentication.Invalidate();
                        if (userID.HasErrors)
                            userID.Invalidate();
                        if (!string.IsNullOrWhiteSpace(password.Value) && password.HasErrors)
                            password.Invalidate();
                        break;
                }
            }
            return null;
        }

        public static ValidationResult ValidateAuthentication(SqlAuthenticationMethod authentication, ValidationContext context)
        {
            if (context.ObjectInstance is ModelValidationContext<SqlConnectionStringBuilder> modelContext)
            {
                PropertyValidationContext<SqlConnectionStringBuilder, string> password = modelContext.Password();
                PropertyValidationContext<SqlConnectionStringBuilder, string> userID = modelContext.UserID();
                PropertyValidationContext<SqlConnectionStringBuilder, bool> integratedSecurity = modelContext.IntegratedSecurity();
                PropertyValidationContext<SqlConnectionStringBuilder, string> attachDBFilename = modelContext.AttachDBFilename();
                password.CheckPropertyChange();
                userID.CheckPropertyChange();
                integratedSecurity.CheckPropertyChange();
                attachDBFilename.CheckPropertyChange();
                if (integratedSecurity.Value)
                {
                    if (authentication != SqlAuthenticationMethod.NotSpecified)
                        return new ValidationResult($"Cannot use '{_descriptor.GetAuthenticationDisplay(authentication)}' with  '{integratedSecurity.DisplayName}'.");
                    if (integratedSecurity.HasErrors)
                        integratedSecurity.Invalidate();
                    return null;
                }
                if (integratedSecurity.HasErrors)
                    integratedSecurity.Invalidate();
                switch (authentication)
                {
                    case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
                        if (string.IsNullOrWhiteSpace(userID.Value))
                        {
                            if (userID.HasErrors)
                                userID.Invalidate();
                            if (string.IsNullOrWhiteSpace(password.Value) == password.HasErrors)
                                password.Invalidate();
                        }
                        else
                        {
                            userID.Invalidate();
                            if (!string.IsNullOrWhiteSpace(password.Value))
                                password.Invalidate();
                        }
                        break;
                    case SqlAuthenticationMethod.ActiveDirectoryInteractive:
                        if (string.IsNullOrWhiteSpace(userID.Value) || userID.HasErrors)
                            userID.Invalidate();
                        break;
                    case SqlAuthenticationMethod.NotSpecified:
                        if (string.IsNullOrWhiteSpace(attachDBFilename.Value))
                        {
                            if (string.IsNullOrWhiteSpace(userID.Value))
                                userID.Invalidate();
                        }
                        else if (userID.HasErrors)
                            userID.Invalidate();
                        if (string.IsNullOrWhiteSpace(password.Value) == password.HasErrors)
                            password.Invalidate();
                        return null;
                    default:
                        if (userID.HasErrors)
                            userID.Invalidate();
                        if (string.IsNullOrWhiteSpace(password.Value) == password.HasErrors)
                            password.Invalidate();
                        return null;
                }
            }
            return null;
        }

        public override bool ShouldIncludeProperty(ModelDescriptor<SqlConnectionStringBuilder> modelDescriptor, PropertyDescriptor propertyDescriptor)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            if (propertyDescriptor.Name.Equals(nameof(SqlConnectionStringBuilder.ConnectionReset)))
#pragma warning restore CS0618 // Type or member is obsolete
                return false;
            return base.ShouldIncludeProperty(modelDescriptor, propertyDescriptor);
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
