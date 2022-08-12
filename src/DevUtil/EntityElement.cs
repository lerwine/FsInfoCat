using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class EntityElement : TypeDefElement
    {
        public const string ElementName_AbstractEntity = "AbstractEntity";

        public const string ElementName_GenericEntity1 = "GenericEntity1";

        public const string ElementName_GenericEntity2 = "GenericEntity2";

        public const string ElementName_Table = "Table";

        public const string ElementName_Implements = "Implements";

        public const string ElementName_Columns = "Columns";

        public EntityType Type { get; }

        public override EntityScope Scope { get; }

        public ReadOnlyCollection<PropertyElement> Properties { get; }

        public ReadOnlyCollection<ImplementsElement> Implements { get; }

        private EntityElement(XElement element) : base(element)
        {
            Scope = element.Parent.Name.LocalName switch
            {
                ElementName_Local => EntityScope.Local,
                ElementName_Upstream => EntityScope.Upstream,
                _ => EntityScope.Core,
            };
            Type = element.Name.LocalName switch
            {
                ElementName_Table => EntityType.Table,
                ElementName_GenericEntity1 => EntityType.GenericEntity1,
                ElementName_GenericEntity2 => EntityType.GenericEntity2,
                _ => EntityType.AbstractEntity,
            };
            element.AddAnnotation(this);
            Collection<PropertyElement> properties = new();
            Properties = new ReadOnlyCollection<PropertyElement>(properties);
            Implements = new ReadOnlyCollection<ImplementsElement>(element.Elements(ElementName_Implements).Elements().Select(e => ImplementsElement.Get(e)).Where(i => i is not null).ToArray());
            foreach (XElement e in element.Elements(ElementName_Columns).Elements())
            {
                PropertyElement p = PropertyElement.Get(e);
                if (p is not null) properties.Add(p);
            }
            foreach (IGrouping<string, PropertyElement> g in Implements.Select(i => i.Target).Where(t => t is not null).SelectMany(t => t.Properties).Distinct().GroupBy(p => p.Name))
            {
                if (properties.Any(p => p.Name == g.Key)) continue;
                PropertyElement ip = g.First();
                EntityElement dt = ip.DeclaringType;
                foreach (PropertyElement pe in g.Skip(1))
                {
                    EntityElement e = pe.DeclaringType;
                    if (e.ImplementsRecursive(dt))
                    {
                        ip = pe;
                        dt = e;
                    }
                }
                properties.Add(ip);
            }
        }

        private bool ImplementsRecursive(EntityElement entity)
        {
            if (entity is null) return false;
            foreach (ImplementsElement i in Implements)
                if (i.Target is not null && (ReferenceEquals(i.Target, entity) || i.Target.ImplementsRecursive(entity))) return true;
            return false;
        }

        public static bool Test(XElement element)
        {
            if (element is null || element.Name.NamespaceName.Length > 0 || element.Parent is null || element.Parent.Name.NamespaceName.Length > 0) return false;
            switch (element.Parent.Name.LocalName)
            {
                case ElementName_EntityTypes:
                    if (!ReferenceEquals(element.Parent.Document?.Root, element.Parent))
                        return false;
                    break;
                case ElementName_Local:
                case ElementName_Upstream:
                    if (!ReferenceEquals(element.Parent.Parent.Document?.Root, element.Parent.Parent))
                        return false;
                    break;
                default:
                    return false;
            }
            return element.Name.LocalName switch
            {
                ElementName_AbstractEntity or ElementName_GenericEntity1 or ElementName_GenericEntity2 or ElementName_Table => true,
                _ => false,
            };
        }

        public static EntityElement FindEntity(XDocument document, EntityType type, EntityScope scope, string name)
        {
            if (document is null) return null;
            string en = type switch
            {
                EntityType.Table => ElementName_Table,
                EntityType.GenericEntity1 => ElementName_GenericEntity1,
                EntityType.GenericEntity2 => ElementName_GenericEntity2,
                _ => ElementName_AbstractEntity,
            };
            EntityElement[] matching = document.Root.Elements(en).Attributes(AttributeName_Name).Where(a => a.Value == name).Take(1).Select(a => Get(a.Parent)).ToArray();
            if (matching.Length > 0) return matching[0];
            return scope switch
            {
                EntityScope.Local => document.Root.Elements(ElementName_Local).Elements(en).Attributes(AttributeName_Name).Where(a => a.Value == name).Take(1).Select(a => Get(a.Parent)).FirstOrDefault(),
                EntityScope.Upstream => document.Root.Elements(ElementName_Upstream).Elements(en).Attributes(AttributeName_Name).Where(a => a.Value == name).Take(1).Select(a => Get(a.Parent)).FirstOrDefault(),
                _ => null,
            };
        }

        public static EntityElement Get(XElement element)
        {
            if (Test(element))
                return element.Annotation<EntityElement>() ?? new EntityElement(element);
            return null;
        }
    }
}
