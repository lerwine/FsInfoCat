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
            foreach ((int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent) in XContainerTestDataAttribute.GetIndents())
            {
                yield return new(new(""), null, IndentLevel, new(""));
                yield return new(new(""), new(), IndentLevel, new(""));
                foreach (string commentText in new[] { " ", "  ", "\r", "\n", "\r\n", "\r\n\r", " \n \r\n " })
                {
                    yield return new(new(commentText), null, IndentLevel, new(" "));
                    yield return new(new(commentText), new(), IndentLevel, new(" "));
                }
                foreach (string commentText in new[] { "Test", "Test  ", "  Test", "  Test  ", "Test\r", "\nTest", "\rTest\n", " \n\r\n Test \r\r\n " })
                {
                    yield return new(new(commentText), null, IndentLevel, new(" Test "));
                    yield return new(new(commentText), new(), IndentLevel, new(" Test "));
                }
                foreach (string commentText in new[] { "Test\rComment", "Test\rComment  ", "  Test\rComment", "  Test\rComment  ", "Test\rComment\r", "\rTest\rComment", "\rTest\rComment\r", " \n\r\n\r Test\rComment \r\r\n ", " \n\r\n\r Test\r\t    Comment \r\r\n " })
                {
                    yield return new(new(commentText), null, IndentLevel, new($"{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}"));
                    yield return new(new(commentText), new(), IndentLevel, new($"{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}"));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    foreach (string commentText in new[] { $"Test{nl}Comment", $"Test{nl}Comment  ", $"  Test{nl}Comment", $"  Test{nl}Comment  ", $"Test{nl}Comment{nl}", $"{nl}Test{nl}Comment", $"{nl}Test{nl}Comment{nl}", $" \r\n{nl} Test{nl}\t    Comment {nl}\r " })
                    {
                        yield return new(new(commentText), null, IndentLevel, new($"{nl}{DefaultLeadIndent}Test{nl}{DefaultLeadIndent}Comment{nl}{DefaultTrailingIndent}"));
                        yield return new(new(commentText), new(), IndentLevel, new($"{nl}{CustomLeadIndent}Test{nl}{CustomLeadIndent}Comment{nl}{CustomTrailingIndent}"));
                    }
                }
            }
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
