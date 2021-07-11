using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests.XNodeNormalizerTestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XContainerTestDataAttribute : Attribute, ITestDataSource
    {
        public bool ExcludeCustomNormalizer { get; set; }

        public bool ExcludeIndentLevel { get; set; }

        public static IEnumerable<(XContainer targetNode, TestXNodeNormalizer normalizer, int indentLevel, XContainer expected)> GetTestData()
        {

            foreach ((int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent) in new (int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent)[] {
                new(-1, "", "", "", ""),
                new(0, XNodeNormalizer.DefaultIndent, "", TestXNodeNormalizer.CustomIndent, ""),
                new(1, $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}", XNodeNormalizer.DefaultIndent, $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}", TestXNodeNormalizer.CustomIndent),
                new(2, $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}", $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}",
                    $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}", $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}")
            })
            {
                yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace));
                yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), null, IndentLevel, XDocument.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
                yield return new(XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                foreach (string xml in new[] { "<summary> </summary>", "<summary>  \t  </summary>", "<summary>  \r  </summary>" })
                {
                    yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    foreach (string xml in new[] { $"<summary>{nl}</summary>", $"<summary>  {nl}  </summary>" })
                    {
                        yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XDocument.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }
                }
                foreach (string xml in new[] { "<summary>Example brief description text.</summary>", "<summary>  Example brief description text.  </summary>", "<summary>  \r Example brief description text.  </summary>",
                    "<summary>  Example brief description text.  \r </summary>", "<summary>  \r Example brief description text. \r\r\n </summary>" })
                {
                    yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    foreach (string xml in new[] { $"<summary>{nl}Example brief description text.</summary>", $"<summary>Example brief description text.{nl}</summary>", $"<summary>  {nl} Example brief description text. {nl} </summary>",
                        $"<summary>  {nl}\r Example brief description text. {nl}\r </summary>" })
                    {
                        yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XDocument.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                            XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }
                }
                foreach (string xml in new[] { "<summary>Example brief\rdescription text.</summary>", "<summary>  Example brief \r\r\n description text.  </summary>", "<summary>  \r Example brief \r\r\n description text.  </summary>",
                    "<summary>  Example brief \r\r\n description text.  \r </summary>", "<summary>  \r Example brief \r description text. \r\r\n </summary>" })
                {
                    yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>",
                        LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>",
                        LoadOptions.PreserveWhitespace));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    foreach (string xml in new[] { $"<summary>Example brief{nl}description text.</summary>", $"<summary>{nl}Example brief {nl} description text.</summary>" })
                    {
                        yield return new(XDocument.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel,
                            XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                            XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }

                    yield return new(XDocument.Parse($"<summary>Example brief {nl} description text.{nl}</summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{nl}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>Example brief {nl} description text.{nl}</summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));

                    yield return new(XDocument.Parse($"<summary>  {nl} Example brief {nl} description text. </summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{nl}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl} Example brief {nl} description text. </summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));

                    yield return new(XDocument.Parse($"<summary>  {nl}\r Example brief {nl}\r description text. {nl}\r </summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{nl}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{nl}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl}\r Example brief {nl}\r description text. {nl}\r </summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
            }
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
            string elementCode;
            if (data[0] is XDocument xDocument)
                elementCode = $"XDocument.Parse({ExtensionMethods.ToPseudoCsText(xDocument.ToString(SaveOptions.DisableFormatting))})";
            else
                elementCode = $"XElement.Parse({ExtensionMethods.ToPseudoCsText(((XElement)data[0]).ToString(SaveOptions.DisableFormatting))})";
            if (ExcludeCustomNormalizer)
            {
                if (ExcludeIndentLevel)
                    return $"XNodeNormalizer.Normalize({elementCode})";
                return $"XNodeNormalizer.Normalize({elementCode}, {data[1]})";
            }
            if (ExcludeIndentLevel)
                return $"XNodeNormalizer.Normalize({elementCode}, {(data[1] is null ? "null" : "(custom normalizer)")})";
            return $"XNodeNormalizer.Normalize({elementCode}, {(data[1] is null ? "null" : "(custom normalizer)")}, {data[2]})";
        }
    }
}
