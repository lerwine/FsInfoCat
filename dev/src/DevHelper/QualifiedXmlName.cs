public class QualifiedXmlName : System.Xml.XmlQualifiedName, System.IEquatable<QualifiedXmlName>
{
    private const string XML_SCHEMA_XSD = "http://www.w3.org/2001/XMLSchema";
    private readonly string _prefix;
    private readonly string _localName;
    private readonly bool _isValid;
    private readonly bool _isAnyNamespace;
    private readonly bool _isAnyPrefix;
    public string Prefix { get { return _prefix; } }
    public string LocalName { get { return _localName; } }
    public bool IsValid { get { return _isValid; } }
    public bool IsAnyNamespace { get { return _isAnyNamespace; } }
    public bool IsAnyPrefix { get { return _isAnyPrefix; } }
    public QualifiedXmlName(string prefix, string localName, string ns)
        : base((null == prefix) ? ((null == localName) ? "" : localName) : prefix + ((string.IsNullOrEmpty(localName)) ? ":" : ":" + localName), ns)
    {
        _isAnyPrefix = null == prefix;
        _isAnyNamespace = null == ns;
        if (IsEmpty)
        {
            _localName = "";
            _prefix = "";
            _isValid = false;
        }
        else
        {
            bool isValid;
            _localName = AsValidNCName(localName, out isValid);
            if (!(string.IsNullOrEmpty(ns) || System.Uri.IsWellFormedUriString(ns, System.UriKind.Absolute)))
                isValid = false;

            if (string.IsNullOrEmpty(prefix))
            {
                _prefix = "";
                _isValid = isValid && ((_isAnyPrefix) ? _localName : ":" + _localName).Equals(Name);
            }
            else if (isValid)
            {
                _prefix = AsValidNCName(prefix, out isValid);
                _isValid = isValid && (_prefix + ":" + _localName).Equals(Name);
            }
            else
            {
                _prefix = AsValidNCName(prefix, out isValid);
                _isValid = false;
            }
        }
    }
    public static string AsValidNCName(string value, out bool isValid)
    {
        if (string.IsNullOrEmpty(value))
        {
            isValid = false;
            return "";
        }
        isValid = System.Xml.XmlConvert.IsStartNCNameChar(value[0]);
        for (int i = 1; isValid && i < value.Length; i++)
            isValid = System.Xml.XmlConvert.IsNCNameChar(value[i]);
        if (isValid)
            return value;
        return System.Xml.XmlConvert.EncodeLocalName(value);
    }
    public bool IsEqualTo(System.Xml.XmlNode node)
    {
        return null != node && !IsEmpty && node.LocalName.Equals(_localName) && ((_isAnyNamespace) ? (_isAnyPrefix || _prefix.Equals(node.Prefix)) :
            Namespace.Equals(node.NamespaceURI));
    }
    public bool IsSameNamespace(System.Xml.XmlNode node, bool allowNsLookup)
    {
        if (null == node || IsEmpty)
            return false;
        if (_isAnyNamespace)
        {
            if (!allowNsLookup)
                return false;
            if (_isAnyPrefix)
                return true;
            if (node is System.Xml.XmlDocument)
            {
                System.Xml.XmlElement element = ((System.Xml.XmlDocument)node).DocumentElement;
                if (element.NamespaceURI.Equals(XML_SCHEMA_XSD))
                {
                    string p = element.GetPrefixOfNamespace(XML_SCHEMA_XSD);
                    if (_prefix.Equals((string.IsNullOrEmpty(p)) ? "xs" : p))
                        return true;
                }
                node = element;
            }
            string ns = node.GetNamespaceOfPrefix(_prefix);
            if (string.IsNullOrEmpty(ns))
                return node.NamespaceURI.Length == 0 && node.GetPrefixOfNamespace(ns).Equals(_prefix);
            return ns.Equals(node.NamespaceURI);
        }
        if (node is System.Xml.XmlDocument)
        {
            System.Xml.XmlElement element = ((System.Xml.XmlDocument)node).DocumentElement;
            if (element.NamespaceURI.Equals(XML_SCHEMA_XSD))
            {
                string a = element.GetAttribute("targetNamespace");
                return (string.IsNullOrEmpty(a)) ? Namespace.Length == 0 : Namespace.Equals(a);
            }
            node = element;
        }
        return node.LocalName.Equals(_localName) && ((_isAnyNamespace) ? (_isAnyPrefix || _prefix.Equals(node.Prefix)) :
            Namespace.Equals(node.NamespaceURI));
    }
    public bool IsSameNamespace(System.Xml.XmlNode node) { return IsSameNamespace(node, false); }
    public bool Equals(QualifiedXmlName other)
    {
        return null != other && (ReferenceEquals(this, other) || _isValid == other._isValid && _isAnyPrefix == other._isAnyPrefix &&
            _isAnyNamespace == other._isAnyNamespace && Namespace.Equals(other.Namespace) && Name.Equals(other.Name));
    }
    public override bool Equals(object other) { return Equals(other as QualifiedXmlName); }
    public override int GetHashCode() { return ToString().GetHashCode(); }
    public override string ToString()
    {
        if (_isAnyNamespace)
            return (_isAnyPrefix) ? _localName : _prefix + ":" + _localName;
        return ((_isAnyPrefix) ? _localName : _prefix + ":" + _localName) + ";" + Namespace;
    }
    public static QualifiedXmlName Create(string name)
    {
        return Create(name, null);
    }
    public static QualifiedXmlName Create(string name, System.Collections.Hashtable nsLookupMap)
    {
        if (null == name)
            return null;
        int i = name.IndexOf(';');
        string ns;
        if (i < 0)
            ns = null;
        else
        {
            ns = name.Substring(i + 1);
            name = name.Substring(0, i);
        }

        i = name.IndexOf(':');
        if (i > -1)
        {
            string prefix = name.Substring(0, i);
            if (null == ns && null != nsLookupMap && nsLookupMap.ContainsKey(prefix))
            {
                object obj = nsLookupMap[prefix];
                ns = (null == obj || obj is string) ? (string)obj : obj.ToString();
            }
            return new QualifiedXmlName(prefix, name.Substring(i + 1), ns);
        }
        return new QualifiedXmlName(null, name, ns);
    }
}
