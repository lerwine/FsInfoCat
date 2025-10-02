using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FsInfoCat.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests;

[TestClass]
public class TypeConversionTests
{
    public static IEnumerable<object[]> GetTryConvertToBooleanTestData()
    {
        yield return new object[] { null, new TryGetExpected<bool> { Returned = false, Result = false } };
        yield return new object[] { "", new TryGetExpected<bool> { Returned = false, Result = false } };
        yield return new object[] { " ", new TryGetExpected<bool> { Returned = false, Result = false } };
        yield return new object[] { "true", new TryGetExpected<bool> { Returned = true, Result = true } };
        yield return new object[] { "false", new TryGetExpected<bool> { Returned = true, Result = false } };
        yield return new object[] { "1", new TryGetExpected<bool> { Returned = true, Result = true } };
        yield return new object[] { "0", new TryGetExpected<bool> { Returned = true, Result = false } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToBooleanTestData), DynamicDataSourceType.Method)]
    public void TryConvertToBooleanTestMethod(string value, TryGetExpected<bool> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToBoolean(value, out bool actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    private static object[] CreateDateTimeTestData(DateTime source, XmlDateTimeSerializationMode dateTimeOption) => dateTimeOption switch
    {
        XmlDateTimeSerializationMode.Local => [XmlConvert.ToString(source, dateTimeOption), dateTimeOption, new TryGetExpected<DateTime> { Returned = true, Result = (source.Kind == DateTimeKind.Utc) ? source.ToLocalTime() : source }],
        XmlDateTimeSerializationMode.Utc => [XmlConvert.ToString(source, dateTimeOption), dateTimeOption, new TryGetExpected<DateTime> { Returned = true, Result = (source.Kind == DateTimeKind.Local) ? source.ToUniversalTime() : source }],
        _ => [XmlConvert.ToString(source, dateTimeOption), new TryGetExpected<DateTime> { Returned = true, Result = source }],
    };

    public static IEnumerable<object[]> GetTryConvertToDateTimeTestData()
    {
        foreach (XmlDateTimeSerializationMode dateTimeOption in Enum.GetValues<XmlDateTimeSerializationMode>())
        {
            yield return new object[] { null, dateTimeOption, new TryGetExpected<DateTime> { Returned = false, Result = default } };
            yield return new object[] { "", dateTimeOption, new TryGetExpected<DateTime> { Returned = false, Result = default } };
            yield return new object[] { " ", dateTimeOption, new TryGetExpected<DateTime> { Returned = false, Result = default } };
        }
        foreach (DateTimeKind kind in Enum.GetValues<DateTimeKind>())
        {
            DateTime source = new(2025, 9, 24, 20, 54, 37, kind);
            foreach (XmlDateTimeSerializationMode dateTimeOption in Enum.GetValues<XmlDateTimeSerializationMode>())
            {
                DateTime result = dateTimeOption switch
                {
                    XmlDateTimeSerializationMode.Local => (source.Kind == DateTimeKind.Utc) ? source.ToLocalTime() : source,
                    XmlDateTimeSerializationMode.Utc => (source.Kind == DateTimeKind.Local) ? source.ToUniversalTime() : source,
                    _ => source
                };
                yield return new object[] { XmlConvert.ToString(source, dateTimeOption), dateTimeOption, new TryGetExpected<DateTime> { Returned = true, Result = result } };
            }
        }
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToDateTimeTestData), DynamicDataSourceType.Method)]
    public void TryConvertToDateTimeTestMethod(string value, XmlDateTimeSerializationMode dateTimeOption, TryGetExpected<DateTime> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToDateTime(value, dateTimeOption, out DateTime actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryConvertToTimeSpanTestData()
    {
        yield return new object[] { null, new TryGetExpected<TimeSpan> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<TimeSpan> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<TimeSpan> { Returned = false, Result = default } };
        yield return new object[] { XmlConvert.ToString(TimeSpan.MinValue), new TryGetExpected<TimeSpan> { Returned = true, Result = TimeSpan.MinValue } };
        foreach (int day in new[] { 0, 1, 7 })
        {
            foreach (int hours in new[] { 0, 12, 59 })
            {
                foreach (int minutes in new[] { 0, 30, 59 })
                {
                    foreach (int seconds in new[] { 0, 30, 59 })
                    {
                        foreach (int milliseconds in new[] { 0, 500, 999 })
                        {
                            foreach (int microseconds in new[] { 0, 500, 999 })
                            {
                                TimeSpan result = new(day, hours, minutes, seconds, milliseconds, microseconds);
                                yield return new object[] { XmlConvert.ToString(result), new TryGetExpected<TimeSpan> { Returned = true, Result = result } };
                            }
                        }
                    }
                }
            }
        }
        yield return new object[] { XmlConvert.ToString(TimeSpan.MaxValue), new TryGetExpected<TimeSpan> { Returned = true, Result = TimeSpan.MaxValue } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToTimeSpanTestData), DynamicDataSourceType.Method)]
    public void TryConvertToTimeSpanTestMethod(string value, TryGetExpected<TimeSpan> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToTimeSpan(value, out TimeSpan actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryConvertToInt16TestData()
    {
        yield return new object[] { null, new TryGetExpected<short> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<short> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<short> { Returned = false, Result = default } };
        foreach (short result in new short[] { short.MinValue, -1, 0, 1, short.MaxValue  })
            yield return new object[] { XmlConvert.ToString(result), new TryGetExpected<short> { Returned = true, Result = result } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToInt16TestData), DynamicDataSourceType.Method)]
    public void TryConvertToInt16TestMethod(string value, TryGetExpected<short> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToInt16(value, out short actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryConvertToInt32TestData()
    {
        yield return new object[] { null, new TryGetExpected<int> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<int> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<int> { Returned = false, Result = default } };
        foreach (int result in new[] { int.MinValue, -1, 0, 1, int.MaxValue })
            yield return new object[] { XmlConvert.ToString(result), new TryGetExpected<int> { Returned = true, Result = result } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToInt32TestData), DynamicDataSourceType.Method)]
    public void TryConvertToInt32TestMethod(string value, TryGetExpected<int> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToInt32(value, out int actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryConvertToInt64TestData()
    {
        yield return new object[] { null, new TryGetExpected<long> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<long> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<long> { Returned = false, Result = default } };
        foreach (long result in new[] { long.MinValue, -1L, 0L, 1L, long.MaxValue })
            yield return new object[] { XmlConvert.ToString(result), new TryGetExpected<long> { Returned = true, Result = result } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToInt64TestData), DynamicDataSourceType.Method)]
    public void TryConvertToInt64TestMethod(string value, TryGetExpected<long> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToInt64(value, out long actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> TryConvertToEnumValueTestData()
    {
        yield return new object[] { null, new TryGetExpected<XmlNodeType> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<XmlNodeType> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<XmlNodeType> { Returned = false, Result = default } };
        foreach (XmlNodeType result in Enum.GetValues<XmlNodeType>())
        {
            yield return new object[] { result.ToString("F"), new TryGetExpected<XmlNodeType> { Returned = true, Result = result } };
            yield return new object[] { XmlConvert.ToString((int)result), new TryGetExpected<XmlNodeType> { Returned = true, Result = result } };
        }
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(TryConvertToEnumValueTestData), DynamicDataSourceType.Method)]
    public void TryConvertToEnumValueTestMethod(string value, TryGetExpected<XmlNodeType> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToEnumValue(value, out XmlNodeType actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetEnumListTestData()
    {
        yield return new object[] { null, Array.Empty<XmlNodeType>() };
        yield return new object[] { "", Array.Empty<XmlNodeType>() };
        yield return new object[] { " ", Array.Empty<XmlNodeType>() };
        foreach (XmlNodeType result in Enum.GetValues<XmlNodeType>())
        {
            yield return new object[] { result.ToString("F"), new XmlNodeType[] { result } };
            yield return new object[] { XmlConvert.ToString((int)result), new XmlNodeType[] { result } };
        }
        foreach (XmlNodeType[] results in new XmlNodeType[][]
        {
            [XmlNodeType.None, XmlNodeType.Element, XmlNodeType.Attribute, XmlNodeType.Text, XmlNodeType.CDATA, XmlNodeType.EntityReference, XmlNodeType.Entity, XmlNodeType.ProcessingInstruction, XmlNodeType.Comment, XmlNodeType.Document,
                XmlNodeType.DocumentType, XmlNodeType.DocumentFragment, XmlNodeType.Notation, XmlNodeType.Whitespace, XmlNodeType.SignificantWhitespace, XmlNodeType.EndElement, XmlNodeType.EndEntity, XmlNodeType.XmlDeclaration],
            [XmlNodeType.Attribute, XmlNodeType.CDATA, XmlNodeType.Comment, XmlNodeType.Document, XmlNodeType.DocumentFragment, XmlNodeType.DocumentType, XmlNodeType.Element, XmlNodeType.EndElement, XmlNodeType.EndEntity, XmlNodeType.Entity,
                        XmlNodeType.EntityReference, XmlNodeType.None, XmlNodeType.Notation, XmlNodeType.ProcessingInstruction, XmlNodeType.SignificantWhitespace, XmlNodeType.Text, XmlNodeType.Whitespace, XmlNodeType.XmlDeclaration],
            [XmlNodeType.Element, XmlNodeType.Text, XmlNodeType.EntityReference, XmlNodeType.ProcessingInstruction, XmlNodeType.Document, XmlNodeType.DocumentFragment, XmlNodeType.Whitespace, XmlNodeType.EndElement, XmlNodeType.XmlDeclaration],
            [XmlNodeType.EndElement, XmlNodeType.Notation, XmlNodeType.Document, XmlNodeType.Entity, XmlNodeType.Text, XmlNodeType.None],
            [XmlNodeType.Whitespace, XmlNodeType.Comment, XmlNodeType.Text],
            [XmlNodeType.XmlDeclaration, XmlNodeType.XmlDeclaration, XmlNodeType.DocumentType, XmlNodeType.Element, XmlNodeType.DocumentType, XmlNodeType.Element],
            [XmlNodeType.Notation, XmlNodeType.Entity, XmlNodeType.None],
            [XmlNodeType.Element, XmlNodeType.EndEntity]
        })
        {
            yield return new object[] { string.Join(" ", results.Select(v => v.ToString("F"))), results.AsEnumerable() };
            yield return new object[] { string.Join(" ", results.Select(v => XmlConvert.ToString((int)v))), results.AsEnumerable() };
            yield return new object[] { string.Join("\n", results.Select(v => v.ToString("F"))), results.AsEnumerable() };
            yield return new object[] { string.Join("\n", results.Select(v => XmlConvert.ToString((int)v))), results.AsEnumerable() };
        }
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(TryConvertToEnumValueTestData), DynamicDataSourceType.Method)]
    public void GetEnumListTestMethod(string value, IEnumerable<XmlNodeType> expected)
    {
        IEnumerable<XmlNodeType> actual = TypeConversionMethods.GetEnumList<XmlNodeType>(value);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual, EnumerableComparer<XmlNodeType>.Default);
    }

    public static IEnumerable<object[]> GetTryConvertToGuidTestData()
    {
        yield return new object[] { null, new TryGetExpected<Guid> { Returned = false, Result = default } };
        yield return new object[] { "", new TryGetExpected<Guid> { Returned = false, Result = default } };
        yield return new object[] { " ", new TryGetExpected<Guid> { Returned = false, Result = default } };
        foreach (Guid result in new[] { Guid.Empty, Guid.Parse("96059cbc30cb42ba952a0c57de39e3df") })
            foreach (string format in new string[] { "N", "D", "B", "P", "X" })
                yield return new object[] { result.ToString(format), new TryGetExpected<Guid> { Returned = true, Result = result } };
    }

    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryConvertToGuidTestData), DynamicDataSourceType.Method)]
    public void TryConvertToGuidTestMethod(string value, TryGetExpected<Guid> expected)
    {
        bool actualReturned = TypeConversionMethods.TryConvertToGuid(value, out Guid actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }
}
