using FsInfoCat.Desktop.Model.ComponentSupport;
using System.Data.OleDb;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class OleDbConnectionStringModelDescriptionBuilder : DbConnectionStringModelDescriptorBuilder<OleDbConnectionStringBuilder>
    {
        public override void ConfigurePropertyBuilder(IPropertyBuilder<OleDbConnectionStringBuilder> propertyBuilder)
        {
            switch (propertyBuilder.PropertyDescriptor.Name)
            {
                case nameof(OleDbConnectionStringBuilder.DataSource):
                case nameof(OleDbConnectionStringBuilder.Provider):
                case nameof(OleDbConnectionStringBuilder.FileName):
                    propertyBuilder.ValidationAttributes.SetRequired(true);
                    break;
            }
            base.ConfigurePropertyBuilder(propertyBuilder);
        }
    }
}
