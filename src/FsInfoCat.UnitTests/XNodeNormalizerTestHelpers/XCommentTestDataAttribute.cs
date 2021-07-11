using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests.XNodeNormalizerTestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XCommentTestDataAttribute : Attribute, ITestDataSource
    {
        public bool ExcludeCustomNormalizer { get; set; }

        public bool ExcludeIndentLevel { get; set; }

        public static IEnumerable<(XComment targetNode, TestXNodeNormalizer normalizer, int indentLevel, XComment expected)> GetTestData()
        {
            yield return new(new(""), null, 0, new(""));
            yield return new(new(""), null, 2, new(""));
            yield return new(new(""), new(), 0, new(""));
            yield return new(new(""), new(), 2, new(""));
            yield return new(new(" "), null, 0, new(" "));
            yield return new(new(" "), new(), 0, new(" "));
            yield return new(new("\n"), null, 0, new(" "));
            yield return new(new("\n"), null, 2, new(" "));
            yield return new(new("\n"), new(), 0, new(" "));
            yield return new(new("\n"), new(), 2, new(" "));
            yield return new(new("Test1"), null, 0, new(" Test1 "));
            yield return new(new("Test1"), new(), 0, new(" Test1 "));
            yield return new(new("Test1\n"), null, 0, new(" Test1 "));
            yield return new(new("Test1\n"), new(), 0, new(" Test1 "));
            yield return new(new("\nTest1"), null, 0, new(" Test1 "));
            yield return new(new("\nTest1"), new(), 0, new(" Test1 "));
            yield return new(new("\n\nTest1\n\r\n"), null, 0, new(" Test1 "));
            yield return new(new("\n\nTest2\n\r\n"), null, 2, new(" Test2 "));
            yield return new(new("\n\rTest2\n\r\n"), new(), 0, new(" Test2 "));
            yield return new(new("\n\nTest2\n\r\n"), new(), 2, new(" Test2 "));
            yield return new(new("Test\nComment1"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}Test\nComment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}Test\nComment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), null, 1, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test" +
                $"\n{XNodeNormalizer.DefaultIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), new(), 1, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test" +
                $"\n{TestXNodeNormalizer.CustomIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), null, 2, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Test" +
                $"\n{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Comment1{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}"));
            yield return new(new("Test\nComment1"), new(), 2, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Test" +
                $"\n{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Comment1{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}"));
            yield return new(new("\r\n        Test\r\n        Comment1\r\n    "), null, 0, new($"{XNodeNormalizer.DefaultNewLine}Test" +
                $"\r\nComment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\r\n        Test\r\n        Comment1\r\n    "), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}Test" +
                $"\r\nComment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}Test" +
                $"\r\nComment2{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}Test" +
                $"\r\nComment2{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), null, 3, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Test" +
                $"\r\n{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Comment2" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), new(), 3,
                new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Test" +
                $"\r\n{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Comment2" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}"));
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
                    return $"{methodInfo.Name}(new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[0]).Value)}), new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[1]).Value)}))";
                return $"{methodInfo.Name}(new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[0]).Value)}), {data[1]}, new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[2]).Value)}))";
            }
            if (ExcludeIndentLevel)
                return $"{methodInfo.Name}(new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[0]).Value)}), {(data[1] is null ? "null" : "(custom normalizer)")}, new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[2]).Value)}))";
            return $"{methodInfo.Name}(new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[0]).Value)}), {(data[1] is null ? "null" : "(custom normalizer)")}, {data[2]}, new XComment({ExtensionMethods.ToPseudoCsText(((XComment)data[3]).Value)}))";
        }
    }
}
