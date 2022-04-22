using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace DevUtil
{
    public class FieldDescriptor : IFieldOrPropertyDescriptor
    {
        public EnhancedPropertyDescriptor Property { get; }

        public FieldInfo FieldInfo { get; }

        public EnhancedTypeDescriptor Type { get; }

        string IFieldOrPropertyDescriptor.Name => FieldInfo.Name;

        bool IFieldOrPropertyDescriptor.IsKey => Property.IsKey;

        EnhancedTypeDescriptor IFieldOrPropertyDescriptor.Owner => Property.Owner;

        public bool NotNull { get; }

        public FieldDescriptor(EnhancedPropertyDescriptor propertyDescriptor, FieldInfo fieldInfo)
        {
            Property = propertyDescriptor;
            FieldInfo = fieldInfo;
            Type = EnhancedDefinedTypeDescriptor.Get(fieldInfo.FieldType);
            switch (Type.Category)
            {
                case TypeCategory.Enum:
                case TypeCategory.Primitive:
                case TypeCategory.Struct:
                    NotNull = true;
                    break;
                case TypeCategory.Nullable:
                    NotNull = false;
                    break;
                default:
                    NotNull = propertyDescriptor.NotNull;
                    break;
            }
        }

        internal static FieldDescriptor Get (EnhancedPropertyDescriptor propertyDescriptor)
        {
            string name = propertyDescriptor.BackingDescriptor.Attributes.OfType<BackingFieldAttribute>().Select(a => a.Name).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            if (name is null) return null;
            FieldInfo fieldInfo = propertyDescriptor.Owner.Type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return (fieldInfo is not null && fieldInfo.FieldType.IsAssignableFrom(propertyDescriptor.BackingDescriptor.PropertyType)) ? new(propertyDescriptor, fieldInfo) : null;
        }
    }
}
