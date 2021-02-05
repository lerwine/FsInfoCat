using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public class XmlDocumentFile : XmlDocument
    {
        private const string XML_PREFIX_XSI = "xsi";
        private const string XML_NAME_SCHEMALOCATION = "schemaLocation";
        private const string XML_XPATH_SCHEMA_LOCATION = "@" + XML_PREFIX_XSI + ":" + XML_NAME_SCHEMALOCATION;
        private const string XML_SCHEMA_URI_SCHEMA_INSTANCE = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly Regex SchemaLocationPattern = new Regex(@"(?:^|\G\s+)(\S+)\s+(\S+)(?=\s|$)", RegexOptions.Compiled);
        private readonly ReadOnlyCollection<SchemaLocation> _schemaLocationRO;
        private readonly Collection<SchemaLocation> _schemaLocation = new Collection<SchemaLocation>();
        private string _schemaLocationSource = "";
        private readonly FileInfo _sourceFile;
        private DirectoryInfo _schemaLocationRoot = null;

        public XmlNamespaceManager Nsmgr { get; }

        public void Load() => Load(_sourceFile.FullName);

        public XmlDocumentFile SaveAs(string path, XmlWriterSettings settings)
        {
            XmlDocumentFile documentFile = new XmlDocumentFile(path);
            documentFile.AppendChild(documentFile.ImportNode(DocumentElement, true));
            if (settings is null)
                settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false, true),
                    Indent = true
                };
            else
                (settings = settings.Clone()).Async = false;
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                documentFile.WriteTo(writer);
                writer.Flush();
            }
            return documentFile;
        }

        public XmlDocumentFile(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            if (path.Trim().Length == 0)
                throw new ArgumentOutOfRangeException("Path cannot be empty", nameof(path));
            try { _sourceFile = new FileInfo(path); }
            catch (Exception exc) { throw new ArgumentOutOfRangeException((string.IsNullOrWhiteSpace(exc.Message)) ? "Invalid path name" : exc.Message, path, nameof(path)); }
            _schemaLocationRO = new ReadOnlyCollection<SchemaLocation>(_schemaLocation);
            Nsmgr = new XmlNamespaceManager(NameTable);
            Nsmgr.AddNamespace(XML_PREFIX_XSI, XML_SCHEMA_URI_SCHEMA_INSTANCE);
            NodeInserted += OnNodeInserted;
            NodeChanged += OnNodeChanged;
            NodeRemoved += OnNodeRemoved;
        }

        public ReadOnlyCollection<SchemaLocation> GetSchemaLocation() => _schemaLocationRO;

        public void SetSchemaLocation(IEnumerable<SchemaLocation> locations)
        {
            if (ReferenceEquals(locations, _schemaLocationRO))
                return;
            Monitor.Enter(_schemaLocation);
            try
            {
                _schemaLocation.Clear();
                if (null != locations)
                {
                    foreach (SchemaLocation loc in locations.Where(l => null != l).Distinct())
                        _schemaLocation.Add(loc);
                }
                SetSchemaLocationAttribute();
            }
            finally { Monitor.Exit(_schemaLocation); }
        }

        public void SetSchemaLocation(params SchemaLocation[] locations) => SetSchemaLocation((IEnumerable<SchemaLocation>)locations);

        private void SetSchemaLocationAttribute()
        {
            if (_schemaLocation.Count == 0)
                _schemaLocationSource = "";
            else if (_sourceFile is null)
                _schemaLocationSource = string.Join(" ", _schemaLocation.Select(s => s.ToString()));
            else
            {
                Uri baseUri = new Uri(_sourceFile.DirectoryName, UriKind.Absolute);
                _schemaLocationSource = string.Join(" ", _schemaLocation.Select(s => s.ToString(baseUri)));
            }
            XmlElement documentElement = DocumentElement;
            if (documentElement is null)
                return;
            XmlAttribute attribute = documentElement.SelectSingleNode(XML_XPATH_SCHEMA_LOCATION, Nsmgr) as XmlAttribute;
            if (_schemaLocation.Count == 0)
            {
                if (null != attribute)
                    documentElement.Attributes.Remove(attribute);
            }
            else
            {
                if (attribute is null)
                {
                    string prefix = GetPrefixOfNamespace(XML_SCHEMA_URI_SCHEMA_INSTANCE);
                    attribute = CreateAttribute((string.IsNullOrEmpty(prefix)) ? XML_PREFIX_XSI : prefix, XML_NAME_SCHEMALOCATION, XML_SCHEMA_URI_SCHEMA_INSTANCE);
                    attribute.Value = _schemaLocationSource;
                    DocumentElement.Attributes.Append(attribute);
                }
                else
                    attribute.Value = _schemaLocationSource;
            }
        }

        private void OnNodeInserted(object sender, XmlNodeChangedEventArgs e)
        {
            Monitor.Enter(_schemaLocation);
            try
            {
                XmlElement documentElement = DocumentElement;
                if (ReferenceEquals(e.Node, documentElement))
                {
                    XmlAttribute attribute = documentElement.SelectSingleNode(XML_XPATH_SCHEMA_LOCATION, Nsmgr) as XmlAttribute;
                    if (null != attribute)
                        OnSchemaLocationAttributeChanged(attribute.Value);
                    else if (_schemaLocation.Count > 0)
                        SetSchemaLocationAttribute();
                }
                else if (e.Node is XmlAttribute attribute && ReferenceEquals(DocumentElement, e.NewParent) && e.Node.LocalName == XML_NAME_SCHEMALOCATION && e.Node.NamespaceURI == XML_SCHEMA_URI_SCHEMA_INSTANCE)
                    OnSchemaLocationAttributeChanged(attribute.Value);
            }
            finally { Monitor.Exit(_schemaLocation); }
        }

        private void OnNodeChanged(object sender, XmlNodeChangedEventArgs e)
        {
            Monitor.Enter(_schemaLocation);
            try
            {
                if (e.Node is XmlAttribute attribute && ReferenceEquals(DocumentElement, e.NewParent) && e.Node.LocalName == XML_NAME_SCHEMALOCATION && e.Node.NamespaceURI == XML_SCHEMA_URI_SCHEMA_INSTANCE)
                    OnSchemaLocationAttributeChanged(attribute.Value);
            }
            finally { Monitor.Exit(_schemaLocation); }
        }

        private void OnNodeRemoved(object sender, XmlNodeChangedEventArgs e)
        {
            if (e.Node is XmlElement)
            {
                if (ReferenceEquals(this, e.OldParent))
                    OnSchemaLocationAttributeChanged("");
            }
            else if (e.Node is XmlAttribute && ReferenceEquals(DocumentElement, e.OldParent) && e.Node.LocalName == XML_NAME_SCHEMALOCATION && e.Node.NamespaceURI == XML_SCHEMA_URI_SCHEMA_INSTANCE)
                OnSchemaLocationAttributeChanged("");
        }

        private void OnSchemaLocationAttributeChanged(string value)
        {
            Monitor.Enter(_schemaLocation);
            try
            {
                SchemaLocation[] schemaLocations;
                if (string.IsNullOrWhiteSpace(value))
                    schemaLocations = new SchemaLocation[0];
                else
                    schemaLocations = SchemaLocationPattern.Matches(value.Trim()).Select(m => (SchemaLocation.TryCreate(m.Groups[1].Value, m.Groups[2].Value, out SchemaLocation location)) ? location : null).Where(l => null != l).ToArray();
                if (schemaLocations.Length == 0)
                {
                    _schemaLocation.Clear();
                    _schemaLocationSource = "";
                }
                else
                {
                    string loc;
                    if (_sourceFile is null)
                        loc = string.Join(" ", schemaLocations.Select(s => s.ToString()));
                    else
                    {
                        Uri uri = new Uri(_sourceFile.DirectoryName, UriKind.Absolute);
                        loc = string.Join(" ", schemaLocations.Select(s => s.ToString(uri)));
                    }
                    if (loc != _schemaLocationSource)
                    {
                        _schemaLocationSource = loc;
                        _schemaLocation.Clear();
                        foreach (SchemaLocation sl in schemaLocations)
                            _schemaLocation.Add(sl);
                    }
                }
            }
            finally { Monitor.Exit(_schemaLocation); }
        }

        public class SchemaLocation : IEquatable<SchemaLocation>
        {
            private Uri _namespace;
            private Uri _location;

            public Uri Namespace
            {
                get => _namespace;
                set
                {
                    if (value is null)
                        throw new ArgumentNullException(nameof(value));
                    _namespace = value;
                }
            }

            public Uri Location
            {
                get => _location;
                set
                {
                    if (value is null)
                        throw new ArgumentNullException(nameof(value));
                    _location = value;
                }
            }

            public SchemaLocation(Uri @namespace, Uri location)
            {
                if (null == @namespace)
                    throw new ArgumentNullException(nameof(@namespace));
                if (location is null)
                    throw new ArgumentNullException(nameof(location));
                if (!@namespace.IsAbsoluteUri)
                    throw new ArgumentException("Namespace must be an absolute URI", nameof(@namespace));
                _namespace = @namespace;
                _location = (location.IsAbsoluteUri) ? location : new Uri(Uri.EscapeUriString(location.OriginalString.Replace("\\", "/")), UriKind.RelativeOrAbsolute);
            }

            public bool Equals(SchemaLocation other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                Uri loc1, loc2;
                if (_namespace.AbsoluteUri != other._namespace.AbsoluteUri || (loc1 = _location).IsAbsoluteUri != (loc2 = other._location).IsAbsoluteUri)
                    return false;
                if (loc1.IsAbsoluteUri)
                    return loc1.AbsoluteUri == loc2.AbsoluteUri;
                return loc1.OriginalString == loc2.OriginalString;
            }

            public override bool Equals(object obj) => Equals(obj as SchemaLocation);

            public override int GetHashCode() => ToString().GetHashCode();

            public Uri ToRelativeLocationUri(Uri baseUri)
            {
                if (baseUri is null || baseUri.OriginalString.Length == 0)
                    return _location;
                if (!baseUri.IsAbsoluteUri)
                    throw new ArgumentOutOfRangeException(nameof(baseUri));

                Uri location = _location;
                if (location.IsAbsoluteUri)
                {
                    if (baseUri.IsFile && location.IsFile)
                        return new Uri(Path.GetRelativePath(baseUri.LocalPath, location.LocalPath), UriKind.RelativeOrAbsolute);
                    return location;
                }
                string absoluteUri = baseUri.AbsoluteUri;
                string locPath = location.OriginalString;
                if (locPath == ".")
                    return baseUri;
                if (locPath.StartsWith("/"))
                    return new Uri(baseUri, location);
                return new Uri(((absoluteUri.EndsWith("/")) ? baseUri : new Uri(absoluteUri + "/", UriKind.Absolute)), location);
            }

            public string ToString(Uri baseUri)
            {
                Uri location = ToRelativeLocationUri(baseUri);
                return _namespace.AbsoluteUri + " " + ((location.IsAbsoluteUri) ? location.AbsoluteUri : location.OriginalString);
            }

            public override string ToString() => _namespace.AbsoluteUri + " " + ((_location.IsAbsoluteUri) ? _location.AbsoluteUri : _location.OriginalString);

            public static bool TryCreate(string @namespace, string location, out SchemaLocation result)
            {
                if (!(string.IsNullOrEmpty(@namespace) || string.IsNullOrEmpty(location)) && Uri.TryCreate(@namespace, UriKind.Absolute, out Uri nsUri) &&
                    Uri.TryCreate(location, UriKind.RelativeOrAbsolute, out Uri locUri) && locUri.OriginalString.Length > 0)
                {
                    result = new SchemaLocation(nsUri, locUri);
                    return true;
                }
                result = null;
                return false;
            }
        }
    }
}
