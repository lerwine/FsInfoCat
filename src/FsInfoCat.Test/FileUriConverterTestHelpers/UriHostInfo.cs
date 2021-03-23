using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class UriHostInfo : HostInfo<UriAuthority>, IEquatable<UriHostInfo>
    {
        [XmlAttribute]
        public HostType Type { get; set; }

        [XmlIgnore]
        public int? Port { get; set; }

        [XmlAttribute(nameof(Port))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string PortString
        {
            get
            {
                int? port = Port;
                return (port.HasValue) ? XmlConvert.ToString(port.Value) : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Port = null;
                else
                    try { Port = XmlConvert.ToInt32(value); }
                    catch { Port = null; }
            }
        }

        public bool Equals([AllowNull] UriHostInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Type == other.Type)
            {
                int? p = Port;
                int? v = other.Port;
                return ((p.HasValue) ? v.HasValue && p.Value == v.Value : !v.HasValue) && base.Equals(other);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UriHostInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Match, Value, Type, Port);
        }
    }
}
