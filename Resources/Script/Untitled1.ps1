Add-Type -TypeDefinition @'
namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    public class TemplateFake
    {
        private XDocument _entityDefinitionsDocument;
        public XDocument EntityDefinitionsDocument { get { return _entityDefinitionsDocument; } }
        public TemplateFake(XDocument document)
        {
            _entityDefinitionsDocument = document;
        }
    
        private void GetAllBaseTypes(XElement entityElement, Collection<XElement> result, IEnumerable<XElement> scopeElements)
        {
            int start = result.Count;
            XAttribute attribute = entityElement.Elements("ExtendsEntity").Attributes("Type").FirstOrDefault();
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = scopeElements.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !(ReferenceEquals(entityElement, attribute.Parent) || result.Contains(attribute.Parent)))
                    result.Add(attribute.Parent);
            }
            attribute = entityElement.Elements("ExtendsGenericEntity").Attributes("TypeDef").FirstOrDefault();
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = scopeElements.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !(ReferenceEquals(entityElement, attribute.Parent) || result.Contains(attribute.Parent)))
                    result.Add(attribute.Parent);
            }
            attribute = entityElement.Attribute("RootInterface");
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = scopeElements.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !(ReferenceEquals(entityElement, attribute.Parent) || result.Contains(attribute.Parent)))
                    result.Add(attribute.Parent);
            }
            foreach (XAttribute attr in entityElement.Elements("ImplementsEntity").Attributes("Type"))
            {
                string name = attr.Value;
                if ((attribute = scopeElements.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !(ReferenceEquals(entityElement, attribute.Parent) || result.Contains(attribute.Parent)))
                    result.Add(attribute.Parent);
            }
            foreach (XAttribute attr in entityElement.Elements("ImplementsGenericEntity").Attributes("TypeDef"))
            {
                string name = attr.Value;
                if ((attribute = scopeElements.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !(ReferenceEquals(entityElement, attribute.Parent) || result.Contains(attribute.Parent)))
                    result.Add(attribute.Parent);
            }
            int end = result.Count;
            for (int i = start; i < end; i++)
                GetAllBaseTypes(result[i], result, scopeElements);
        }

        public Collection<XElement> GetAllBaseTypes(XElement entityElement)
        {
            Collection<XElement> result = new();
            switch (entityElement.Parent.Name.LocalName)
            {
                case "Local":
                    GetAllBaseTypes(entityElement, result, EntityDefinitionsDocument.Root.Elements("Local").Elements("Entity").Concat(EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity")));
                    break;
                case "Upstream":
                    GetAllBaseTypes(entityElement, result, EntityDefinitionsDocument.Root.Elements("Upstream").Elements("Entity").Concat(EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity")));
                    break;
                default:
                    GetAllBaseTypes(entityElement, result, EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity"));
                    break;
            }
            return result;
        }

        public IEnumerable<XElement> GetBaseTypes(XElement entityElement)
        {
            IEnumerable<XElement> source;
            switch (entityElement.Parent.Name.LocalName)
            {
                case "Local":
                    source = EntityDefinitionsDocument.Root.Elements("Local").Elements("Entity").Concat(EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity"));
                    break;
                case "Upstream":
                    source = EntityDefinitionsDocument.Root.Elements("Upstream").Elements("Entity").Concat(EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity"));
                    break;
                default:
                    source = EntityDefinitionsDocument.Root.Elements("Root").Elements("Entity");
                    break;
            }
            XAttribute attribute = entityElement.Elements("ExtendsEntity").Attributes("Type").FirstOrDefault();
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = source.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !ReferenceEquals(entityElement, attribute.Parent))
                    yield return attribute.Parent;
            }
            attribute = entityElement.Elements("ExtendsGenericEntity").Attributes("TypeDef").FirstOrDefault();
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = source.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !ReferenceEquals(entityElement, attribute.Parent))
                    yield return attribute.Parent;
            }
            attribute = entityElement.Attribute("RootInterface");
            if (attribute is not null)
            {
                string name = attribute.Value;
                if ((attribute = source.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !ReferenceEquals(entityElement, attribute.Parent))
                    yield return attribute.Parent;
            }
            foreach (XAttribute attr in entityElement.Elements("ImplementsEntity").Attributes("Type"))
            {
                string name = attr.Value;
                if ((attribute = source.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !ReferenceEquals(entityElement, attribute.Parent))
                    yield return attribute.Parent;
            }
            foreach (XAttribute attr in entityElement.Elements("ImplementsGenericEntity").Attributes("TypeDef"))
            {
                string name = attr.Value;
                if ((attribute = source.Attributes("Name").Where(a => a.Value == name).FirstOrDefault()) is not null && !ReferenceEquals(entityElement, attribute.Parent))
                    yield return attribute.Parent;
            }
        }
    }
}
'@ -ReferencedAssemblies 'System.Xml', 'System.Linq', 'System.Xml.Linq' -ErrorAction Stop;
$Path = 'c:\users\lerwi\git\fsinfocat\src\FsInfoCat\Resources\EntityDefinitions.xml';
$EmptyNamespace = [System.Xml.Linq.XNamespace]::Get('');
$XDocument = [System.Xml.Linq.XDocument]::Load($Path);
$TemplateFake = [Test.TemplateFake]::new($XDocument);
$EntityElement = @($XDocument.Root.Elements($EmptyNamespace.GetName('Local')).Elements($EmptyNamespace.GetName('Entity'))) | Select-Object -First 1;
$null -ne $EntityElement;
<#
Float
NewIdNavRef
NewRelatedEntity
NewCollectionNavigation

ByteArray
ByteValues

@TypeParam => typeparam/@cref
IsDefaultNull
IsNullable


typeparam/@cref => @TypeDef
#>