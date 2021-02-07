using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        public class SnIdValues : IdValues, IEquatable<SnIdValues>
        {
            public uint? SerialNumber { get; }
            public byte? Ordinal { get; }

            public SnIdValues(uint? serialNumber, byte? ordinal, string value, string absoluteUri) : base(value, absoluteUri)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public bool Equals([AllowNull] SnIdValues other) => null != other && (ReferenceEquals(this, other) ||
                (base.Equals(this) && SerialNumber == other.SerialNumber && Ordinal == other.Ordinal));

            public override bool Equals(object obj) => Equals(obj as SnIdValues);

            public override int GetHashCode() => HashCode.Combine(SerialNumber, Ordinal, AbsoluteUri, Value);

            public override string ToString()
            {
                return (SerialNumber.HasValue) ?
                    ((Ordinal.HasValue) ?
                        $"{{ SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = {Ordinal.Value.ToString("x2")},{base.ToString().Substring(1)}"
                        : $"{{ SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = null,{base.ToString().Substring(1)}"
                    )
                    : ((Ordinal.HasValue) ?
                        $"{{ SerialNumber = null, Ordinal = {Ordinal.Value.ToString("x2")},{base.ToString().Substring(1)}"
                        : $"{{ SerialNumber = null, Ordinal = null,{base.ToString().Substring(1)}"
                    );
            }
        }
    }
}