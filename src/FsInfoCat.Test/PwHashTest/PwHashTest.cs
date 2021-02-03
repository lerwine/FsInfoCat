using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FsInfoCat.Util;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PwHashTest
    {
        public class HashResult
        {
            public byte[] HashBytes { get; }
            public byte[] SaltBytes { get; }
            public ulong[] AllBits { get; }
            public string ExpectedToString { get; }
            public ulong HashBits000_03f { get; }
            public ulong HashBits040_07f { get; }
            public ulong HashBits080_0bf { get; }
            public ulong HashBits0c0_0ff { get; }
            public ulong HashBits100_13f { get; }
            public ulong HashBits140_17f { get; }
            public ulong HashBits180_1bf { get; }
            public ulong HashBits1c0_1ff { get; }
            public ulong SaltBits { get; }
            internal HashResult(string b64HashAndSalt)
            {
                byte[] allBytes = Convert.FromBase64String(b64HashAndSalt);
                HashBytes = allBytes.Take(PwHash.HASH_BYTES_LENGTH).ToArray();
                SaltBytes = allBytes.Skip(PwHash.HASH_BYTES_LENGTH).ToArray();
                ExpectedToString = b64HashAndSalt;
                AllBits = new ulong[9];
                for (int i = 0; i < 9; i++)
                    AllBits[i] = BitConverter.ToUInt64(allBytes, i << 3);
                HashBits000_03f = AllBits[0];
                HashBits040_07f = AllBits[1];
                HashBits080_0bf = AllBits[2];
                HashBits0c0_0ff = AllBits[3];
                HashBits100_13f = AllBits[4];
                HashBits140_17f = AllBits[5];
                HashBits180_1bf = AllBits[6];
                HashBits1c0_1ff = AllBits[7];
                SaltBits = AllBits[8];
            }
            public IEnumerable<byte> GetAllBytes()
            {
                return HashBytes.Concat(SaltBytes);
            }
        }
        public class TestData
        {
            public string Password { get; }
            public HashResult Result1 { get; }
            public HashResult Result2 { get; }
            TestData(string password, string b64HashAndSalt1, string b64HashAndSalt2)
            {
                Password = password;
                Result1 = new HashResult(b64HashAndSalt1);
                Result2 = new HashResult(b64HashAndSalt2);
            }
            public static readonly TestData[] TestDataItems = new TestData[]
            {
                new TestData(" ", "mK01ofxZvvbFnMOxewqGKUQMs11R1NyRL9udIavY9AWcPftFiw8GYfKVZ2bHfPvcic1hfzl+xcF1epGegj4LmuSoWP4uYVlC",
                    "MnWRpq2LFMecUqzzuNoWrWWOPgy6+lN66qUSzh3b6sbUzx1gYKQQPpVcVwFnHYCSD5aF+5EY58mlUVBmKBmeeYAG4r01DmuU"),
                new TestData("Test", "lDzQy9SoZCFF4rmYklH7kkksw5faQvt3TvAalDv941FV1xyn4sBVBwhnbR/4IkIp1NJ11x6bSKkXdoUNAHMaUSAbarcLi7Cu",
                    "RzNCsJjHo/DkYzE3M7AKtRYjz/N556whUmhRLaYER6gblzQUTyf7ohBmCm99DzNf2+QIiGZBb8SnjLHlseZ0fIVkv+UDAMTV"),
                new TestData("ThisIsMyPassword", "SOWmrmZFbO8iHdm4r+O8Qe5mfV6kAJltl1DFO0GeVMBDI9lmAeMixe4Q4nsj8dPuBKKzPKAoR6xav92GinHNTkRbb6KPKLnB",
                    "cILw7m1q/iOI2PeV16B5fTrggtRGDGyzW+e9OkUbuwgfAz3cqvj/NQs36aSWAD9BTV3T0LrJCgh2ESV6jrYSelod712TTSBA")
            };
        }
        private static readonly byte[] _salt1 = new byte[] { 0x39, 0x4f, 0x3d, 0x56, 0xd4, 0x1c, 0xdb, 0xea };
        private static readonly byte[] _salt2 = new byte[] { 0x35, 0x02, 0x68, 0x46, 0xf3, 0x87, 0xe9, 0xf0 };

        // ulong hashBits000_03f, ulong hashBits040_07f, ulong hashBits080_0bf, ulong hashBits0c0_0ff, ulong hashBits100_13f, ulong hashBits140_17f, ulong hashBits180_1bf,
        // ulong hashBits1c0_1ff, ulong saltBits, HashResult expectedResult
        static object[] GetConstructor8TestCases()
        {
            return TestData.TestDataItems.Select(d =>
                    new object[] {
                        d.Result1.HashBits000_03f, d.Result1.HashBits040_07f, d.Result1.HashBits080_0bf, d.Result1.HashBits0c0_0ff, d.Result1.HashBits100_13f,
                        d.Result1.HashBits140_17f, d.Result1.HashBits180_1bf, d.Result1.HashBits1c0_1ff, d.Result1.SaltBits, d.Result1
                    }
                )
                .Concat(TestData.TestDataItems.Select(d =>
                    new object[] {
                        d.Result2.HashBits000_03f, d.Result2.HashBits040_07f, d.Result2.HashBits080_0bf, d.Result2.HashBits0c0_0ff, d.Result2.HashBits100_13f,
                        d.Result2.HashBits140_17f, d.Result2.HashBits180_1bf, d.Result2.HashBits1c0_1ff, d.Result2.SaltBits, d.Result2
                    }
                )).ToArray();
        }

        // IEnumerable<byte> sha512HashBytes, int hashStartIndex, IEnumerable<byte> saltBytes, int saltStartIndex, HashResult expectedResult
        static object[] GetConstructor4TestCases()
        {
            return TestData.TestDataItems.Select(d =>
                    new object[] { d.Result1.HashBytes, 0, d.Result1.SaltBytes, 0, d.Result1 })
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result1.HashBytes), i + 1, (new byte[i + 1]).Concat(d.Result1.GetAllBytes()), i + 1 + d.Result1.HashBytes.Length, d.Result1 }
                ))
                .Concat(TestData.TestDataItems.Select(d =>
                    new object[] { d.Result2.HashBytes, 0, d.Result2.SaltBytes, 0, d.Result2 }
                ))
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result2.HashBytes), i + 1, (new byte[i + 1]).Concat(d.Result2.GetAllBytes()), i + 1 + d.Result2.HashBytes.Length, d.Result2 }
                )).ToArray();
        }

        // IEnumerable<byte> hashAndSaltBytes, int startIndex, HashResult expectedResult
        static object[] GetConstructor2TestCases()
        {
            return TestData.TestDataItems.Select(d =>
                    new object[] { d.Result1.GetAllBytes().ToArray(), 0, d.Result1 }
                )
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result1.GetAllBytes()).ToArray(), i + 1, d.Result1 }
                ))
                .Concat(TestData.TestDataItems.Select(d =>
                    new object[] { d.Result1.GetAllBytes(), 0, d.Result1 }
                ))
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result1.GetAllBytes()), i + 1, d.Result1 }
                ))
                .Concat(TestData.TestDataItems.Select(d =>
                    new object[] { d.Result2.GetAllBytes().ToArray(), 0, d.Result2 }
                ))
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result2.GetAllBytes()).ToArray(), i + 1, d.Result2 }
                ))
                .Concat(TestData.TestDataItems.Select(d =>
                    new object[] { d.Result2.GetAllBytes(), 0, d.Result2 }
                ))
                .Concat(TestData.TestDataItems.Select((d, i) =>
                    new object[] { (new byte[i + 1]).Concat(d.Result2.GetAllBytes()), i + 1, d.Result2 }
                ))

                .ToArray();
        }

        // string base64EncodedHash, HashResult expectedResult
        static object[] GetImportTestCases()
        {
            return (new object[]
            {
                new object[] { null, null },
                new object[] { "", null }
            }).Concat(TestData.TestDataItems.Select(d =>
                new object[] { d.Result1.ExpectedToString, d.Result1 }
            )).Concat(TestData.TestDataItems.Select(d =>
                new object[] { d.Result2.ExpectedToString, d.Result2 }
            )).ToArray();
        }

        // string rawPW, bool nullExpected
        static object[] GetCreate1TestCases()
        {
            return (new object[]
            {
                new object[] { null, true },
                new object[] { "", true }
            }).Concat(TestData.TestDataItems.Select(d =>
                new object[] { d.Password, false }
            )).ToArray();
        }

        // string rawPW, ulong saltBits, HashResult expectedResult
        static object[] GetCreate2TestCases()
        {
            return (new object[]
            {
                new object[] { null, TestData.TestDataItems[0].Result1.SaltBits, null },
                new object[] { "", TestData.TestDataItems[1].Result1.SaltBits, null }
            }).Concat(TestData.TestDataItems.Select(d =>
                new object[] { d.Password, d.Result1.SaltBits, d.Result1 }
            )).Concat(TestData.TestDataItems.Select(d =>
                new object[] { d.Password, d.Result2.SaltBits, d.Result2 }
            )).ToArray();
        }

        // PwHash? hash, string rawPw, bool expectedResult
        static object[] GetTest2TestCases()
        {
            return (new object[]
            {
                new object[] { null, null, true },
                new object[] { null, "", true }
            }).Concat(TestData.TestDataItems.SelectMany(x =>
                new object[]
                {
                    new object[] { new PwHash(x.Result1.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(x.Result2.GetAllBytes(), 0), "", false }
                }
            )).Concat(TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Password, x.Password == y.Password },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Password, x.Password == y.Password }
                    }
                )
            )).ToArray();
        }

        // PwHash a, string rawPw, bool expectedResult
        static object[] GetTest1TestCases()
        {
            return TestData.TestDataItems.SelectMany(x =>
                new object[]
                {
                    new object[] { new PwHash(x.Result1.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(x.Result2.GetAllBytes(), 0), "", false }
                }
            ).Concat(TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Password, x.Password == y.Password },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Password, x.Password == y.Password }
                    }
                )
            )).ToArray();
        }

        // PwHash hash, string expectedResult
        static object[] GetToStringTestCases()
        {
            return TestData.TestDataItems.SelectMany(d => new object[]
            {
                new object[] { new PwHash(d.Result1.GetAllBytes(), 0), d.Result1.ExpectedToString },
                new object[] { new PwHash(d.Result2.GetAllBytes(), 0), d.Result2.ExpectedToString }
            }).ToArray();
        }

        // PwHash target, PwHash other, bool expectedResult
        static object[] GetEqualsPwHashTestCases()
        {
            return TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), new PwHash(y.Result1.GetAllBytes(), 0), x.Result1.ExpectedToString == y.Result1.ExpectedToString },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), new PwHash(y.Result2.GetAllBytes(), 0), x.Result2.ExpectedToString == y.Result2.ExpectedToString },
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), new PwHash(y.Result2.GetAllBytes(), 0), false },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), new PwHash(y.Result1.GetAllBytes(), 0), false }
                    }
                )
            ).ToArray();
        }

        // PwHash target, string other, bool expectedResult
        static object[] GetEqualsStringTestCases()
        {
            return TestData.TestDataItems.SelectMany(x =>
                new object[]
                {
                    new object[] { new PwHash(x.Result1.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(x.Result2.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(x.Result1.GetAllBytes(), 0), "", false },
                    new object[] { new PwHash(x.Result2.GetAllBytes(), 0), "", false }
                }
            ).Concat(TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Result1.ExpectedToString, x.Result1.ExpectedToString == y.Result1.ExpectedToString },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Result2.ExpectedToString, x.Result2.ExpectedToString == y.Result2.ExpectedToString },
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Result2.ExpectedToString, false },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Result1.ExpectedToString, false }
                    }
                )
            )).ToArray();
        }

        // PwHash target, object obj, bool expectedResult
        static object[] GetEqualsObjectTestCases()
        {
            return TestData.TestDataItems.SelectMany(d =>
                new object[]
                {
                    new object[] { new PwHash(d.Result1.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(d.Result2.GetAllBytes(), 0), null, false },
                    new object[] { new PwHash(d.Result1.GetAllBytes(), 0), true, false },
                    new object[] { new PwHash(d.Result2.GetAllBytes(), 0), false, false },
                    new object[] { new PwHash(d.Result1.GetAllBytes(), 0), "", false },
                    new object[] { new PwHash(d.Result2.GetAllBytes(), 0), "", false }
                }
            ).Concat(TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Result1.ExpectedToString, x.Result1.ExpectedToString == y.Result1.ExpectedToString },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Result2.ExpectedToString, x.Result2.ExpectedToString == y.Result2.ExpectedToString },
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), y.Result2.ExpectedToString, false },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), y.Result1.ExpectedToString, false }
                    }
                )
            )).Concat(TestData.TestDataItems.SelectMany(x =>
                TestData.TestDataItems.SelectMany(y =>
                    new object[]
                    {
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), new PwHash(y.Result1.GetAllBytes(), 0), x.Result1.ExpectedToString == y.Result1.ExpectedToString },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), new PwHash(y.Result2.GetAllBytes(), 0), x.Result2.ExpectedToString == y.Result2.ExpectedToString },
                        new object[] { new PwHash(x.Result1.GetAllBytes(), 0), new PwHash(y.Result2.GetAllBytes(), 0), false },
                        new object[] { new PwHash(x.Result2.GetAllBytes(), 0), new PwHash(y.Result1.GetAllBytes(), 0), false }
                    }
                )
            )).ToArray();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Property("Priority", 1)]
        public void ByteLengthTest()
        {
            Assert.AreEqual(_salt1.Length, PwHash.SALT_BYTES_LENGTH, "Salt byte length test failed");
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(Encoding.ASCII.GetBytes("TestPassword").Concat(_salt1).ToArray());
                Assert.AreEqual(sha.Hash.Length, PwHash.HASH_BYTES_LENGTH, "Hash byte length test failed");
            }
            Assert.AreEqual(PwHash.HASH_BYTES_LENGTH + PwHash.SALT_BYTES_LENGTH, PwHash.TOTAL_BYTES_LENGTH, "Total byte length test failed");
        }

        [TestCaseSource("GetConstructor8TestCases")]
        [Property("Priority", 2)]
        public void Constructor8Test(ulong hashBits000_03f, ulong hashBits040_07f, ulong hashBits080_0bf, ulong hashBits0c0_0ff, ulong hashBits100_13f,
            ulong hashBits140_17f,ulong hashBits180_1bf, ulong hashBits1c0_1ff, ulong saltBits, HashResult expectedResult)
        {
            PwHash target = new PwHash(hashBits000_03f, hashBits040_07f, hashBits080_0bf, hashBits0c0_0ff, hashBits100_13f, hashBits140_17f, hashBits180_1bf,
                hashBits1c0_1ff, saltBits);
            Assert.AreEqual(target.HashBits000_03f, hashBits000_03f);
            Assert.AreEqual(target.HashBits040_07f, hashBits040_07f);
            Assert.AreEqual(target.HashBits080_0bf, hashBits080_0bf);
            Assert.AreEqual(target.HashBits0c0_0ff, hashBits0c0_0ff);
            Assert.AreEqual(target.HashBits100_13f, hashBits100_13f);
            Assert.AreEqual(target.HashBits140_17f, hashBits140_17f);
            Assert.AreEqual(target.HashBits180_1bf, hashBits180_1bf);
            Assert.AreEqual(target.SaltBits, saltBits);
            byte[] bytes = target.GetSha512HashBytes().ToArray();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.HashBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.HashBytes), Convert.ToBase64String(bytes.ToArray()));
            bytes = target.GetSaltBytes();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.SaltBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.SaltBytes), Convert.ToBase64String(bytes.ToArray()));
            IEnumerable<byte> b = target.GetHashAndSaltBytes();
            Assert.IsNotNull(b);
            Assert.AreEqual(expectedResult.SaltBytes.Length + expectedResult.HashBytes.Length, b.Count());
            Assert.AreEqual(Convert.ToBase64String(expectedResult.GetAllBytes().ToArray()), Convert.ToBase64String(b.ToArray()));
            Assert.AreEqual(expectedResult.ExpectedToString, target.ToString());
        }

        [TestCaseSource("GetConstructor4TestCases")]
        [Property("Priority", 2)]
        public void Constructor4Test(IEnumerable<byte> sha512HashBytes, int hashStartIndex, IEnumerable<byte> saltBytes, int saltStartIndex, HashResult expectedResult)
        {
            PwHash target = new PwHash(sha512HashBytes, hashStartIndex, saltBytes, saltStartIndex);
            Assert.AreEqual(target.HashBits000_03f, expectedResult.HashBits000_03f);
            Assert.AreEqual(target.HashBits040_07f, expectedResult.HashBits040_07f);
            Assert.AreEqual(target.HashBits080_0bf, expectedResult.HashBits080_0bf);
            Assert.AreEqual(target.HashBits0c0_0ff, expectedResult.HashBits0c0_0ff);
            Assert.AreEqual(target.HashBits100_13f, expectedResult.HashBits100_13f);
            Assert.AreEqual(target.HashBits140_17f, expectedResult.HashBits140_17f);
            Assert.AreEqual(target.HashBits180_1bf, expectedResult.HashBits180_1bf);
            Assert.AreEqual(target.SaltBits, expectedResult.SaltBits);
            byte[] bytes = target.GetSha512HashBytes().ToArray();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.HashBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.HashBytes), Convert.ToBase64String(bytes.ToArray()));
            bytes = target.GetSaltBytes();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.SaltBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.SaltBytes), Convert.ToBase64String(bytes.ToArray()));
            IEnumerable<byte> b = target.GetHashAndSaltBytes();
            Assert.IsNotNull(b);
            Assert.AreEqual(expectedResult.SaltBytes.Length + expectedResult.HashBytes.Length, b.Count());
            Assert.AreEqual(Convert.ToBase64String(expectedResult.GetAllBytes().ToArray()), Convert.ToBase64String(b.ToArray()));
            Assert.AreEqual(expectedResult.ExpectedToString, target.ToString());
        }

        [TestCaseSource("GetConstructor2TestCases")]
        [Property("Priority", 2)]
        public void Constructor2Test(IEnumerable<byte> hashAndSaltBytes, int startIndex, HashResult expectedResult)
        {
            PwHash target = new PwHash(hashAndSaltBytes, startIndex);
            Assert.AreEqual(target.HashBits000_03f, expectedResult.HashBits000_03f);
            Assert.AreEqual(target.HashBits040_07f, expectedResult.HashBits040_07f);
            Assert.AreEqual(target.HashBits080_0bf, expectedResult.HashBits080_0bf);
            Assert.AreEqual(target.HashBits0c0_0ff, expectedResult.HashBits0c0_0ff);
            Assert.AreEqual(target.HashBits100_13f, expectedResult.HashBits100_13f);
            Assert.AreEqual(target.HashBits140_17f, expectedResult.HashBits140_17f);
            Assert.AreEqual(target.HashBits180_1bf, expectedResult.HashBits180_1bf);
            Assert.AreEqual(target.SaltBits, expectedResult.SaltBits);
            byte[] bytes = target.GetSha512HashBytes().ToArray();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.HashBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.HashBytes), Convert.ToBase64String(bytes.ToArray()));
            bytes = target.GetSaltBytes();
            Assert.IsNotNull(bytes);
            Assert.AreEqual(expectedResult.SaltBytes.Length, bytes.Length);
            Assert.AreEqual(Convert.ToBase64String(expectedResult.SaltBytes), Convert.ToBase64String(bytes.ToArray()));
            IEnumerable<byte> b = target.GetHashAndSaltBytes();
            Assert.IsNotNull(b);
            Assert.AreEqual(expectedResult.SaltBytes.Length + expectedResult.HashBytes.Length, b.Count());
            Assert.AreEqual(Convert.ToBase64String(expectedResult.GetAllBytes().ToArray()), Convert.ToBase64String(b.ToArray()));
            Assert.AreEqual(expectedResult.ExpectedToString, target.ToString());
        }

        [TestCaseSource("GetImportTestCases")]
        [Property("Priority", 3)]
        public void ImportTest(string base64EncodedHash, HashResult expectedResult)
        {
            PwHash? target = PwHash.Import(base64EncodedHash);
            if (null == expectedResult)
                Assert.IsFalse(target.HasValue);
            else
            {
                Assert.IsTrue(target.HasValue);
                Assert.AreEqual(target.Value.HashBits000_03f, expectedResult.HashBits000_03f);
                Assert.AreEqual(target.Value.HashBits040_07f, expectedResult.HashBits040_07f);
                Assert.AreEqual(target.Value.HashBits080_0bf, expectedResult.HashBits080_0bf);
                Assert.AreEqual(target.Value.HashBits0c0_0ff, expectedResult.HashBits0c0_0ff);
                Assert.AreEqual(target.Value.HashBits100_13f, expectedResult.HashBits100_13f);
                Assert.AreEqual(target.Value.HashBits140_17f, expectedResult.HashBits140_17f);
                Assert.AreEqual(target.Value.HashBits180_1bf, expectedResult.HashBits180_1bf);
                Assert.AreEqual(target.Value.SaltBits, expectedResult.SaltBits);
                byte[] bytes = target.Value.GetSha512HashBytes().ToArray();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(expectedResult.HashBytes.Length, bytes.Length);
                Assert.AreEqual(Convert.ToBase64String(expectedResult.HashBytes), Convert.ToBase64String(bytes.ToArray()));
                bytes = target.Value.GetSaltBytes();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(expectedResult.SaltBytes.Length, bytes.Length);
                Assert.AreEqual(Convert.ToBase64String(expectedResult.SaltBytes), Convert.ToBase64String(bytes.ToArray()));
                IEnumerable<byte> b = target.Value.GetHashAndSaltBytes();
                Assert.IsNotNull(b);
                Assert.AreEqual(expectedResult.SaltBytes.Length + expectedResult.HashBytes.Length, b.Count());
                Assert.AreEqual(Convert.ToBase64String(expectedResult.GetAllBytes().ToArray()), Convert.ToBase64String(b.ToArray()));
                Assert.AreEqual(expectedResult.ExpectedToString, target.Value.ToString());
            }
        }

        [TestCaseSource("GetCreate1TestCases")]
        [Property("Priority", 3)]
        public void Create1Test(string rawPW, bool nullExpected)
        {
            PwHash? target = PwHash.Create(rawPW);
            Assert.AreNotEqual(nullExpected, target.HasValue);
        }

        [TestCaseSource("GetCreate2TestCases")]
        [Property("Priority", 3)]
        public void Create2Test(string rawPW, ulong saltBits, HashResult expectedResult)
        {
            PwHash? target = PwHash.Create(rawPW, saltBits);
            if (null == expectedResult)
                Assert.IsFalse(target.HasValue);
            else
            {
                Assert.IsTrue(target.HasValue);
                Assert.AreEqual(target.Value.HashBits000_03f, expectedResult.HashBits000_03f);
                Assert.AreEqual(target.Value.HashBits040_07f, expectedResult.HashBits040_07f);
                Assert.AreEqual(target.Value.HashBits080_0bf, expectedResult.HashBits080_0bf);
                Assert.AreEqual(target.Value.HashBits0c0_0ff, expectedResult.HashBits0c0_0ff);
                Assert.AreEqual(target.Value.HashBits100_13f, expectedResult.HashBits100_13f);
                Assert.AreEqual(target.Value.HashBits140_17f, expectedResult.HashBits140_17f);
                Assert.AreEqual(target.Value.HashBits180_1bf, expectedResult.HashBits180_1bf);
                Assert.AreEqual(target.Value.SaltBits, expectedResult.SaltBits);
                byte[] bytes = target.Value.GetSha512HashBytes().ToArray();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(expectedResult.HashBytes.Length, bytes.Length);
                Assert.AreEqual(Convert.ToBase64String(expectedResult.HashBytes), Convert.ToBase64String(bytes.ToArray()));
                bytes = target.Value.GetSaltBytes();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(expectedResult.SaltBytes.Length, bytes.Length);
                Assert.AreEqual(Convert.ToBase64String(expectedResult.SaltBytes), Convert.ToBase64String(bytes.ToArray()));
                IEnumerable<byte> b = target.Value.GetHashAndSaltBytes();
                Assert.IsNotNull(b);
                Assert.AreEqual(expectedResult.SaltBytes.Length + expectedResult.HashBytes.Length, b.Count());
                Assert.AreEqual(Convert.ToBase64String(expectedResult.GetAllBytes().ToArray()), Convert.ToBase64String(b.ToArray()));
                Assert.AreEqual(expectedResult.ExpectedToString, target.Value.ToString());
            }
        }

        [TestCaseSource("GetTest2TestCases")]
        [Property("Priority", 3)]
        public void Test2Test(PwHash? hash, string rawPw, bool expectedResult)
        {
            bool actual = PwHash.Test(hash, rawPw);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCaseSource("GetTest1TestCases")]
        [Property("Priority", 3)]
        public void Test1Test(PwHash a, string rawPw, bool expectedResult)
        {
            bool actual = a.Test(rawPw);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCaseSource("GetToStringTestCases")]
        [Property("Priority", 3)]
        public void ToStringTest(PwHash hash, string expectedResult)
        {
            string actual = hash.ToString();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCaseSource("GetEqualsPwHashTestCases")]
        [Property("Priority", 3)]
        public void EqualsPwHashTest(PwHash target, PwHash other, bool expectedResult)
        {
            bool actual = target.Equals(other);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCaseSource("GetEqualsStringTestCases")]
        [Property("Priority", 3)]
        public void EqualsStringTest(PwHash target, string other, bool expectedResult)
        {
            bool actual = target.Equals(other);
            Assert.AreEqual(expectedResult, actual);
        }

        [TestCaseSource("GetEqualsObjectTestCases")]
        [Property("Priority", 3)]
        public void EqualsObjectTest(PwHash target, object obj, bool expectedResult)
        {
            bool actual = target.Equals(obj);
            Assert.AreEqual(expectedResult, actual);
        }
    }
}
