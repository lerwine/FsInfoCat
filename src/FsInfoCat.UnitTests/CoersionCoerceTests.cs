using System;
using System.Collections.Generic;
using System.Linq;
using FsInfoCat.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests;

[TestClass]
public class CoersionCoerceTests
{
    public TestContext TestContext { get; set; }

    public static IEnumerable<object[]> GetArrayCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return [Array.Empty<int>(), Array.Empty<int>()];
        yield return [new[] { 1 }, new[] { 1 }];
        yield return [Enumerable.Repeat(1, 2), new[] { 1, 1 }];
        yield return [Array.Empty<byte>(), Array.Empty<int>()];
        yield return [Enumerable.Empty<int>(), Array.Empty<int>()];
        yield return [new[] { 1.0 }, new[] { 1 }];
        yield return [new[] { "12" }, new[] { 12 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetArrayCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ArrayCoersionCoerceSuccessTestMethod(object obj, int[] expected)
    {
        ArrayCoersion<int> target = ArrayCoersion<int>.Default;
        var actual = target.Coerce(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetArrayCoersionCoerceFailTestData()
    {
        yield return [new[] { uint.MaxValue }];
        yield return [new[] { "" }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetArrayCoersionCoerceFailTestData), DynamicDataSourceType.Method)]
    public void ArrayCoersionCoerceFailTestMethod(object obj)
    {
        ArrayCoersion<int> target = ArrayCoersion<int>.Default;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetNotEmptyOrNullValueArrayCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return [Array.Empty<int>(), null];
        yield return [new[] { 1 }, new[] { 1 }];
        yield return [Enumerable.Repeat(1, 2), new[] { 1, 1 }];
        yield return [Array.Empty<byte>(), null];
        yield return [Enumerable.Empty<int>(), null];
        yield return [new[] { 1.0 }, new[] { 1 }];
        yield return [new[] { "12" }, new[] { 12 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNotEmptyOrNullValueArrayCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NotEmptyOrNullValueArrayCoersionCoerceSuccessTestMethod(object obj, int[] expected)
    {
        NotEmptyOrNullValueArrayCoersion<int> target = new();
        var actual = target.Coerce(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetNotEmptyOrNullValueArrayCoersionCoerceFailTestData()
    {
        yield return [Enumerable.Repeat(uint.MaxValue, 2)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNotEmptyOrNullValueArrayCoersionCoerceFailTestData), DynamicDataSourceType.Method)]
    public void NotEmptyOrNullValueArrayCoersionCoerceFailTestMethod(object obj)
    {
        NotEmptyOrNullValueArrayCoersion<int> target = new();
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetByteArrayCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return [Array.Empty<byte>(), Array.Empty<byte>()];
        yield return [new byte[] { 1 }, new byte[] { 1 }];
        yield return [Array.Empty<int>(), Array.Empty<byte>()];
        yield return [Enumerable.Empty<int>(), Array.Empty<byte>()];
        yield return [Enumerable.Range(1, 5), new byte[] { 1, 2, 3, 4, 5 }];
        yield return [new int[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }];
        yield return [new string[] { "12" }, new byte[] { 12 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetByteArrayCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ByteArrayCoersionCoerceSuccessTestMethod(object obj, byte[] expected)
    {
        ByteArrayCoersion target = ByteArrayCoersion.Default;
        var actual = target.Coerce(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetByteArrayCoersionCoerceFailTestData()
    {
        yield return [""];
        yield return [new int[] { 1, 2, 256 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetByteArrayCoersionCoerceFailTestData), DynamicDataSourceType.Method)]
    public void ByteArrayCoersionCoerceFailTestMethod(object obj)
    {
        ByteArrayCoersion target = ByteArrayCoersion.Default;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetParseBinHexSuccessTestData()
    {
        yield return [null, new byte[] { }];
        yield return ["", new byte[] { }];
        yield return ["29", new byte[] { 0x29 }];
        yield return ["0x50", new byte[] { 0x50 }];
        yield return ["d0", new byte[] { 0xd0 }];
        yield return ["0xce", new byte[] { 0xce }];
        yield return ["4E", new byte[] { 0x4e }];
        yield return ["0xB7", new byte[] { 0xb7 }];
        yield return ["4111", new byte[] { 0x41, 0x11 }];
        yield return ["0x8436", new byte[] { 0x84, 0x36 }];
        yield return ["3ba6", new byte[] { 0x3b, 0xa6 }];
        yield return ["0xd695", new byte[] { 0xd6, 0x95 }];
        yield return ["786C", new byte[] { 0x78, 0x6c }];
        yield return ["0x7A48", new byte[] { 0x7a, 0x48 }];
        yield return ["fd151da17c1678a91d8211e016", new byte[] { 0xfd, 0x15, 0x1d, 0xa1, 0x7c, 0x16, 0x78, 0xa9, 0x1d, 0x82, 0x11, 0xe0, 0x16 }];
        yield return ["0xa197e576a55d54f710e2cbeae1e02b", new byte[] { 0xa1, 0x97, 0xe5, 0x76, 0xa5, 0x5d, 0x54, 0xf7, 0x10, 0xe2, 0xcb, 0xea, 0xe1, 0xe0, 0x2b }];
        yield return ["1C46679CABE56E4909357139E16E43EDC3", new byte[] { 0x1c, 0x46, 0x67, 0x9c, 0xab, 0xe5, 0x6e, 0x49, 0x09, 0x35, 0x71, 0x39, 0xe1, 0x6e, 0x43, 0xed, 0xc3 }];
        yield return ["0xED723718DF4D69A8D20FE5DD", new byte[] { 0xed, 0x72, 0x37, 0x18, 0xdf, 0x4d, 0x69, 0xa8, 0xd2, 0x0f, 0xe5, 0xdd }];
        yield return ["78-f6-5b-2c 50-54-66-65`r`n1c-77-ed-33 ca-04-27-28`r`n", new byte[]
        {
            0x78, 0xf6, 0x5b, 0x2c, 0x50, 0x54, 0x66, 0x65,
            0x1c, 0x77, 0xed, 0x33, 0xca, 0x04, 0x27, 0x28
        }];
        yield return ["F2-FA-67-B5 C4-19-9B-C5`n77-4B-B0-37 DD-A1-46-F0`n", new byte[]
        {
            0xf2, 0xfa, 0x67, 0xb5, 0xc4, 0x19, 0x9b, 0xc5,
            0x77, 0x4b, 0xb0, 0x37, 0xdd, 0xa1, 0x46, 0xf0
        }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBinHex(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetParseBinHexSuccessTestData), DynamicDataSourceType.Method)]
    public void ParseBinHexSuccessTestMethod(string input, byte[] expected)
    {
        IEnumerable<byte> actual = ByteArrayCoersion.ParseBinHex(input);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, [.. actual]);
    }

    public static IEnumerable<object[]> GetParseBinHexFailTestData()
    {
        yield return ["0"];
        yield return ["0x"];
        yield return ["4"];
        yield return ["0x3"];
        yield return ["E"];
        yield return ["0xE"];
        yield return ["431"];
        yield return ["0x79f"];
        yield return ["611c9961b2526b00e934829b16be5"];
        yield return ["0xfc4bc039383afae745f6586f7529f"];
        yield return ["D5C4DC26A872A05940B77DCE9D523BE"];
        yield return ["0xAB881553542058A1A97B9A8B6AB4378"];
        yield return ["xf1"];
        yield return ["x66"];
        yield return ["x9b6a"];
        yield return ["xD7D1"];
        yield return ["x1be5a8bdf7636b010b47ce5fd2ae5a6981ee46"];
        yield return ["x78D29C1385DE4DE586B62253"];
        yield return ["f6-49-02-35 46-fe-58-93`n17-db-6-24 ed-1c-4b-f5"];
        yield return ["e4-1e-8f-73 17-8e3-ca-ec`n49-f9-cb-17 c9-b5-8e-76"];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBinHex(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetParseBinHexFailTestData), DynamicDataSourceType.Method)]
    public void ParseBinHexFailTestMethod(string input)
    {
        Assert.Throws<FormatException>(() => ByteArrayCoersion.ParseBinHex(input));
    }

    public static IEnumerable<object[]> GetTryParseBinHexTestData()
    {
        yield return [null, new TryGetExpected<byte[]> { Returned = true, Result = [] }];
        yield return ["", new TryGetExpected<byte[]> { Returned = true, Result = [] }];
        yield return ["42", new TryGetExpected<byte[]> { Returned = true, Result = [0x42] }];
        yield return ["0x99", new TryGetExpected<byte[]> { Returned = true, Result = [0x99] }];
        yield return ["a4", new TryGetExpected<byte[]> { Returned = true, Result = [0xa4] }];
        yield return ["D8", new TryGetExpected<byte[]> { Returned = true, Result = [0xd8] }];
        yield return ["0xb9", new TryGetExpected<byte[]> { Returned = true, Result = [0xb9] }];
        yield return ["0xBD", new TryGetExpected<byte[]> { Returned = true, Result = [0xbd] }];
        yield return ["7292", new TryGetExpected<byte[]> { Returned = true, Result = [0x72, 0x92] }];
        yield return ["0x9036", new TryGetExpected<byte[]> { Returned = true, Result = [0x90, 0x36] }];
        yield return ["bb62", new TryGetExpected<byte[]> { Returned = true, Result = [0xbb, 0x62] }];
        yield return ["2F39", new TryGetExpected<byte[]> { Returned = true, Result = [0x2f, 0x39] }];
        yield return ["0xe5dd", new TryGetExpected<byte[]> { Returned = true, Result = [0xe5, 0xdd] }];
        yield return ["0x369D", new TryGetExpected<byte[]> { Returned = true, Result = [0x36, 0x9d] }];
        yield return ["4535e6b6cbb02961550fcdd79e2c0ff0948c", new TryGetExpected<byte[]> { Returned = true, Result = [0x45, 0x35, 0xe6, 0xb6, 0xcb, 0xb0, 0x29, 0x61, 0x55, 0x0f, 0xcd, 0xd7, 0x9e, 0x2c, 0x0f, 0xf0, 0x94, 0x8c] }];
        yield return ["77494A8F4BEF414D9BC10F85C5CD17", new TryGetExpected<byte[]> { Returned = true, Result = [0x77, 0x49, 0x4a, 0x8f, 0x4b, 0xef, 0x41, 0x4d, 0x9b, 0xc1, 0x0f, 0x85, 0xc5, 0xcd, 0x17] }];
        yield return ["0x3de611f8d166d9af1e6ed95b40221934ea60", new TryGetExpected<byte[]> { Returned = true, Result = [0x3d, 0xe6, 0x11, 0xf8, 0xd1, 0x66, 0xd9, 0xaf, 0x1e, 0x6e, 0xd9, 0x5b, 0x40, 0x22, 0x19, 0x34, 0xea, 0x60] }];
        yield return ["0xCB67A1F829AD2420FD3E", new TryGetExpected<byte[]> { Returned = true, Result = [0xcb, 0x67, 0xa1, 0xf8, 0x29, 0xad, 0x24, 0x20, 0xfd, 0x3e] }];
        yield return ["58-be-fc-1f 83-f0-b3-60`r`n6e-9d-f7-8a c4-3c-b7-8a`r`n", new TryGetExpected<byte[]> { Returned = true, Result = [
            0x58, 0xbe, 0xfc, 0x1f, 0x83, 0xf0, 0xb3, 0x60,
            0x6e, 0x9d, 0xf7, 0x8a, 0xc4, 0x3c, 0xb7, 0x8a
        ] }];
        yield return ["02-AC-96-67 2E-F5-73-C2`n3F-59-B7-54 A9-1B-67-9F`n", new TryGetExpected<byte[]> { Returned = true, Result = [
            0x02, 0xac, 0x96, 0x67, 0x2e, 0xf5, 0x73, 0xc2,
            0x3f, 0x59, 0xb7, 0x54, 0xa9, 0x1b, 0x67, 0x9f
        ] }];
        yield return ["x68", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["9", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0x7", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["x5d", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["xD9", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["b", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["C", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0xA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0xB", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["328", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["x2084", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0x875", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["xe68a", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["xC6E6", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["bb62", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["3DF", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0x2c1", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0xE4F", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["x1602aeb695f2", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["x3072CBD6422EEF62AA0C", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["58c6db7b119eb585eee6c6f", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["64D0A78D978E11B", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0x625525cc88a5ea0f2", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["0x494B2E058DBDE6739836FBC77FD65", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["d6-58-05-35 ca-1-28-28`r`ne6-8c-18-aa 9a-76-35-51`r`n", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["BA-1B-11-DA 42-C6-D9-DC`nA1-82-24-7A B3-89-AF-DE`n", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["25-b7-d8-53 ef-58-6a-16`r`n9d-386-1f-e9 c7-74-e0-dc`r`n", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["68-98-3B-CCB 60-0C-0A-0A`nA8-AE-86-84 7E-93-B6-6A`n", new TryGetExpected<byte[]> { Returned = false, Result = null }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.TryParseBinHex(string, out IEnumerable{byte})"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTryParseBinHexTestData), DynamicDataSourceType.Method)]
    public void TryParseBinHexTestMethod(string input, TryGetExpected<byte[]> expected)
    {
        bool actualReturned = ByteArrayCoersion.TryParseBinHex(input, out IEnumerable<byte> actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        if (expected.Result is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expected.Result, actualResult);
        }
    }

    public static IEnumerable<object[]> GetParseBase64SuccessTestData()
    {
        yield return [null, new byte[] { }];
        yield return ["", new byte[] { }];
        yield return ["AA==", new byte[] { 0x00 }];
        yield return ["ZQ==", new byte[] { 0x65 }];
        yield return ["/w==", new byte[] { 0xff }];
        yield return ["AAA=", new byte[] { 0x00, 0x00 }];
        yield return ["1k8=", new byte[] { 0xd6, 0x4f }];
        yield return ["//8=", new byte[] { 0xff, 0xff }];
        yield return ["AAAA", new byte[] { 0x00, 0x00, 0x00 }];
        yield return ["IvbW", new byte[] { 0x22, 0xf6, 0xd6 }];
        yield return ["////", new byte[] { 0xff, 0xff, 0xff }];
        yield return ["AAAAAA==", new byte[] { 0x00, 0x00, 0x00, 0x00 }];
        yield return ["CKVXkw==", new byte[] { 0x08, 0xa5, 0x57, 0x93 }];
        yield return ["/////w==", new byte[] { 0xff, 0xff, 0xff, 0xff }];
        yield return ["AAAAAAA=", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }];
        yield return ["p+o2kx4=", new byte[] { 0xa7, 0xea, 0x36, 0x93, 0x1e }];
        yield return ["//////8=", new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff }];
        yield return ["AAAAAAAA", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }];
        yield return ["b8kQucuF", new byte[] { 0x6f, 0xc9, 0x10, 0xb9, 0xcb, 0x85 }];
        yield return ["////////", new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        }];
        yield return ["QHjvfvHCelnogDmfJVjRKmKXxr2LVUg1b4YvX39Ahc+MVE+sJrZ2xI32cQHuyxNOG6kOwJvGYw==", new byte[]
        {
            0x40, 0x78, 0xef, 0x7e, 0xf1, 0xc2, 0x7a, 0x59, 0xe8, 0x80, 0x39, 0x9f, 0x25, 0x58, 0xd1, 0x2a, 0x62, 0x97, 0xc6, 0xbd, 0x8b, 0x55, 0x48, 0x35, 0x6f, 0x86, 0x2f, 0x5f,
            0x7f, 0x40, 0x85, 0xcf, 0x8c, 0x54, 0x4f, 0xac, 0x26, 0xb6, 0x76, 0xc4, 0x8d, 0xf6, 0x71, 0x01, 0xee, 0xcb, 0x13, 0x4e, 0x1b, 0xa9, 0x0e, 0xc0, 0x9b, 0xc6, 0x63
        }];
        yield return ["/////////////////////////////////////////////////////////////////////////w==", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        }];
        yield return ["Y5y6V/euIESooARHYn6wrN9v9XPRkraVy0X41HXaSkrFKa7leZzXiuNN3hfmf9/pkrYG/I/DFLk=", new byte[]
        {
            0x63, 0x9c, 0xba, 0x57, 0xf7, 0xae, 0x20, 0x44, 0xa8, 0xa0, 0x04, 0x47, 0x62, 0x7e, 0xb0, 0xac, 0xdf, 0x6f, 0xf5, 0x73, 0xd1, 0x92, 0xb6, 0x95, 0xcb, 0x45, 0xf8, 0xd4,
            0x75, 0xda, 0x4a, 0x4a, 0xc5, 0x29, 0xae, 0xe5, 0x79, 0x9c, 0xd7, 0x8a, 0xe3, 0x4d, 0xde, 0x17, 0xe6, 0x7f, 0xdf, 0xe9, 0x92, 0xb6, 0x06, 0xfc, 0x8f, 0xc3, 0x14, 0xb9
        }];
        yield return ["//////////////////////////////////////////////////////////////////////////8=", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        }];
        yield return ["D0FRpnjhrXmwpmB+ppIDaJD4AgeFqYubkGjSUqYJHP6K2RVnHqVz7Hm2S87x9sXPk1C1+MLvRFrb", new byte[]
        {
            0x0f, 0x41, 0x51, 0xa6, 0x78, 0xe1, 0xad, 0x79, 0xb0, 0xa6, 0x60, 0x7e, 0xa6, 0x92, 0x03, 0x68, 0x90, 0xf8, 0x02, 0x07, 0x85, 0xa9, 0x8b, 0x9b, 0x90, 0x68, 0xd2, 0x52,
            0xa6, 0x09, 0x1c, 0xfe, 0x8a, 0xd9, 0x15, 0x67, 0x1e, 0xa5, 0x73, 0xec, 0x79, 0xb6, 0x4b, 0xce, 0xf1, 0xf6, 0xc5, 0xcf, 0x93, 0x50, 0xb5, 0xf8, 0xc2, 0xef, 0x44, 0x5a, 0xdb
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00
        }];
        yield return ["MSsw8UsVSulqfsRDi/V10KulN2tJcwGLXZHJxudtt07xqhfMrjwRSEY2WudaFvpRng72Csv583+VIA==", new byte[]
        {
            0x31, 0x2b, 0x30, 0xf1, 0x4b, 0x15, 0x4a, 0xe9, 0x6a, 0x7e, 0xc4, 0x43, 0x8b, 0xf5, 0x75, 0xd0, 0xab, 0xa5, 0x37, 0x6b, 0x49, 0x73, 0x01, 0x8b, 0x5d, 0x91, 0xc9, 0xc6,
            0xe7, 0x6d, 0xb7, 0x4e, 0xf1, 0xaa, 0x17, 0xcc, 0xae, 0x3c, 0x11, 0x48, 0x46, 0x36, 0x5a, 0xe7, 0x5a, 0x16, 0xfa, 0x51, 0x9e, 0x0e, 0xf6, 0x0a, 0xcb, 0xf9, 0xf3, 0x7f, 0x95,
            0x20
        }];
        yield return ["/////////////////////////////////////////////////////////////////////////////w==", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAA==", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00
        }];
        yield return ["AZxHQX4ZcLQ8/7iJNhkag5DA8xHa+k2b7BAfeFeo/TI2AIxFK76r0gG18RW8pY0LcedkJeEO77y7`r`nig==", new byte[]
        {
            0x01, 0x9c, 0x47, 0x41, 0x7e, 0x19, 0x70, 0xb4, 0x3c, 0xff, 0xb8, 0x89, 0x36, 0x19, 0x1a, 0x83, 0x90, 0xc0, 0xf3, 0x11, 0xda, 0xfa, 0x4d, 0x9b, 0xec, 0x10, 0x1f, 0x78,
            0x57, 0xa8, 0xfd, 0x32, 0x36, 0x00, 0x8c, 0x45, 0x2b, 0xbe, 0xab, 0xd2, 0x01, 0xb5, 0xf1, 0x15, 0xbc, 0xa5, 0x8d, 0x0b, 0x71, 0xe7, 0x64, 0x25, 0xe1, 0x0e, 0xef, 0xbc, 0xbb,
            0x8a
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n/w==", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00
        }];
        yield return ["xOi8QAO+zbqgAQ3ogELwPLHOcg6+8HfoHOhNqY+Ka9AZTrdN2uM3x+n7lNPvG7E9KUjFgZrKBdp2RH8=", new byte[]
        {
            0xc4, 0xe8, 0xbc, 0x40, 0x03, 0xbe, 0xcd, 0xba, 0xa0, 0x01, 0x0d, 0xe8, 0x80, 0x42, 0xf0, 0x3c, 0xb1, 0xce, 0x72, 0x0e, 0xbe, 0xf0, 0x77, 0xe8, 0x1c, 0xe8, 0x4d, 0xa9,
            0x8f, 0x8a, 0x6b, 0xd0, 0x19, 0x4e, 0xb7, 0x4d, 0xda, 0xe3, 0x37, 0xc7, 0xe9, 0xfb, 0x94, 0xd3, 0xef, 0x1b, 0xb1, 0x3d, 0x29, 0x48, 0xc5, 0x81, 0x9a, 0xca, 0x05, 0xda, 0x76,
            0x44, 0x7f
        }];
        yield return ["//////////////////////////////////////////////////////////////////////////////8=", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA=", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00
        }];
        yield return ["fmRrLzz90TEZrTS5JkhBZm4vP0yTPr0qTq2E4M53bczvu/94Ht2y0IWk7zc/DZVUWEqmuMsWMxI1`r`nsOM=", new byte[]
        {
            0x7e, 0x64, 0x6b, 0x2f, 0x3c, 0xfd, 0xd1, 0x31, 0x19, 0xad, 0x34, 0xb9, 0x26, 0x48, 0x41, 0x66, 0x6e, 0x2f, 0x3f, 0x4c, 0x93, 0x3e, 0xbd, 0x2a, 0x4e, 0xad, 0x84, 0xe0,
            0xce, 0x77, 0x6d, 0xcc, 0xef, 0xbb, 0xff, 0x78, 0x1e, 0xdd, 0xb2, 0xd0, 0x85, 0xa4, 0xef, 0x37, 0x3f, 0x0d, 0x95, 0x54, 0x58, 0x4a, 0xa6, 0xb8, 0xcb, 0x16, 0x33, 0x12, 0x35,
            0xb0, 0xe3
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n//8=", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00
        }];
        yield return ["7nwqAsEDN8KinktMrc0amyPnpq/v8GlCDBoR39C9xBohok8DAcHlchZ5CC0wco8Y8XhNjsLEucpmHHah", new byte[]
        {
            0xee, 0x7c, 0x2a, 0x02, 0xc1, 0x03, 0x37, 0xc2, 0xa2, 0x9e, 0x4b, 0x4c, 0xad, 0xcd, 0x1a, 0x9b, 0x23, 0xe7, 0xa6, 0xaf, 0xef, 0xf0, 0x69, 0x42, 0x0c, 0x1a, 0x11, 0xdf,
            0xd0, 0xbd, 0xc4, 0x1a, 0x21, 0xa2, 0x4f, 0x03, 0x01, 0xc1, 0xe5, 0x72, 0x16, 0x79, 0x08, 0x2d, 0x30, 0x72, 0x8f, 0x18, 0xf1, 0x78, 0x4d, 0x8e, 0xc2, 0xc4, 0xb9, 0xca, 0x66,
            0x1c, 0x76, 0xa1
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////////", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAAA", new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        }];
        yield return ["wTx2vijxnAG1nR/3dJFxaxCnA0lhjtPUVBXiyg/YkMtut6OE8XlqvNUvFI5OTn42xeSoANpcCWnU`r`nMXYb", new byte[]
        {
            0xc1, 0x3c, 0x76, 0xbe, 0x28, 0xf1, 0x9c, 0x01, 0xb5, 0x9d, 0x1f, 0xf7, 0x74, 0x91, 0x71, 0x6b, 0x10, 0xa7, 0x03, 0x49, 0x61, 0x8e, 0xd3, 0xd4, 0x54, 0x15, 0xe2, 0xca,
            0x0f, 0xd8, 0x90, 0xcb, 0x6e, 0xb7, 0xa3, 0x84, 0xf1, 0x79, 0x6a, 0xbc, 0xd5, 0x2f, 0x14, 0x8e, 0x4e, 0x4e, 0x7e, 0x36, 0xc5, 0xe4, 0xa8, 0x00, 0xda, 0x5c, 0x09, 0x69,
            0xd4, 0x31, 0x76, 0x1b
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n////", new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff
        }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBase64(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetParseBase64SuccessTestData), DynamicDataSourceType.Method)]
    public void ParseBase64SuccessTestMethod(string input, byte[] expected)
    {
        IEnumerable<byte> actual = ByteArrayCoersion.ParseBase64(input);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, [.. actual]);
    }

    public static IEnumerable<object[]> GetParseBase64FailTestData()
    {
        yield return ["="];
        yield return ["=="];
        yield return ["s"];
        yield return ["0="];
        yield return ["+=="];
        yield return ["AA="];
        yield return ["0=="];
        yield return ["/w="];
        yield return ["AA="];
        yield return ["EtM"];
        yield return ["//="];
        yield return ["AAA"];
        yield return ["ksZ"];
        yield return ["///"];
        yield return ["AAAAA=="];
        yield return ["UYayuA="];
        yield return ["////w=="];
        yield return ["AAAAAAA"];
        yield return ["E145mg="];
        yield return ["//////8"];
        yield return ["AAAAAAA"];
        yield return ["moqpC9c"];
        yield return ["///////"];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="];
        yield return ["6JN3ST1kuglh1XxcNIfQVnrHhgBAvc+TYlnKEBdMcnhLCARBXSJLzjxgr9KRA45s1eOt8IjOw=="];
        yield return ["/////////////////////////////////////////////////////////////////////////w="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="];
        yield return ["Y2kzaoAEmvO0LShVIxLgPujXMRvovekihAMk7rjHlnt7OMIQCtc2hZa51xYjD4M8sHe8mBdRo/c"];
        yield return ["/////////////////////////////////////////////////////////////////////////8="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"];
        yield return ["2yeDSNa2z+Z/2S/VlZPiQZJLodT5SEzmt/7k6iIYDLgkWDqybYvZk4eUMIevi64NoFvVNYd/yTG"];
        yield return ["///////////////////////////////////////////////////////////////////////////"];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=="];
        yield return ["e9Z11+tZtql4gsaQBJwfLAnbiM+9x7uZzWaIKwKqkMzJ7RM58nAI5DINh4GZn12Mb9YgnOdwzfMItw="];
        yield return ["////////////////////////////////////////////////////////////////////////////w=="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAA="];
        yield return ["hOXekd02pYz1XDucu1RG4cATMfVCyKV9EjHDAJT9G3vQXR0J9Yvy46ag14fwk0qi06dX8lqUQGg`r`n7Q=="];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n/w="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="];
        yield return ["TMWTLGgjtqrdFBolJ2IV1agc2PPJk5G7BfbGUq5o6Y03w8RBsiLGHpZ1txyEWZIOmAvNQ8EL+GlYfzE"];
        yield return ["//////////////////////////////////////////////////////////////////////////////8="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA="];
        yield return ["XLk3k/27M4069XNcPgU01UAFdLRZEiVUe6JFefOV7qAhNmFCrnA98UlkK96FH1EN5fEXQemuDys1`r`nKK0"];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n//8="];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"];
        yield return ["2gPh8s1d/PSrc1ZJzihFhs6RnkglQa7c9WcjBl6Oihz3efu1kUJFKQSWIdFeBQBK9YDXkhyx/EHfgVg"];
        yield return ["///////////////////////////////////////////////////////////////////////////////"];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA"];
        yield return ["k14s4hDDKchlI4D2GjXctIBDKpL59vAzcpSBgnZSAtkjaEXR2d6bs6SCaqvQGGfkWbBNslWRW6J`r`nochY"];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n///"];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBase64(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetParseBase64FailTestData), DynamicDataSourceType.Method)]
    public void ParseBase64FailTestMethod(string input)
    {
        Assert.Throws<FormatException>(() => ByteArrayCoersion.ParseBase64(input));
    }

    public static IEnumerable<object[]> GetTryParseBase64TestData()
    {
        yield return ["AA==", new TryGetExpected<byte[]> { Returned = true, Result = [0x00] }];
        yield return ["Cw==", new TryGetExpected<byte[]> { Returned = true, Result = [0x0b] }];
        yield return ["/w==", new TryGetExpected<byte[]> { Returned = true, Result = [0xff] }];
        yield return ["AAA=", new TryGetExpected<byte[]> { Returned = true, Result = [0x00, 0x00] }];
        yield return ["y8o=", new TryGetExpected<byte[]> { Returned = true, Result = [0xcb, 0xca] }];
        yield return ["//8=", new TryGetExpected<byte[]> { Returned = true, Result = [0xff, 0xff] }];
        yield return ["AAAA", new TryGetExpected<byte[]> { Returned = true, Result = [0x00, 0x00, 0x00] }];
        yield return ["dDgq", new TryGetExpected<byte[]> { Returned = true, Result = [0x74, 0x38, 0x2a] }];
        yield return ["////", new TryGetExpected<byte[]> { Returned = true, Result = [0xff, 0xff, 0xff] }];
        yield return ["AAAAAA==", new TryGetExpected<byte[]> { Returned = true, Result = [0x00, 0x00, 0x00, 0x00] }];
        yield return ["pVBtag==", new TryGetExpected<byte[]> { Returned = true, Result = [0xa5, 0x50, 0x6d, 0x6a] }];
        yield return ["/////w==", new TryGetExpected<byte[]> { Returned = true, Result = [0xff, 0xff, 0xff, 0xff] }];
        yield return ["AAAAAAA=", new TryGetExpected<byte[]> { Returned = true, Result = [0x00, 0x00, 0x00, 0x00, 0x00] }];
        yield return ["FxDfEmk=", new TryGetExpected<byte[]> { Returned = true, Result = [0x17, 0x10, 0xdf, 0x12, 0x69] }];
        yield return ["//////8=", new TryGetExpected<byte[]> { Returned = true, Result = [0xff, 0xff, 0xff, 0xff, 0xff] }];
        yield return ["AAAAAAAA", new TryGetExpected<byte[]> { Returned = true, Result = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00] }];
        yield return ["gCpnJ722", new TryGetExpected<byte[]> { Returned = true, Result = [0x80, 0x2a, 0x67, 0x27, 0xbd, 0xb6] }];
        yield return ["////////", new TryGetExpected<byte[]> { Returned = true, Result = [0xff, 0xff, 0xff, 0xff, 0xff, 0xff] }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        }];
        yield return ["XevkNcquA8ELbO6py3oev28ZKfKcGHZo9BY4hJZXkoB71gI0FzcncGpQ4xRziE2aGwvLNP5B9w==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x5d, 0xeb, 0xe4, 0x35, 0xca, 0xae, 0x03, 0xc1, 0x0b, 0x6c, 0xee, 0xa9, 0xcb, 0x7a, 0x1e, 0xbf, 0x6f, 0x19, 0x29, 0xf2, 0x9c, 0x18, 0x76, 0x68, 0xf4, 0x16, 0x38, 0x84,
                0x96, 0x57, 0x92, 0x80, 0x7b, 0xd6, 0x02, 0x34, 0x17, 0x37, 0x27, 0x70, 0x6a, 0x50, 0xe3, 0x14, 0x73, 0x88, 0x4d, 0x9a, 0x1b, 0x0b, 0xcb, 0x34, 0xfe, 0x41, 0xf7
            ]
        }];
        yield return ["/////////////////////////////////////////////////////////////////////////w==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        }];
        yield return ["3VC1LM5BDe0cVdTsxmWPM9uij36dhFupngKq8+uGJ+C2XE7qud4pvLNzbRM2irvMgpPz6KEGiS0=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xdd, 0x50, 0xb5, 0x2c, 0xce, 0x41, 0x0d, 0xed, 0x1c, 0x55, 0xd4, 0xec, 0xc6, 0x65, 0x8f, 0x33, 0xdb, 0xa2, 0x8f, 0x7e, 0x9d, 0x84, 0x5b, 0xa9, 0x9e, 0x02, 0xaa, 0xf3,
                0xeb, 0x86, 0x27, 0xe0, 0xb6, 0x5c, 0x4e, 0xea, 0xb9, 0xde, 0x29, 0xbc, 0xb3, 0x73, 0x6d, 0x13, 0x36, 0x8a, 0xbb, 0xcc, 0x82, 0x93, 0xf3, 0xe8, 0xa1, 0x06, 0x89, 0x2d
            ]
        }];
        yield return ["//////////////////////////////////////////////////////////////////////////8=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        }];
        yield return ["AZqXQeSk7fNIKL2M0eFTcxvCU5SwQcE/pJiBWnPFgCeHHTAGKhG6vA/2OcndXeKQt+2/dTlSNrQW", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x01, 0x9a, 0x97, 0x41, 0xe4, 0xa4, 0xed, 0xf3, 0x48, 0x28, 0xbd, 0x8c, 0xd1, 0xe1, 0x53, 0x73, 0x1b, 0xc2, 0x53, 0x94, 0xb0, 0x41, 0xc1, 0x3f, 0xa4, 0x98, 0x81, 0x5a,
                0x73, 0xc5, 0x80, 0x27, 0x87, 0x1d, 0x30, 0x06, 0x2a, 0x11, 0xba, 0xbc, 0x0f, 0xf6, 0x39, 0xc9, 0xdd, 0x5d, 0xe2, 0x90, 0xb7, 0xed, 0xbf, 0x75, 0x39, 0x52, 0x36, 0xb4, 0x16
            ]
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        }];
        yield return ["5LS3dd1/Puj+1yKG8qqzQsniVKU61ZMSZQE/7wjWLVtN5GAaAFjP9fxb/+8Dxe8h8/YQLDyi1sGDGw==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xe4, 0xb4, 0xb7, 0x75, 0xdd, 0x7f, 0x3e, 0xe8, 0xfe, 0xd7, 0x22, 0x86, 0xf2, 0xaa, 0xb3, 0x42, 0xc9, 0xe2, 0x54, 0xa5, 0x3a, 0xd5, 0x93, 0x12, 0x65, 0x01, 0x3f, 0xef,
                0x08, 0xd6, 0x2d, 0x5b, 0x4d, 0xe4, 0x60, 0x1a, 0x00, 0x58, 0xcf, 0xf5, 0xfc, 0x5b, 0xff, 0xef, 0x03, 0xc5, 0xef, 0x21, 0xf3, 0xf6, 0x10, 0x2c, 0x3c, 0xa2, 0xd6, 0xc1, 0x83, 0x1b
            ]
        }];
        yield return ["/////////////////////////////////////////////////////////////////////////////w==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAA==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00
            ]
        }];
        yield return ["/4UNkeGFacXq778ygMX1JczUD8UJ7tA2LRPu/DwVUCjvUbnlQ1j1wbxDObZHVjkakly7oYBjHBbA`r`nsw==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0x85, 0x0d, 0x91, 0xe1, 0x85, 0x69, 0xc5, 0xea, 0xef, 0xbf, 0x32, 0x80, 0xc5, 0xf5, 0x25, 0xcc, 0xd4, 0x0f, 0xc5, 0x09, 0xee, 0xd0, 0x36, 0x2d, 0x13, 0xee, 0xfc,
                0x3c, 0x15, 0x50, 0x28, 0xef, 0x51, 0xb9, 0xe5, 0x43, 0x58, 0xf5, 0xc1, 0xbc, 0x43, 0x39, 0xb6, 0x47, 0x56, 0x39, 0x1a, 0x92, 0x5c, 0xbb, 0xa1, 0x80, 0x63, 0x1c, 0x16, 0xc0,
                0xb3
            ]
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n/w==", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            ]
        }];
        yield return ["S+Bd3oLEHvNs3uZwbUJQDEDACJeq495XjikRhat9Q8WKHeYiSGa6oJ9Xbq8H3//M/A/FJsLaTxWrqGo=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x4b, 0xe0, 0x5d, 0xde, 0x82, 0xc4, 0x1e, 0xf3, 0x6c, 0xde, 0xe6, 0x70, 0x6d, 0x42, 0x50, 0x0c, 0x40, 0xc0, 0x08, 0x97, 0xaa, 0xe3, 0xde, 0x57, 0x8e, 0x29, 0x11, 0x85,
                0xab, 0x7d, 0x43, 0xc5, 0x8a, 0x1d, 0xe6, 0x22, 0x48, 0x66, 0xba, 0xa0, 0x9f, 0x57, 0x6e, 0xaf, 0x07, 0xdf, 0xff, 0xcc, 0xfc, 0x0f, 0xc5, 0x26, 0xc2, 0xda, 0x4f, 0x15, 0xab,
                0xa8, 0x6a
            ]
        }];
        yield return ["//////////////////////////////////////////////////////////////////////////////8=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            ] }];
        yield return ["Q0++wzeprqEc4RTPK4mJPlnZJvXvwVI55GpXUTSyBsSUYe+eKWCu5B0DD30KKDfvkKyMnRFpuZgH`r`nNKs=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x43, 0x4f, 0xbe, 0xc3, 0x37, 0xa9, 0xae, 0xa1, 0x1c, 0xe1, 0x14, 0xcf, 0x2b, 0x89, 0x89, 0x3e, 0x59, 0xd9, 0x26, 0xf5, 0xef, 0xc1, 0x52, 0x39, 0xe4, 0x6a, 0x57, 0x51,
                0x34, 0xb2, 0x06, 0xc4, 0x94, 0x61, 0xef, 0x9e, 0x29, 0x60, 0xae, 0xe4, 0x1d, 0x03, 0x0f, 0x7d, 0x0a, 0x28, 0x37, 0xef, 0x90, 0xac, 0x8c, 0x9d, 0x11, 0x69, 0xb9, 0x98, 0x07,
                0x34, 0xab
            ]
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n//8=", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00
            ] }];
        yield return ["Gi+3C0+1IeZrsL5yvjktBpI07u2kyjUPdG7XmYb10dvTbkcUNwnZ8jhk+RHS13WrF/igmHfSBFDlEwaL", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x1a, 0x2f, 0xb7, 0x0b, 0x4f, 0xb5, 0x21, 0xe6, 0x6b, 0xb0, 0xbe, 0x72, 0xbe, 0x39, 0x2d, 0x06, 0x92, 0x34, 0xee, 0xed, 0xa4, 0xca, 0x35, 0x0f, 0x74, 0x6e, 0xd7, 0x99,
                0x86, 0xf5, 0xd1, 0xdb, 0xd3, 0x6e, 0x47, 0x14, 0x37, 0x09, 0xd9, 0xf2, 0x38, 0x64, 0xf9, 0x11, 0xd2, 0xd7, 0x75, 0xab, 0x17, 0xf8, 0xa0, 0x98, 0x77, 0xd2, 0x04, 0x50, 0xe5,
                0x13, 0x06, 0x8b
            ]
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////////", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff
            ]
        }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAAA", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00
            ]
        }];
        yield return ["XRGAp+4uNNA//zjY+hR9x40dLKaAjZ1mEF5RVwdhd+trwRcnTKtpdLDsjHGYu+hfrFN9G2PjUCdj`r`nyqw4", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0x5d, 0x11, 0x80, 0xa7, 0xee, 0x2e, 0x34, 0xd0, 0x3f, 0xff, 0x38, 0xd8, 0xfa, 0x14, 0x7d, 0xc7, 0x8d, 0x1d, 0x2c, 0xa6, 0x80, 0x8d, 0x9d, 0x66, 0x10, 0x5e, 0x51, 0x57,
                0x07, 0x61, 0x77, 0xeb, 0x6b, 0xc1, 0x17, 0x27, 0x4c, 0xab, 0x69, 0x74, 0xb0, 0xec, 0x8c, 0x71, 0x98, 0xbb, 0xe8, 0x5f, 0xac, 0x53, 0x7d, 0x1b, 0x63, 0xe3, 0x50, 0x27, 0x63,
                0xca, 0xac, 0x38
            ]
        }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n////", new TryGetExpected<byte[]>
        {
            Returned = true,
            Result =
            [
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
                0xff, 0xff, 0xff
            ]
        }];
        yield return ["=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["8", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["z=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["m==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["Q==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/w=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/Vw", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["//=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["gNP", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["///", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["lRi+A==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/////w=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/ZtN9Uc", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/////8=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["73Qniw6", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["///////", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["nPTSd9AI8/+ywWiM12ushTgA3N7fW4gL6jAaQhoBYgBFJemm+7eYxxyzRBNszhrOMR/EmURYWw=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////w==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["YK16Q7KOw8mbN7sZlvew1wz0r414lbk9jrNpf4RnXu5munrKDmKuA3Ep8yvj0ipI4O3yaBKy9w=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["//////////////////////////////////////////////////////////////////////////8", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["M1F67I+VTY5XDztBftfG2KQh02s04uZX2J9suD2LNM0MwsrKsPEsnHUOP2KH6c07KACYeNurvkq", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////////", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["eZMpapI0TKPdhR5lVnapGkGEuuQV+4Zy3cgwOqhGe05/SyPlmz1zaNuLR6COX+CjNo24yjSj50U8tw=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////////w==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["HCX6ifLEERAsMMqeBOXZc7/ipL9DBQRZB/zTTEjSJNF00EcK5eEFlnZrb0LebukF/vRxuMsPtP+`r`nfw==", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n/w=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["EAqeQf8ZEwNsT434idjgZDGDxSgG7gku1hVkMW1k5wzWm+3BrE8gSrRwTcwhk+0f7zkgLfh1qcx12XE", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/////////////////////////////////////////////////////////////////////////////8=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["CZHhnqsT7wqc/+w3FnShg6a0zWDCf7nisTDw8LLHyBFbpeRclqKTotXMt5aIQ3bZ8H630Qdpeng`r`nfL8=", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n//8", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["oJ1khgqMW7CYfekbD7Iuqy6tZqe3VjzdaPbhvwYx+uIJMYR7wLGd2R7ANWMoGXVr/A8KGSdOLdVJBNX", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["///////////////////////////////////////////////////////////////////////////////", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA`r`nAAA", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["/q3kwfNXIIDpToS8Y7EphprNEoTkuGcAdgWj/Dj7wDw6h2Ae9n5Ea2gMuKb0Rk4SaD/S7F63NKk`r`nGR0k", new TryGetExpected<byte[]> { Returned = false, Result = null }];
        yield return ["////////////////////////////////////////////////////////////////////////////`r`n///", new TryGetExpected<byte[]> { Returned = false, Result = null }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.TryParseBase64(string, out IEnumerable{byte})"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTryParseBase64TestData), DynamicDataSourceType.Method)]
    public void TryParseBase64TestMethod(string input, TryGetExpected<byte[]> expected)
    {
        bool actualReturned = ByteArrayCoersion.TryParseBase64(input, out IEnumerable<byte> actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        if (expected.Result is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expected.Result, actualResult);
        }
    }

    public static IEnumerable<object[]> GetEnumerableCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return new object[] { Enumerable.Empty<int>(), Enumerable.Empty<int>() };
        yield return new object[] { Array.Empty<int>(), Array.Empty<int>() };
        yield return new object[] { Enumerable.Range(1, 10), Enumerable.Range(1, 10) };
        yield return new object[] { new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 } };
        yield return new object[] { Enumerable.Empty<byte>() };
        yield return new object[] { Array.Empty<byte>() };
        yield return new object[] { new object[] { 1, 2, 3 }, new int[] { 1, 2, 3 } };
        yield return new object[] { new object[] { "1", "2", "3" }, new int[] { 1, 2, 3 } };
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetEnumerableCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void EnumerableCoersionCoerceSuccessTestMethod(object obj, IEnumerable<int> expected)
    {
        EnumerableCoersion<int> target = EnumerableCoersion<int>.Default;
        var actual = target.Coerce(obj);
        if (obj is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetEnumerableCoersionCoerceFailTestData()
    {
        yield return new[] { new object[] { uint.MaxValue } };
        yield return new[] { new object[] { "" } };
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetEnumerableCoersionCoerceFailTestData), DynamicDataSourceType.Method)]
    public void EnumerableCoersionCoerceFailTestMethod(object obj)
    {
        EnumerableCoersion<int> target = EnumerableCoersion<int>.Default;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceDefaultSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, dateTime];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 988, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceDefaultSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceDefaultSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceDefaultFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceDefaultFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceDefaultFailTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.Default;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToLocalSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime.ToLocalTime()];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, DateTime.SpecifyKind(dateTime, DateTimeKind.Local)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 988, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToLocal;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToLocalFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToLocal;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToUtcSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime.ToUniversalTime()];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 988, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToUtc;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToUtcFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToUtc;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Unspecified)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSeconds;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSeconds;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsLocal;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsLocalFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsLocal;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsUtc;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToSecondsUtcFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToSecondsUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToSecondsUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsUtc;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Unspecified)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutes;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutes;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesLocal;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesLocalTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesLocalTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesLocal;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesUtc;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToMinutesUtcFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToMinutesUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToMinutesUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesUtc;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Unspecified)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHours;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHours;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursLocal;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursLocalFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursLocal;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursUtc;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursUtc;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Unspecified)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDays;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDays;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysLocal;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysLocalFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysLocal;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return ["2025-05-31 10:21:59.988", new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysUtc;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Year, actual.Year);
        Assert.AreEqual(expected.Month, actual.Month);
        Assert.AreEqual(expected.Day, actual.Day);
        Assert.AreEqual(expected.Hour, actual.Hour);
        Assert.AreEqual(expected.Minute, actual.Minute);
        Assert.AreEqual(expected.Second, actual.Second);
        Assert.AreEqual(expected.Millisecond, actual.Millisecond);
        Assert.AreEqual(expected.Microsecond, actual.Microsecond);
        Assert.AreEqual(expected.Kind, actual.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToDaysUtcFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToDaysUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToDaysUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysUtc;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetNonNullStringCoersionCoerceSuccessTestData()
    {
        yield return [null, ""];
        yield return ["", ""];
        yield return ["\n", "\n"];
        yield return [" ", " "];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "\nTest\t"];
        yield return ["\tTest\nAgain\t", "\tTest\nAgain\t"];
        yield return ["\tTest\n Again\t", "\tTest\n Again\t"];
        yield return ["\tTest \n Again\t", "\tTest \n Again\t"];
        yield return ["\tLast  Test\t", "\tLast  Test\t"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), ""];
        yield return ["Test".ToCharArray(), "Test"];
        yield return [" \n ".ToCharArray(), " \n "];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonNullStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NonNullStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        NonNullStringCoersion target = NonNullStringCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNonWhiteSpaceOrEmptyStringCoersionCoerceSuccessTestData()
    {
        yield return [null, ""];
        yield return ["", ""];
        yield return ["\n", ""];
        yield return [" ", ""];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "\nTest\t"];
        yield return ["\tTest\nAgain\t", "\tTest\nAgain\t"];
        yield return ["\tTest\n Again\t", "\tTest\n Again\t"];
        yield return ["\tTest \n Again\t", "\tTest \n Again\t"];
        yield return ["\tLast  Test\t", "\tLast  Test\t"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), ""];
        yield return [" \n ".ToCharArray(), ""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonWhiteSpaceOrEmptyStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NonWhiteSpaceOrEmptyStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        NonWhiteSpaceOrEmptyStringCoersion target = NonWhiteSpaceOrEmptyStringCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNormalizedOrEmptyStringCoersionCoerceSuccessTestData()
    {
        yield return [null, ""];
        yield return ["", ""];
        yield return ["\n", ""];
        yield return [" ", ""];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "Test"];
        yield return ["\tTest\nAgain\t", "Test Again"];
        yield return ["\tTest\n Again\t", "Test Again"];
        yield return ["\tTest \n Again\t", "Test Again"];
        yield return ["\tLast  Test\t", "Last Test"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), ""];
        yield return ["Test".ToCharArray(), "Test"];
        yield return [" \n ".ToCharArray(), ""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNormalizedOrEmptyStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NormalizedOrEmptyStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        NormalizedOrEmptyStringCoersion target = NormalizedOrEmptyStringCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrNormalizedStringCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return ["", null];
        yield return ["\n", null];
        yield return [" ", null];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "Test"];
        yield return ["\tTest\nAgain\t", "Test Again"];
        yield return ["\tTest\n Again\t", "Test Again"];
        yield return ["\tTest \n Again\t", "Test Again"];
        yield return ["\tLast  Test\t", "Last Test"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), null];
        yield return ["Test".ToCharArray(), "Test"];
        yield return [" \n ".ToCharArray(), null];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrNormalizedStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrNormalizedStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrNormalizedStringCoersion target = NullIfWhiteSpaceOrNormalizedStringCoersion.Default;
        var actual = target.Coerce(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrTrimmedStringCoersionCoerceSuccessTestData()
    {
        yield return [null, null];
        yield return ["", null];
        yield return ["\n", null];
        yield return [" ", null];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "Test"];
        yield return ["\tTest\nAgain\t", "Test\nAgain"];
        yield return ["\tTest\n Again\t", "Test\n Again"];
        yield return ["\tTest \n Again\t", "Test \n Again"];
        yield return ["\tLast  Test\t", "Last  Test"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), null];
        yield return ["Test".ToCharArray(), "Test"];
        yield return [" \n ".ToCharArray(), null];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrTrimmedStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrTrimmedStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrTrimmedStringCoersion target = NullIfWhiteSpaceOrTrimmedStringCoersion.Default;
        var actual = target.Coerce(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetTrimmedNonNullStringCoersionCoerceSuccessTestData()
    {
        yield return [null, ""];
        yield return ["", ""];
        yield return ["\n", ""];
        yield return [" ", ""];
        yield return ["Test", "Test"];
        yield return ["\nTest\t", "Test"];
        yield return ["\tTest\nAgain\t", "Test\nAgain"];
        yield return ["\tTest\n Again\t", "Test\n Again"];
        yield return ["\tTest \n Again\t", "Test \n Again"];
        yield return ["\tLast  Test\t", "Last  Test"];
        yield return [1, "1"];
        yield return [Array.Empty<char>(), ""];
        yield return ["Test".ToCharArray(), "Test"];
        yield return [" \n ".ToCharArray(), ""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTrimmedNonNullStringCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void TrimmedNonNullStringCoersionCoerceSuccessTestMethod(object obj, string expected)
    {
        TrimmedNonNullStringCoersion target = TrimmedNonNullStringCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceDefaultSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 58, 3, 997)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 2, 997, 3)];
        yield return [TimeSpan.MaxValue, TimeSpan.MaxValue];
        yield return ["1.23:02:58.0039970", new TimeSpan(1, 23, 2, 58, 3, 997)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceDefaultSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToDefaultSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.Default;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceDefaultFailTestData()
    {
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceDefaultFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceDefaultFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.Default;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToSecondsSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 58, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 2, 0, 0)];
        yield return ["1.23:02:58.0039970", new TimeSpan(1, 23, 2, 58, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToSecondsSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToSecondsSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizeToSeconds;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToSecondsFailTestData()
    {
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToSecondsFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToSecondsFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizeToSeconds;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToMinutesSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 0, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 0, 0, 0)];
        yield return ["1.23:02:58.0039970", new TimeSpan(1, 23, 2, 0, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToMinutesSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToMinutesSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToMinutes;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToMinutesFailTestData()
    {
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToMinutesFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToMinutesFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToMinutes;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToHoursSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 0, 0, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 0, 0, 0, 0)];
        yield return ["1.23:02:58.0039970", new TimeSpan(1, 23, 0, 0, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToHoursSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToHoursSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToHours;
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCoerceToHoursFailTestData()
    {
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCoerceToHoursFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCoerceToHoursFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToHours;
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }

    public static IEnumerable<object[]> GetValueCoersionCoerceSuccessTestData()
    {
        yield return [int.MinValue, int.MinValue];
        yield return [(long)int.MinValue, int.MinValue];
        yield return [-1, -1];
        yield return [0, 0];
        yield return [(byte)1, 1];
        yield return [1, 1];
        yield return [1.5, 2];
        yield return [int.MaxValue, int.MaxValue];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetValueCoersionCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ValueCoersionCoerceSuccessTestMethod(object obj, int expected)
    {
        ValueCoersion<int> target = new();
        var actual = target.Coerce(obj);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetValueCoersionCoerceFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return [((long)int.MinValue) - 1];
        yield return [uint.MaxValue];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetValueCoersionCoerceFailTestData), DynamicDataSourceType.Method)]
    public void ValueCoersionCoerceFailTestMethod(object obj)
    {
        ValueCoersion<int> target = new();
        Assert.Throws<NotSupportedException>(() => target.Coerce(obj));
    }
}
