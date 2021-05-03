using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Desktop.Util
{
    public interface ComponentModelPropertyValue
    {
        object Value { get; }
        string Name { get; }
        string DisplayName { get; }
        string Category { get; }
        string Description { get; }
        bool IsReadOnly { get; }
        Type PropertyType { get; }
        TypeConverter Converter { get; }
    }

    public class ComponentPropertyValue<T> : ComponentModelPropertyValue
    {
        private readonly object _component;
        private readonly PropertyDescriptor _propertyDescriptor;

        public T Value => (T)_propertyDescriptor.GetValue(_component);

        object ComponentModelPropertyValue.Value => Value;

        public string Name => _propertyDescriptor.Name;

        public string DisplayName { get; }

        public string Category { get; }

        public string Description => _propertyDescriptor.Description ?? "";

        public bool IsReadOnly => _propertyDescriptor.IsReadOnly;

        Type ComponentModelPropertyValue.PropertyType => typeof(T);

        TypeConverter ComponentModelPropertyValue.Converter => _propertyDescriptor.Converter;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private ComponentPropertyValue(PropertyDescriptor propertyDescriptor, object component)
        {
            _component = component;
            _propertyDescriptor = propertyDescriptor;
            DisplayName = string.IsNullOrWhiteSpace(propertyDescriptor.DisplayName) ? propertyDescriptor.Name : propertyDescriptor.DisplayName;
            Category = string.IsNullOrWhiteSpace(propertyDescriptor.Category) ? CategoryAttribute.Default.Category : propertyDescriptor.Category;
        }

        public static IEnumerable<ComponentModelPropertyValue> Create(object component, bool includeReadOnly = false)
        {
            Type g = typeof(ComponentPropertyValue<>);
            return ComponentModelHelper.GetModelProperties(component, includeReadOnly)
                .Select(pd => (ComponentModelPropertyValue)Activator.CreateInstance(g.MakeGenericType(pd.PropertyType), pd, component));
        }
    }
}
