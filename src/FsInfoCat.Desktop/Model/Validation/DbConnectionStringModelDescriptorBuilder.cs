using FsInfoCat.Desktop.Model.ComponentSupport;
using System.ComponentModel;
using System.Data.Common;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class DbConnectionStringModelDescriptorBuilder<T> : ModelDescriptorBuilder<T>
        where T : DbConnectionStringBuilder
    {
        public override bool ShouldIncludeProperty(ModelDescriptor<T> modelDescriptor, PropertyDescriptor propertyDescriptor)
        {
            switch (propertyDescriptor.Name)
            {
                case nameof(DbConnectionStringBuilder.Count):
                case nameof(DbConnectionStringBuilder.IsFixedSize):
                case nameof(DbConnectionStringBuilder.IsReadOnly):
                case nameof(DbConnectionStringBuilder.Keys):
                case nameof(DbConnectionStringBuilder.Values):
                    return false;
                default:
                    return base.ShouldIncludeProperty(modelDescriptor, propertyDescriptor);
            }
        }

        public override void ConfigurePropertyBuilder(IPropertyBuilder<T> propertyBuilder)
        {
            if (propertyBuilder.PropertyDescriptor.Name == nameof(DbConnectionStringBuilder.ConnectionString))
                propertyBuilder.ValidationAttributes.SetRequired(true);
            base.ConfigurePropertyBuilder(propertyBuilder);
        }
    }
}
