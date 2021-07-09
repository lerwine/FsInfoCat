using Microsoft.VisualStudio.TestTools.UnitTesting;
using FsInfoCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.UnitTests
{
    [TestClass()]
    public class XNodeNormalizerTests
    {
        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods for <see cref="XNodeNormalizer.NonNormalizedLineSeparatorRegex"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods.</param>
        /// <param name="replacement">The replacement text for the <see cref="Regex.Replace(string, string)"/> method if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="true"/>;
        /// otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, string)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", null, null, DisplayName = "(space)")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("\t", null, null, DisplayName = "(tab)")]
        [DataRow("\n", null, null, DisplayName = "\"\\n\"")]
        [DataRow("\r\n", null, null, DisplayName = "\"\\r\\n\"")]
        [DataRow("\t\n", null, null, DisplayName = "\"\\t\\n\"")]
        [DataRow("\t\r\n", null, null, DisplayName = "\"\\t\\r\\n\"")]
        [DataRow("\n\t", null, null, DisplayName = "\"\\n\\t\"")]
        [DataRow("\r\n\t", null, null, DisplayName = "\"\\r\\n\\t\"")]
        [DataRow("\r", "\n", "\n", DisplayName = "\"\\r\"")]
        [DataRow("\x00", "\r\n", "\r\n", DisplayName = "\"\\x00\"")]
        [DataRow("\x1f", "\n", "\n", DisplayName = "\"\\x1f\"")]
        [DataRow("\f", "\r\n", "\r\n", DisplayName = "\"\\f\"")]
        [DataRow("\v", "\n", "\n", DisplayName = "\"\\v\"")]
        [DataRow("\u2028", "\r\n", "\r\n", DisplayName = "\"\\u2028\"")]
        [DataRow("\u2029", "\n", "\n", DisplayName = "\"\\u2029\"")]
        [DataRow("\n\r", "\r\n", "\n\r\n", DisplayName = "\"\\n\\r\", \"\\r\\n\"")]
        [DataRow("\n\r", "\n", "\n\n", DisplayName = "\"\\n\\r\", \"\\n\"")]
        [DataRow("\r\t", "\n", "\n\t", DisplayName = "\"\\r\\t\", \"\\n\"")]
        [DataRow("\r\t", "\r\n", "\r\n\t", DisplayName = "\"\\r\\t\", \"\\r\\n\"")]
        [DataRow(" \x00 \x1f \f\v \n\r\n\r\u2028X", "\r\n", " \r\n \r\n \r\n\r\n \n\r\n\r\n\r\nX", DisplayName = "\" \\x00 \\x1f \\f\\v \\n\\r\\n\\r\\u2028X\", \"\\r\\n\"")]
        [DataRow(" \x00 \x1f \f\v \n\r\n\r\u2028X", "\n", " \n \n \n\n \n\r\n\n\nX", DisplayName = "\"\\x00\", \"\\n\"")]
        [DataRow("a\b \x1a \n\r\r\n\r\u2029 ", "\r\n", "a\r\n \r\n \n\r\n\r\n\r\n\r\n ", DisplayName = "\"a\\b \\x1a \\n\\r\\r\\n\\r\\u2029 \", \"\\r\\n\"")]
        [DataRow("a\b \x1a \n\r\r\n\r\u2029 ", "\n", "a\n \n \n\n\r\n\n\n ", DisplayName = "\"a\\b \\x1a \\n\\r\\r\\n\\r\\u2029 \", \"\\n\"")]
        [DataTestMethod()]
        public void NonNormalizedLineSeparatorRegexTest([DisallowNull] string input, string replacement, string expected)
        {
            bool isMatchResult = XNodeNormalizer.NonNormalizedLineSeparatorRegex.IsMatch(input);
            if (replacement is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual = XNodeNormalizer.NonNormalizedLineSeparatorRegex.Replace(input, replacement);
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods for <see cref="XNodeNormalizer.NonNormalizedWhiteSpaceRegex"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods.</param>
        /// <param name="replacement">The replacement text for the <see cref="Regex.Replace(string, string)"/> method if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="true"/>;
        /// otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, string)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", null, null, DisplayName = "(space)")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("\t", " ", " ", DisplayName = "(tab)")]
        [DataRow("\n", null, null, DisplayName = "\"\\n\"")]
        [DataRow("\r\n", null, null, DisplayName = "\"\\r\\n\"")]
        [DataRow("\t\n", " ", " \n", DisplayName = "\"\\t\\n\"")]
        [DataRow("\t\r\n", " ", " \r\n", DisplayName = "\"\\t\\r\\n\"")]
        [DataRow("\n\t", " ", "\n ", DisplayName = "\"\\n\\t\"")]
        [DataRow("\r\n\t", " ", "\r\n ", DisplayName = "\"\\r\\n\\t\"")]
        [DataTestMethod()]
        public void NonNormalizedWhiteSpaceRegexTest([DisallowNull] string input, string replacement, string expected)
        {
            bool isMatchResult = XNodeNormalizer.NonNormalizedWhiteSpaceRegex.IsMatch(input);
            if (replacement is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual = XNodeNormalizer.NonNormalizedWhiteSpaceRegex.Replace(input, replacement);
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods for <see cref="XNodeNormalizer.ExtraneousLineWhitespaceRegex"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods.</param>
        /// <param name="replacement">The replacement text for the <see cref="Regex.Replace(string, string)"/> method if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="true"/>;
        /// otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, string)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", "", "", DisplayName = "(space)")]
        [DataRow("     ", "", "", DisplayName = "(space*4)")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("\n", null, null, DisplayName = "\"\\n\"")]
        [DataRow("\r\n", null, null, DisplayName = "\"\\r\\n\"")]
        [DataRow(" \n", "", "\n", DisplayName = "\" \\n\"")]
        [DataRow("    \n", "", "\n", DisplayName = "\"    \\n\"")]
        [DataRow(" \r\n", "", "\r\n", DisplayName = "\" \\r\\n\"")]
        [DataRow("    \r\n", "", "\r\n", DisplayName = "\"    \\r\\n\"")]
        [DataRow("\n ", "", "\n", DisplayName = "\"\\n \"")]
        [DataRow("\n    ", "", "\n", DisplayName = "\"\\n    \"")]
        [DataRow("\r\n ", "", "\r\n", DisplayName = "\"\\r\\n \"")]
        [DataRow("\r\n    ", "", "\r\n", DisplayName = "\"\\r\\n    \"")]
        [DataRow(" abc123~!@ ", "", "abc123~!@", DisplayName = "\" abc123~!@ \"")]
        [DataRow("     abc123~!@     ", "", "abc123~!@", DisplayName = "\"     abc123~!@     \"")]
        [DataRow("\nabc123~!@\r\n", null, null, DisplayName = "\"\\nabc123~!@\\r\\n\"")]
        [DataRow(" \n abc123~!@ \r\n ", "", "\nabc123~!@\r\n", DisplayName = "\" \\n abc123~!@ \\r\\n \"")]
        [DataRow("   \n   abc123~!@   \r\n   ", "", "\nabc123~!@\r\n", DisplayName = "\"   \\n   abc123~!@   \\r\\n   \"")]
        [DataRow("   \n   abc123~!@\n\r\n   \r\n   ", "", "\nabc123~!@\n\r\n\r\n", DisplayName = "\"   \\n   abc123~!@\\n\\r\\n   \\r\\n   \"")]
        [DataRow("Test\nComment", null, null, DisplayName = "Test\\nComment")]
        [DataRow(" Test\nComment ", "", "Test\nComment", DisplayName = " Test\\nComment ")]
        [DataRow("Test \n Comment", "", "Test\nComment", DisplayName = "Test \\n Comment")]
        [DataRow(" Test \n\r\n Comment ", "", "Test\n\r\nComment", DisplayName = " Test \\n\\r\\n Comment ")]
        [DataRow(" Test \n\r   \n Comment ", "", "Test\n\r\nComment", DisplayName = " Test \\n   \\r\\n Comment ")]
        [DataTestMethod()]
        public void ExtraneousLineWhitespaceRegexTest([DisallowNull] string input, string replacement, string expected)
        {
            bool isMatchResult = XNodeNormalizer.ExtraneousLineWhitespaceRegex.IsMatch(input);
            if (replacement is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual = XNodeNormalizer.ExtraneousLineWhitespaceRegex.Replace(input, replacement);
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, MatchEvaluator)"/> methods for <see cref="XNodeNormalizer.TrailingNewLineWithOptWs"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, MatchEvaluator)"/> methods.</param>
        /// <param name="toAppend">The text that the <see cref="MatchEvaluator"/> appends to the <see cref="Capture.Value"/> at index <c>1</c> of the <see cref="Match.Groups"/> property, replacing
        /// any whitespace that was captured as well; An empty string if the <see cref="MatchEvaluator"/> should return only the <see cref="Capture.Value"/> at index <c>1</c> of
        /// the <see cref="Match.Groups"/> property; otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, MatchEvaluator)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", null, null, DisplayName = "(space)")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("\t", null, null, DisplayName = "(tab)")]
        [DataRow("\n", "    ", "\n    ", DisplayName = "\"\\n\"")]
        [DataRow("\r\n", "    ", "\r\n    ", DisplayName = "\"\\r\\n\"")]
        [DataRow("\t\n", "    ", "\t\n    ", DisplayName = "\"\\t\\n\"")]
        [DataRow("\t\r\n", "    ", "\t\r\n    ", DisplayName = "\"\\t\\r\\n\"")]
        [DataRow("\n\t", "    ", "\n    ", DisplayName = "\"\\n\\t\"")]
        [DataRow("\r\n\t", "    ", "\r\n    ", DisplayName = "\"\\r\\n\\t\"")]
        [DataRow("abc123~!@\nTest", null, null, DisplayName = "\"abc123~!@\\nTest\"")]
        [DataRow("\r\n\r\nabc123~!@\nTest\n\r\n", "    ", "\r\n\r\nabc123~!@\nTest\n\r\n    ", DisplayName = "\"\\r\\n\\r\\nabc123~!@\\nTest\\n\\r\\n\"")]
        [DataRow("\r\n\r\nabc123~!@\nTest\n\r\n    ", "    ", "\r\n\r\nabc123~!@\nTest\n\r\n    ", DisplayName = "\"\\r\\n\\r\\nabc123~!@\\nTest\\n\\r\\n    \"")]
        [DataRow("\r\n\r\nabc123~!@\nTest\n\r\n        ", "    ", "\r\n\r\nabc123~!@\nTest\n\r\n    ", DisplayName = "\"\\r\\n\\r\\nabc123~!@\\nTest\\n\\r\\n        \"")]
        [DataTestMethod()]
        public void TrailingNewLineWithOptWsTest([DisallowNull] string input, string toAppend, string expected)
        {
            bool isMatchResult = XNodeNormalizer.TrailingNewLineWithOptWs.IsMatch(input);
            if (toAppend is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual;
                if (toAppend.Length > 0)
                    actual = XNodeNormalizer.TrailingNewLineWithOptWs.Replace(input, match =>
                    {
                        Assert.AreEqual(3, match.Groups.Count);
                        Assert.IsTrue(match.Groups[1].Success);
                        return $"{match.Groups[1].Value}{toAppend}";
                    });
                else
                    actual = XNodeNormalizer.TrailingNewLineWithOptWs.Replace(input, match =>
                    {
                        Assert.AreEqual(3, match.Groups.Count);
                        Assert.IsTrue(match.Groups[1].Success);
                        return match.Groups[1].Value;
                    });
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods for <see cref="XNodeNormalizer.EmptyOrWhiteSpaceLineRegex"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods.</param>
        /// <param name="replacement">The replacement text for the <see cref="Regex.Replace(string, string)"/> method if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="true"/>;
        /// otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, string)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("  \n  \r\nabc123~!@\r\n  \n  ", "", "abc123~!@", DisplayName = "\"  \\n  \\r\\nabc123~!@\\r\\n  \\n  \"")]
        [DataRow("\n\r\n  abc123~!@  ", "", "  abc123~!@  ", DisplayName = "\"\\n\\r\\n  abc123~!@  \"")]
        [DataRow("\n\r\n  abc123~!@  \r\n\n", "", "  abc123~!@  ", DisplayName = "\"\\n\\r\\n  abc123~!@  \\r\\n\\n\"")]
        [DataRow("\n\r\nabc123~!@\r\n\n", "", "abc123~!@", DisplayName = "\"\\n\\r\\nabc123~!@\\r\\n\\n\"")]

        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", "", "", DisplayName = "(space)")]
        [DataRow("\t", "", "", DisplayName = "(tab)")]
        [DataRow("\n", "", "", DisplayName = "\"\\n\"")]
        [DataRow("\r\n", "", "", DisplayName = "\"\\r\\n\"")]
        [DataRow("\n\t", "", "", DisplayName = "\"\\n\\t\"")]
        [DataRow("\t\n", "", "", DisplayName = "\"\\t\\n\"")]
        [DataRow("\r\n\t", "", "", DisplayName = "\"\\r\\n\\t\"")]
        [DataRow("\t\r\n", "", "", DisplayName = "\"\\t\\r\\n\"")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("  abc123~!@  \r\n\n", "", "  abc123~!@  ", DisplayName = "\"  abc123~!@  \\r\\n\\n\"")]
        [DataRow("\r\nabc123~!@\n", "", "abc123~!@", DisplayName = "\"\\r\\nabc123~!@\\n\"")]
        [DataRow("Test\n\nValue", "", "Test\nValue", DisplayName = "\"Test\\n\\nValue\"")]
        [DataRow("Test\n\nValue\n", "", "Test\nValue", DisplayName = "\"Test\\n\\nValue\\n\"")]
        [DataTestMethod()]
        public void EmptyOrWhiteSpaceLineRegexTest([DisallowNull] string input, string replacement, string expected)
        {
            bool isMatchResult = XNodeNormalizer.EmptyOrWhiteSpaceLineRegex.IsMatch(input);
            if (replacement is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual = XNodeNormalizer.EmptyOrWhiteSpaceLineRegex.Replace(input, replacement);
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        /// <summary>
        /// Tests the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods for <see cref="XNodeNormalizer.NewLineRegex"/>.
        /// </summary>
        /// <param name="input">The input string for the <see cref="Regex.IsMatch(string)"/> and <see cref="Regex.Replace(string, string)"/> methods.</param>
        /// <param name="toPrepend">The text that the <see cref="MatchEvaluator"/> prepends to the <see cref="Capture.Value"/> at index <c>1</c> of the <see cref="Match.Groups"/> property, replacing
        /// any whitespace that was captured as well; otherwise, <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        /// <param name="expected">The expected return value from the <see cref="Regex.Replace(string, string)"/> method invocation.
        /// This can be <see langword="null"/> if <see cref="Regex.IsMatch(string)"/> is expected to return <see langword="false"/>.</param>
        [DataRow("", null, null, DisplayName = "(blank)")]
        [DataRow(" ", null, null, DisplayName = "(space)")]
        [DataRow("abc123~!@", null, null, DisplayName = "\"abc123~!@\"")]
        [DataRow("\n", "    ", "    \n", DisplayName = "\"\\n\"")]
        [DataRow("\r\n", "    ", "    \r\n", DisplayName = "\"\\r\\n\"")]
        [DataRow(" \n", "    ", "     \n", DisplayName = "\" \\n\"")]
        [DataRow(" \r\n", "    ", "     \r\n", DisplayName = "\" \\r\\n\"")]
        [DataRow("\n ", "    ", "    \n ", DisplayName = "\"\\n \"")]
        [DataRow("\r\n ", "    ", "    \r\n ", DisplayName = "\"\\r\\n \"")]
        [DataRow("\n\r\nabc123~!@", "    ", "    \n    \r\nabc123~!@", DisplayName = "\"\\n\\r\\nabc123~!@\"")]
        [DataRow("\n\r\nabc123~!@ Test", "    ", "    \n    \r\nabc123~!@ Test", DisplayName = "\"\\n\\r\\nabc123~!@ Test\"")]
        [DataRow("\n\r\nabc123~!@\r\n\nTest", "    ", "    \n    \r\nabc123~!@    \r\n    \nTest", DisplayName = "\"\\n\\r\\nabc123~!@\\r\\n\\nTest\"")]
        [DataRow("\n\r\nabc123~!@\r\n\nTest\n ", "    ", "    \n    \r\nabc123~!@    \r\n    \nTest    \n ", DisplayName = "\"\\n\\r\\nabc123~!@\\r\\n\\nTest\\n \"")]
        [DataTestMethod()]
        public void NewLineRegexTest([DisallowNull] string input, string toPrepend, string expected)
        {
            bool isMatchResult = XNodeNormalizer.NewLineRegex.IsMatch(input);
            if (toPrepend is not null)
            {
                Assert.IsTrue(isMatchResult);
                string actual = XNodeNormalizer.NewLineRegex.Replace(input, match =>
                {
                    Assert.AreEqual(1, match.Groups.Count);
                    return $"{toPrepend}{match.Value}";
                });
                Assert.AreEqual(expected, actual);
            }
            else
                Assert.IsFalse(isMatchResult);
        }

        public static IEnumerable<(XComment targetNode, TestXNodeNormalizer normalizer, int indentLevel, XComment expected)> GetNormalizeXCommentTestData()
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
            yield return new(new("Test1"), null, 0, new("Test1"));
            yield return new(new("Test1"), new(), 0, new("Test1"));
            yield return new(new("Test1\n"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test1\n"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\nTest1"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\nTest1"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\nTest1\n\r\n"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\nTest2\n\r\n"), null, 2, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Test2" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}"));
            yield return new(new("\n\rTest1\n\r\n"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\nTest2\n\r\n"), new(), 2, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Test2" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}"));
            yield return new(new("Test\nComment1"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("Test\nComment1"), null, 1, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Comment1{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}"));
            yield return new(new("Test\nComment1"), new(), 1, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Comment1{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}"));
            yield return new(new("\r\n        Test\r\n        Comment1\r\n    "), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\r\n        Test\r\n        Comment1\r\n    "), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Comment1{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), null, 0, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Comment2{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), new(), 0, new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Comment2{XNodeNormalizer.DefaultNewLine}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), null, 2, new($"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}Comment2" +
                $"{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}"));
            yield return new(new("\n\rTest\r\n\rComment2\n\r"), new(), 2,
                new($"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Test" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}Comment2" +
                $"{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}"));
        }

        public static IEnumerable<object[]> GetNormalizeXCommentTestData1() => GetNormalizeXCommentTestData().Where(t => t.normalizer is null && t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method without the optional parameters.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXCommentTestData1), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXCommentTest1([DisallowNull] XComment targetNode, [DisallowNull] XComment expected)
        {
            XComment actual = new(targetNode);
            XNodeNormalizer.Normalize(actual);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXCommentTestData2() => GetNormalizeXCommentTestData().Where(t => t.normalizer is null)
            .Select(t => new object[] { t.targetNode, t.indentLevel, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XComment, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XComment, int)"/> method.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XComment, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XComment, int)"/> method has been invoked.</param>
        /// </summary>
        [DynamicData(nameof(GetNormalizeXCommentTestData2), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXCommentTest2([DisallowNull] XComment targetNode, int indentLevel, [DisallowNull] XComment expected)
        {
            XComment actual = new(targetNode);
            XNodeNormalizer.Normalize(actual, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXCommentTestData3() => GetNormalizeXCommentTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXCommentTestData3), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXCommentTest3([DisallowNull] XComment targetNode, TestXNodeNormalizer normalizer, int indentLevel, [DisallowNull] XComment expected)
        {
            XComment actual = new(targetNode);
            XNodeNormalizer.Normalize(actual, normalizer, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXCommentTestData4() => GetNormalizeXCommentTestData().Where(t => t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.normalizer, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method without the last optional parameter.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XComment, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXCommentTestData4), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXCommentTest4([DisallowNull] XComment targetNode, TestXNodeNormalizer normalizer, [DisallowNull] XComment expected)
        {
            XComment actual = new(targetNode);
            XNodeNormalizer.Normalize(actual, normalizer);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<(XContainer targetNode, TestXNodeNormalizer normalizer, int indentLevel, XContainer expected)> GetNormalizeXContainerTestData()
        {
            yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), null, 0, XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace));
            yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), new(), 0, XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
            yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), null, 2, XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace));
            yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), new(), 2, XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary/>", LoadOptions.PreserveWhitespace), null, 0, XDocument.Parse("<summary/>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary/>", LoadOptions.PreserveWhitespace), new(), 0, XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), null, 0, XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), new(), 0, XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>    </summary>", LoadOptions.PreserveWhitespace), null, 0, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>    </summary>", LoadOptions.PreserveWhitespace), new(), 0, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>    </summary>", LoadOptions.PreserveWhitespace), null, 2, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>    </summary>", LoadOptions.PreserveWhitespace), new(), 2, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>  \r  </summary>", LoadOptions.PreserveWhitespace), null, 0, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>  \r  </summary>", LoadOptions.PreserveWhitespace), new(), 0, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>  \r  </summary>", LoadOptions.PreserveWhitespace), null, 2, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>  \r  </summary>", LoadOptions.PreserveWhitespace), new(), 2, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace), null, 0,
                XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace), new(), 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
    //        yield return new(XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace), null, 1,
    //            XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
    //        yield return new(XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace), new(), 1,
    //            XDocument.Parse(@"<summary>
    //    Example brief description text.
    //</summary>", LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>\rExample brief description text.\n\r</summary>", LoadOptions.PreserveWhitespace), null, 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>\rExample brief description text.\n\r</summary>", LoadOptions.PreserveWhitespace), new(), 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>Example brief\rdescription text.</summary>", LoadOptions.PreserveWhitespace), null, 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            yield return new(XDocument.Parse("<summary>Example brief\rdescription text.</summary>", LoadOptions.PreserveWhitespace), new(), 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            //yield return new(XDocument.Parse("<summary>Example brief\rdescription text.</summary>", LoadOptions.PreserveWhitespace), null, 3,
            //    XDocument.Parse(@"<summary>
            //    Example brief
            //    description text.
            //</summary>", LoadOptions.PreserveWhitespace));
            //yield return new(XDocument.Parse("<summary>Example brief\rdescription text.</summary>", LoadOptions.PreserveWhitespace), new(), 3,
            //    XDocument.Parse(@"<summary>
            //    Example brief
            //    description text.
            //</summary>", LoadOptions.PreserveWhitespace));
            yield return new(XElement.Parse("<summary>\n\t\t\tExample brief description text.\n\t\t</summary>", LoadOptions.PreserveWhitespace), null, 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            yield return new(XElement.Parse("<summary>\n\t\t\tExample brief description text.\n\t\t</summary>", LoadOptions.PreserveWhitespace), new(), 0,
                XDocument.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}</summary>",
                LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace), null, 0,
            //                XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace), new(), 0,
            //                XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace), null, 2,
            //                XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace), new(), 2,
            //                XElement.Parse(@"<remarks>Example with <see cref=""System.Linq.XElement"" /> node nested within text.</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Example with <see cref=\"System.Linq.XElement\" /> node\rnested within text.</remarks>", LoadOptions.PreserveWhitespace), null, 0,
            //                XElement.Parse(@"<remarks>
            //    Example with <see cref=""System.Linq.XElement"" /> node
            //    nested within text.
            //</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Example with <see cref=\"System.Linq.XElement\" /> node\rnested within text.</remarks>", LoadOptions.PreserveWhitespace), new(), 0,
            //                XElement.Parse(@"<remarks>
            //    Example with <see cref=""System.Linq.XElement"" /> node
            //    nested within text.
            //</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Example with <see cref=\"System.Linq.XElement\" /> node\rnested within text.</remarks>", LoadOptions.PreserveWhitespace), null, 2,
            //                XElement.Parse(@"<remarks>
            //            Example with <see cref=""System.Linq.XElement"" /> node
            //            nested within text.
            //        </remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Example with <see cref=\"System.Linq.XElement\" /> node\rnested within text.</remarks>", LoadOptions.PreserveWhitespace), new(), 2,
            //                XElement.Parse(@"<remarks>
            //            Example with <see cref=""System.Linq.XElement"" /> node
            //            nested within text.
            //        </remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>
            //                Example with <langword>true</langword> langword<para>... and paragraph.</para><list type=""bullet"">
            //                    <item>
            //                        <term>With a</term>
            //                        <description>bulleted list as well</description>
            //                    </item>
            //                    <item><term>And an item</term><description>that should not need reformatted</description></item>
            //                </list>
            //            </remarks>", LoadOptions.PreserveWhitespace), null, 0, XElement.Parse(@"<remarks>
            //    Example with <langword>true</langword> langword<para>... and paragraph.</para>
            //    <list type=""bullet"">
            //        <item>
            //            <term>With a</term>
            //            <description>bulleted list as well</description>
            //        </item>
            //        <item><term>And an item</term><description>that should not need reformatted</description></item>
            //    </list>
            //</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse(@"<remarks>
            //                Example with <langword>true</langword> langword<para>... and paragraph.</para><list type=""bullet"">
            //                    <item>
            //                        <term>With a</term>
            //                        <description>bulleted list as well</description>
            //                    </item>
            //                    <item><term>And an item</term><description>that should be re-formatted</description></item>
            //                </list>
            //            </remarks>", LoadOptions.PreserveWhitespace), new(), 0, XElement.Parse(@"<remarks>
            //    Example with <see langword=\""true"" /> langword
            //    <para>... and paragraph.</para>
            //    <list type=""bullet"">
            //        <item>
            //            <term>With a</term>
            //            <description>bulleted list as well</description>
            //        </item>
            //        <item>
            //            <term>And an item</term>
            //            <description>that should be re-formatted</description>
            //        </item>
            //    </list>
            //</remarks>", LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Single-line xample with <langword>true</langword> langword<para>... and paragraph.</para><list type=\"bullet\"><item><term>With a</term><description>bulleted list as well</description></item><item><term>And an item</term><description>that should not need reformatted</description></item></list></remarks>",
            //                LoadOptions.PreserveWhitespace), null, 0, XElement.Parse("<remarks>Single-line with <langword>true</langword> langword<para>... and paragraph.</para><list type=\"bullet\"><item><term>With a</term><description>bulleted list as well</description></item><item><term>And an item</term><description>that should not need reformatted</description></item></list></remarks>",
            //                LoadOptions.PreserveWhitespace));
            //            yield return new(XElement.Parse("<remarks>Single-line with <langword>true</langword> langword<para>... and paragraph.</para><list type=\"bullet\"><item><term>With a</term><description>bulleted list as well</description></item><item><term>And an item</term><description>that should be re-formatted</description></item></list></remarks>",
            //                LoadOptions.PreserveWhitespace), new(), 0, XElement.Parse(@"<remarks>
            //    Single-line with <see langword=\""true"" /> langword
            //    <para>... and paragraph.</para>
            //    <list type=""bullet"">
            //        <item>
            //            <term>With a</term>
            //            <description>bulleted list as well</description>
            //        </item>
            //        <item>
            //            <term>And an item</term>
            //            <description>that should be re-formatted</description>
            //        </item>
            //    </list>
            //</remarks>", LoadOptions.PreserveWhitespace));
        }

        public static IEnumerable<object[]> GetNormalizeXContainerTestData1() => GetNormalizeXContainerTestData().Where(t => t.normalizer is null && t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method without the optional parameters.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXContainerTestData1), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXContainerTest1([DisallowNull] XContainer targetNode, [DisallowNull] XContainer expected)
        {
            XContainer actual = (targetNode is XDocument d) ? new XDocument(d) : new XElement((XElement)targetNode);
            XNodeNormalizer.Normalize(actual);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXContainerTestData2() => GetNormalizeXContainerTestData().Where(t => t.normalizer is null)
            .Select(t => new object[] { t.targetNode, t.indentLevel, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XContainer, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, int)"/> method.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XContainer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXContainerTestData2), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXContainerTest2([DisallowNull] XContainer targetNode, int indentLevel, [DisallowNull] XContainer expected)
        {
            XContainer actual = (targetNode is XDocument d) ? new XDocument(d) : new XElement((XElement)targetNode);
            XNodeNormalizer.Normalize(actual, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXContainerTestData3() => GetNormalizeXContainerTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXContainerTestData3), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXContainerTest3([DisallowNull] XContainer targetNode, TestXNodeNormalizer normalizer, int indentLevel, [DisallowNull] XContainer expected)
        {
            XContainer actual = (targetNode is XDocument d) ? new XDocument(d) : new XElement((XElement)targetNode);
            XNodeNormalizer.Normalize(actual, normalizer, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXContainerTestData4() => GetNormalizeXContainerTestData().Where(t => t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.normalizer, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method without the last optional parameter.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XContainer, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXContainerTestData4), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXContainerTest4([DisallowNull] XContainer targetNode, TestXNodeNormalizer normalizer, [DisallowNull] XContainer expected)
        {
            XContainer actual = (targetNode is XDocument d) ? new XDocument(d) : new XElement((XElement)targetNode);
            XNodeNormalizer.Normalize(actual, normalizer);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }
        /*<summary>     text</summary>
         */
        public static IEnumerable<(XText targetNode, TestXNodeNormalizer normalizer, int indentLevel, XText expected)> GetNormalizeXTextTestData()
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
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 0, new XCData($"Test{XNodeNormalizer.DefaultNewLine}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 0, new XCData($"Test{XNodeNormalizer.DefaultNewLine}String"));
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
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 1, new XCData($"Test{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 1, new XCData($"Test{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}String"));
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
            yield return new(new XCData("\rTest\n\r\rString\r"), null, 3, new XCData($"Test{XNodeNormalizer.DefaultNewLine}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}String"));
            yield return new(new XCData("\rTest\n\r\rString\r"), new(), 3, new XCData($"Test{XNodeNormalizer.DefaultNewLine}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}String"));
        }

        public static IEnumerable<object[]> GetNormalizeXTextTestData1() => GetNormalizeXTextTestData().Where(t => t.normalizer is null && t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method without the optional parameters.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXTextTestData1), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXTextTest1([DisallowNull] XText targetNode, [DisallowNull] XText expected)
        {
            XText actual = (targetNode is XCData d) ? new XCData(d) : new XText(targetNode);
            XNodeNormalizer.Normalize(actual);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXTextTestData2() => GetNormalizeXTextTestData().Where(t => t.normalizer is null)
            .Select(t => new object[] { t.targetNode, t.indentLevel, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XText,, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XText, int)"/> method.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XText, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XText, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXTextTestData2), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXTextTest2([DisallowNull] XText targetNode, int indentLevel, [DisallowNull] XText expected)
        {
            XText actual = (targetNode is XCData d) ? new XCData(d) : new XText(targetNode);
            XNodeNormalizer.Normalize(actual, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXTextTestData3() => GetNormalizeXTextTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected });


        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="indentLevel">The initial indent level to pass to the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXTextTestData3), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXTextTest3([DisallowNull] XText targetNode, TestXNodeNormalizer normalizer, int indentLevel, [DisallowNull] XText expected)
        {
            XText actual = (targetNode is XCData d) ? new XCData(d) : new XText(targetNode);
            XNodeNormalizer.Normalize(actual, normalizer, indentLevel);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public static IEnumerable<object[]> GetNormalizeXTextTestData4() => GetNormalizeXTextTestData().Where(t => t.indentLevel == 0)
            .Select(t => new object[] { t.targetNode, t.normalizer, t.expected });

        /// <summary>
        /// Tests the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method without the last optional parameter.
        /// </summary>
        /// <param name="targetNode">The target node to pass to the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method.</param>
        /// <param name="normalizer">The normalizer instance or <see langword="null"/>.</param>
        /// <param name="expected">The expected contents of the <paramref name="targetNode"/> after the <see cref="XNodeNormalizer.Normalize(XText, XNodeNormalizer, int)"/> method has been invoked.</param>
        [DynamicData(nameof(GetNormalizeXTextTestData4), DynamicDataSourceType.Method)]
        [DataTestMethod()]
        public void NormalizeXTextTest4([DisallowNull] XText targetNode, TestXNodeNormalizer normalizer, [DisallowNull] XText expected)
        {
            XText actual = (targetNode is XCData d) ? new XCData(d) : new XText(targetNode);
            XNodeNormalizer.Normalize(actual, normalizer);
            Assert.AreEqual(expected.ToString(SaveOptions.DisableFormatting), actual.ToString(SaveOptions.DisableFormatting));
        }

        public class TestXNodeNormalizer : XNodeNormalizer
        {
            public const string CustomIndent = "  ";
            protected override string IndentString => CustomIndent;
            protected override void Normalize(XContainer container, Context context)
            {
                bool emptyToWhitespace;
                if (container is XElement el && el.Name.LocalName == "summary")
                {
                    if (el.IsEmpty)
                    {
                        el.Value = "";
                        return;
                    }
                    emptyToWhitespace = el.DescendantNodes().OfType<XText>().Any(t => LineSeparatorCharRegex.IsMatch(t.Value));
                    if (!emptyToWhitespace)
                    {
                        emptyToWhitespace = el.DescendantNodes().OfType<XText>().Any(t => t.Value.Length > 0);
                        if (el.DescendantNodes().OfType<XText>().Any(t => !string.IsNullOrWhiteSpace(t.Value)))
                        {
                            if (el.FirstNode is XText lt)
                            {
                                lt.Value = $"{NewLine}{lt.Value}";
                                if (el.LastNode is XText ln)
                                {
                                    if (!ReferenceEquals(lt, ln))
                                        ln.Value = $"{ln.Value}{NewLine}";
                                }
                                else
                                    el.Add(new XText(NewLine));
                            }
                            else
                            {
                                el.FirstNode.AddBeforeSelf(new XText(NewLine));
                                if (el.LastNode is XText ln)
                                    ln.Value = $"{ln.Value}{NewLine}";
                                else
                                    el.Add(new XText(NewLine));
                            }
                        }

                    }
                }
                else
                    emptyToWhitespace = false;
                foreach (XElement element in container.Elements("langword").ToArray())
                    element.ReplaceWith(new XElement("see", new XAttribute("cref", element.Value)));
                base.Normalize(container, context);
                if (container is XElement e && e.IsEmpty)
                    e.Value = emptyToWhitespace ? " " : "";
            }
            protected override bool ShouldForceLineBreak(XElement element, Context parentContext)
            {
                if (parentContext.Depth == 0 || (element.Document is not null && (ReferenceEquals(element, element.Document.Root) || ReferenceEquals(element.Parent, element.Document.Root))))
                    return true;
                return element.Name.LocalName switch
                {
                    "para" or "list" => true,
                    _ => element.DescendantNodes().OfType<XText>().Any(t => NewLineRegex.IsMatch(t.Value)),
                };
            }
        }
    }
}
