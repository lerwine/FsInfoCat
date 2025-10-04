using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using FsInfoCat.UnitTests.Helpers;
using Microsoft.CodeAnalysis;
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
        foreach (string input in new string[] { "82c986f20e3a4d12bfa183d00427a652", "7e032dd3-e35e-4c35-bbe4-aa194713ebb9", "{e2e84db2-919c-4383-897f-f78a2dfb5910}", "(3c69c773-b9df-49a3-9ebd-bfc283f7d570)",
                "{0xd593098a,0x20c4,0x4fc4,{0x85,0xc3,0x6e,0xcb,0xe4,0xd8,0xdf,0x62}}", "F560464D03414EF0B0C0D16AB323EE4B", "3F588C3E-4CC6-4B70-9D90-35B518DFEFCD", "{AFFDB1FB-2894-43F0-9849-D3413A3A041A}",
                "(3C540E12-1D6A-4935-846B-0A6F36C737C8)", "{0x2ED64E47,0x4993,0x4E8D,{0x87,0xE5,0xAA,0x46,0x6C,0xCC,0x60,0xB8}}" })
            yield return new object[] { input, new TryGetExpected<Guid> { Returned = true, Result = Guid.Parse(input) } };
        foreach (string input in new string[] { "d39ca85ca43148078ac238eece063b6", "f2e81fe-c8c3-4a90-bcc1-8b1ea522cf0c", "{cb2c975d-d6e9-4ea-ab7e-948ba1e7413c}", "(239ea2df-77a6-43de-8c6-e3521540feee)",
                "{0x28e6dfc7,0xd092,0x4c4f,{0x9d,0x57,0x92,0x51,0x74,0x10,0xff}}", "0B21B8BB7A674E4186FDCC9CA706936", "5D939C6B-04EB-46FF8625-1AB571EE10F7", "{F19D3F1D-CFE2-493E-ADB4531736215D4C}",
                "(B1DB2059-4575-49F7-0F7605A56128)", "{0x865AC0DA,0xEF83,{0xBA,0x0C,0xA6,0xA6,0x86,0x4E,0x8D,0xD5}}", "{2f8a1a94-0111-4ab4-8423-0fadb6ff446f", "(f5ea142c-3c94-492a-97d0-91b9b8f41efd",
                "{0xee98ad54,0xa84e,0x47a0,{0xb8,0x33,0x49,0x5e,0x8e,0x99,0xb9,0x13}", "8CAB29E6-044B-4253-91FD-F1DC4578C4AD}", "1664DFF9-DD41-4E08-94E1-ED61A8A02ECF)",
                "0xC9F0F25E,0x6359,0x4EE9,{0x9C,0x06,0x45,0x9B,0xE4,0xA2,0x3B,0xD2}}", "{0x8318f839,0xd8fd,0x4dd3,0xbb,0x9d,0xab,0x23,0x88,0x0a,0x08,0x77}}",
                "0xD48A1C3C,0x6860,0x4D1B,{0x9A,0x9D,0x7B,0xE9,0x31,0x13,0x74,0x5D}" })
            yield return new object[] { input, new TryGetExpected<Guid> { Returned = false, Result = default } };
        foreach (string pathAndQuery in new string[] { "621e2dfa9ea74425bab87cdcd37966eb", "2fecafd8-34e9-43e7-9582-8ea41fb58aba", "{652fecfc-509e-4e9e-866c-a6e5d0a226ff}",
                "(eed44599-4dee-43ae-ad93-08a769781a4d)", "{0xbf20562a,0x3b29,0x4588,{0xb6,0x8e,0xa6,0xa8,0x7f,0xec,0xf5,0xb1}}", "22FB2BEE10204EFDAD424A98A21CFCA3", "8CB49605-19B4-4DE8-90B1-EF6D51B76EE6",
                "{3C7DCB8B-470F-4717-BFB8-89F81B2E7D86}", "(9ACFC326-94A5-4AB5-8395-688A5F8FDC7D)", "{0x742D54D0,0x1E0C,0x4483,{0x8C,0x7B,0xB9,0x49,0x3F,0xF5,0x8E,0xDE}}" })
            yield return new object[] { new Uri($"urn:uuid:{pathAndQuery}", UriKind.Absolute).AbsoluteUri, new TryGetExpected<Guid> { Returned = true, Result = Guid.Parse(pathAndQuery) } };
        foreach (string pathAndQuery in new string[] { "d186d3ede3f4329bc6657e22a984d00", "1b9af329-367f-4a39-91cb-87dca675feb", "{8935ddd5-883-4579-92d4-68e2229d2045}",
                "(1f968468-cf32-4713-852-9e959f29c173)", "{0xf0a0c372,0x16b7,0x40e,{0x99,0x2f,0x20,0xfb,0x7a,0x71,0x4e,0x9d}}", "CB89C82EA7D1477B19F8D7EC1565324", "5A17CA68-3A9F-4EAD-8A45-1B12C9ACE46",
                "{1E6B2675-513C-45F4-F52E48579AD4}", "(369D1840-B93F-4ECA-BEF4)", "{0x2DF58E64,0x7FBC,0x48FF,{0x8E,0x4F,0x9C,0xB4,0xEB,0xC,0x1F,0xAD}}" })
            yield return new object[] { new Uri($"urn:{pathAndQuery}", UriKind.Absolute).AbsoluteUri, new TryGetExpected<Guid> { Returned = false, Result = default } };
        foreach (string input in new string[] { "uuid:202faaf7ea6f427fb696bce35892bab3", "uuid:854a7f67-9982-44cc-bdd4-66b2434578dd", "uuid:%7B45978926-7661-43de-9763-9aa588446695%7D",
                "uuid:(deb8dc32-76b5-402e-958e-80f2e821b6f4)", "uuid:%7B0x4864552b,0x9895,0x497c,%7B0x91,0x12,0x9f,0xe7,0x26,0xe0,0x23,0x8d%7D%7D", "uuid:86E1B90B614C441CB9EF04460869F996",
                "uuid:AB25B647-371D-4D42-8144-28EE87AB4B07", "uuid:%7B50348A4F-2C93-40D7-93B3-36D39A822170%7D", "uuid:(8CEBF5B3-C590-42F2-82A5-91DCE21651C8)",
                "uuid:%7B0xB417C3F5,0x57AB,0x4FA8,%7B0x87,0xA1,0x62,0xF7,0x78,0x64,0x1C,0x2B%7D%7D", "urn:d4252e57df024a7f83f2c816c914905c", "urn:b256c94d-3863-4b3b-991b-df6f2a8ec9ca",
                "urn:{59be2b59-f422-4bd7-a255-9da0aa868ea2}", "urn:(d264cd67-de90-4ee8-950e-05c657cb47e1)", "urn:{0x446e3d8d,0x368e,0x4211,{0xa7,0xa8,0xf9,0xdc,0x42,0xb8,0x00,0x6d}}",
                "urn:C5304659CBDA4621B3EF9418B26CF070", "urn:7297B8AE-5602-4D04-8AFA-BC2B89AFBAB1", "urn:{F541BD84-D2EB-402B-ACEE-25738EF4DA12}", "urn:(B5CC9B0C-39B3-4D42-B1B9-396D96068C6C)",
                "urn:{0x762D9535,0x8073,0x4218,{0xB5,0x12,0x6D,0x9B,0xBC,0x66,0xA7,0x7B}}" })
            yield return new object[] { input, new TryGetExpected<Guid> { Returned = false, Result = default } };
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
