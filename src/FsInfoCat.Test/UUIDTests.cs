using FsInfoCat.Desktop.Model.Bits;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Test
{
    public class UUIDTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Binary16Test()
        {
            Binary16 target = new Binary16();
            byte expected = 0;
            ushort unsigned = 0;
            short signed = 0;
            Assert.That(target.Low, Is.EqualTo(expected));
            Assert.That(target.High, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));

            unsigned = 1;
            signed = 1;
            target = new Binary16(unsigned);
            expected = 1;
            Assert.That(target.Low, Is.EqualTo(expected));
            expected = 0;
            Assert.That(target.High, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            byte[] expBuffer = BitConverter.GetBytes(unsigned);
            byte[] actualBuffer = target.ToArray();
            Assert.That(actualBuffer.Length, Is.EqualTo(Binary16.BYTE_COUNT));
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            unsigned = 0xff00;
            signed = -256;
            target = new Binary16(unsigned);
            expected = 0;
            Assert.That(target.Low, Is.EqualTo(expected));
            expected = 0xff;
            Assert.That(target.High, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            expBuffer = BitConverter.GetBytes(unsigned);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            unsigned = 0xffff;
            signed = -1;
            target = new Binary16(signed);
            expected = 0xff;
            Assert.That(target.Low, Is.EqualTo(expected));
            Assert.That(target.High, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            expBuffer = BitConverter.GetBytes(unsigned);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
        }

        [Test]
        public void Binary32Test()
        {
            Binary32 target = new Binary32();
            ushort expected = 0;
            uint unsigned = 0;
            int signed = 0;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));

            unsigned = 1;
            signed = 1;
            target = new Binary32(unsigned);
            expected = 1;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            byte[] expBuffer = BitConverter.GetBytes(unsigned);
            byte[] actualBuffer = target.ToArray();
            Assert.That(actualBuffer.Length, Is.EqualTo(Binary32.BYTE_COUNT));
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            unsigned = 0xffff;
            signed = 0xffff;
            target = new Binary32(signed);
            expected = 0xffff;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            expBuffer = BitConverter.GetBytes(unsigned);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            unsigned = 0xffff0000;
            signed = -65536;
            target = new Binary32(unsigned);
            expected = 0;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0xffff;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            expBuffer = BitConverter.GetBytes(unsigned);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            unsigned = 0xffffffff;
            signed = -1;
            target = new Binary32(signed);
            expected = 0xffff;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(signed));
            Assert.That(target.Unsigned, Is.EqualTo(unsigned));
            expBuffer = BitConverter.GetBytes(unsigned);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
        }

        [Test]
        public void Binary64Test()
        {
            Binary64 target = new Binary64();
            uint expected = 0;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(0L));
            Assert.That(target.Unsigned, Is.EqualTo(0UL));

            target = new Binary64(1UL);
            expected = 1;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(1L));
            Assert.That(target.Unsigned, Is.EqualTo(1UL));
            byte[] expBuffer = BitConverter.GetBytes(1UL);
            byte[] actualBuffer = target.ToArray();
            Assert.That(actualBuffer.Length, Is.EqualTo(Binary64.BYTE_COUNT));
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            target = new Binary64(0xFFFFFFFFL);
            expected = 0xffffffff;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(0xFFFFFFFFL));
            Assert.That(target.Unsigned, Is.EqualTo(0xFFFFFFFFUL));
            expBuffer = BitConverter.GetBytes(0xFFFFFFFFUL);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            target = new Binary64(0xffffffff00000000UL);
            expected = 0;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            expected = 0xffffffff;
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(-4294967296L));
            Assert.That(target.Unsigned, Is.EqualTo(0xffffffff00000000UL));
            expBuffer = BitConverter.GetBytes(0xffffffff00000000UL);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            target = new Binary64(-1L);
            expected = 0xffffffff;
            Assert.That(target.Low.Unsigned, Is.EqualTo(expected));
            Assert.That(target.High.Unsigned, Is.EqualTo(expected));
            Assert.That(target.Signed, Is.EqualTo(-1L));
            Assert.That(target.Unsigned, Is.EqualTo(0xffffffffffffffffUL));
            expBuffer = BitConverter.GetBytes(0xffffffffffffffffUL);
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
        }

        [Test]
        public void Binary128Test()
        {
            Binary128 target = new Binary128();
            Assert.That(target.Low.Unsigned, Is.EqualTo(0UL));
            Assert.That(target.High.Unsigned, Is.EqualTo(0UL));
            Assert.That(target.UUID, Is.EqualTo(Guid.Empty));

            target = new Binary128(1UL, 0UL);
            Assert.That(target.Low.Unsigned, Is.EqualTo(1UL));
            Assert.That(target.High.Unsigned, Is.EqualTo(0UL));
            Assert.That(target.UUID, Is.Not.EqualTo(Guid.Empty));
            byte[] expBuffer = BitConverter.GetBytes(1UL).Concat(BitConverter.GetBytes(0UL)).ToArray();
            byte[] actualBuffer = target.ToArray();
            Assert.That(actualBuffer.Length, Is.EqualTo(Binary128.BYTE_COUNT));
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
            actualBuffer = target.UUID.ToByteArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            target = new Binary128(-1L, -1L);
            Assert.That(target.Low.Signed, Is.EqualTo(-1L));
            Assert.That(target.High.Signed, Is.EqualTo(-1L));
            Assert.That(target.UUID, Is.EqualTo(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff")));
            expBuffer = BitConverter.GetBytes(-1L).Concat(BitConverter.GetBytes(-1L)).ToArray();
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
            actualBuffer = target.UUID.ToByteArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));

            Guid guid = Guid.NewGuid();
            expBuffer = guid.ToByteArray();
            target = new Binary128(guid);
            Assert.That(target.UUID, Is.EqualTo(guid));
            actualBuffer = target.ToArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
            actualBuffer = target.UUID.ToByteArray();
            Assert.That(actualBuffer, Is.EqualTo(expBuffer));
            Assert.That(target.Low.Unsigned, Is.EqualTo(BitConverter.ToUInt64(expBuffer, 0)));
            Assert.That(target.High.Unsigned, Is.EqualTo(BitConverter.ToUInt64(expBuffer, 8)));
        }
    }
}
