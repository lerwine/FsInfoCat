using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Generator
{
    public static class WriterExtensionMethods
    {
        public static void WriteLines(this TextWriter writer, params string[] lines)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (lines is null) return;
            foreach (string s in lines)
            {
                if (string.IsNullOrEmpty(s))
                    writer.WriteLine();
                else
                    writer.WriteLine(s);
            }
        }

        public static void WriteDocumentationXml(this TextWriter writer, IEnumerable<XElement> documentationElements)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (documentationElements is null) throw new ArgumentNullException(nameof(documentationElements));
            foreach (string line in documentationElements.SelectMany(e => SourceGenerator.NewlineRegex.Split(e.ToString())).Select(s => s.TrimEnd()))
            {
                if (line.Length > 0)
                {
                    writer.Write("/// ");
                    writer.WriteLine(line);
                }
                else
                    writer.WriteLine("///");
            }
        }

        public static void WriteDisplayAttribute(this IndentedTextWriter writer, XElement fieldOrPropertyElement, Action<string, IXmlLineInfo> assertResourceName)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (fieldOrPropertyElement is null) throw new ArgumentNullException(nameof(fieldOrPropertyElement));
            if (assertResourceName is null) throw new ArgumentNullException(nameof(assertResourceName));
            XElement displayElement = fieldOrPropertyElement.Element(XmlNames.Display);
            XAttribute attribute = displayElement.Attribute(XmlNames.Label);
            string text = attribute.Value;
            assertResourceName(text, attribute);
            writer.Write($"[Display(Name = nameof(Properties.Resources.{text})");
            text = (attribute = displayElement.Attribute(XmlNames.ShortName))?.Value;
            if (!string.IsNullOrWhiteSpace(text))
            {
                assertResourceName(text, attribute);
                writer.Write($", ShortName = nameof(Properties.Resources.{text})");
            }
            text = (attribute = displayElement.Attribute(XmlNames.Description))?.Value;
            if (!string.IsNullOrWhiteSpace(text))
            {
                assertResourceName(text, attribute);
                writer.Write($", Description = nameof(Properties.Resources.{text})");
            }
            writer.WriteLine($", ResourceType = typeof(Properties.Resources))]");
        }
    }
}
