using FsInfoCat.Desktop.Model.ComponentSupport;
using System.Data.Odbc;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class OdbcConnectionStringModelDescriptionBuilder : DbConnectionStringModelDescriptorBuilder<OdbcConnectionStringBuilder>
    {
        public override void ConfigurePropertyBuilder(IPropertyBuilder<OdbcConnectionStringBuilder> propertyBuilder)
        {
            switch (propertyBuilder.PropertyDescriptor.Name)
            {
                case nameof(OdbcConnectionStringBuilder.Driver):
                case nameof(OdbcConnectionStringBuilder.Dsn):
                    propertyBuilder.ValidationAttributes.SetRequired(true);
                    break;
            }
            base.ConfigurePropertyBuilder(propertyBuilder);
        }
    }
}
