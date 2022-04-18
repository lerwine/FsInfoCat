using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DevUtil
{
    public class EnhancedPropertyDescriptor : IFieldOrPropertyDescriptor
    {
        public string Name { get; }

        public bool IsKey { get; }

        public bool NotNull { get; }

        public EnhancedTypeDescriptor Type { get; }

        public EnhancedTypeDescriptor Owner { get; }

        public FieldDescriptor BackingField { get; }

        public EnhancedPropertyDescriptor BaseDescriptor { get; }

        public PropertyDescriptor BackingDescriptor { get; }

        public IFieldOrPropertyDescriptor EffectiveDescriptor { get; }

        EnhancedPropertyDescriptor IFieldOrPropertyDescriptor.Property => this;

        private EnhancedPropertyDescriptor(EnhancedTypeDescriptor owner, PropertyDescriptor backingDescriptor)
        {
            Owner = owner;
            Name = (BackingDescriptor = backingDescriptor).Name;
            Type = EnhancedTypeDescriptor.Get(backingDescriptor.PropertyType);
            IsKey = backingDescriptor.Attributes.OfType<KeyAttribute>().Any();
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
                    NotNull = backingDescriptor.Attributes.OfType<NotNullAttribute>().Any();
                    break;
            }
            BackingField = FieldDescriptor.Get(this);
            if (BackingField is null)
                EffectiveDescriptor = this;
            else
                EffectiveDescriptor = BackingField;
        }

        private EnhancedPropertyDescriptor(EnhancedTypeDescriptor owner, EnhancedPropertyDescriptor baseDescriptor, PropertyDescriptor backingDescriptor)
        {
            Owner = owner;
            Name = (BaseDescriptor = baseDescriptor).Name;
            if (backingDescriptor is null)
            {
                BackingDescriptor = baseDescriptor.BackingDescriptor;
                Type = baseDescriptor.Type;
                IsKey = baseDescriptor.IsKey;
                NotNull = baseDescriptor.NotNull;
                EffectiveDescriptor = this;
            }
            else
            {
                Type = EnhancedTypeDescriptor.Get((BackingDescriptor = backingDescriptor).PropertyType);
                IsKey = baseDescriptor.IsKey || backingDescriptor.Attributes.OfType<KeyAttribute>().Any();
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
                        NotNull = baseDescriptor.NotNull || backingDescriptor.Attributes.OfType<NotNullAttribute>().Any();
                        break;
                }
                BackingField = FieldDescriptor.Get(this);
                if (BackingField is null)
                    EffectiveDescriptor = this;
                else
                    EffectiveDescriptor = BackingField;
            }
        }

        internal static Collection<EnhancedPropertyDescriptor> Create(EnhancedTypeDescriptor owner)
        {
            Collection<EnhancedPropertyDescriptor> properties = new();
            IEnumerable<IGrouping<string, PropertyDescriptor>> propertiesByName = TypeDescriptor.GetProperties(owner.Type).OfType<PropertyDescriptor>().GroupBy(p => p.Name);
            if (owner.BaseType is null)
                foreach (PropertyDescriptor propertyDescriptor in propertiesByName.Select(g => g.FirstOrDefault(p => owner.Type.Equals(p.ComponentType)) ?? g.First()))
                    properties.Add(new(owner, propertyDescriptor));
            else
            {
                foreach (IGrouping<string, PropertyDescriptor> png in propertiesByName)
                {
                    PropertyDescriptor pd = png.FirstOrDefault(p => owner.Type.Equals(p.ComponentType));
                    EnhancedPropertyDescriptor baseDescriptor = owner.BaseType.Properties.FirstOrDefault(p => p.Name == png.Key);
                    if (baseDescriptor is null)
                        properties.Add(new(owner, pd ?? png.First()));
                    else if (pd is null)
                        properties.Add(new(owner, baseDescriptor, pd));
                }
                foreach (EnhancedPropertyDescriptor pd in owner.BaseType.Properties)
                    if (!properties.Any(p => p.Name == pd.Name))
                        properties.Add(new(owner, pd, null));
            }
            return properties;
        }
    }
}
