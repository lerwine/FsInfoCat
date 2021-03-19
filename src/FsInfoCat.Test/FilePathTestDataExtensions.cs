using System.Collections.Generic;
using System.Xml.Linq;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    static class FilePathTestDataExtensions
    {
        static bool IsTestDataElement(this XElement source)
        {
            if (source is null || source.Name.LocalName != "TestData")
                return false;
            XDocument doc = source.Document;
            return !(doc is null) && ReferenceEquals(source, doc.Root);
        }


        static XElement GetTestDataElement(this XElement source)
        {
            if (source.Name.LocalName == "TestData" && source.Parent)
            }

        static IEnumerable<XElement> Windows(this XElement source)
        {
            switch (source.Name.LocalName)
            }
        }
    }
}
