using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using static CodeGeneration.Constants;

namespace CodeGeneration
{
    public class EntityGenerationInfo : ITypeGenerationInfo
    {
        private ReadOnlyCollection<EntityGenerationInfo> _allOrdered = null;
        private EntityGenerationInfo(XElement entityElement, Collection<PropertyGenerationInfo> properties,
            Collection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> uniqueConstraints)
        {
            Name = (Source = entityElement).Attribute(NAME_Name)?.Value;
            CsName = Name.Replace("{", "<").Replace("}", ">");
            TableName = entityElement.Attribute(NAME_TableName)?.Value;
            string name = entityElement.Attribute(NAME_SingularName)?.Value;
            if (string.IsNullOrWhiteSpace(name))
            {
                int index = name.IndexOf("{");
                name = (index < 0) ? Name : Name.Substring(0, index);
                SingularName = (name.Length > 1 && name.StartsWith("I")) ? name.Substring(1) : name;
            }
            else
                SingularName = name;

            BaseTypes = new(entityElement.Elements(XNAME_ExtendsGenericEntity).Select(e => (false, e.Attribute(XNAME_Type)?.Value, Get(entityElement.FindEntityByName(e.Attribute(XNAME_TypeDef)?.Value))))
                .Concat(entityElement.Elements(XNAME_ExtendsEntity).Attributes(XNAME_Type).Select(a => (false, a.Value, Get(entityElement.FindEntityByName(a.Value)))))
                .Concat(entityElement.Elements(XNAME_BaseType).Attributes(XNAME_Type).Select(a => (true, a.Value, (EntityGenerationInfo)null)))
                .Concat(entityElement.Elements(XNAME_RootInterface).Attributes(XNAME_Type).Select(a => (false, a.Value, Get(entityElement.FindEntityByName(a.Value)))))
                .Concat(entityElement.Elements().Select(e =>
                {
                    if (e.Name == XNAME_ImplementsGenericEntity)
                        return (false, e.Attribute(XNAME_Type)?.Value, Get(entityElement.FindEntityByName(e.Attribute(XNAME_TypeDef)?.Value)));
                    if (e.Name == XNAME_ImplementsEntity)
                    {
                        string n = e.Attribute(XNAME_Type)?.Value;
                        return (false, n, Get(entityElement.FindEntityByName(n)));
                    }
                    if (e.Name == XNAME_Implements)
                        return (true, e.Attribute(XNAME_Type)?.Value, (EntityGenerationInfo)null);
                    return (false, null, null);
                })).Where(t => t.Item1 ? t.Item2 is not null : t.Item3 is not null).Select(t => (t.Item2 ?? t.Item3.Name, t.Item3)).ToArray());
            Properties = new ReadOnlyCollection<PropertyGenerationInfo>(properties);
            UniqueConstraints = new(uniqueConstraints);
            CheckConstraint cc = CheckConstraint.Import(entityElement.Element(XNAME_Check));
            foreach ((string Name, EntityGenerationInfo Entity) in BaseTypes)
            {
                CheckConstraint bc = Entity?.CheckConstraints;
                if (bc is not null)
                    cc = (cc is null) ? bc : cc.And(bc);
            }
            CheckConstraints = cc;
        }
        public string Name { get; }
        public string CsName { get; }
        public XElement Source { get; }
        public string TableName { get; }
        public ReadOnlyCollection<(string Name, EntityGenerationInfo Entity)> BaseTypes { get; }
        public ReadOnlyCollection<PropertyGenerationInfo> Properties { get; }
        public ReadOnlyCollection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> UniqueConstraints { get; }
        public CheckConstraint CheckConstraints { get; }
        public string SingularName { get; }

        public bool Extends(EntityGenerationInfo other) => !(other is null || ReferenceEquals(this, other)) &&
            (BaseTypes.Any(e => ReferenceEquals(other, e.Entity)) || BaseTypes.Any(e => e.Entity.Extends(other)));
        IEnumerable<EntityGenerationInfo> GetAllBaseTypes() => (BaseTypes.Count > 0) ?
            BaseTypes.Select(b => b.Entity).Concat(BaseTypes.SelectMany(b => b.Entity.GetAllBaseTypes())).Distinct() : Enumerable.Empty<EntityGenerationInfo>();
        public ReadOnlyCollection<EntityGenerationInfo> GetAllBaseTypesOrdered()
        {
            if (_allOrdered is not null)
                return _allOrdered;
            if (BaseTypes.Count == 0)
                _allOrdered = new(Array.Empty<EntityGenerationInfo>());
            else
            {
                LinkedList<EntityGenerationInfo> result = new();
                result.AddLast(BaseTypes[0].Entity);
                foreach (EntityGenerationInfo g in GetAllBaseTypes().Skip(1))
                {
                    LinkedListNode<EntityGenerationInfo> node = result.Last;
                    while (g.Extends(node.Value))
                    {
                        if ((node = node.Previous) is null)
                            break;
                    }
                    if (node is null)
                        result.AddFirst(g);
                    else if (!ReferenceEquals(node.Value, g))
                        result.AddAfter(node, g);
                }
                _allOrdered = new(result.ToArray());
            }
            return _allOrdered;
        }
        public static EntityGenerationInfo Get(XElement entityElement)
        {
            if (entityElement is null)
                return null;
            EntityGenerationInfo result = entityElement.Annotation<EntityGenerationInfo>();
            if (result is not null)
                return result;
            if (entityElement.IsEntityElement())
            {
                Collection<PropertyGenerationInfo> properties = new();
                Collection<(string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties)> uniqueConstraints = new();
                result = new EntityGenerationInfo(entityElement, properties, uniqueConstraints);
                entityElement.AddAnnotation(result);
                foreach (XElement element in entityElement.Elements(XNAME_Properties).Elements())
                {
                    PropertyGenerationInfo property = PropertyGenerationInfo.Get(element);
                    if (property is not null)
                        properties.Add(property);
                }
                foreach (PropertyGenerationInfo bp in result.GetAllBaseTypes().SelectMany(t => t.Properties).Distinct())
                {
                    string n = bp.Name;
                    PropertyGenerationInfo ep = properties.FirstOrDefault(p => p.Name == n);
                    if (ep is null)
                        properties.Add(bp);
                    else if (!ReferenceEquals(ep.DeclaringType, result) && bp.DeclaringType.Extends(ep.DeclaringType))
                        properties[properties.IndexOf(ep)] = bp;
                }
                foreach ((string Name, ReadOnlyCollection<PropertyGenerationInfo> Properties) uc in entityElement.Elements(XNAME_Unique).Attributes(XNAME_Name).Select<XAttribute, (string, PropertyGenerationInfo[])>(a => (
                    a.Value,
                    a.Parent.Elements(XNAME_Property).Attributes(XNAME_Name).Select(n => n.Value).Select(n => properties.FirstOrDefault(p => p.Name == n))
                    .Where(p => p is not null).ToArray()
                )).Where(t => t.Item2.Length > 0).Select(t => (t.Item1, new ReadOnlyCollection<PropertyGenerationInfo>(t.Item2))))
                    uniqueConstraints.Add(uc);
            }
            return null;
        }
        IEnumerable<IMemberGenerationInfo> ITypeGenerationInfo.GetMembers() => Properties.Cast<IMemberGenerationInfo>();
    }
}
