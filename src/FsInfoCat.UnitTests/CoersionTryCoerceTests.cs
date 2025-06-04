using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests;

[TestClass]
public class CoersionTryCoerceTests
{
    public TestContext TestContext { get; set; }

    public static IEnumerable<object[]> GetArrayCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetArrayCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ArrayCoersionTryCoerceSuccessTestMethod(int[] expected)
    {
        ArrayCoersion<int> target = ArrayCoersion<int>.Default;
        var actual = target.TryCoerce(expected, out int[] actualResult);
        Assert.IsTrue(actual);
        if (expected is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreSame(expected, actualResult);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }

    public static IEnumerable<object[]> GetNotEmptyOrNullValueArrayCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNotEmptyOrNullValueArrayCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NotEmptyOrNullValueArrayCoersionTryCoerceSuccessTestMethod(object obj, bool expectNull)
    {
        NotEmptyOrNullValueArrayCoersion<int> target = new();
        var actual = target.TryCoerce(obj, out int[] actualResult);
        Assert.IsTrue(actual);
        if (expectNull)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreSame(obj, actualResult);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }

    public static IEnumerable<object[]> GetByteArrayCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetByteArrayCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ByteArrayCoersionTryCoerceSuccessTestMethod(byte[] expected)
    {
        ByteArrayCoersion target = ByteArrayCoersion.Default;
        var actual = target.TryCoerce(expected, out byte[] actualResult);
        Assert.IsTrue(actual);
        if (expected is null)
            Assert.IsNull(actualResult);
        else
            Assert.AreSame(expected, actualResult);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }

    public static IEnumerable<object[]> GetEnumerableCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetEnumerableCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void EnumerableCoersionTryCoerceSuccessTestMethod(object obj)
    {
        EnumerableCoersion<int> target = EnumerableCoersion<int>.Default;
        var actual = target.TryCoerce(obj, out IEnumerable<int> actualResult);
        Assert.IsTrue(actual);
        if (obj is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreSame(obj, actualResult);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionTryCoerceDefaultFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionTryCoerceDefaultFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionTryCoerceDefaultFailTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.Default;
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCoerceToHoursUtcFailTestData()
    {
        yield return [null];
        yield return [""];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCoerceToHoursUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCoerceToHoursUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursUtc;
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out DateTime actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Year, actualResult.Year);
        Assert.AreEqual(expected.Month, actualResult.Month);
        Assert.AreEqual(expected.Day, actualResult.Day);
        Assert.AreEqual(expected.Hour, actualResult.Hour);
        Assert.AreEqual(expected.Minute, actualResult.Minute);
        Assert.AreEqual(expected.Second, actualResult.Second);
        Assert.AreEqual(expected.Millisecond, actualResult.Millisecond);
        Assert.AreEqual(expected.Microsecond, actualResult.Microsecond);
        Assert.AreEqual(expected.Kind, actualResult.Kind);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }

    public static IEnumerable<object[]> GetNonNullStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNonNullStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NonNullStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        NonNullStringCoersion target = NonNullStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        Assert.IsNotNull(actualResult);
        Assert.AreEqual(expected, actualResult);
    }

    public static IEnumerable<object[]> GetNonWhiteSpaceOrEmptyStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNonWhiteSpaceOrEmptyStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NonWhiteSpaceOrEmptyStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        NonWhiteSpaceOrEmptyStringCoersion target = NonWhiteSpaceOrEmptyStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        Assert.IsNotNull(actualResult);
        Assert.AreEqual(expected, actualResult);
    }

    public static IEnumerable<object[]> GetNormalizedOrEmptyStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNormalizedOrEmptyStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NormalizedOrEmptyStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        NormalizedOrEmptyStringCoersion target = NormalizedOrEmptyStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        Assert.IsNotNull(actualResult);
        Assert.AreEqual(expected, actualResult);
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrNormalizedStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNullIfWhiteSpaceOrNormalizedStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrNormalizedStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrNormalizedStringCoersion target = NullIfWhiteSpaceOrNormalizedStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        if (expected is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expected, actualResult);
        }
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrTrimmedStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetNullIfWhiteSpaceOrTrimmedStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrTrimmedStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrTrimmedStringCoersion target = NullIfWhiteSpaceOrTrimmedStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        if (expected is null)
            Assert.IsNull(actualResult);
        else
        {
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expected, actualResult);
        }
    }

    public static IEnumerable<object[]> GetTrimmedNonNullStringCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetTrimmedNonNullStringCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void TrimmedNonNullStringCoersionTryCoerceSuccessTestMethod(object obj, string expected)
    {
        TrimmedNonNullStringCoersion target = TrimmedNonNullStringCoersion.Default;
        var actual = target.TryCoerce(obj, out string actualResult);
        Assert.IsTrue(actual);
        Assert.IsNotNull(actualResult);
        Assert.AreEqual(expected, actualResult);
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
        var actual = target.TryCoerce(obj, out TimeSpan actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Days, actualResult.Days);
        Assert.AreEqual(expected.Hours, actualResult.Hours);
        Assert.AreEqual(expected.Minutes, actualResult.Minutes);
        Assert.AreEqual(expected.Seconds, actualResult.Seconds);
        Assert.AreEqual(expected.Milliseconds, actualResult.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actualResult.Microseconds);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out TimeSpan actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Days, actualResult.Days);
        Assert.AreEqual(expected.Hours, actualResult.Hours);
        Assert.AreEqual(expected.Minutes, actualResult.Minutes);
        Assert.AreEqual(expected.Seconds, actualResult.Seconds);
        Assert.AreEqual(expected.Milliseconds, actualResult.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actualResult.Microseconds);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out TimeSpan actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Days, actualResult.Days);
        Assert.AreEqual(expected.Hours, actualResult.Hours);
        Assert.AreEqual(expected.Minutes, actualResult.Minutes);
        Assert.AreEqual(expected.Seconds, actualResult.Seconds);
        Assert.AreEqual(expected.Milliseconds, actualResult.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actualResult.Microseconds);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
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
        var actual = target.TryCoerce(obj, out TimeSpan actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected.Days, actualResult.Days);
        Assert.AreEqual(expected.Hours, actualResult.Hours);
        Assert.AreEqual(expected.Minutes, actualResult.Minutes);
        Assert.AreEqual(expected.Seconds, actualResult.Seconds);
        Assert.AreEqual(expected.Milliseconds, actualResult.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actualResult.Microseconds);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }

    public static IEnumerable<object[]> GetValueCoersionTryCoerceSuccessTestData()
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
    [DynamicData(nameof(GetValueCoersionTryCoerceSuccessTestData), DynamicDataSourceType.Method)]
    public void ValueCoersionTryCoerceSuccessTestMethod(object obj, int expected)
    {
        ValueCoersion<int> target = new();
        var actual = target.TryCoerce(obj, out int actualResult);
        Assert.IsTrue(actual);
        Assert.AreEqual(expected, actualResult);
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
        var actual = target.TryCoerce(obj, out _);
        Assert.IsFalse(actual);
    }
}
