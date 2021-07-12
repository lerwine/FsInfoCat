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
        public bool IsDocument { get; }

        public bool ExcludeCustomNormalizer { get; set; }

        public bool ExcludeIndentLevel { get; set; }

        public XContainerTestDataAttribute(bool isDocument)
        {
            IsDocument = isDocument;
        }

        public static IEnumerable<(int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent)> GetIndents()
        {
            yield return new(-1, "", "", "", "");
            yield return new(0, XNodeNormalizer.DefaultIndent, "", TestXNodeNormalizer.CustomIndent, "");
            yield return new(1, $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}", XNodeNormalizer.DefaultIndent, $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}", TestXNodeNormalizer.CustomIndent);
            yield return new(2, $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}", $"{XNodeNormalizer.DefaultIndent}{XNodeNormalizer.DefaultIndent}",
                    $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}", $"{TestXNodeNormalizer.CustomIndent}{TestXNodeNormalizer.CustomIndent}");
        }

        public static IEnumerable<(XDocument targetNode, TestXNodeNormalizer normalizer, int indentLevel, XDocument expected)> GetDocumentSpecialTestData()
        {
            foreach ((int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent) in GetIndents())
            {
                yield return new(new XDocument(), null, IndentLevel, new XDocument());
                yield return new(new XDocument(), new(), IndentLevel, new XDocument());
                yield return new(XDocument.Parse("<!-- Test Comment --><summary/>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<!-- Test Comment -->{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary/>", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test Comment --><summary/>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<!-- Test Comment -->{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<summary/><!-- Test Comment -->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<summary/>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<!-- Test Comment -->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<summary/><!-- Test Comment -->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<!-- Test Comment -->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \t One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary/>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<!-- Another One -->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \t One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<!-- Another One -->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \n One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary/>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Another\n{DefaultLeadIndent}One{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \n One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Another\n{CustomLeadIndent}One{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary/>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Another\r\n{DefaultLeadIndent}One{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary/><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Another\r\n{CustomLeadIndent}One{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));

                yield return new(XDocument.Parse("<!-- Test\rComment --><summary></summary><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                    XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary></summary>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Another\r\n{DefaultLeadIndent}One{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                yield return new(XDocument.Parse("<!-- Test\rComment --><summary></summary><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                    XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                    $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}" +
                    $"<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Another\r\n{CustomLeadIndent}One{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));

                foreach (string txt in new[] { " ", "    ", " \t ", "  \r  ", "  \n  ", "  \r\n  ", "\n  \r\n  \r" })
                {
                    yield return new(XDocument.Parse($"<!-- Test\rComment --><summary>{txt}</summary><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                        $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary> </summary>{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}" +
                        $"<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Another\r\n{DefaultLeadIndent}One{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                    yield return new(XDocument.Parse($"<!-- Test\rComment --><summary>{txt}</summary><!--Another \r\n\r One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                        $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}" +
                        $"<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Another\r\n{CustomLeadIndent}One{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    yield return new(XDocument.Parse($"<!--Test{nl}Comment --><summary>{nl}</summary>{nl}<!--Another {nl}\n\r One-->", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XDocument.Parse($"{DefaultTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Test{nl}{DefaultLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->" +
                        $"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}<summary> </summary>{nl}{DefaultTrailingIndent}" +
                        $"<!--{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Another{nl}{DefaultLeadIndent}One{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                    yield return new(XDocument.Parse($"<!--Test{nl}Comment --><summary>{nl}</summary>{nl}<!--Another {nl}\n\r One-->", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XDocument.Parse($"{CustomTrailingIndent}<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Test{nl}{CustomLeadIndent}Comment{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->" +
                        $"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}<summary>{nl}{CustomTrailingIndent}</summary>{nl}{CustomTrailingIndent}" +
                        $"<!--{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Another{nl}{CustomLeadIndent}One{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                    yield return new(XDocument.Parse($"{nl}<!--{nl}Test {nl} Comment {nl} -->{nl}<summary>{nl}</summary>{nl}<!--{nl}Another {nl}\n\r One{nl}--> {nl} ", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XDocument.Parse($"{DefaultTrailingIndent}<!--{nl}{DefaultLeadIndent}Test{nl}{DefaultLeadIndent}Comment{nl}{DefaultTrailingIndent}-->" +
                        $"{nl}{DefaultTrailingIndent}<summary> </summary>{nl}{DefaultTrailingIndent}" +
                        $"<!--{nl}{DefaultLeadIndent}Another{nl}{DefaultLeadIndent}One{nl}{DefaultTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                    yield return new(XDocument.Parse($"{nl}<!--{nl}Test {nl} Comment {nl} -->{nl}<summary>{nl}</summary>{nl}<!--{nl}Another {nl}\n\r One{nl}--> {nl} ", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XDocument.Parse($"{CustomTrailingIndent}<!--{nl}{CustomLeadIndent}Test{nl}{CustomLeadIndent}Comment{nl}{CustomTrailingIndent}-->" +
                        $"{nl}{CustomTrailingIndent}<summary>{nl}{CustomTrailingIndent}</summary>{nl}{CustomTrailingIndent}" +
                        $"<!--{nl}{CustomLeadIndent}Another{nl}{CustomLeadIndent}One{nl}{CustomTrailingIndent}-->", LoadOptions.PreserveWhitespace));
                }
            }
        }

        public static IEnumerable<(XElement targetNode, TestXNodeNormalizer normalizer, int indentLevel, XElement expected)> GetElementTestData()
        {
            foreach ((int IndentLevel, string DefaultLeadIndent, string DefaultTrailingIndent, string CustomLeadIndent, string CustomTrailingIndent) in GetIndents())
            {
                yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace));
                yield return new(XElement.Parse("<summary/>", LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                yield return new(XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace));
                yield return new(XElement.Parse("<summary></summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                foreach (string xml in new[] { "<summary> </summary>", "<summary>  \t  </summary>", "<summary>  \r  </summary>" })
                {
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
                foreach (string xml in new[] { "<summary>Example brief description text.</summary>", "<summary>  Example brief description text.  </summary>", "<summary>  \r Example brief description text.  </summary>",
                    "<summary>  Example brief description text.  \r </summary>", "<summary>  \r Example brief description text. \r\r\n </summary>" })
                {
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
                foreach (string nl in new[] { "\n", "\r\n" })
                {
                    foreach (string xml in new[] { $"<summary>{nl}</summary>", $"<summary>  {nl}  </summary>" })
                    {
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary> </summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel, XElement.Parse($"<summary>{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }
                    foreach (string xml in new[] { $"<summary>{nl}Example brief description text.</summary>", $"<summary>Example brief description text.{nl}</summary>", $"<summary>  {nl} Example brief description text. {nl} </summary>",
                        $"<summary>  {nl}\r Example brief description text. {nl}\r </summary>" })
                    {
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel, XElement.Parse("<summary>Example brief description text.</summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                            XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }
                    foreach (string xml in new[] { $"<summary>Example brief{nl}description text.</summary>", $"<summary>{nl}Example brief {nl} description text.</summary>" })
                    {
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel,
                            XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                        yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                            XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    }

                    yield return new(XElement.Parse($"<summary>Example brief {nl} description text.{nl}</summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{nl}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>Example brief {nl} description text.{nl}</summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl} Example brief {nl} description text. </summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{nl}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl} Example brief {nl} description text. </summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl}\r Example brief {nl}\r description text. {nl}\r </summary>", LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{nl}{DefaultLeadIndent}Example brief{nl}{DefaultLeadIndent}description text.{nl}{DefaultTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse($"<summary>  {nl}\r Example brief {nl}\r description text. {nl}\r </summary>", LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{nl}{CustomLeadIndent}Example brief{nl}{CustomLeadIndent}description text.{nl}{CustomTrailingIndent}</summary>", LoadOptions.PreserveWhitespace));
                }
                foreach (string xml in new[] { "<summary>Example brief\rdescription text.</summary>", "<summary>  Example brief \r\r\n description text.  </summary>", "<summary>  \r Example brief \r\r\n description text.  </summary>",
                    "<summary>  Example brief \r\r\n description text.  \r </summary>", "<summary>  \r Example brief \r description text. \r\r\n </summary>" })
                {
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), null, IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example brief{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}</summary>",
                        LoadOptions.PreserveWhitespace));
                    yield return new(XElement.Parse(xml, LoadOptions.PreserveWhitespace), new(), IndentLevel,
                        XElement.Parse($"<summary>{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example brief{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}description text.{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}</summary>",
                        LoadOptions.PreserveWhitespace));
                }
                foreach (XName name in new[] { XName.Get("remarks"), XName.Get("description"), XName.Get("summary") })
                {
                    foreach (string extraSpace in new[] { "", " ", "\r", " \n "})
                    {
                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText($" node  nested within text{extraSpace}")), null, IndentLevel,
                            new XElement(name, new XText("Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node nested within text")));
                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText($" node  nested within text{extraSpace}")), new(), IndentLevel,
                            new XElement(name, new XText("Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node nested within text")));

                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node  nested within text"), new XElement("para", $"And Paragraph!{extraSpace}")), null, IndentLevel,
                            new XElement(name, new XText("Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node nested within text"), new XElement("para", "And Paragraph!")));
                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node  nested within text"), new XElement("para", $"And Paragraph!{extraSpace}")), new(), IndentLevel,
                            new XElement(name,
                                new XText($"{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText($" node nested within text{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}"),
                                new XElement("para", "And Paragraph!"),
                                new XText($"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}")));

                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node  nested\rwithin text"), new XElement("para", $"And Paragraph!{extraSpace}")), null, IndentLevel,
                            new XElement(name, new XText($"{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText($" node nested{XNodeNormalizer.DefaultNewLine}{DefaultLeadIndent}within text"),
                            new XElement("para", "And Paragraph!"), new XText($"{XNodeNormalizer.DefaultNewLine}{DefaultTrailingIndent}")));
                        yield return new(new XElement(name, new XText($"{extraSpace}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText(" node  nested\rwithin text"), new XElement("para", $"And Paragraph!{extraSpace}")), new(), IndentLevel,
                            new XElement(name,
                                new XText($"{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}Example with "), new XElement("see", new XAttribute("cref", "System.Linq.XElement")), new XText($" node nested within text{XNodeNormalizer.DefaultNewLine}{CustomLeadIndent}"),
                                new XElement("para", "And Paragraph!"),
                                new XText($"{XNodeNormalizer.DefaultNewLine}{CustomTrailingIndent}")));
                    }
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
            if (IsDocument)
            {
                if (ExcludeCustomNormalizer)
                {
                    if (ExcludeIndentLevel)
                        return GetElementTestData().Where(t => t.normalizer is null && t.indentLevel == 0).Select(t => new object[] { new XDocument(t.targetNode), new XDocument(t.expected) })
                            .Concat(GetDocumentSpecialTestData().Where(t => t.normalizer is null && t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.expected }));
                    return GetElementTestData().Where(t => t.normalizer is null).Select(t => new object[] { new XDocument(t.targetNode), t.indentLevel, new XDocument(t.expected) })
                            .Concat(GetDocumentSpecialTestData().Where(t => t.normalizer is null).Select(t => new object[] { t.targetNode, t.indentLevel, t.expected }));
                }
                if (ExcludeIndentLevel)
                    return GetElementTestData().Where(t => t.indentLevel == 0).Select(t => new object[] { new XDocument(t.targetNode), t.normalizer, new XDocument(t.expected) })
                            .Concat(GetDocumentSpecialTestData().Where(t => t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.normalizer, new XDocument(t.expected) }));
                return GetElementTestData().Select(t => new object[] { new XDocument(t.targetNode), t.normalizer, t.indentLevel, new XDocument(t.expected) })
                            .Concat(GetDocumentSpecialTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected }));
            }
            if (ExcludeCustomNormalizer)
            {
                if (ExcludeIndentLevel)
                    return GetElementTestData().Where(t => t.normalizer is null && t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.expected });
                return GetElementTestData().Where(t => t.normalizer is null).Select(t => new object[] { t.targetNode, t.indentLevel, t.expected });
            }
            if (ExcludeIndentLevel)
                return GetElementTestData().Where(t => t.indentLevel == 0).Select(t => new object[] { t.targetNode, t.normalizer, t.expected });
            return GetElementTestData().Select(t => new object[] { t.targetNode, t.normalizer, t.indentLevel, t.expected });
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
