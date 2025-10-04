using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // TODO: Implement Dynamic Data method
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetParseBase64SuccessTestData)} not implemented.");
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
        // TODO: Implement Dynamic Data method
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetParseBase64FailTestData)} not implemented.");
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
        // TODO: Implement Dynamic Data method
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryParseBase64TestData)} not implemented.");
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
