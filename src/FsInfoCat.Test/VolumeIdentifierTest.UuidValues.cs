using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        public class UuidValues : IdValues, IEquatable<UuidValues>
        {
            public Guid? UUID { get; }

            public UuidValues(Guid? uuid, string value, string absoluteUri) : base(value, absoluteUri)
            {
                UUID = uuid;
            }

            public bool Equals([AllowNull] UuidValues other) => null != other && (ReferenceEquals(this, other) ||
                (base.Equals(this) && UUID == other.UUID));

            public override bool Equals(object obj) => Equals(obj as SnIdValues);

            public override int GetHashCode() => HashCode.Combine(UUID, AbsoluteUri, Value);

            public override string ToString()
            {
                return (UUID.HasValue) ?
                    $"{{ UUID = {UUID.Value.ToString("d").ToLower()},{base.ToString().Substring(1)}"
                    : $"{{ UUID = null,{base.ToString().Substring(1)}";
            }
        }
    }
}
