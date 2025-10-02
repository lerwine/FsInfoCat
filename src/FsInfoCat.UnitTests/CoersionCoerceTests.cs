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
        yield return ["c504791a0ac682499d5f0f1cd8681cfa", new byte[] { 0xc5, 0x04, 0x79, 0x1a, 0x0a, 0xc6, 0x82, 0x49, 0x9d, 0x5f, 0x0f, 0x1c, 0xd8, 0x68, 0x1c, 0xfa }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBinHex(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetParseBinHexSuccessTestData), DynamicDataSourceType.Method)]
    public void ParseBinHexSuccessTestMethod(string binHex, byte[] expected)
    {
        throw new AssertInconclusiveException($"Test Method {nameof(ParseBinHexSuccessTestMethod)} not implemented.");
    }

    public static IEnumerable<object[]> GetTryParseBinHexTestData()
    {
        yield return ["c504791a0ac682499d5f0f1cd8681cfa", new TryGetExpected<byte[]> { Returned = true, Result = [0xc5, 0x04, 0x79, 0x1a, 0x0a, 0xc6, 0x82, 0x49, 0x9d, 0x5f, 0x0f, 0x1c, 0xd8, 0x68, 0x1c, 0xfa] }];
    }

    /// <summary>
    /// Test method for <see cref="ByteArrayCoersion.ParseBinHex(string)"/>
    /// </summary>
    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTryParseBinHexTestData), DynamicDataSourceType.Method)]
    public void TryParseBinHexTestMethod(string binHex, TryGetExpected<byte[]> expected)
    {
        throw new AssertInconclusiveException($"Test Method {nameof(TryParseBinHexTestMethod)} not implemented.");
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
