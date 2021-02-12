using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DevHelper.PsHelp.Command;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp
{
    [PsHelpXmlRoot(ElementName.helpItems)]
    public class HelpItems
    {
        private Collection<SchemaLocationPair> _schemaLocation = null;
        private Collection<CommandHelp> _commands = null;
        public const string Attribute_Name_scheme = "scheme";
        public const string Attribute_Name_schemaLocation = "schemaLocation";
        public const string Attribute_Value_maml = "maml";

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; }

        [XmlAttribute(Attribute_Name_scheme, Form = XmlSchemaForm.Unqualified)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Schema { get => Attribute_Value_maml; set { } }

        [XmlIgnore]
        public Collection<SchemaLocationPair> SchemaLocation
        {
            get
            {
                Collection<SchemaLocationPair> schemaLocation = _schemaLocation;
                if (schemaLocation is null)
                    _schemaLocation = schemaLocation = new Collection<SchemaLocationPair>();
                return schemaLocation;
            }
            set => _schemaLocation = value;
        }

        [XmlAttribute(Attribute_Name_schemaLocation, Namespace = PsHelpUtil.Namespace_URI_xsi, Form = XmlSchemaForm.Qualified)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __SchemaLocationString
        {
            get
            {
                string[] pairs = SchemaLocation.Where(s => null != s).Select(s => s.ToString()).ToArray();
                switch (pairs.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return pairs[0];
                }
                return string.Join(" ", pairs);
            }
            set
            {
                Collection<SchemaLocationPair> schemaLocation = new Collection<SchemaLocationPair>();
                int index = 0;
                while ((index = SchemaLocationPair.TryParseNext(value, index, out SchemaLocationPair result)) > -1)
                    schemaLocation.Add(result);
                _schemaLocation = schemaLocation;
            }
        }

        public Collection<CommandHelp> Commands
        {
            get
            {
                Collection<CommandHelp> commands = _commands;
                if (commands is null)
                    _commands = commands = new Collection<CommandHelp>();
                return commands;
            }
            set => _commands = value;
        }

        public HelpItems()
        {
            Xmlns = new XmlSerializerNamespaces();
            string ns = NamespaceURI.xsi.GetStringValue(out string defaultPrefix);
            Xmlns.Add(defaultPrefix, ns);
        }

        public class SchemaLocationPair : IEquatable<SchemaLocationPair>
        {
            public static readonly Regex NonWsPairRegex = new Regex(@"(^|\G)\s*(?<u>\S+)\s+(?<p>\S+)(\s+|$)");
            private Uri _schemaUrl;
            private string _location;
            public Uri SchemaUrl
            {
                get => _schemaUrl;
                set
                {
                    if (value is null)
                        throw new ArgumentNullException(nameof(value));
                    if (!value.IsAbsoluteUri)
                        throw new ArgumentException($"'{nameof(value)}' cannot be relative.", nameof(value));
                    _schemaUrl = value;
                }
            }
            public string Location
            {
                get => _location;
                set
                {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException($"'{nameof(value)}' cannot be null or empty.", nameof(value));
                    _location = (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute)) ? value : Uri.EscapeUriString(value);;
                }
            }
            public SchemaLocationPair(Uri schemaUrl, string location)
            {
                if (schemaUrl is null)
                    throw new ArgumentNullException(nameof(schemaUrl));
                if (string.IsNullOrWhiteSpace(location))
                    throw new ArgumentException($"'{nameof(location)}' cannot be null or empty.", nameof(location));
                if (!schemaUrl.IsAbsoluteUri)
                    throw new ArgumentException($"'{nameof(schemaUrl)}' cannot be relative.", nameof(schemaUrl));
                _schemaUrl = schemaUrl;
                _location = (Uri.IsWellFormedUriString(location, UriKind.RelativeOrAbsolute)) ? location : Uri.EscapeUriString(location);
            }

            private static bool TryCreate(string url, string location, out SchemaLocationPair result)
            {
                if (!(string.IsNullOrEmpty(url) || string.IsNullOrEmpty(location)) && Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                {
                    result = new SchemaLocationPair(uri, location);
                    return true;
                }
                result = null;
                return false;
            }

            public static int TryParseNext(string input, int startat, out SchemaLocationPair result)
            {
                if (startat > -1 && startat < input.Length)
                {
                    Match match = NonWsPairRegex.Match(input, startat);
                    if (match.Success)
                    {
                        if (SchemaLocationPair.TryCreate(match.Groups["u"].Value, match.Groups["p"].Value, out result))
                            return match.Index + match.Length;
                    }
                }
                result = null;
                return -1;
            }

            public bool Equals(SchemaLocationPair other) => !(other is null) && (ReferenceEquals(this, other) || _schemaUrl.AbsoluteUri.Equals(other._schemaUrl.AbsoluteUri) && _location.Equals(other._location));

            public override bool Equals(object obj) => Equals(obj as SchemaLocationPair);

            public override int GetHashCode() => ToString().GetHashCode();

            public override string ToString() => $"{_schemaUrl.AbsoluteUri} {_location}";
        }
    }
}
