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

        public static IEnumerable<ColumnProperty> GetProperties<TTarget>()
            where TTarget : DependencyObject
        {
            Type type = typeof(TTarget);
            Stack<Type> types = new();
            while (type != typeof(DependencyObject))
            {
                types.Push(type);
                if ((type = type.BaseType) is null)
                    break;
            }
            return types.SelectMany(t => _columnProperties.TryGetValue(t, out Collection<ColumnProperty> collection) ? collection : Enumerable.Empty<ColumnProperty>());
        }
    }
}
