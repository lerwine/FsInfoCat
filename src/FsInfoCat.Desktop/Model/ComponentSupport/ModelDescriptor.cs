using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Provides information about the characteristics and properties of a type.
    /// </summary>
    /// <typeparam name="TModel">The type of the target object.</typeparam>
    /// <seealso cref="IEquatable{ModelDescriptor{TModel}}" />
    /// <seealso cref="IModelDescriptor" />
    /// <seealso cref="IReadOnlyDictionary{string, IModelPropertyDescriptor{TModel}}" />
    /// <seealso cref="IEqualityComparer{TModel}" />
    public sealed class ModelDescriptor<TModel> : IEquatable<ModelDescriptor<TModel>>, IModelDescriptor,
        IReadOnlyDictionary<string, IModelPropertyDescriptor<TModel>>, IEqualityComparer<TModel>
        where TModel : class
    {
        /// <summary>
        /// Gets the objects that represent the properties of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="IModelPropertyDescriptor{TModel}"/> objects that represent the properties of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public ReadOnlyCollection<IModelPropertyDescriptor<TModel>> Properties { get; }

        IReadOnlyList<IModelPropertyDescriptor> IModelDescriptor.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        Type IModelDescriptor.ComponentType => typeof(TModel);

        /// <summary>
        /// Gets the simple name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="RuntimeType.Name"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public string SimpleName { get; }

        /// <summary>
        /// Gets the full name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="Type.FullName"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public string FullName { get; }

        public IEnumerable<string> Keys => Properties.Select(p => p.Name);

        public IModelPropertyDescriptor<TModel> this[string key] => Properties.FirstOrDefault(p => p.Name.Equals(key));

        IModelPropertyDescriptor IReadOnlyDictionary<string, IModelPropertyDescriptor>.this[string key] => throw new NotImplementedException();

        int IReadOnlyCollection<KeyValuePair<string, IModelPropertyDescriptor<TModel>>>.Count => Properties.Count;

        int IReadOnlyCollection<KeyValuePair<string, IModelPropertyDescriptor>>.Count => Properties.Count;

        IEnumerable<IModelPropertyDescriptor<TModel>> IReadOnlyDictionary<string, IModelPropertyDescriptor<TModel>>.Values => Properties;

        IEnumerable<IModelPropertyDescriptor> IReadOnlyDictionary<string, IModelPropertyDescriptor>.Values => Properties;

        internal ModelDescriptor(ModelDescriptorBuilder<TModel> builder)
        {
            Type t = typeof(TModel);
            Collection<IModelPropertyDescriptor<TModel>> properties = new Collection<IModelPropertyDescriptor<TModel>>();
            SimpleName = t.Name;
            FullName = t.FullName;
            Properties = new ReadOnlyCollection<IModelPropertyDescriptor<TModel>>(properties);
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(t).OfType<PropertyDescriptor>()
                .Where(p => builder.ShouldIncludeProperty(this, p)))
            {
                IPropertyBuilder<TModel> propertyBuilder = builder.CreatePropertyBuilder(this, pd);
                properties.Add(propertyBuilder.Build());
            }
        }

        public bool ContainsKey(string key) => Properties.Any(p => p.Name.Equals(key));

        IEnumerator<KeyValuePair<string, IModelPropertyDescriptor<TModel>>> IEnumerable<KeyValuePair<string, IModelPropertyDescriptor<TModel>>>.GetEnumerator() =>
            Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor<TModel>>(p.Name, p)).GetEnumerator();

        IEnumerator<KeyValuePair<string, IModelPropertyDescriptor>> IEnumerable<KeyValuePair<string, IModelPropertyDescriptor>>.GetEnumerator() =>
            Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor>(p.Name, p)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor<TModel>>(p.Name, p))).GetEnumerator();

        public bool TryGetValue(string key, out IModelPropertyDescriptor<TModel> value)
        {
            value = Properties.FirstOrDefault(p => p.Name.Equals(key));
            return !(value is null);
        }

        bool IReadOnlyDictionary<string, IModelPropertyDescriptor>.TryGetValue(string key, out IModelPropertyDescriptor value)
        {
            value = Properties.FirstOrDefault(p => p.Name.Equals(key));
            return !(value is null);
        }

        public bool Equals(ModelDescriptor<TModel> other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Properties.Count == other.Properties.Count &&
                Properties.OrderBy(p => p.Name).SequenceEqual(other.Properties.OrderBy(p => p.Name));
        }

        bool IEquatable<IModelDescriptor>.Equals(IModelDescriptor other) => other is ModelDescriptor<TModel> d && Equals(d);

        public override bool Equals(object obj) => obj is ModelDescriptor<TModel> d && Equals(d);

        public override int GetHashCode() =>
            Properties.Select(pd => pd.GetHashCode()).Concat(new int[] { FullName.GetHashCode() }).ToAggregateHashCode();

        public override string ToString()
        {
            if (Properties.Count == 0)
                return $@"{nameof(ModelDescriptor<TModel>)}<{SimpleName}> {{ }}";
            return $@"{nameof(ModelDescriptor<TModel>)}<{SimpleName}> {{\n\t{
                string.Join("\n\t", Properties.Select(pd => pd.PropertyType.ToCsTypeName() + " " + pd.Name + (pd.IsReadOnly ? " { get; }" : " { get; set; }")))
            }\n}}";
        }

        public bool Equals(TModel x, TModel y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            return Properties.All(pd => pd.Equals(pd.GetValue(x), pd.GetValue(y)));
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x is TModel a)
                return y is TModel b && Equals(a, b);
            if (y is TModel)
                return false;
            return (x is null) ? y is null : x.Equals(y);
        }

        public int GetHashCode(TModel obj) =>
            Properties.Select(pd => pd.GetHashCode(obj)).Concat(new int[] { FullName.GetHashCode() }).ToAggregateHashCode();

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj is TModel m)
                return GetHashCode(m);
            return (obj is null) ? 0 : obj.GetHashCode();
        }
    }
}
