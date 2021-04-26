using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct UUID
    {
        /* Name string is a fully-qualified domain name */
        /* 6ba7b810-9dad-11d1-80b4-00c04fd430c8 */
        public static readonly UUID NameSpace_DNS = new UUID(0x6ba7b810, -25171 /*0x9dad*/, 0x11d1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /* Name string is a URL */
        /* 6ba7b811-9dad-11d1-80b4-00c04fd430c8 */
        public static readonly UUID NameSpace_URL = new UUID(0x6ba7b811, -25171 /*0x9dad*/, 0x11d1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /* Name string is an ISO OID */
        /* 6ba7b812-9dad-11d1-80b4-00c04fd430c8 */
        public static readonly UUID NameSpace_OID = new UUID(0x6ba7b812, -25171 /*0x9dad*/, 0x11d1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /* Name string is an X.500 DN (in DER or a text output format) */
        /* 6ba7b814-9dad-11d1-80b4-00c04fd430c8 */
        public static readonly UUID NameSpace_X500 = new UUID(0x6ba7b814, -25171 /*0x9dad*/, 0x11d1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        //[System.Runtime.InteropServices.FieldOffset(0)]
        //private ulong _bits_low;
        private int _time_low;
        private short _time_mid;
        private short _time_hi_and_version;
        private byte _clock_seq_hi_and_reserved;
        //[System.Runtime.InteropServices.FieldOffset(8)]
        //private ulong _bits_hi;
        private byte _clock_seq_low;
        private byte _node0;
        private byte _node1;
        private byte _node2;
        private byte _node3;
        private byte _node4;
        private byte _node5;

        public UUID(UUID source, int time_low, short time_mid, short time_hi_and_version)
        {
            _time_low = time_low;
            //htons
            _time_mid = time_mid;
            _time_hi_and_version = time_hi_and_version;
            _clock_seq_hi_and_reserved = source._clock_seq_hi_and_reserved;
            _clock_seq_low = source._clock_seq_low;
            _node0 = source._node0;
            _node1 = source._node1;
            _node2 = source._node2;
            _node3 = source._node3;
            _node4 = source._node4;
            _node5 = source._node5;
        }

        public UUID(int time_low, short time_mid, short time_hi_and_version,
            byte clock_seq_hi_and_reserved, byte clock_seq_low, byte node0, byte node1, byte node2, byte node3, byte node4, byte node5)
        {
            _time_low = time_low;
            _time_mid = time_mid;
            _time_hi_and_version = time_hi_and_version;
            _clock_seq_hi_and_reserved = clock_seq_hi_and_reserved;
            _clock_seq_low = clock_seq_low;
            _node0 = node0;
            _node1 = node1;
            _node2 = node2;
            _node3 = node3;
            _node4 = node4;
            _node5 = node5;
        }

        public IEnumerable<byte> GetBytes() => BitConverter.GetBytes(_time_low).Concat(BitConverter.GetBytes(_time_mid)).Concat(BitConverter.GetBytes(_time_hi_and_version))
            .Concat(new byte[] { _clock_seq_hi_and_reserved, _clock_seq_low, _node0, _node1, _node2, _node3, _node4, _node5 });

        public static UUID CreateMD5FromName(UUID namespaceId, string name)
        {
            //htonl = System.Net.IPAddress.NetworkToHostOrder; htns = System.Net.IPAddress.HostToNetworkOrder
            UUID uuid = new UUID(namespaceId, System.Net.IPAddress.HostToNetworkOrder(namespaceId._time_low), System.Net.IPAddress.HostToNetworkOrder(namespaceId._time_mid),
                System.Net.IPAddress.HostToNetworkOrder(namespaceId._time_hi_and_version));
            UInt128 u = new UInt128(uuid.GetBytes().ToArray());
            return new UUID(System.Net.IPAddress.NetworkToHostOrder(u.GetInt32(0)), (short)System.Net.IPAddress.NetworkToHostOrder(u.GetInt16(2)),
                (short)((System.Net.IPAddress.NetworkToHostOrder(u.GetInt16(3)) & 0x0FFF) | 0x3000), (byte)((u[8] & 0x3f) | 0x80), u[9], u[10], u[11], u[12], u[13], u[14], u[15]);
        }
    }
}
