using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests;

[TestClass]
public class CoersionCastSuccessTests
{
    public TestContext TestContext { get; set; }

    public static IEnumerable<object[]> GetArrayCoersionCastSuccessTestData()
    {
        yield return [null];
        yield return [Array.Empty<int>()];
        yield return [new[] { 1 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetArrayCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void ArrayCoersionCastSuccessTestMethod(int[] expected)
    {
        ArrayCoersion<int> target = ArrayCoersion<int>.Default;
        var actual = target.Cast(expected);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreSame(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetArrayCoersionCastFailTestData()
    {
        yield return [Enumerable.Repeat(1, 2)];
        yield return [Array.Empty<byte>()];
        yield return [new[] { 1.0 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetArrayCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void ArrayCoersionCastFailTestMethod(object obj)
    {
        ArrayCoersion<int> target = ArrayCoersion<int>.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNotEmptyOrNullValueArrayCoersionCastSuccessTestData()
    {
        yield return [null, true];
        yield return [Array.Empty<int>(), true];
        yield return [new int[] { 1 }, false];
        yield return [new int[] { 1, 2, 3 }, false];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNotEmptyOrNullValueArrayCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NotEmptyOrNullValueArrayCoersionCastSuccessTestMethod(object obj, bool expectNull)
    {
        NotEmptyOrNullValueArrayCoersion<int> target = new();
        var actual = target.Cast(obj);
        if (expectNull)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreSame(obj, actual);
        }
    }

    public static IEnumerable<object[]> GetNotEmptyOrNullValueArrayCoersionCastFailTestData()
    {
        yield return [Array.Empty<byte>()];
        yield return [Enumerable.Empty<byte>()];
        yield return [Enumerable.Repeat(1u, 2)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNotEmptyOrNullValueArrayCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NotEmptyOrNullValueArrayCoersionCastFailTestMethod(object obj)
    {
        NotEmptyOrNullValueArrayCoersion<int> target = new();
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetByteArrayCoersionCastSuccessTestData()
    {
        yield return [null];
        yield return [Array.Empty<byte>()];
        yield return [new byte[] { 1 }];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetByteArrayCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void ByteArrayCoersionCastSuccessTestMethod(byte[] expected)
    {
        ByteArrayCoersion target = ByteArrayCoersion.Default;
        var actual = target.Cast(expected);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreSame(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetByteArrayCoersionCastFailTestData()
    {
        yield return [""];
        yield return [Array.Empty<int>()];
        yield return [new [] {1, 2, 3}];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetByteArrayCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void ByteArrayCoersionCastFailTestMethod(object obj)
    {
        ByteArrayCoersion target = ByteArrayCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetEnumerableCoersionCastSuccessTestData()
    {
        yield return [null];
        yield return new[] { Enumerable.Empty<int>() };
        yield return new[] { Array.Empty<int>() };
        yield return new[] { Enumerable.Range(1, 10) };
        yield return new[] { new int[] { 1, 2, 3 } };
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetEnumerableCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void EnumerableCoersionCastSuccessTestMethod(object obj)
    {
        EnumerableCoersion<int> target = EnumerableCoersion<int>.Default;
        var actual = target.Cast(obj);
        if (obj is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreSame(obj, actual);
        }
    }

    public static IEnumerable<object[]> GetEnumerableCoersionCastFailTestData()
    {
        yield return new[] { Enumerable.Empty<byte>() };
        yield return new[] { Array.Empty<byte>() };
        yield return new[] { new object[] { 1, 2, 3 } };
        yield return new[] { new object[] { "1", "2", "3" } };
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetEnumerableCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void EnumerableCoersionCastFailTestMethod(object obj)
    {
        EnumerableCoersion<int> target = EnumerableCoersion<int>.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastDefaultSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, dateTime];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastDefaultSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastDefaultSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.Default;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastDefaultFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastDefaultFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastDefaultFailTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToLocalSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime.ToLocalTime()];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, DateTime.SpecifyKind(dateTime, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToLocal;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToLocalFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToLocal;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToUtcSuccessTestData()
    {
        DateTime dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc);
        yield return [dateTime, dateTime];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local);
        yield return [dateTime, dateTime.ToUniversalTime()];
        dateTime = new(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified);
        yield return [dateTime, DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToUtc;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToUtc;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSeconds;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSeconds;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsLocal;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsLocalFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsLocal;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 59, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsUtc;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToSecondsUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToSecondsUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToSecondsUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToSecondsUtc;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutes;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutes;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesLocal;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesLocalTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesLocalTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesLocal;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 21, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesUtc;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToMinutesUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToMinutesUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToMinutesUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToMinutesUtc;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHours;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHours;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursLocal;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursLocalFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursLocal;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 10, 0, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursUtc;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToHoursUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToHoursUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToHoursUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToHoursUtc;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Unspecified)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDays;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDays;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysLocalSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysLocalSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysLocalSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysLocal;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysLocalFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysLocalFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysLocalFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysLocal;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysUtcSuccessTestData()
    {
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Utc), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Local), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()];
        yield return [new DateTime(2025, 5, 31, 10, 21, 59, 988, 716, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, 0, DateTimeKind.Utc)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysUtcSuccessTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysUtcSuccessTestMethod(object obj, DateTime expected)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysUtc;
        var actual = target.Cast(obj);
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

    public static IEnumerable<object[]> GetDateTimeCoersionCastToDaysUtcFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return ["2025-05-31 10:21:59"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetDateTimeCoersionCastToDaysUtcFailTestData), DynamicDataSourceType.Method)]
    public void DateTimeCoersionCastToDaysUtcFailTestMethod(object obj)
    {
        DateTimeCoersion target = DateTimeCoersion.NormalizedToDaysUtc;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNonNullStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonNullStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NonNullStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        NonNullStringCoersion target = NonNullStringCoersion.Default;
        var actual = target.Cast(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNonNullStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonNullStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NonNullStringCoersionCastFailTestMethod(object obj)
    {
        NonNullStringCoersion target = NonNullStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNonWhiteSpaceOrEmptyStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonWhiteSpaceOrEmptyStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NonWhiteSpaceOrEmptyStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        NonWhiteSpaceOrEmptyStringCoersion target = NonWhiteSpaceOrEmptyStringCoersion.Default;
        var actual = target.Cast(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNonWhiteSpaceOrEmptyStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNonWhiteSpaceOrEmptyStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NonWhiteSpaceOrEmptyStringCoersionCastFailTestMethod(object obj)
    {
        NonWhiteSpaceOrEmptyStringCoersion target = NonWhiteSpaceOrEmptyStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNormalizedOrEmptyStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNormalizedOrEmptyStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NormalizedOrEmptyStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        NormalizedOrEmptyStringCoersion target = NormalizedOrEmptyStringCoersion.Default;
        var actual = target.Cast(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetNormalizedOrEmptyStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNormalizedOrEmptyStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NormalizedOrEmptyStringCoersionCastFailTestMethod(object obj)
    {
        NormalizedOrEmptyStringCoersion target = NormalizedOrEmptyStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrNormalizedStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrNormalizedStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrNormalizedStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrNormalizedStringCoersion target = NullIfWhiteSpaceOrNormalizedStringCoersion.Default;
        var actual = target.Cast(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrNormalizedStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrNormalizedStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrNormalizedStringCoersionCastFailTestMethod(object obj)
    {
        NullIfWhiteSpaceOrNormalizedStringCoersion target = NullIfWhiteSpaceOrNormalizedStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrTrimmedStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrTrimmedStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrTrimmedStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        NullIfWhiteSpaceOrTrimmedStringCoersion target = NullIfWhiteSpaceOrTrimmedStringCoersion.Default;
        var actual = target.Cast(obj);
        if (expected is null)
            Assert.IsNull(actual);
        else
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    public static IEnumerable<object[]> GetNullIfWhiteSpaceOrTrimmedStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetNullIfWhiteSpaceOrTrimmedStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void NullIfWhiteSpaceOrTrimmedStringCoersionCastFailTestMethod(object obj)
    {
        NullIfWhiteSpaceOrTrimmedStringCoersion target = NullIfWhiteSpaceOrTrimmedStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetTrimmedNonNullStringCoersionCastSuccessTestData()
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
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTrimmedNonNullStringCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void TrimmedNonNullStringCoersionCastSuccessTestMethod(object obj, string expected)
    {
        TrimmedNonNullStringCoersion target = TrimmedNonNullStringCoersion.Default;
        var actual = target.Cast(obj);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetTrimmedNonNullStringCoersionCastFailTestData()
    {
        yield return [1];
        yield return [Array.Empty<char>()];
        yield return ["Test".ToCharArray()];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTrimmedNonNullStringCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void TrimmedNonNullStringCoersionCastFailTestMethod(object obj)
    {
        TrimmedNonNullStringCoersion target = TrimmedNonNullStringCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastDefaultSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 58, 3, 997)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 2, 997, 3)];
        yield return [TimeSpan.MaxValue, TimeSpan.MaxValue];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastDefaultSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToDefaultSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.Default;
        var actual = target.Cast(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastDefaultFailTestData()
    {
        yield return [""];
        yield return ["1.23:02:58.0039970"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastDefaultFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastDefaultFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.Default;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToSecondsSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 58, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 2, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToSecondsSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToSecondsSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizeToSeconds;
        var actual = target.Cast(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToSecondsFailTestData()
    {
        yield return [""];
        yield return ["1.23:02:58.0039970"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToSecondsFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToSecondsFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizeToSeconds;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToMinutesSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 2, 0, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 59, 0, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToMinutesSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToMinutesSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToMinutes;
        var actual = target.Cast(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToMinutesFailTestData()
    {
        yield return [""];
        yield return ["1.23:02:58.0039970"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToMinutesFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToMinutesFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToMinutes;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToHoursSuccessTestData()
    {
        yield return [TimeSpan.Zero, TimeSpan.Zero];
        yield return [new TimeSpan(1, 23, 2, 58, 3, 997), new TimeSpan(1, 23, 0, 0, 0, 0)];
        yield return [new TimeSpan(50, 1, 59, 2, 997, 3), new TimeSpan(50, 1, 0, 0, 0, 0)];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToHoursSuccessTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToHoursSuccessTestMethod(object obj, TimeSpan expected)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToHours;
        var actual = target.Cast(obj);
        Assert.AreEqual(expected.Days, actual.Days);
        Assert.AreEqual(expected.Hours, actual.Hours);
        Assert.AreEqual(expected.Minutes, actual.Minutes);
        Assert.AreEqual(expected.Seconds, actual.Seconds);
        Assert.AreEqual(expected.Milliseconds, actual.Milliseconds);
        Assert.AreEqual(expected.Microseconds, actual.Microseconds);
    }

    public static IEnumerable<object[]> GetTimeSpanCoersionCastToHoursFailTestData()
    {
        yield return [""];
        yield return ["1.23:02:58.0039970"];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetTimeSpanCoersionCastToHoursFailTestData), DynamicDataSourceType.Method)]
    public void TimeSpanCoersionCastToHoursFailTestMethod(object obj)
    {
        TimeSpanCoersion target = TimeSpanCoersion.NormalizedToHours;
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }

    public static IEnumerable<object[]> GetValueCoersionCastSuccessTestData()
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
    [DynamicData(nameof(GetValueCoersionCastSuccessTestData), DynamicDataSourceType.Method)]
    public void ValueCoersionCastSuccessTestMethod(object obj, int expected)
    {
        ValueCoersion<int> target = new();
        var actual = target.Cast(obj);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetValueCoersionCastFailTestData()
    {
        yield return [null];
        yield return [""];
        yield return [((long)int.MinValue) - 1];
        yield return ["0"];
        yield return [uint.MaxValue];
    }

    [DataTestMethod, Priority(0)]
    [DynamicData(nameof(GetValueCoersionCastFailTestData), DynamicDataSourceType.Method)]
    public void ValueCoersionCastFailTestMethod(object obj)
    {
        ValueCoersion<int> target = new();
        Assert.Throws<InvalidCastException>(() => target.Cast(obj));
    }
}
