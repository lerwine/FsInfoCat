using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        public class ParseResultValues : IEquatable<ParseResultValues>
        {
            public Guid? UUID { get; }
            public uint? SerialNumber { get; }
            public byte? Ordinal { get; }

            public ParseResultValues(Guid? uuid, uint? serialNumber, byte? ordinal)
            {
                UUID = uuid;
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public bool Equals([AllowNull] ParseResultValues other) => !(other is null) && (ReferenceEquals(this, other) ||
                (UUID == other.UUID && SerialNumber == other.SerialNumber && Ordinal == other.Ordinal));

            public override bool Equals(object obj) => Equals(obj as ParseResultValues);

            public override int GetHashCode() => HashCode.Combine(UUID, SerialNumber, Ordinal);

            public override string ToString()
            {
                return (UUID.HasValue) ?
                    (SerialNumber.HasValue) ?
                        ((Ordinal.HasValue) ?
                            $"{{ UUID = {UUID.Value:d}, SerialNumber = {SerialNumber.Value:x4}, Ordinal = {Ordinal.Value:x2} }}"
                            : $"{{ UUID = {UUID.Value:d}, SerialNumber = {SerialNumber.Value:x4}, Ordinal = null }}"
                        )
                        : ((Ordinal.HasValue) ?
                            $"{{ UUID = {UUID.Value:d}, SerialNumber = null, Ordinal = {Ordinal.Value:x2} }}"
                            : $"{{ UUID = {UUID.Value:d}, SerialNumber = null, Ordinal = null }}"
                        )
                    : (SerialNumber.HasValue) ?
                        ((Ordinal.HasValue) ?
                            $"{{ UUID = null, SerialNumber = {SerialNumber.Value:x4}, Ordinal = {Ordinal.Value:x2} }}"
                            : $"{{ UUID = null, SerialNumber = {SerialNumber.Value:x4}, Ordinal = null }}"
                        )
                        : ((Ordinal.HasValue) ?
                            $"{{ UUID = null, SerialNumber = null, Ordinal = {Ordinal.Value:x2} }}"
                            : "{ UUID = null, SerialNumber = null, Ordinal = null }"
                        );
            }

        }
    }
}
