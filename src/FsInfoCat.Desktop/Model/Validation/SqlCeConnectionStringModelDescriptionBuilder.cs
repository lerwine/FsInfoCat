using FsInfoCat.Desktop.Model.ComponentSupport;
using System.Data.SqlServerCe;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class SqlCeConnectionStringModelDescriptionBuilder : DbConnectionStringModelDescriptorBuilder<SqlCeConnectionStringBuilder>
    {
        public override void ConfigurePropertyBuilder(IPropertyBuilder<SqlCeConnectionStringBuilder> propertyBuilder)
        {
            if (propertyBuilder.PropertyDescriptor.Name == nameof(SqlCeConnectionStringBuilder.DataSource))
                propertyBuilder.ValidationAttributes.SetRequired(true);
            base.ConfigurePropertyBuilder(propertyBuilder);
        }
    }
}
