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
Set-Variable -Name 'TypeDbPath' -Option Constant -Scope 'Script' -Value ($PSScriptRoot | Join-Path -ChildPath "TypeDb.xml");
Set-Variable -Name 'TypeDbXml' -Option Constant -Scope 'Script' -Value ([Xml]::new());

enum ElementName {
    Namespace;
    Enum;
    Struct;
    Interface;
    Class;
    Base;
    Implements;
    GenericType;
}

enum AttributeName {
    Namespace;
    Name
}
class XmlBuilder {
    [System.Xml.XmlElement]$Element;
    [XmlBuilder] FindFirst([string]$ElementName) {
        $XmlElement = $this.Element.SelectSingleNode($([System.Xml.XmlConvert]::VerifyNCName($ElementName)));
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [XmlBuilder] AppendChild([string]$Name) { return [XmlBuilder]@{ Element = $this.Element.AppendChild($this.Element.OwnerDocument.CreateElement($([System.Xml.XmlConvert]::VerifyNCName($Name)))) }; }
    [System.Xml.XmlElement[]] FindAll([string]$ElementName) { return @($this.Element.SelectNodes($([System.Xml.XmlConvert]::VerifyNCName($ElementName)))); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [string]$Value) {
        $XmlElement = $null;
        if ([string]::IsNullOrEmpty($Value)) {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`"]");
        } else {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`"]");
        }
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [string]$Value) {
        if ([string]::IsNullOrEmpty($Value)) {
            return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`"]");
        }
        return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`"]");
    }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [string]$Value) {
        $XmlElement = $null;
        if ([string]::IsNullOrEmpty($Value)) {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`" or count(@$AttributeName)=0]");
        } else {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`" or count(@$AttributeName)=0]");
        }
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [string]$Value) {
        if ([string]::IsNullOrEmpty($Value)) {
            return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`" or count(@$AttributeName)=0]");
        }
        return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`" or count(@$AttributeName)=0]");
    }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [string]$Value) {
        $XmlElement = $null;
        if ([string]::IsNullOrEmpty($Value)) {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`")]");
        } else {
            $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`")]");
        }
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [string]$Value) {
        if ([string]::IsNullOrEmpty($Value)) {
            return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"`")]");
        }
        return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName))=`"$($Value.Replace('"', '&quot;'))`")]");
    }
    [XmlBuilder] FindFirstHas([string]$ElementName, [string]$AttributeName) {
        $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(count(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName)))=0)]");
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [System.Xml.XmlElement[]] FindAllHas([string]$ElementName, [string]$AttributeName) { return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[not(count(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName)))=0)]"); }
    [XmlBuilder] FindFirstDoesNotHave([string]$ElementName, [string]$AttributeName) {
        $XmlElement = $this.Element.SelectSingleNode("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[count(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName)))=0]");
        if ($null -eq $XmlElement) { return $null; }
        return [XmlBuilder]@{ Element = $XmlElement };
    }
    [System.Xml.XmlElement[]] FindAllDoesNotHave([string]$ElementName, [string]$AttributeName) { return $this.Element.SelectNodes("$([System.Xml.XmlConvert]::VerifyNCName($ElementName))[count(@$([System.Xml.XmlConvert]::VerifyNCName($AttributeName)))=0]"); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Boolean]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Char]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [SByte]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Byte]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Int16]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt16]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Int32]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt32]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Int64]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [UInt64]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Single]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Double]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Decimal]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [DateTime]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [Guid]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualTo([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindFirstEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualTo([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindAllEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindFirstEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllEqualToOrDoesNotHave([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindAllEqualToOrDoesNotHave($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [XmlBuilder] FindFirstNotEqualTo([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindFirstNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [System.Xml.XmlElement[]] FindAllNotEqualTo([string]$ElementName, [string]$AttributeName, [TimeSpan]$Value) { return $this.FindAllNotEqualTo($ElementName, $AttributeName, [System.Xml.XmlConvert]::ToString($Value)); }
    [void] RemoveAttribute([string]$Name) {
        $XmlAttribute = $this.Element.SelectSingleNode("@$([System.Xml.XmlConvert]::VerifyNCName($Name))");
        if ($null -ne $XmlAttribute) { $this.Element.Attributes.Remove($XmlAttribute); }
    }
    [string] GetString([string]$Attribute) {
        $XmlAttribute = $this.Element.SelectSingleNode("@$([System.Xml.XmlConvert]::VerifyNCName($Attribute))");
        if ($null -eq $XmlAttribute) { return $null }
        return $XmlAttribute.Value;
    }
    [string] GetString([string]$Attribute, [string]$DefaultValue) {
        $XmlAttribute = $this.Element.SelectSingleNode("@$([System.Xml.XmlConvert]::VerifyNCName($Attribute))");
        if ($null -eq $XmlAttribute) { return $DefaultValue }
        return $XmlAttribute.Value;
    }
    [void] SetString([string]$Attribute, [string]$Value) {
        $XmlAttribute = $this.Element.SelectSingleNode("@$([System.Xml.XmlConvert]::VerifyNCName($Attribute))");
        if ($null -eq $Value) { $Value = '' }
        if ($null -ne $XmlAttribute) {
            $XmlAttribute.Value = $Value;
        } else {
            $this.Element.Attributes.Append($this.Element.OwnerDocument.CreateAttribute($Attribute)).Value = $Value;
        }
    }
    [Nullable[Guid]] GetGuid([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToGuid($Value); } catch { }
        }
        return $null;
    }
    [Nullable[bool]] GetBoolean([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToBoolean($Value); } catch { }
        }
        return $null;
    }
    [bool] GetBoolean([string]$Attribute, [bool]$DefaultValue) {
        $Value = $this.GetBoolean($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetBoolean([string]$Attribute, [bool]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[char]] GetChar([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToChar($Value); } catch { }
        }
        return $null;
    }
    [char] GetChar([string]$Attribute, [char]$DefaultValue) {
        $Value = $this.GetChar($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetChar([string]$Attribute, [char]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[SByte]] GetSByte([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToSByte($Value); } catch { }
        }
        return $null;
    }
    [SByte] GetSByte([string]$Attribute, [SByte]$DefaultValue) {
        $Value = $this.GetSByte($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetSByte([string]$Attribute, [SByte]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[byte]] GetByte([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToByte($Value); } catch { }
        }
        return $null;
    }
    [byte] GetByte([string]$Attribute, [byte]$DefaultValue) {
        $Value = $this.GetByte($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetByte([string]$Attribute, [byte]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[Int16]] GetShort([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToInt16($Value); } catch { }
        }
        return $null;
    }
    [Int16] GetShort([string]$Attribute, [Int16]$DefaultValue) {
        $Value = $this.GetInt16($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetShort([string]$Attribute, [Int16]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[UInt16]] GetUShort([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToUInt16($Value); } catch { }
        }
        return $null;
    }
    [UInt16] GetUShort([string]$Attribute, [UInt16]$DefaultValue) {
        $Value = $this.GetUInt16($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetUShort([string]$Attribute, [UInt16]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[int]] GetInt([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToInt32($Value); } catch { }
        }
        return $null;
    }
    [int] GetInt([string]$Attribute, [int]$DefaultValue) {
        $Value = $this.GetInt32($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetInt([string]$Attribute, [int]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[UInt32]] GetUInt([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToUInt32($Value); } catch { }
        }
        return $null;
    }
    [UInt32] GetUInt([string]$Attribute, [UInt32]$DefaultValue) {
        $Value = $this.GetUInt32($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetUInt([string]$Attribute, [UInt32]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[Int64]] GetLong([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToInt64($Value); } catch { }
        }
        return $null;
    }
    [Int64] GetLong([string]$Attribute, [Int64]$DefaultValue) {
        $Value = $this.GetInt64($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetLong([string]$Attribute, [Int64]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[UInt64]] GetULong([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToUInt64($Value); } catch { }
        }
        return $null;
    }
    [UInt64] GetULong([string]$Attribute, [UInt64]$DefaultValue) {
        $Value = $this.GetUInt64($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetULong([string]$Attribute, [UInt64]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[Single]] GetFloat([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToSingle($Value); } catch { }
        }
        return $null;
    }
    [Single] GetFloat([string]$Attribute, [Single]$DefaultValue) {
        $Value = $this.GetSingle($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetFloat([string]$Attribute, [Single]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[double]] GetDouble([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToDouble($Value); } catch { }
        }
        return $null;
    }
    [double] GetDouble([string]$Attribute, [double]$DefaultValue) {
        $Value = $this.GetDouble($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetDouble([string]$Attribute, [double]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[decimal]] GetDecimal([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToDecimal($Value); } catch { }
        }
        return $null;
    }
    [decimal] GetDecimal([string]$Attribute, [decimal]$DefaultValue) {
        $Value = $this.GetDecimal($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetDecimal([string]$Attribute, [decimal]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[DateTime]] GetDateTime([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToDateTime($Value); } catch { }
        }
        return $null;
    }
    [DateTime] GetDateTime([string]$Attribute, [DateTime]$DefaultValue) {
        $Value = $this.GetDateTime($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetDateTime([string]$Attribute, [DateTime]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Guid] GetGuid([string]$Attribute, [Guid]$DefaultValue) {
        $Value = $this.GetGuid($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetGuid([string]$Attribute, [Guid]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
    [Nullable[TimeSpan]] GetTimeSpan([string]$Attribute) {
        $Value = $this.GetString($Attribute);
        if ($null -ne $Value -and ($Value = $Value.Trim()).Length -gt 0) {
            try { return [System.Xml.XmlConvert]::ToTimeSpan($Value); } catch { }
        }
        return $null;
    }
    [TimeSpan] GetTimeSpan([string]$Attribute, [TimeSpan]$DefaultValue) {
        $Value = $this.GetTimeSpan($Attribute);
        if ($null -ne $Value) { return $Value }
        return $DefaultValue;
    }
    [void] SetTimeSpan([string]$Attribute, [TimeSpan]$Value) { SetString($Attribute, [System.Xml.XmlConvert]::ToString($Value)); }
<#
$TypeNames | % {
    $t = $n = $_;
    switch ($_) {
        'Boolean' {
            $t = 'bool';
            break;
        }
        'Int16' {
            $n = 'Short';
            break;
        }
        'UInt16' {
            $n = 'UShort';
            break;
        }
        'Int32' {
            $t = 'int';
            $n = 'Int';
            break;
        }
        'UInt32' {
            $n = 'UInt';
            break;
        }
        'Int64' {
            $n = 'Long';
            break;
        }
        'UInt64' {
            $n = 'ULong';
            break;
        }
        'Single' {
            $n = 'Float';
            break;
        }
        'Double' {
            $t = 'double';
            break;
        }
        'Decimal' {
            $t = 'decimal';
            break;
        }
        'Byte' {
            $t = 'byte';
            break;
        }
        'String' {
            $t = 'string';
            break;
        }
        'Char' {
            $t = 'char';
            break;
        }
    }
    @"
[Nullable[$t]] Get$n([string]`$Attribute) {
    `$Value = `$this.GetString(`$Attribute);
    if (`$null -ne `$Value -and (`$Value = `$Value.Trim()).Length -gt 0) {
        try { return [System.Xml.XmlConvert]::To$_(`$Value); } catch { }
    }
    return `$null;
}
[$t] Get$n([string]`$Attribute, [$t]`$DefaultValue) {
    `$Value = `$this.Get$_(`$Attribute);
    if (`$null -ne `$Value) { return `$Value }
    return `$DefaultValue;
}
[void] Set$n([string]`$Attribute, [$t]`$Value) { SetString(`$Attribute, [System.Xml.XmlConvert]::ToString(`$Value)); }
"@ }
#>
}

Function Add-TypeReference {
    [CmdletBinding()]
    Param (
        # InputType
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ ($_.IsGenericTypeDefinition -or -not $_.IsGenericType) -and -not ($_.IsGenericParameter -or $_.IsArray -or $_.IsByRef) })]
        [Type]$InputType,

        [ValidateScript({[System.Xml.XmlConvert]::VerifyNCName($_) })]
        [string]$ElementName,

        [Parameter(Mandatory = $true)]
        [XmlBuilder]$Parent
    )
    $RefBuilder = $Parent.AppendChild($ElementName);
    $TypeBuilder = $null;
    if ($InputType.IsGenericType -and -not $InputType.IsGenericTypeDefinition) {
        $TypeBuilder = Import-ClrType -InputType $InputType.GetGenericTypeDefinition() -PassThru;
        $InputType.GetGenericArguments() | Add-GenericTypeArguments -Parent $RefBuilder;
    } else {
        $TypeBuilder = Import-ClrType -InputType $InputType -PassThru;
    }
    $RefBuilder.SetString('Name', $TypeBuilder.GetString('Name'));
    $RefBuilder.SetString('Namespace', $BaseBuTypeBuilderilder.Element.Parent.SelectSingleNode("Name").Value);
}

Function Import-PropertyInfo {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.PropertyInfo]$PropertyInfo,

        [Parameter(Mandatory = $true)]
        [XmlBuilder]$Parent,

        [switch]$Force,

        [switch]$PassThru
    )

    Process {
        $PropertyBuilder = $Parent.FindFirstEqualTo('Property', 'Name', $PropertyInfo.Name);
        if ($Force.IsPresent -and $null -ne $PropertyBuilder) {
            $Parent.Element.ChildNodes.Remove($PropertyBuilder.Element) | Out-Null;
            $PropertyBuilder = $null;
        }
        if ($null -eq $PropertyBuilder) {
            $PropertyBuilder = $NsBuilder.AppendChild('Property');
            $PropertyBuilder.SetString('Name', $PropertyInfo.Name);
            if ($PropertyInfo.CanRead) {
                if (-not $PropertyInfo.CanWrite) { $PropertyBuilder.SetBoolean('CanWrite', $false) }
            } else {
                if ($PropertyInfo.CanWrite) {
                    $PropertyBuilder.SetBoolean('CanRead', $false);
                } else {
                    if ($null -eq $PropertyInfo.GetGetMethod()) { $PropertyBuilder.SetBoolean('CanRead', $false) }
                    $PropertyBuilder.SetBoolean('CanWrite', $false);
                }
            }
            $RefBuilder = $Parent.AppendChild('Type');
            $TypeBuilder = $null;
            if ($PropertyInfo.PropertyType.IsGenericType -and -not $PropertyInfo.PropertyType.IsGenericTypeDefinition) {
                $TypeBuilder = Import-ClrType -InputType $PropertyInfo.PropertyType.GetGenericTypeDefinition() -PassThru;
                $PropertyInfo.PropertyType.GetGenericArguments() | Add-GenericTypeArguments -Parent $RefBuilder;
            } else {
                $TypeBuilder = Import-ClrType -InputType $PropertyInfo.PropertyType -PassThru;
            }
            $RefBuilder.SetString('Name', $TypeBuilder.GetString('Name'));
            $RefBuilder.SetString('Namespace', $BaseBuTypeBuilderilder.Element.Parent.SelectSingleNode("Name").Value);
            switch ($TypeBuilder.Element.LocalName) {
                'Enum' { break; }
                'Struct' {
                    if ($TypeBuilder.GetBoolean('IsNullable', $false)) { $PropertyBuilder.SetBoolean('IsNullAssignable', $true) }
                    break;
                }
                Default {
                    $PropertyBuilder.SetBoolean('IsNullAssignable', $true);
                    break;
                }
            }
        }

        if ($PassThru.IsPresent) { $PropertyBuilder | Write-Output }
    }
}

Function Add-GenericTypeArguments {
    [CmdletBinding()]
    Param (
        # InputType
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ ($_.IsGenericTypeDefinition -or -not $_.IsGenericType) -and -not ($_.IsGenericParameter -or $_.IsArray -or $_.IsByRef) })]
        [Type]$InputType,

        [Parameter(Mandatory = $true)]
        [XmlBuilder]$Parent
    )
}

Function Import-ClrType {
    [CmdletBinding()]
    Param (
        # InputType
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ ($_.IsGenericTypeDefinition -or -not $_.IsGenericType) -and -not ($_.IsGenericParameter -or $_.IsArray -or $_.IsByRef) })]
        [Type]$InputType,

        [switch]$Force,

        [switch]$PassThru
    )

    Process {
        $ns = $InputType.Namespace;
        if ($null -eq $ns) { $ns = '' }
        $NsBuilder = $Script:RootBuilder.FindFirstEqualTo('Namespace', 'Name', $ns);
        if ($null -eq $NsBuilder) {
            $NsBuilder = $Script:RootBuilder.AppendChild('Namespace');
            $NsBuilder.SetString('Name', $ns);
        }
        $ElementName = 'Class';
        if ($InputType.IsEnum) {
            $ElementName = 'Enum';
        } else {
            if ($InputType.IsValueType) {
                $ElementName = 'Struct';
            } else {
                if ($InputType.IsInterface) { $ElementName = 'Interface' }
            }
        }
        $TypeBuilder = $NsBuilder.FindFirstEqualTo($ElementName, 'Name', $InputType.Name);
        if ($Force.IsPresent -and $null -ne $TypeBuilder) {
            $NsBuilder.Element.ChildNodes.Remove($TypeBuilder.Element) | Out-Null;
            $TypeBuilder = $null;
        }
        if ($null -eq $TypeBuilder) {
            $TypeBuilder = $NsBuilder.AppendChild($ElementName);
            $TypeBuilder.SetString('Name', $InputType.Name);
            if ($InputType.IsValueType -and $InputType.IsGenericType() -and $InputType.GetGenericTypeDefinition().Equals([Nullable`1])) { $TypeBuilder.SetBoolean('IsNullable', $true) }
            if ($InputType.IsGenericTypeDefinition()) {
                $InputType.GetGenericArguments() | Add-GenericTypeArguments -Parent $TypeBuilder;
            }
            if ($ElementName -eq 'Class' -and $null -ne $InputType.BaseType) { $InputType.BaseType | Add-TypeReference -Parent $TypeBuilder -ElementName 'BaseType' }
            if ($ElementName -ne 'Enum') {
                $InputType.GetInterfaces() | Add-TypeReference -Parent $TypeBuilder -ElementName 'Implements';
                $InputType.GetProperties() | Import-PropertyInfo -Parent $TypeBuilder;
            } else {
                $InputType.GetEnumUnderlyingType() | Add-TypeReference -Parent $TypeBuilder -ElementName 'UnderlyingType';
                $InputType.GetEnumNames() | ForEach-Object {
                    $Builder = $TypeBuilder.AppendChild('Field');
                    $Builder.SetString('Name', $_);
                }
            }
        }

        if ($PassThru.IsPresent) { $TypeBuilder | Write-Output }
    }
}

if ([System.IO.File]::Exists($Script:TypeDbPath)) {
    $Script:TypeDbXml.Load($Script:TypeDbPath);
    if ($null -eq $Script:TypeDbXml.DocumentElement) { return }
} else {
    $Script:TypeDbXml.AppendChild($Script:TypeDbXml.CreateElement('TypeDb')) | Out-Null;
}
Set-Variable -Name 'RootBuilder' -Option Constant -Scope 'Script' -Value ([XmlBuilder]::new($Script:TypeDbXml.DocumentElement));
