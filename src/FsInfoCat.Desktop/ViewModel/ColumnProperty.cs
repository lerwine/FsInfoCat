using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ColumnProperty
    {
        private static readonly Dictionary<Type, Collection<ColumnProperty>> _columnProperties = new();

        public Type TargetType { get; }

        public Type OwnerType => DependencyProperty.OwnerType;

        public Type PropertyType => DependencyProperty.PropertyType;

        public bool IsReadOnly => DependencyProperty.ReadOnly;

        public string Name => DependencyProperty.Name;

        public string DisplayName { get; }

        public string ShortName { get; }

        public string Prompt { get; }

        public string GroupName { get; }

        public string Description { get; }

        public int? Order { get; }

        /// <summary>
        /// Gets the registered dependency property that corresponds to a column value.
        /// </summary>
        /// <value>
        /// The registered view model dependency property that corresponds to a column value.
        /// </value>
        public DependencyProperty DependencyProperty { get; }

        private ColumnProperty([DisallowNull] DependencyProperty dependencyProperty, [DisallowNull] string displayName, [DisallowNull] string shortName, [DisallowNull] string prompt,
            [DisallowNull] string groupName, [DisallowNull] string description, int? order, [DisallowNull] Type targetType)
        {
            DisplayName = displayName;
            ShortName = shortName;
            Prompt = prompt;
            GroupName = groupName;
            Description = description;
            Order = order;
            TargetType = targetType;
        }

        internal static void Add([DisallowNull] DependencyProperty dependencyProperty, [DisallowNull] string displayName, [DisallowNull] string shortName, [DisallowNull] string prompt,
            [DisallowNull] string groupName, [DisallowNull] string description, int? order, [DisallowNull] Type targetType)
        {
            lock (_columnProperties)
            {
                if (!_columnProperties.TryGetValue(targetType, out Collection<ColumnProperty> collection))
                {
                    collection = new();
                    _columnProperties.Add(targetType, collection);
                }
                collection.Add(new(dependencyProperty, displayName, shortName, prompt, groupName, description, order, targetType));
            }
        }

        private static IEnumerable<ColumnProperty> PrivateGetProperties(Type type)
        {
            Stack<Type> types = new();
            while (type != typeof(DependencyObject))
            {
                types.Push(type);
                if ((type = type.BaseType) is null)
                    break;
            }
            return types.SelectMany(t => _columnProperties.TryGetValue(t, out Collection<ColumnProperty> collection) ? collection : Enumerable.Empty<ColumnProperty>());
        }

        private static IEnumerable<ColumnProperty> GetOrdered(IEnumerable<ColumnProperty> source) => source.Select((Item, OrginalIndex) =>
            (Item, OrginalIndex, Order: Item.Order ?? int.MaxValue)).OrderBy(t => t.Order).ThenBy(t => t.OrginalIndex).Select(t => t.Item);

        public static IEnumerable<ColumnProperty> GetProperties(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(DependencyObject).IsAssignableFrom(type))
                throw new ArgumentOutOfRangeException(nameof(type));
            return PrivateGetProperties(type);
        }

        public static IEnumerable<ColumnProperty> GetProperties<TTarget>() where TTarget : DependencyObject => PrivateGetProperties(typeof(TTarget));

        public static IEnumerable<ColumnProperty> GetOrderedProperties<TTarget>() where TTarget : DependencyObject => GetOrdered(GetProperties<TTarget>());

        public static IEnumerable<ColumnProperty> GetOrderedProperties(Type type) => GetOrdered(GetProperties(type));
    }
}
