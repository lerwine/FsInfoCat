using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests.XNodeNormalizerTestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XTextTestDataAttribute : Attribute, ITestDataSource
    {
        public bool ExcludeCustomNormalizer { get; set; }

        public bool ExcludeIndentLevel { get; set; }

        public static IEnumerable<(XText targetNode, TestXNodeNormalizer normalizer, int indentLevel, XText expected)> GetTestData()
        {
            yield return new(new XText(""), null, 0, new XText(""));
            yield return new(new XText(""), new(), 0, new XText(""));
            yield return new(new XCData(""), null, 0, new XCData(""));
            yield return new(new XCData(""), new(), 0, new XCData(""));
            yield return new(new XText("Test"), null, 0, new XText("Test"));
            yield return new(new XText("Test"), new(), 0, new XText("Test"));
            yield return new(new XCData("Test"), null, 0, new XCData("Test"));
            yield return new(new XCData("Test"), new(), 0, new XCData("Test"));
            yield return new(new XText("Test\r"), null, 0, new XText("Test"));
            yield return new(new XText("Test\r"), new(), 0, new XText("Test"));
            yield return new(new XCData("\rTest"), null, 0, new XCData("Test"));
            yield return new(new XCData("\rTest"), new(), 0, new XCData("Test"));
            yield return new(new XText("\rTest\rString\r"), null, 0, new XText($"Test{XNodeNormalizer.DefaultNewLine}String"));
            yield return new(new XText("\rTest\rString\r"), new(), 0, new XText($"Test{XNodeNormalizer.DefaultNewLine}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 0, new XCData("Test\nString"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 0, new XCData("Test\nString"));
            yield return new(new XText(""), null, 1, new XText(""));
            yield return new(new XText(""), new(), 1, new XText(""));
            yield return new(new XCData(""), null, 1, new XCData(""));
            yield return new(new XCData(""), new(), 1, new XCData(""));
            yield return new(new XText("Test"), null, 1, new XText("Test"));
            yield return new(new XText("Test"), new(), 1, new XText("Test"));
            yield return new(new XCData("Test"), null, 1, new XCData("Test"));
            yield return new(new XCData("Test"), new(), 1, new XCData("Test"));
            yield return new(new XText("Test\r"), null, 1, new XText("Test"));
            yield return new(new XText("Test\r"), new(), 1, new XText("Test"));
            yield return new(new XCData("\rTest"), null, 1, new XCData("Test"));
            yield return new(new XCData("\rTest"), new(), 1, new XCData("Test"));
            yield return new(new XText("\rTest\rString\r"), null, 1, new XText($"Test{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XText("\rTest\rString\r"), new(), 1, new XText($"Test{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 1, new XCData($"Test\n{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 1, new XCData($"Test\n{TestXNodeNormalizer.CustomIndent}String"));
            yield return new(new XText(""), null, 3, new XText(""));
            yield return new(new XText(""), new(), 3, new XText(""));
            yield return new(new XCData(""), null, 3, new XCData(""));
            yield return new(new XCData(""), new(), 3, new XCData(""));
            yield return new(new XText("Test"), null, 3, new XText("Test"));
            yield return new(new XText("Test"), new(), 3, new XText("Test"));
            yield return new(new XCData("Test"), null, 3, new XCData("Test"));
            yield return new(new XCData("Test"), new(), 3, new XCData("Test"));
            yield return new(new XText("Test\r"), null, 3, new XText("Test"));
            yield return new(new XText("Test\r"), new(), 3, new XText("Test"));
            yield return new(new XCData("\rTest"), null, 3, new XCData("Test"));
            yield return new(new XCData("\rTest"), new(), 3, new XCData("Test"));
            yield return new(new XText("\rTest\rString\r"), null, 3, new XText($"Test{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XText("\rTest\rString\r"), new(), 3, new XText($"Test{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 3, new XCData($"Test\n{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 3, new XCData($"Test\n{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}String"));
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            if (ExcludeCustomNormalizer)
            {
                if (ExcludeIndentLevel)
                    return GetTestData().Where(t => t.normalizer is null && t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.expected });
                return GetTestData().Where(t => t.normalizer is null).Select(t => new object[] { t.targetNode, t.indentLevel, t.expected });
            }
            if (ExcludeIndentLevel)
                return GetTestData().Where(t => t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.normalizer, t.expected });
            return GetTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected });
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (ExcludeCustomNormalizer)
            {
                if (ExcludeIndentLevel)
                    return $"{methodInfo.Name}(new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[0]).Value)}), new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[1]).Value)}))";
                return $"{methodInfo.Name}(new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[0]).Value)}), {data[1]}, new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[2]).Value)}))";
            }
            if (ExcludeIndentLevel)
                return $"{methodInfo.Name}(new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[0]).Value)}), {(data[1] is null ? "null" : "(custom normalizer)")}, new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[2]).Value)}))";
            return $"{methodInfo.Name}(new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[0]).Value)}), {(data[1] is null ? "null" : "(custom normalizer)")}, {data[2]}, new {(data[0] is XCData ? "XCData" : "XText")}({ExtensionMethods.ToPseudoCsText(((XText)data[3]).Value)}))";
        }
    }
}
