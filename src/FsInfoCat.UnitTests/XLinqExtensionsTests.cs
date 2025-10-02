using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FsInfoCat.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests;

[TestClass]
public class XLinqExtensionsTests
{
    private readonly XNodeEqualityComparer _nodeEqualityComparer = new();
    private readonly EnumerableComparer<XNode> _nodeEnumEqualityComparer = new(new XNodeEqualityComparer());

    public static IEnumerable<object[]> GetAttributeValueOrDefaultTestData1a()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "Test")), XName.Get("A"), "Dflt", "Test" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "Test")), XName.Get("A"), "", "Test" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "Test")), XName.Get("A"), null, "Test" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "")), XName.Get("A"), "Dflt", "" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "")), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "")), XName.Get("A"), null, "" };
        yield return new object[] { new XElement("Base", new XElement("X", new XAttribute("A", "Test"))), XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { new XElement("Base", new XElement("X", new XAttribute("A", "Test"))), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("Base", new XElement("X", new XAttribute("A", "Test"))), XName.Get("A"), null, null };
        yield return new object[] { new XElement("Base", new XAttribute("B", "Test")), XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { new XElement("Base", new XAttribute("B", "Test")), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("Base", new XAttribute("B", "Test")), XName.Get("A"), null, null };
        yield return new object[] { new XElement("Base", new XElement("A", "Test")), XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { new XElement("Base", new XElement("A", "Test")), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("Base", new XElement("A", "Test")), XName.Get("A"), null, null };
        yield return new object[] { new XElement("A", "Test"), XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { new XElement("A", "Test"), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("A", "Test"), XName.Get("A"), null, null };
        yield return new object[] { new XElement("Base"), XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { new XElement("Base"), XName.Get("A"), "", "" };
        yield return new object[] { new XElement("Base"), XName.Get("A"), null, null };
        yield return new object[] { null, XName.Get("A"), "Dflt", "Dflt" };
        yield return new object[] { null, XName.Get("A"), "", "" };
        yield return new object[] { null, XName.Get("A"), null, null };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.AttributeValueOrDefault(XElement, XName, string)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetAttributeValueOrDefaultTestData1a), DynamicDataSourceType.Method)]
    public void AttributeValueOrDefaultTestMethod1a(XElement element, XName attributeName, string defaultValue, string expected)
    {
        string actual = element.AttributeValueOrDefault(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetAttributeValueOrDefaultTestData1b()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "Test")), XName.Get("A"), "Test" };
        yield return new object[] { new XElement("Base", new XAttribute("A", "")), XName.Get("A"), "" };
        yield return new object[] { new XElement("Base", new XElement("X", new XAttribute("A", "Test"))), XName.Get("A"), null };
        yield return new object[] { new XElement("Base", new XAttribute("B", "Test")), XName.Get("A"), null };
        yield return new object[] { new XElement("Base", new XElement("A", "Test")), XName.Get("A"), null };
        yield return new object[] { new XElement("A", "Test"), XName.Get("A"), null };
        yield return new object[] { new XElement("Base"), XName.Get("A"), null };
        yield return new object[] { null, XName.Get("A"), null };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.AttributeValueOrDefault(XElement, XName, string)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetAttributeValueOrDefaultTestData1b), DynamicDataSourceType.Method)]
    public void AttributeValueOrDefaultTestMethod1b(XElement element, XName attributeName, string expected)
    {
        string actual = element.AttributeValueOrDefault(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetAttributeValueOrDefaultTestData2a()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "7")), XName.Get("A"), (string v) => XmlConvert.ToInt32(v), 12, 7 };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.AttributeValueOrDefault{T}(XElement, XName, Func{string, T}, T)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetAttributeValueOrDefaultTestData2a), DynamicDataSourceType.Method)]
    public void AttributeValueOrDefaultTestMethod2a(XElement element, XName attributeName, Func<string, int> convert, int ifNotPresent, int expected)
    {
        int actual = element.AttributeValueOrDefault(attributeName, convert, ifNotPresent);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetAttributeValueOrDefaultTestData2b()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "7")), XName.Get("A"), (string v) => XmlConvert.ToInt32(v), 7 };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.AttributeValueOrDefault{T}(XElement, XName, Func{string, T}, T)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetAttributeValueOrDefaultTestData2b), DynamicDataSourceType.Method)]
    public void AttributeValueOrDefaultTestMethod2b(XElement element, XName attributeName, Func<string, int> convert, int expected)
    {
        int actual = element.AttributeValueOrDefault(attributeName, convert);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAdjacentNodesTestData()
    {
        XText child1Child = new("Nested Text");
        XElement element1Child = new("Child2");
        XNode[] element0Children =
        [
            new XElement("Child0"),
            new XElement("Child1",
                new XAttribute("AttrA", "Third Value"),
                child1Child
            )
        ];
        XNode[] rootChildren =
        [
            new XText("Normal text"),
            new XElement("Element0", element0Children),
            new XComment("Comment Text"),
            new XElement("Element1", new XAttribute("AttrB", "Fourth Value"), element1Child ),
            new XCData("Data Text"),
            new XElement("Element3", new XAttribute("Attrc", "Fifth Value") )
        ];
        XElement root = new("R",
        [
            .. new XObject[]
            {
                new XAttribute("attr0", "First value"),
                new XAttribute("attr1", "Second value")
            },
            .. rootChildren,
        ]);
        XDocument doc = new(root);
        yield return new object[] { child1Child, Enumerable.Empty<XNode>() };
        yield return new object[] { element1Child, Enumerable.Empty<XNode>() };
        for (var i = 0; i < element0Children.Length; i++)
            yield return new object[] { element0Children[i], element0Children.Take(i).Skip(1) };
        for (var i = 0; i < rootChildren.Length; i++)
            yield return new object[] { rootChildren[i], rootChildren.Take(i).Skip(1) };
        yield return new object[] { root, Enumerable.Empty<XNode>() };
        yield return new object[] { doc, Enumerable.Empty<XNode>() };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAdjacentNodes{T}(T)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAdjacentNodesTestData), DynamicDataSourceType.Method)]
    public void GetAdjacentNodesTestMethod(XNode node, IEnumerable<XNode> expected)
    {
        IEnumerable<XNode> actual = node.GetAdjacentNodes();
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual, _nodeEnumEqualityComparer);
    }

    public static IEnumerable<object[]> GetGetAttributeBooleanTestData1()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "true")), XName.Get("A"), true };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeBoolean(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeBooleanTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeBooleanTestMethod1(XElement element, XName attributeName, bool? expected)
    {
        bool? actual = element.GetAttributeBoolean(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeBooleanTestData2()
    {
        yield return new object[] { new XElement("Base", new XAttribute("A", "true")), XName.Get("A"), false, true };
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeBoolean(XElement, XName, bool)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeBooleanTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeBooleanTestMethod2(XElement element, XName attributeName, bool defaultValue, bool expected)
    {
        bool actual = element.GetAttributeBoolean(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeBytesTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeBytesTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeBytes(XElement, XName, BinaryStringAffinity)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeBytesTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeBytesTestMethod1(XElement element, XName attributeName, BinaryStringAffinity affinity, byte[] expected)
    {
        byte[] actual = element.GetAttributeBytes(attributeName, affinity);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeBytesTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeBytesTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeBytes(XElement, XName, BinaryStringAffinity, byte[])"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeBytesTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeBytesTestMethod2(XElement element, XName attributeName, BinaryStringAffinity affinity, byte[] defaultValue, byte[] expected)
    {
        byte[] actual = element.GetAttributeBytes(attributeName, affinity, defaultValue);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeDateTimeTestData1a()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeDateTimeTestData1a)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeDateTime(XElement, XName, DateTime, XmlDateTimeSerializationMode)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeDateTimeTestData1a), DynamicDataSourceType.Method)]
    public void GetAttributeDateTimeTestMethod1a(XElement element, XName attributeName, DateTime defaultValue, XmlDateTimeSerializationMode dateTimeOption, DateTime expected)
    {
        DateTime actual = element.GetAttributeDateTime(attributeName, defaultValue, dateTimeOption);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeDateTimeTestData1b()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeDateTimeTestData1b)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeDateTime(XElement, XName, DateTime, XmlDateTimeSerializationMode)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeDateTimeTestData1b), DynamicDataSourceType.Method)]
    public void GetAttributeDateTimeTestMethod1b(XElement element, XName attributeName, DateTime defaultValue, DateTime expected)
    {
        DateTime actual = element.GetAttributeDateTime(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeDateTimeTestData2a()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeDateTimeTestData2a)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeDateTime(XElement, XName, XmlDateTimeSerializationMode)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeDateTimeTestData2a), DynamicDataSourceType.Method)]
    public void GetAttributeDateTimeTestMethod2a(XElement element, XName attributeName, XmlDateTimeSerializationMode dateTimeOption, DateTime? expected)
    {
        DateTime? actual = element.GetAttributeDateTime(attributeName, dateTimeOption);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeDateTimeTestData2b()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeDateTimeTestData2b)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeDateTime(XElement, XName, XmlDateTimeSerializationMode)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeDateTimeTestData2b), DynamicDataSourceType.Method)]
    public void GetAttributeDateTimeTestMethod2b(XElement element, XName attributeName, DateTime? expected)
    {
        DateTime? actual = element.GetAttributeDateTime(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeEnumFlagsTestData()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeEnumFlagsTestData)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeEnumFlags{TEnum}(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeEnumFlagsTestData), DynamicDataSourceType.Method)]
    public void GetAttributeEnumFlagsTestMethod(XElement element, XName attributeName, IEnumerable<XmlNodeType> expected)
    {
        IEnumerable<XmlNodeType> actual = element.GetAttributeEnumFlags<XmlNodeType>(attributeName);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeEnumValueTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeEnumValueTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeEnumValue{TEnum}(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeEnumValueTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeEnumValueTestMethod1(XElement element, XName attributeName, XmlNodeType? expected)
    {
        XmlNodeType? actual = element.GetAttributeEnumValue<XmlNodeType>(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeEnumValueTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeEnumValueTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeEnumValue{TEnum}(XElement, XName, TEnum)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeEnumValueTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeEnumValueTestMethod2(XElement element, XName attributeName, XmlNodeType defaultValue, XmlNodeType expected)
    {
        XmlNodeType actual = element.GetAttributeEnumValue(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeGuidTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeGuidTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeGuid(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeGuidTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeGuidTestMethod1(XElement element, XName attributeName, Guid? expected)
    {
        Guid? actual = element.GetAttributeGuid(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeGuidTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeGuidTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeGuid(XElement, XName, Guid)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeGuidTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeGuidTestMethod2(XElement element, XName attributeName, Guid defaultValue, Guid expected)
    {
        Guid actual = element.GetAttributeGuid(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt16TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt16TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt16(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt16TestData1), DynamicDataSourceType.Method)]
    public void GetAttributeInt16TestMethod1(XElement element, XName attributeName, short? expected)
    {
        short? actual = element.GetAttributeInt16(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt16TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt16TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt16(XElement, XName, short)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt16TestData2), DynamicDataSourceType.Method)]
    public void GetAttributeInt16TestMethod2(XElement element, XName attributeName, short defaultValue, short expected)
    {
        short actual = element.GetAttributeInt16(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt32TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt32TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt32(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt32TestData1), DynamicDataSourceType.Method)]
    public void GetAttributeInt32TestMethod1(XElement element, XName attributeName, int? expected)
    {
        int? actual = element.GetAttributeInt32(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt32TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt32TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt32(XElement, XName, int)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt32TestData2), DynamicDataSourceType.Method)]
    public void GetAttributeInt32TestMethod2(XElement element, XName attributeName, int defaultValue, int expected)
    {
        int actual = element.GetAttributeInt32(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt64TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt64TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt64(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt64TestData1), DynamicDataSourceType.Method)]
    public void GetAttributeInt64TestMethod1(XElement element, XName attributeName, long? expected)
    {
        long? actual = element.GetAttributeInt64(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeInt64TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeInt64TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeInt64(XElement, XName, long)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeInt64TestData2), DynamicDataSourceType.Method)]
    public void GetAttributeInt64TestMethod2(XElement element, XName attributeName, long defaultValue, long expected)
    {
        long actual = element.GetAttributeInt64(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeTimeSpanTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeTimeSpanTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeTimeSpan(XElement, XName)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeTimeSpanTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeTimeSpanTestMethod1(XElement element, XName attributeName, TimeSpan? expected)
    {
        TimeSpan? actual = element.GetAttributeTimeSpan(attributeName);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeTimeSpanTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeTimeSpanTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeTimeSpan(XElement, XName, TimeSpan)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeTimeSpanTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeTimeSpanTestMethod2(XElement element, XName attributeName, TimeSpan defaultValue, TimeSpan expected)
    {
        TimeSpan actual = element.GetAttributeTimeSpan(attributeName, defaultValue);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeValueTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeValueTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeValue(XElement, XName, Func{string})"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeValueTestData1), DynamicDataSourceType.Method)]
    public void GetAttributeValueTestMethod1(XElement element, XName attributeName, Func<string> getDefaultValue, string expected)
    {
        string actual = element.GetAttributeValue(attributeName, getDefaultValue);
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetGetAttributeValueTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetGetAttributeValueTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.GetAttributeValue{T}(XElement, XName, Func{string, T}, Func{T})"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetGetAttributeValueTestData2), DynamicDataSourceType.Method)]
    public void GetAttributeValueTestMethod2(XElement element, XName attributeName, Func<string, int> getDefaultValue, Func<int> ifNotPresent, int expected)
    {
        int actual = element.GetAttributeValue(attributeName, getDefaultValue, ifNotPresent);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetToFsInfoCatExportXmlnsTestData()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetToFsInfoCatExportXmlnsTestData)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.ToFsInfoCatExportXmlns(string)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetToFsInfoCatExportXmlnsTestData), DynamicDataSourceType.Method)]
    public void ToFsInfoCatExportXmlnsTestMethod(string name, XName expected)
    {
        XName actual = name.ToFsInfoCatExportXmlns();
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> GetTryGetAttributeBooleanTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeBooleanTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeBoolean(XElement, XName, out bool)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeBooleanTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeBooleanTestMethod1(XElement element, XName attributeName, TryGetExpected<bool> expected)
    {
        bool actualReturned = element.TryGetAttributeBoolean(attributeName, out bool actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeBooleanTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeBooleanTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeBoolean(XElement, XName, out bool?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeBooleanTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeBooleanTestMethod2(XElement element, XName attributeName, TryGetExpected<bool?> expected)
    {
        bool actualReturned = element.TryGetAttributeBoolean(attributeName, out bool? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeBytesTestData()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeBytesTestData)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeBytes(XElement, XName, BinaryStringAffinity, out byte[])"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeBytesTestData), DynamicDataSourceType.Method)]
    public void TryGetAttributeBytesTestMethod(XElement element, XName attributeName, BinaryStringAffinity affinity, TryGetExpected<byte[]> expected)
    {
        bool actualReturned = element.TryGetAttributeBytes(attributeName, affinity, out byte[] actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeDateTimeTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeDateTimeTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeDateTime(XElement, XName, out DateTime)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeDateTimeTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeDateTimeTestMethod1(XElement element, XName attributeName, TryGetExpected<DateTime> expected)
    {
        bool actualReturned = element.TryGetAttributeDateTime(attributeName, out DateTime actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeDateTimeTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeDateTimeTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeDateTime(XElement, XName, out DateTime?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeDateTimeTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeDateTimeTestMethod2(XElement element, XName attributeName, TryGetExpected<DateTime?> expected)
    {
        bool actualReturned = element.TryGetAttributeDateTime(attributeName, out DateTime? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeDateTimeTestData3()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeDateTimeTestData3)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeDateTime(XElement, XName, XmlDateTimeSerializationMode, out DateTime)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeDateTimeTestData3), DynamicDataSourceType.Method)]
    public void TryGetAttributeDateTimeTestMethod3(XElement element, XName attributeName, XmlDateTimeSerializationMode dateTimeOption, TryGetExpected<DateTime> expected)
    {
        bool actualReturned = element.TryGetAttributeDateTime(attributeName, dateTimeOption, out DateTime actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeDateTimeTestData4()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeDateTimeTestData4)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeDateTime(XElement, XName, XmlDateTimeSerializationMode, out DateTime?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeDateTimeTestData4), DynamicDataSourceType.Method)]
    public void TryGetAttributeDateTimeTestMethod4(XElement element, XName attributeName, XmlDateTimeSerializationMode dateTimeOption, TryGetExpected<DateTime?> expected)
    {
        bool actualReturned = element.TryGetAttributeDateTime(attributeName, dateTimeOption, out DateTime? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeEnumValueTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeEnumValueTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeEnumValue{TEnum}(XElement, XName, out TEnum)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeEnumValueTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeEnumValueTestMethod1(XElement element, XName attributeName, TryGetExpected<XmlNodeType> expected)
    {
        bool actualReturned = element.TryGetAttributeEnumValue(attributeName, out XmlNodeType actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeEnumValueTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeEnumValueTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeEnumValue{TEnum}(XElement, XName, out TEnum?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeEnumValueTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeEnumValueTestMethod2(XElement element, XName attributeName, TryGetExpected<XmlNodeType?> expected)
    {
        bool actualReturned = element.TryGetAttributeEnumValue(attributeName, out XmlNodeType? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeGuidTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeGuidTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeGuid(XElement, XName, out Guid)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeGuidTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeGuidTestMethod1(XElement element, XName attributeName, TryGetExpected<Guid> expected)
    {
        bool actualReturned = element.TryGetAttributeGuid(attributeName, out Guid actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeGuidTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeGuidTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeGuid(XElement, XName, out Guid?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeGuidTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeGuidTestMethod2(XElement element, XName attributeName, TryGetExpected<Guid?> expected)
    {
        bool actualReturned = element.TryGetAttributeGuid(attributeName, out Guid? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt16TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt16TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt16(XElement, XName, out short)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt16TestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt16TestMethod1(XElement element, XName attributeName, TryGetExpected<short> expected)
    {
        bool actualReturned = element.TryGetAttributeInt16(attributeName, out short actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt16TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt16TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt16(XElement, XName, out short?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt16TestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt16TestMethod2(XElement element, XName attributeName, TryGetExpected<short?> expected)
    {
        bool actualReturned = element.TryGetAttributeInt16(attributeName, out short? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt32TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt32TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt32(XElement, XName, out int)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt32TestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt32TestMethod1(XElement element, XName attributeName, TryGetExpected<int> expected)
    {
        bool actualReturned = element.TryGetAttributeInt32(attributeName, out int actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt32TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt32TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt32(XElement, XName, out int?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt32TestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt32TestMethod2(XElement element, XName attributeName, TryGetExpected<int?> expected)
    {
        bool actualReturned = element.TryGetAttributeInt32(attributeName, out int? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt64TestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt64TestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt64(XElement, XName, out long)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt64TestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt64TestMethod1(XElement element, XName attributeName, TryGetExpected<long> expected)
    {
        bool actualReturned = element.TryGetAttributeInt64(attributeName, out long actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeInt64TestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeInt64TestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeInt64(XElement, XName, out long?)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeInt64TestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeInt64TestMethod2(XElement element, XName attributeName, TryGetExpected<long?> expected)
    {
        bool actualReturned = element.TryGetAttributeInt64(attributeName, out long? actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeTimeSpanTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeTimeSpanTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeTimeSpan(XElement, XName, out TimeSpan)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeTimeSpanTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeTimeSpanTestMethod1(XElement element, XName attributeName, TryGetExpected<TimeSpan> expected)
    {
        bool actualReturned = element.TryGetAttributeTimeSpan(attributeName, out TimeSpan actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeTimeSpanTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeTimeSpanTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeTimeSpan(XElement, XName, out TimeSpan)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeTimeSpanTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeTimeSpanTestMethod2(XElement element, XName attributeName, TryGetExpected<TimeSpan> expected)
    {
        bool actualReturned = element.TryGetAttributeTimeSpan(attributeName, out TimeSpan actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeValueTestData1()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeValueTestData1)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeValue(XElement, XName, out string)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeValueTestData1), DynamicDataSourceType.Method)]
    public void TryGetAttributeValueTestMethod1(XElement element, XName attributeName, TryGetExpected<string> expected)
    {
        bool actualReturned = element.TryGetAttributeValue(attributeName, out string actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }

    public static IEnumerable<object[]> GetTryGetAttributeValueTestData2()
    {
        throw new AssertInconclusiveException($"Dynamic Data Method {nameof(GetTryGetAttributeValueTestData2)} not implemented.");
    }

    /// <summary>
    /// Test method for <see cref="XLinqExtensions.TryGetAttributeValue{T}(XElement, XName, Func{string, T}, out T)"/>
    /// </summary>
    [DataTestMethod, Priority(1)]
    [DynamicData(nameof(GetTryGetAttributeValueTestData2), DynamicDataSourceType.Method)]
    public void TryGetAttributeValueTestMethod2(XElement element, XName attributeName, Func<string, XmlNodeType> converter, TryGetExpected<XmlNodeType> expected)
    {
        bool actualReturned = element.TryGetAttributeValue(attributeName, converter, out XmlNodeType actualResult);
        Assert.AreEqual(expected.Returned, actualReturned);
        Assert.AreEqual(expected.Result, actualResult);
    }
}
