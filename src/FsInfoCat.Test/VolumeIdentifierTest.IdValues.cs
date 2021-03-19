using FsInfoCat.Util;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        public class IdValues : IEquatable<IdValues>
        {
            private Uri _absoluteUri;
            public string Value { get; }

            public string AbsoluteUri { get; }

            public IdValues(string value, string absoluteUri)
            {
                Value = value;
                AbsoluteUri = absoluteUri;
                if (null != absoluteUri && Uri.TryCreate(absoluteUri, UriKind.RelativeOrAbsolute, out Uri uri))
                    _absoluteUri = uri;
                else
                    _absoluteUri = null;
            }

            public bool Equals([AllowNull] IdValues other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                if (null == _absoluteUri)
                {
                    if (null != other._absoluteUri)
                        return false;
                }
                else
                {
                    if (null == other._absoluteUri || !_absoluteUri.AuthorityCaseInsensitiveEquals(other._absoluteUri))
                        return false;
                }
                return Value == other.Value;
            }

            public override bool Equals(object obj) => Equals(obj as IdValues);

            public override int GetHashCode() => HashCode.Combine(AbsoluteUri, Value);

            public override string ToString() => (AbsoluteUri is null) ?
                ((Value is null) ?
                    "{ Value = null, AbsoluteUri = null }"
                    : $"{{ Value = \"{Value}\", AbsoluteUri = null }}"
                )
                : ((Value is null) ?
                    $"{{ Value = null, AbsoluteUri = \"{AbsoluteUri}\" }}"
                    : $"{{ Value = \"{Value}\", AbsoluteUri = \"{AbsoluteUri}\" }}"
                );
        }
    }
}
