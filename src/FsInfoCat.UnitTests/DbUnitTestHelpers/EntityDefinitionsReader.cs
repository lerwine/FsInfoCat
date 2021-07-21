using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace FsInfoCat.UnitTests.DbUnitTestHelpers
{
    public class EntityDefinitionsReader
    {
        static readonly Regex NewLineRegex = new(@"\r\n?|[\n\p{Zl}\p{Zp}]", RegexOptions.Compiled);
        static readonly Regex NormalizeWsRegex = new(@" ((?![\r\n])\s)*|(?! )((?![\r\n])\s)+", RegexOptions.Compiled);
        static readonly Regex NormalizeNewLineRegex = new(@"[\v\t\p{Zl}\p{Zp}]|\r(?!\n)", RegexOptions.Compiled);
        static readonly Regex TrimOuterBlankLinesRegex = new(@"^(\s*(\r\n?|\n))+|((\r\n?|\n)\s*)+$", RegexOptions.Compiled);
        static readonly Regex StripWsRegex = new(@"^ [ \t\u0085\p{Zs}]+(?=[\r\n\v\t\p{Zl}\p{Zp}])|(?<=[\r\n\v\t\p{Zl}\p{Zp}])[ \t\u0085\p{Zs}]+", RegexOptions.Compiled);
        static readonly Regex LeadingWsRegex = new(@"^\s+", RegexOptions.Compiled);
        static readonly Regex LeadingEmptyLine = new(@"^([^\r\n\S]+)?(\r\n?|\n)", RegexOptions.Compiled);
        static readonly Regex TrailingEmptyLine = new(@"(\r\n?|\n)([^\r\n\S]+)?$", RegexOptions.Compiled);
        static readonly Regex TrailingWsRegex = new(@"\s+$", RegexOptions.Compiled);
        const string NAME_Root = "Root";
        const string NAME_Upstream = "Upstream";
        const string NAME_Local = "Local";
        const string NAME_Entity = "Entity";
        const string NAME_Enum = "Enum";
        const string NAME_Name = "Name";
        const string NAME_FullName = "FullName";
        const string NAME_Field = "Field";
        const string NAME_CollectionNavigation = "CollectionNavigation";
        const string NAME_NewCollectionNavigation = "NewCollectionNavigation";
        const string NAME_RelatedEntity = "RelatedEntity";
        const string NAME_NewRelatedEntity = "NewRelatedEntity";
        const string NAME_ItemType = "ItemType";
        const string NAME_Reference = "Reference";
        const string NAME_AmbientEnum = "AmbientEnum";
        const string NAME_Default = "Default";
        const string NAME_Value = "Value";
        const string NAME_EnumTypes = "EnumTypes";
        const string NAME_Properties = "Properties";
        const string NAME_ExtendsEntity = "ExtendsEntity";
        const string NAME_ExtendsGenericEntity = "ExtendsGenericEntity";
        const string NAME_Implements = "Implements";
        const string NAME_ImplementsEntity = "ImplementsEntity";
        const string NAME_ImplementsGenericEntity = "ImplementsGenericEntity";
        const string NAME_RootInterface = "RootInterface";
        const string NAME_Type = "Type";
        const string NAME_TypeDef = "TypeDef";
        const string NAME_PrimaryKey = "PrimaryKey";
        const string NAME_ForeignKey = "ForeignKey";
        const string NAME_AmbientBoolean = "AmbientBoolean";
        const string NAME_AmbientInt = "AmbientInt";
        const string NAME_AmbientByte = "AmbientByte";
        const string NAME_AmbientSByte = "AmbientSByte";
        const string NAME_AmbientShort = "AmbientShort";
        const string NAME_AmbientUShort = "AmbientUShort";
        const string NAME_AmbientFloat = "AmbientFloat";
        const string NAME_AmbientDouble = "AmbientDouble";
        const string NAME_summary = "summary";
        const string NAME_remarks = "remarks";
        const string NAME_seealso = "seealso";
        const string NAME_IsFlags = "IsFlags";
        const string NAME_typeparam = "typeparam";
        const string NAME_cref = "cref";
        const string NAME_NewIdNavRef = "NewIdNavRef";
        const string NAME_UniqueIdentifier = "UniqueIdentifier";
        const string NAME_value = "value";
        const string NAME_DefaultNull = "DefaultNull";
        const string NAME_AllowNull = "AllowNull";
        const string NAME_IsGenericWritable = "IsGenericWritable";
        const string NAME_DisplayNameResource = "DisplayNameResource";
        const string NAME_DescriptionResource = "DescriptionResource";
        const string NAME_ResourceType = "ResourceType";
        const string NAME_TableName = "TableName";
        const string NAME_ColName = "ColName";
        const string NAME_Byte = "Byte";
        const string NAME_SByte = "SByte";
        const string NAME_ByteArray = "ByteArray";
        const string NAME_MultiStringValue = "MultiStringValue";
        const string NAME_MD5Hash = "MD5Hash";
        const string NAME_ByteValues = "ByteValues";
        const string NAME_Short = "Short";
        const string NAME_UShort = "UShort";
        const string NAME_Int = "Int";
        const string NAME_UInt = "UInt";
        const string NAME_Long = "Long";
        const string NAME_ULong = "ULong";
        const string NAME_Double = "Double";
        const string NAME_Float = "Float";
        const string NAME_Decimal = "Decimal";
        const string NAME_NVarChar = "NVarChar";
        const string NAME_Char = "Char";
        const string NAME_DateTime = "DateTime";
        const string NAME_TimeSpan = "TimeSpan";
        const string NAME_Bit = "Bit";
        const string NAME_Text = "Text";
        const string NAME_VolumeIdentifier = "VolumeIdentifier";
        const string NAME_DriveType = "DriveType";
        const string NAME_MaxLength = "MaxLength";
        const string NAME_CreatedOn = "CreatedOn";
        const string NAME_ModifiedOn = "ModifiedOn";
        const string NAME_UpstreamId = "UpstreamId";
        const string NAME_LastSynchronizedOn = "LastSynchronizedOn";
        static readonly XName XNAME_Root = XName.Get(NAME_Root);
        static readonly XName XNAME_Upstream = XName.Get(NAME_Upstream);
        static readonly XName XNAME_Local = XName.Get(NAME_Local);
        static readonly XName XNAME_Entity = XName.Get(NAME_Entity);
        static readonly XName XNAME_Enum = XName.Get(NAME_Enum);
        static readonly XName XNAME_Name = XName.Get(NAME_Name);
        static readonly XName XNAME_FullName = XName.Get(NAME_FullName);
        static readonly XName XNAME_Field = XName.Get(NAME_Field);
        static readonly XName XNAME_CollectionNavigation = XName.Get(NAME_CollectionNavigation);
        static readonly XName XNAME_NewCollectionNavigation = XName.Get(NAME_NewCollectionNavigation);
        static readonly XName XNAME_RelatedEntity = XName.Get(NAME_RelatedEntity);
        static readonly XName XNAME_NewRelatedEntity = XName.Get(NAME_NewRelatedEntity);
        static readonly XName XNAME_ItemType = XName.Get(NAME_ItemType);
        static readonly XName XNAME_Reference = XName.Get(NAME_Reference);
        static readonly XName XNAME_AmbientEnum = XName.Get(NAME_AmbientEnum);
        static readonly XName XNAME_Default = XName.Get(NAME_Default);
        static readonly XName XNAME_Value = XName.Get(NAME_Value);
        static readonly XName XNAME_EnumTypes = XName.Get(NAME_EnumTypes);
        static readonly XName XNAME_Properties = XName.Get(NAME_Properties);
        static readonly XName XNAME_ExtendsEntity = XName.Get(NAME_ExtendsEntity);
        static readonly XName XNAME_ExtendsGenericEntity = XName.Get(NAME_ExtendsGenericEntity);
        static readonly XName XNAME_Implements = XName.Get(NAME_Implements);
        static readonly XName XNAME_ImplementsEntity = XName.Get(NAME_ImplementsEntity);
        static readonly XName XNAME_ImplementsGenericEntity = XName.Get(NAME_ImplementsGenericEntity);
        static readonly XName XNAME_RootInterface = XName.Get(NAME_RootInterface);
        static readonly XName XNAME_Type = XName.Get(NAME_Type);
        static readonly XName XNAME_TypeDef = XName.Get(NAME_TypeDef);
        static readonly XName XNAME_PrimaryKey = XName.Get(NAME_PrimaryKey);
        static readonly XName XNAME_ForeignKey = XName.Get(NAME_ForeignKey);
        static readonly XName XNAME_AmbientBoolean = XName.Get(NAME_AmbientBoolean);
        static readonly XName XNAME_AmbientInt = XName.Get(NAME_AmbientInt);
        static readonly XName XNAME_AmbientByte = XName.Get(NAME_AmbientByte);
        static readonly XName XNAME_AmbientSByte = XName.Get(NAME_AmbientSByte);
        static readonly XName XNAME_AmbientShort = XName.Get(NAME_AmbientShort);
        static readonly XName XNAME_AmbientUShort = XName.Get(NAME_AmbientUShort);
        static readonly XName XNAME_AmbientFloat = XName.Get(NAME_AmbientFloat);
        static readonly XName XNAME_AmbientDouble = XName.Get(NAME_AmbientDouble);
        static readonly XName XNAME_summary = XName.Get(NAME_summary);
        static readonly XName XNAME_remarks = XName.Get(NAME_remarks);
        static readonly XName XNAME_seealso = XName.Get(NAME_seealso);
        static readonly XName XNAME_IsFlags = XName.Get(NAME_IsFlags);
        static readonly XName XNAME_typeparam = XName.Get(NAME_typeparam);
        static readonly XName XNAME_cref = XName.Get(NAME_cref);
        static readonly XName XNAME_NewIdNavRef = XName.Get(NAME_NewIdNavRef);
        static readonly XName XNAME_UniqueIdentifier = XName.Get(NAME_UniqueIdentifier);
        static readonly XName XNAME_value = XName.Get(NAME_value);
        static readonly XName XNAME_DefaultNull = XName.Get(NAME_DefaultNull);
        static readonly XName XNAME_AllowNull = XName.Get(NAME_AllowNull);
        static readonly XName XNAME_IsGenericWritable = XName.Get(NAME_IsGenericWritable);
        static readonly XName XNAME_DisplayNameResource = XName.Get(NAME_DisplayNameResource);
        static readonly XName XNAME_DescriptionResource = XName.Get(NAME_DescriptionResource);
        static readonly XName XNAME_ResourceType = XName.Get(NAME_ResourceType);
        static readonly XName XNAME_TableName = XName.Get(NAME_TableName);
        static readonly XName XNAME_ColName = XName.Get(NAME_ColName);
        static readonly XName XNAME_Byte = XName.Get(NAME_Byte);
        static readonly XName XNAME_SByte = XName.Get(NAME_SByte);
        static readonly XName XNAME_ByteArray = XName.Get(NAME_ByteArray);
        static readonly XName XNAME_MultiStringValue = XName.Get(NAME_MultiStringValue);
        static readonly XName XNAME_MD5Hash = XName.Get(NAME_MD5Hash);
        static readonly XName XNAME_ByteValues = XName.Get(NAME_ByteValues);
        static readonly XName XNAME_Short = XName.Get(NAME_Short);
        static readonly XName XNAME_UShort = XName.Get(NAME_UShort);
        static readonly XName XNAME_Int = XName.Get(NAME_Int);
        static readonly XName XNAME_UInt = XName.Get(NAME_UInt);
        static readonly XName XNAME_Long = XName.Get(NAME_Long);
        static readonly XName XNAME_ULong = XName.Get(NAME_ULong);
        static readonly XName XNAME_Double = XName.Get(NAME_Double);
        static readonly XName XNAME_Float = XName.Get(NAME_Float);
        static readonly XName XNAME_Decimal = XName.Get(NAME_Decimal);
        static readonly XName XNAME_NVarChar = XName.Get(NAME_NVarChar);
        static readonly XName XNAME_Char = XName.Get(NAME_Char);
        static readonly XName XNAME_DateTime = XName.Get(NAME_DateTime);
        static readonly XName XNAME_TimeSpan = XName.Get(NAME_TimeSpan);
        static readonly XName XNAME_Bit = XName.Get(NAME_Bit);
        static readonly XName XNAME_Text = XName.Get(NAME_Text);
        static readonly XName XNAME_VolumeIdentifier = XName.Get(NAME_VolumeIdentifier);
        static readonly XName XNAME_DriveType = XName.Get(NAME_DriveType);
        static readonly XName XNAME_MaxLength = XName.Get(NAME_MaxLength);
        static readonly XName XNAME_CreatedOn = XName.Get(NAME_CreatedOn);
        static readonly XName XNAME_ModifiedOn = XName.Get(NAME_ModifiedOn);
        static readonly XName XNAME_UpstreamId = XName.Get(NAME_UpstreamId);
        static readonly XName XNAME_LastSynchronizedOn = XName.Get(NAME_LastSynchronizedOn);

        XDocument EntityDefinitionsDocument
        {
            get
            {
                if (_document is Exception exception)
                    throw new AssertInconclusiveException(string.IsNullOrWhiteSpace(exception.Message) ? $"An unexpected {exception.GetType().Name} has occurred" : exception.Message, exception);
                return (XDocument)_document;
            }
        }

        static readonly EntityDefinitionsReader Instance = new();
        private readonly object _document;
        //private readonly ReadOnlyCollection<(string Selector, ReadOnlyCollection<string> Fields)> _keys;
        private EntityDefinitionsReader()
        {
            Collection<(string Selector, ReadOnlyCollection<string> Field)> keys = new();
            object document;
            try
            {
                if (TestHelper.ProjectDirectory is null)
                    throw new InvalidOperationException($"Could not find parent directory with the same name as the test assembly ({System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}). Ensure that the subdirectory for the current test project is named accordingly.");
                string resourcesDirectory = Path.Combine(Path.GetDirectoryName(TestHelper.ProjectDirectory), typeof(BaseDbContext).Namespace, "Resources");
                string path = Path.Combine(resourcesDirectory, "EntityDefinitions.xsd");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Could not find entity configuration schema file {path}", path);
                XmlReaderSettings readerSettings = new() { ValidationType = ValidationType.Schema };
                //XmlSchema schema = readerSettings.Schemas.Add("", path);
                readerSettings.Schemas.Add("", path);
                Collection<ValidationEventArgs> validationArgs = new();
                readerSettings.ValidationEventHandler += new ValidationEventHandler((object sender, ValidationEventArgs e) => validationArgs.Add(e));
                path = Path.Combine(resourcesDirectory, "EntityDefinitions.xml");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Could not find entity configuration schema file {path}", path);
                using XmlReader reader = XmlReader.Create(path, readerSettings);
                XDocument d = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
                if (d.Root is null)
                    throw new InvalidDataException("Not root element found.");
                document = d;
                //Collection<XmlSchemaKey> keyElements = new();
                //GetAllSchemaKeys(schema, keyElements);
                //foreach (XmlSchemaKey key in keyElements)
                //{
                //    string selector = key.Selector?.XPath;
                //    if (string.IsNullOrEmpty(selector))
                //        continue;
                //    string[] fields = key.Fields.OfType<XmlSchemaXPath>().Select(f => f.XPath).Where(f => !(string.IsNullOrEmpty(f) || f.Contains('/'))).ToArray();
                //    if (fields.Length > 0)
                //        keys.Add(new(selector, new ReadOnlyCollection<string>(fields)));
                //}
            }
            catch (Exception exception)
            {
                document = exception;
            }
            //finally
            //{
            //    _keys = new(keys);
            //}
            _document = document;
        }

        //static void GetAllSchemaKeys(XmlSchemaObject obj, Collection<XmlSchemaKey> keyCollection)
        //{
        //    if (obj is null)
        //        return;
        //    if (obj is XmlSchema schema)
        //    {
        //        foreach (XmlSchemaElement element in schema.Elements.Values)
        //            GetAllSchemaKeys(element, keyCollection);
        //        foreach (XmlSchemaComplexType complexType in schema.SchemaTypes.Values.OfType<XmlSchemaComplexType>())
        //        {
        //            GetAllSchemaKeys(complexType.Particle, keyCollection);
        //            GetAllSchemaKeys(complexType.ContentModel?.Content, keyCollection);
        //        }
        //        foreach (XmlSchemaObject o in schema.Groups.Values)
        //            GetAllSchemaKeys(o, keyCollection);
        //    }
        //    else if (obj is XmlSchemaGroupBase groupBase)
        //    {
        //        foreach (XmlSchemaObject o in groupBase.Items)
        //            GetAllSchemaKeys(o, keyCollection);
        //    }
        //    else if (obj is XmlSchemaElement element)
        //    {
        //        foreach (XmlSchemaKey key in element.Constraints.OfType<XmlSchemaKey>())
        //            keyCollection.Add(key);
        //        if (element.ElementSchemaType is XmlSchemaComplexType complexType)
        //        {
        //            GetAllSchemaKeys(complexType.Particle, keyCollection);
        //            GetAllSchemaKeys(complexType.ContentModel?.Content, keyCollection);
        //        }
        //    }
        //    else if (obj is XmlSchemaComplexContentRestriction complexContentRestriction)
        //        GetAllSchemaKeys(complexContentRestriction.Particle, keyCollection);
        //    else if (obj is XmlSchemaComplexContentExtension complexContentExtension)
        //        GetAllSchemaKeys(complexContentExtension.Particle, keyCollection);
        //}

        XElement FindRootEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Root)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootByName(string name) { return (name is null) ? null : (FindRootEntityByName(name) ?? FindRootEnumByName(name)); }

        XElement FindLocalEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindLocalEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Local)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindLocalByName(string name) { return (name is null) ? null : (FindLocalEntityByName(name) ?? FindLocalEnumByName(name)); }

        XElement FindUpstreamEntityByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_Entity).Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEntityByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamEnumByName(string name)
        {
            if (name is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root?.Element(XNAME_Upstream)?.Elements(XNAME_EnumTypes).Elements().Attributes(XNAME_Name).FirstOrDefault(a => a.Value == name);
            if (matchingAttribute is null)
                return FindRootEnumByName(name);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamByName(string name) { return (name is null) ? null : (FindUpstreamEntityByName(name) ?? FindUpstreamEnumByName(name)); }

        XElement FindRootPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Root).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Root).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return null;
            return matchingAttribute.Parent;
        }

        XElement FindRootMemberByFullName(string fullName) { return (fullName is null) ? null : (FindRootPropertyByFullName(fullName) ?? FindRootFieldByFullName(fullName)); }

        XElement FindLocalPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Local).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootPropertyByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindLocalFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Local).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootFieldByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindLocalMemberByFullName(string fullName) { return (fullName is null) ? null : (FindLocalPropertyByFullName(fullName) ?? FindLocalFieldByFullName(fullName)); }

        XElement FindUpstreamPropertyByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Upstream).Elements(XNAME_Entity).Elements(XNAME_Properties).Elements().Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootPropertyByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamFieldByFullName(string fullName)
        {
            if (fullName is null)
                return null;
            XAttribute matchingAttribute = EntityDefinitionsDocument.Root.Element(XNAME_Upstream).Elements(XNAME_EnumTypes).Elements().Elements(XNAME_Field).Attributes(XNAME_FullName)
                .FirstOrDefault(a => a.Value == fullName);
            if (matchingAttribute is null)
                return FindRootFieldByFullName(fullName);
            return matchingAttribute.Parent;
        }

        XElement FindUpstreamMemberByFullName(string fullName) { return (fullName is null) ? null : (FindUpstreamPropertyByFullName(fullName) ?? FindUpstreamFieldByFullName(fullName)); }

        XElement GetEnumPropertyEnumType(XElement enumPropertyElement)
        {
            XElement parent = enumPropertyElement?.Parent;
            if (parent is null || enumPropertyElement.Name != XNAME_Enum)
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value),
                NAME_Local => FindLocalEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value),
                NAME_Upstream => FindUpstreamEnumByName(enumPropertyElement.Attribute(XNAME_Name)?.Value),
                _ => null,
            };
        }

        XElement GetEnumPropertyDefaultField(XElement enumPropertyElement)
        {
            XElement parent = enumPropertyElement?.Parent;
            if (parent is null || enumPropertyElement.Name != XNAME_Enum)
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value),
                NAME_Local => FindLocalFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value),
                NAME_Upstream => FindUpstreamFieldByFullName(enumPropertyElement.Element(XNAME_Default)?.Value),
                _ => null,
            };
        }

        XElement GetEnumFieldAmbientEnumField(XElement enumFieldElement)
        {
            XElement parent = enumFieldElement?.Parent;
            if (parent is null || enumFieldElement.Name != XNAME_Enum)
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault()),
                NAME_Local => FindLocalFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault()),
                NAME_Upstream => FindUpstreamFieldByFullName(enumFieldElement.Elements(XNAME_AmbientEnum).Attributes(XNAME_Value).Select(a => a.Value).FirstOrDefault()),
                _ => null,
            };
        }

        XElement GetRelatedEntityReferenceEntity(XElement relatedEntityPropertyElement)
        {
            XElement parent = relatedEntityPropertyElement?.Parent;
            if (parent is null || (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootEntityByName(relatedEntityPropertyElement.Attribute(XNAME_Reference)?.Value),
                NAME_Local => FindLocalEntityByName(relatedEntityPropertyElement.Attribute(XNAME_Reference)?.Value),
                NAME_Upstream => FindUpstreamEntityByName(relatedEntityPropertyElement.Attribute(XNAME_Reference)?.Value),
                _ => null,
            };
        }

        XElement GetCollectionNavigationItemEntity(XElement collectionNavigationPropertyElement)
        {
            XElement parent = collectionNavigationPropertyElement?.Parent;
            if (parent is null || (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation && collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootEntityByName(collectionNavigationPropertyElement.Attribute(XNAME_ItemType)?.Value),
                NAME_Local => FindLocalEntityByName(collectionNavigationPropertyElement.Attribute(XNAME_ItemType)?.Value),
                NAME_Upstream => FindUpstreamEntityByName(collectionNavigationPropertyElement.Attribute(XNAME_ItemType)?.Value),
                _ => null,
            };
        }

        record CodeValues(string SQL, string CLR);
        interface IColumnConstraint { }
        record ColumnDefinition(string Name, bool IsPrimaryKey, CodeValues DefaultValue, ReadOnlyCollection<IColumnConstraint> Constraints, PropertyDefinition Property, XElement Source);
        record ForeignKeyConstraint(string Name, string TableName, string KeyName) : IColumnConstraint;
        record RangeColumnConstraint(CodeValues MaxValue, CodeValues MinValue) : IColumnConstraint;
        record LengthColumnConstraint(CodeValues MaxLength, CodeValues MinLength) : IColumnConstraint;
        record TableDefinition(string Name, ReadOnlyCollection<ColumnDefinition> Columns, XElement Source);

        record PropertyDefinition(string Name, string ClrType, bool AllowsNull, bool IsGenericWritable, ReadOnlyCollection<PropertyDefinition> Base, XElement Source);

        record EntityDefinition(string Name, TableDefinition Table, ReadOnlyCollection<PropertyDefinition> Properties, ReadOnlyCollection<EntityDefinition> BaseDefinitions, ReadOnlyCollection<string> BaseTypeNames, XElement Source);

        private static readonly ReadOnlyCollection<XName> NameKeyed = new(new[] { XNAME_Entity, XNAME_Field, XNAME_Byte, XNAME_SByte, XNAME_ByteArray, XNAME_MultiStringValue, XNAME_MD5Hash, XNAME_ByteValues, XNAME_Short, XNAME_UShort, XNAME_Int,
            XNAME_UInt, XNAME_Long, XNAME_ULong, XNAME_Double, XNAME_Float, XNAME_Decimal, XNAME_NVarChar, XNAME_Char, XNAME_DateTime, XNAME_TimeSpan, XNAME_UniqueIdentifier, XNAME_NewIdNavRef, XNAME_Bit, XNAME_Text, XNAME_VolumeIdentifier,
            XNAME_DriveType, XNAME_Enum, XNAME_RelatedEntity, XNAME_NewRelatedEntity, XNAME_CollectionNavigation, XNAME_NewCollectionNavigation, XNAME_Byte, XNAME_SByte, XNAME_Short, XNAME_UShort, XNAME_Int, XNAME_UInt, XNAME_Long,
            XNAME_ULong });

        private static readonly ReadOnlyCollection<XName> TypeKeyed = new(new[] { XNAME_ImplementsGenericEntity, XNAME_ImplementsEntity, XNAME_Implements });

        string ToXPath(XNode node)
        {
            if (node is XDocument)
                return "/";
            if (node is XElement element)
            {
                string leaf;
                if (NameKeyed.Contains(element.Name))
                {
                    string name = element.Attribute(XNAME_Name)?.Value;
                    leaf = string.IsNullOrWhiteSpace(name) ? null : $"{element.Name}[@{NAME_Name}=\"{name.Replace("\"", "&quot;")}\"]";
                }
                else if (TypeKeyed.Contains(element.Name))
                {
                    string type = element.Attribute(XNAME_Name)?.Value;
                    leaf = string.IsNullOrWhiteSpace(type) ? null : $"{element.Name}[@{NAME_Name}=\"{type.Replace("\"", "&quot;")}\"]";
                }
                else
                    leaf = null;
                if (leaf is null)
                {
                    int count = element.NodesBeforeSelf().OfType<XElement>().Count(e => e.Name == element.Name);
                    if (count > 0 || element.NodesAfterSelf().OfType<XElement>().Any(e => e.Name == element.Name))
                        leaf = $"{element.Name}[@{count + 1}]";
                    else
                        leaf = element.Name.ToString();
                }
                return (node.Parent is null) ? ((node.Document is null) ? leaf : $"/{leaf}") : $"{ToXPath(node.Parent)}/{leaf}";
            }
            return (node.Parent is null) ? ((node.Document is null) ? "." : "/") : ToXPath(node.Parent);
        }

        //EntityDefinition GetEntityDefinition(XElement entityElement)
        //{
        //    EntityDefinition entityDefinition = entityElement.Annotation<EntityDefinition>();
        //    if (entityDefinition is not null)
        //        return entityDefinition;
        //    XElement parentElement;
        //    if (entityElement is null || entityElement?.Name != XNAME_Entity || (parentElement = entityElement?.Parent) is null)
        //        return null;
        //    Func<string, XElement> findEntityByName;
        //    if (parentElement.Name == XNAME_Root)
        //        findEntityByName = FindRootEntityByName;
        //    else if (parentElement.Name == XNAME_Local)
        //        findEntityByName = FindLocalEntityByName;
        //    else if (parentElement.Name == XNAME_Upstream)
        //        findEntityByName = FindUpstreamByName;
        //    else
        //        return null;
        //    IEnumerable<(string Name, EntityDefinition Definition)> baseTypes = entityElement.Elements(XNAME_ExtendsEntity)
        //        .Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //        {
        //            XElement element = findEntityByName(e.Attribute(XNAME_Type)?.Value);
        //            if (element is null)
        //                throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_Type)?.Value}\" at {ToXPath(e)}");
        //            return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //        }).Concat(entityElement.Elements(XNAME_ExtendsGenericEntity)
        //        .Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //    {
        //        XElement element = findEntityByName(e.Attribute(XNAME_TypeDef)?.Value);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_TypeDef)?.Value}\" at {ToXPath(e)}");
        //        return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //    })).Concat(entityElement.Elements().Select<XElement, (string Name, EntityDefinition Definition)>(e =>
        //    {
        //        XElement element;
        //        if (e.Name == XNAME_Implements)
        //            return new(e.Attribute(XNAME_Type)?.Value, null);
        //        if (e.Name == XNAME_ImplementsEntity)
        //        {
        //            element = findEntityByName(e.Attribute(XNAME_Type)?.Value);
        //            if (element is null)
        //                throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_Type)?.Value}\" at {ToXPath(e)}");
        //            return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //        }
        //        if (e.Name != XNAME_ImplementsGenericEntity)
        //            return new (null, null);
        //        element = findEntityByName(e.Attribute(XNAME_TypeDef)?.Value);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{e.Attribute(XNAME_TypeDef)?.Value}\" at {ToXPath(e)}");
        //        return new(e.Attribute(XNAME_Type)?.Value, GetEntityDefinition(element));
        //    }).Where(t => t.Name is not null));
        //    string rootInterface = entityElement.Attribute(XNAME_RootInterface)?.Value;
        //    if (!string.IsNullOrWhiteSpace(rootInterface))
        //    {
        //        XElement element = findEntityByName(rootInterface);
        //        if (element is null)
        //            throw new InvalidDataException($"Could not find element \"{rootInterface}\" at {ToXPath(entityElement)}");
        //        baseTypes = new (string Name, EntityDefinition Definition)[] { new(rootInterface, GetEntityDefinition(element)) }.Concat(baseTypes);
        //    }
        //    ReadOnlyCollection<EntityDefinition> baseDefinitions = new(baseTypes.Select(b => b.Definition).Where(d => d is not null).ToArray());
        //    ReadOnlyCollection<PropertyDefinition> properties = new(entityElement.Elements(XNAME_Properties).Elements()
        //        .Select(p => GetPropertyDefinition(p, baseDefinitions)).ToArray());
        //    TableDefinition tableDefinition = GetTableDefinition(entityElement, baseDefinitions, properties);
        //    entityDefinition = new(entityElement.Attribute(XNAME_Name)?.Value.Replace("{", "<").Replace("}", ">"), tableDefinition,
        //        properties, baseDefinitions, new ReadOnlyCollection<string>(baseTypes.Select(b => b.Name).ToArray()), entityElement);
        //    entityElement.AddAnnotation(entityDefinition);
        //    return entityDefinition;
        //}

        //private TableDefinition GetTableDefinition(XElement entityElement, ReadOnlyCollection<EntityDefinition> baseDefinitions, ReadOnlyCollection<PropertyDefinition> properties)
        //{
        //    TableDefinition tableDefinition = entityElement.Annotation<TableDefinition>();
        //    if (tableDefinition is not null)
        //        return tableDefinition;
        //    string tableName = entityElement.Attribute(XNAME_TableName)?.Value;
        //    if (tableName is null)
        //        return null;
        //    Collection<IColumnDefinition> columns = new();
        //    foreach (PropertyDefinition pd in properties)
        //    {
        //        IColumnDefinition columnDefinition = GetColumnDefinition(pd, baseDefinitions);
        //        if (columnDefinition is not null)
        //            columns.Add(columnDefinition);
        //    }
        //    tableDefinition = new TableDefinition(tableName, new ReadOnlyCollection<IColumnDefinition>(columns), entityElement);
        //    entityElement.AddAnnotation(tableDefinition);
        //    return tableDefinition;
        //}

        //private IColumnDefinition GetColumnDefinition(PropertyDefinition pd, ReadOnlyCollection<EntityDefinition> baseDefinitions)
        //{
        //    IColumnDefinition columnDefinition = pd.Source.Annotation<IColumnDefinition>();
        //    if (columnDefinition is not null)
        //        return columnDefinition;
        //    string propertyName = pd.Source.Attribute(XNAME_ColName)?.Value ?? pd.Source.Attribute(XNAME_Name)?.Value;
        //    SqlColType? sqlType = ToSqlSqlColType(pd.Source, out string sqlExpr);
        //    if (!sqlType.HasValue || string.IsNullOrWhiteSpace(propertyName))
        //        return null;
        //    switch (sqlType.Value)
        //    {
        //        case SqlColType.UNSIGNED_TINYINT:
        //        case SqlColType.TINYINT:
        //        case SqlColType.UNSIGNED_SMALLINT:
        //        case SqlColType.SMALLINT:
        //        case SqlColType.UNSIGNED_INT:
        //        case SqlColType.INT:
        //        case SqlColType.BIGINT:
        //        case SqlColType.UNSIGNED_BIGINT:
        //        case SqlColType.BINARY:
        //        case SqlColType.NVARCHAR:
        //        case SqlColType.CHARACTER:
        //        case SqlColType.REAL:
        //        case SqlColType.NUMERIC:
        //            break;
        //        case SqlColType.DATETIME:
        //        case SqlColType.TIME:
        //        case SqlColType.UNIQUEIDENTIFIER:
        //        case SqlColType.BIT:
        //        case SqlColType.TEXT:
        //        case SqlColType.BLOB:
        //        case SqlColType.NULL:
        //            break;
        //        default:
        //            break;
        //    }
        //    throw new NotImplementedException();
        //}

        //PropertyDefinition GetPropertyDefinition(XElement propertyElement, IReadOnlyCollection<EntityDefinition> baseDefinitions)
        //{
        //    string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
        //    ReadOnlyCollection<PropertyDefinition> baseProperties = new(baseDefinitions.SelectMany(d => d.Properties.Where(p => p.Name == propertyName)).ToArray());

        //    PropertyDefinition propertyDefinition = propertyElement.Annotation<PropertyDefinition>();
        //    if (propertyDefinition is not null)
        //        return propertyDefinition;
        //    bool allowsNull = propertyElement.Attributes(XNAME_AllowNull).Any(a => a.Value == "true") || propertyElement.Elements(XNAME_DefaultNull).Any() ||
        //            baseProperties.Any(p => p.AllowsNull);
        //    bool isGenericWritable = propertyElement.Attributes(XNAME_IsGenericWritable).Any(a => a.Value == "true") || baseProperties.Any(p => p.IsGenericWritable);
        //    switch (propertyElement.Name.LocalName)
        //    {
        //        case NAME_Byte:
        //        case NAME_SByte:
        //        case NAME_Short:
        //        case NAME_UShort:
        //        case NAME_Int:
        //        case NAME_UInt:
        //        case NAME_Long:
        //        case NAME_ULong:
        //        case NAME_Float:
        //        case NAME_Double:
        //        case NAME_Decimal:
        //        case NAME_Char:
        //            propertyDefinition = new PropertyDefinition(propertyName,
        //                (allowsNull ? $"{propertyElement.Name.LocalName}?" : propertyElement.Name.LocalName).ToLower(),
        //                allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_ByteArray:
        //            propertyDefinition = new PropertyDefinition(propertyName, "byte[]", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_Text:
        //        case NAME_NVarChar:
        //            propertyDefinition = new PropertyDefinition(propertyName, "string", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_VolumeIdentifier:
        //        case NAME_DriveType:
        //        case NAME_MD5Hash:
        //        case NAME_DateTime:
        //        case NAME_TimeSpan:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? $"{propertyElement.Name.LocalName}?" : propertyElement.Name.LocalName,
        //                allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //        case NAME_ByteValues:
        //        case NAME_MultiStringValue:
        //            propertyDefinition = new PropertyDefinition(propertyName, propertyElement.Name.LocalName, allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_UniqueIdentifier:
        //        case NAME_NewIdNavRef:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? "Guid?" : "Guid", allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_Bit:
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? "bool?" : "bool", allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_Enum:
        //            string tn = propertyElement.Attribute(XNAME_Type)?.Value;
        //            propertyDefinition = new PropertyDefinition(propertyName, allowsNull ? $"{tn}?" : tn, allowsNull, isGenericWritable, baseProperties,
        //                propertyElement);
        //            break;
        //        case NAME_CollectionNavigation:
        //        case NAME_NewCollectionNavigation:
        //            propertyDefinition = new PropertyDefinition(propertyName, $"IEnumerable<{propertyElement.Attribute(XNAME_ItemType)?.Value}>", allowsNull,
        //                isGenericWritable,
        //                baseProperties, propertyElement);
        //            break;
        //        case NAME_RelatedEntity:
        //        case NAME_NewRelatedEntity:
        //            propertyDefinition = new PropertyDefinition(propertyName,
        //                (propertyElement.Attribute(XNAME_TypeDef)?.Value ?? propertyElement.Attribute(XNAME_Reference)?.Value), allowsNull, isGenericWritable,
        //                baseProperties, propertyElement);
        //            break;
        //        default:
        //            propertyDefinition = new PropertyDefinition(propertyName, "object", allowsNull, isGenericWritable, baseProperties, propertyElement);
        //            break;
        //    }
        //    propertyElement.AddAnnotation(propertyDefinition);
        //    return propertyDefinition;
        //}
        /*

foreach (XElement entityElement in entityDefinitionsElement.Elements().Elements(XNAME_Entity))
{
string typeName = entityElement.Attribute(XNAME_Name)?.Value;
if (string.IsNullOrWhiteSpace(typeName))
continue;

foreach (XElement propertyElement in entityElement.Elements(XNAME_Field))
{
XName elementName = propertyElement.Name;
string propertyName = propertyElement.Attribute(XNAME_Name)?.Value;
if (string.IsNullOrWhiteSpace(propertyName))
   continue;
string expected = $"{typeName}.{propertyName}";
XAttribute attribute;
switch (propertyElement.Name)
{
   case NAME_Byte:
   case NAME_SByte:
   case NAME_Short:
   case NAME_UShort:
   case NAME_Int:
   case NAME_UInt:
   case NAME_Long:
   case NAME_ULong:
   case NAME_Float:
   case NAME_Double:
   case NAME_Decimal:
       if ((attribute = propertyElement.Attribute(XNAME_MaxValue)) is not null)
           WriteLine("")
       //(@MaxValue|@MinValue|Default)
       break;
   case NAME_NVarChar:
   case NAME_ByteValues:
   case NAME_ByteArray:
       //(@MaxLength|@MinLength|Default)
       break;
   case NAME_Char:
   case NAME_Text:
   case NAME_Bit:
   case NAME_VolumeIdentifier:
   case NAME_DateTime:
   case NAME_DriveType:
   case NAME_Enum:
       //Default
       break;
   case NAME_TimeSpan:
       //(Default|DefaultZero)
       break;
}
}
}
*/
        IEnumerable<XElement> GetBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Array.Empty<XElement>();

            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            return parent.Name.LocalName switch
            {
                NAME_Root => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null),
                NAME_Local => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                    .Select(n => FindLocalEntityByName(n)).Where(e => e is not null),
                NAME_Upstream => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                    .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null),
                _ => Array.Empty<XElement>(),
            };
        }

        void GetAllProperties(XElement entityElement, Collection<(string Name, LinkedList<XElement> Sources)> collection)
        {
            foreach (XElement baseEntity in GetBaseEntities(entityElement).Reverse())
                GetAllProperties(baseEntity, collection);
            foreach (XAttribute attribute in entityElement.Elements(XNAME_Properties).Elements().Attributes(XNAME_Name))
            {
                XElement propertyElement = attribute.Parent;
                string propertyName = attribute.Value;
                if (collection.Any(t => t.Name == propertyName))
                {
                    (string Name, LinkedList<XElement> Sources) property = collection.First(t => t.Name == propertyName);
                    if (!property.Sources.Any(e => ReferenceEquals(e, propertyElement)))
                        property.Sources.AddFirst(attribute.Parent);
                }
                else
                {
                    (string Name, LinkedList<XElement> Sources) property = new(propertyName, new LinkedList<XElement>());
                    property.Sources.AddLast(attribute.Parent);
                    collection.Add(property);
                }
            }
        }

        void GetAllBaseEntities(XElement entityElement, int level, Collection<(XElement Element, int Level)> collection, Func<IEnumerable<string>, IEnumerable<XElement>> getEntities)
        {
            IEnumerable<string> names = entityElement.Elements().Select(e => (
                IsType: e.Name == XNAME_ExtendsEntity || e.Name == XNAME_ImplementsEntity,
                IsDef: e.Name == XNAME_ExtendsGenericEntity || e.Name == XNAME_ImplementsGenericEntity,
                Element: e
            )).Where(t => t.IsType || t.IsDef).SelectMany(t => t.Element.Attributes(t.IsType ? XNAME_Type : XNAME_TypeDef)).Select(a => a.Value);
            int nextLevel = level + 1;
            foreach (XElement baseEntity in getEntities(names))
            {
                IEnumerable<(XElement Element, int Level)> items = collection.Where(e => ReferenceEquals(e.Element, baseEntity));
                if (items.Any())
                {
                    (XElement Element, int Level) t = items.First();
                    if (level < t.Level)
                    {
                        collection.Remove(t);
                        collection.Add(new(baseEntity, level));
                    }
                }
                else
                {
                    collection.Add(new(baseEntity, level));
                    GetAllBaseEntities(baseEntity, nextLevel, collection, getEntities);
                }
            }
        }

        XElement[] GetAllBaseEntities(XElement entityElement)
        {
            XElement parent = entityElement?.Parent;
            if (parent is null || entityElement?.Name != XNAME_Entity)
                return Array.Empty<XElement>();
            Collection<(XElement Element, int Level)> result = new();
            Func<IEnumerable<string>, IEnumerable<XElement>> getEntities;
            switch (parent.Name.LocalName)
            {
                case NAME_Root:
                    getEntities = names => names.Distinct().Select(n => FindRootEntityByName(n)).Where(e => e is not null);
                    break;
                case NAME_Local:
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindLocalEntityByName(n)).Where(e => e is not null);
                    break;
                case NAME_Upstream:
                    getEntities = names => entityElement.Attributes(XNAME_RootInterface).Select(a => a.Value).Concat(names).Distinct()
                        .Select(n => FindUpstreamEntityByName(n)).Where(e => e is not null);
                    break;
                default:
                    return Array.Empty<XElement>();
            }
            GetAllBaseEntities(entityElement, 0, result, getEntities);
            return result.OrderBy(t => t.Level).Select(t => t.Element).ToArray();
        }

        static XElement[] GetAllBaseProperties(XElement propertyElement, XElement[] orderedBaseEntities, out XElement baseProperty, out bool isNew, out bool doNotEmit)
        {
            string propertyName = propertyElement?.Attribute(XNAME_Name)?.Value;
            if (propertyName is null || orderedBaseEntities is null || orderedBaseEntities.Length == 0)
            {
                baseProperty = propertyElement;
                isNew = doNotEmit = false;
                return Array.Empty<XElement>();
            }
            XName baseName;
            XName inheritedName;
            switch (propertyElement.Name.LocalName)
            {
                case NAME_NewIdNavRef:
                    isNew = false;
                    doNotEmit = true;
                    baseName = XNAME_UniqueIdentifier;
                    inheritedName = XNAME_NewIdNavRef;
                    break;
                case NAME_NewRelatedEntity:
                    isNew = true;
                    doNotEmit = false;
                    baseName = XNAME_RelatedEntity;
                    inheritedName = XNAME_NewRelatedEntity;
                    break;
                case NAME_NewCollectionNavigation:
                    isNew = true;
                    doNotEmit = false;
                    baseName = XNAME_CollectionNavigation;
                    inheritedName = XNAME_NewCollectionNavigation;
                    break;
                default:
                    isNew = doNotEmit = false;
                    baseProperty = propertyElement;
                    return orderedBaseEntities.Elements(XNAME_Properties).Elements(propertyElement.Name).Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent).ToArray();
            }
            IEnumerable<XElement> results = orderedBaseEntities.Elements(XNAME_Properties).Elements().Where(e => e.Name == baseName || e.Name == inheritedName)
                .Attributes(XNAME_Name).Where(a => a.Value == propertyName).Select(a => a.Parent);
            baseProperty = results.Where(e => e.Name == baseName).DefaultIfEmpty(propertyElement).First();
            return results.ToArray();
        }

        XElement GetRelatedEntityPrimaryKeyProperty(XElement relatedEntityPropertyElement)
        {
            XElement parent = relatedEntityPropertyElement?.Parent;
            if (parent is null || (relatedEntityPropertyElement.Name != XNAME_RelatedEntity && relatedEntityPropertyElement.Name != XNAME_NewRelatedEntity))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                NAME_Local => FindLocalPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                NAME_Upstream => FindUpstreamPropertyByFullName(relatedEntityPropertyElement.Attribute(XNAME_PrimaryKey)?.Value),
                _ => null,
            };
        }

        XElement GetCollectionNavigationForeignKeyProperty(XElement collectionNavigationPropertyElement)
        {
            XElement parent = collectionNavigationPropertyElement?.Parent;
            if (parent is null || (collectionNavigationPropertyElement.Name != XNAME_CollectionNavigation && collectionNavigationPropertyElement.Name != XNAME_NewCollectionNavigation))
                return null;
            return parent.Name.LocalName switch
            {
                NAME_Root => FindRootPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                NAME_Local => FindLocalPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                NAME_Upstream => FindUpstreamPropertyByFullName(collectionNavigationPropertyElement.Attribute(XNAME_ForeignKey)?.Value),
                _ => null,
            };
        }

        SqlColType? ToSqlSqlColType(XElement memberElement, out string sqlExpr)
        {
            if (memberElement is null || memberElement.Name.NamespaceName.Length > 0)
            {
                sqlExpr = SqlColType.NULL.ToString("F");
                return SqlColType.NULL;
            }

            SqlColType? result;
            switch (memberElement.Name.LocalName)
            {
                case NAME_NVarChar:
                    sqlExpr = $"{SqlColType.NVARCHAR}({memberElement.Attribute(XNAME_MaxLength)?.Value})";
                    return SqlColType.NVARCHAR;
                case NAME_VolumeIdentifier:
                    sqlExpr = $"{SqlColType.NVARCHAR}(1024)";
                    return SqlColType.NVARCHAR;
                case NAME_Char:
                    sqlExpr = $"{SqlColType.CHARACTER}(1)";
                    return SqlColType.CHARACTER;
                case NAME_ByteValues:
                case NAME_ByteArray:
                    sqlExpr = $"{SqlColType.BINARY}({memberElement.Attribute(XNAME_MaxLength)?.Value})";
                    return SqlColType.BINARY;
                case NAME_MD5Hash:
                    sqlExpr = $"{SqlColType.BINARY}(16)";
                    return SqlColType.BINARY;
                case NAME_Enum:
                    result = (memberElement.Attribute(XNAME_Type)?.Value ?? "") switch
                    {
                        NAME_Byte => SqlColType.UNSIGNED_TINYINT,
                        NAME_SByte => SqlColType.TINYINT,
                        NAME_Short => SqlColType.SMALLINT,
                        NAME_UShort => SqlColType.UNSIGNED_SMALLINT,
                        NAME_UInt => SqlColType.UNSIGNED_INT,
                        NAME_Long => SqlColType.BIGINT,
                        NAME_ULong => SqlColType.UNSIGNED_BIGINT,
                        _ => SqlColType.INT
                    };
                    break;
                case NAME_RelatedEntity:
                case NAME_NewRelatedEntity:
                case NAME_CollectionNavigation:
                case NAME_NewCollectionNavigation:
                    result = null;
                    break;
                default:
                    result = memberElement.Name.LocalName switch
                    {
                        NAME_Byte => SqlColType.UNSIGNED_TINYINT,
                        NAME_DriveType => SqlColType.UNSIGNED_TINYINT,
                        NAME_SByte => SqlColType.TINYINT,
                        NAME_Short => SqlColType.SMALLINT,
                        NAME_UShort => SqlColType.UNSIGNED_SMALLINT,
                        NAME_Int => SqlColType.INT,
                        NAME_UInt => SqlColType.UNSIGNED_INT,
                        NAME_Long => SqlColType.BIGINT,
                        NAME_ULong => SqlColType.UNSIGNED_BIGINT,
                        NAME_Float => SqlColType.REAL,
                        NAME_Double => SqlColType.REAL,
                        NAME_Decimal => SqlColType.NUMERIC,
                        NAME_DateTime => SqlColType.DATETIME,
                        NAME_TimeSpan => SqlColType.TIME,
                        NAME_NewIdNavRef => SqlColType.UNIQUEIDENTIFIER,
                        NAME_UniqueIdentifier => SqlColType.UNIQUEIDENTIFIER,
                        NAME_Bit => SqlColType.BIT,
                        NAME_Text => SqlColType.TEXT,
                        NAME_MultiStringValue => SqlColType.TEXT,
                        _ => SqlColType.BLOB
                    };
                    break;
            }
            sqlExpr = (result.HasValue) ? result.Value.ToString("F") : null;
            return result;
        }
    }
    enum SqlColType
    {
        NVARCHAR,
        CHARACTER,
        BINARY,
        TINYINT,
        UNSIGNED_TINYINT,
        SMALLINT,
        UNSIGNED_SMALLINT,
        INT,
        UNSIGNED_INT,
        BIGINT,
        UNSIGNED_BIGINT,
        REAL,
        NUMERIC,
        DATETIME,
        TIME,
        UNIQUEIDENTIFIER,
        BIT,
        TEXT,
        BLOB,
        NULL
    }
}
