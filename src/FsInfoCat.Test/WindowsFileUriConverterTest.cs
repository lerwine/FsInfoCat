using FsInfoCat.Test.Helpers;
using FsInfoCat.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class WindowsFileUriConverterTest
    {
        public static IEnumerable<TestCaseData> GetFsPathRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FsPathRegex");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFsPathRegexTestCases))]
        public string FsPathRegexTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriConverter.FS_PATH_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FsPathRegex", base64Encoded, "root", "host", "path");
        }

        //public static IEnumerable<TestCaseData> GetFsPathPatternTestCases()
        //{
        //    return UrlHelperTest.GetUriTestData().Select(element =>
        //    {
        //        XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FsPathPattern");
        //        Console.WriteLine($"Emitting {expected}");
        //        bool base64Encoded = element.GetAttribute("Base64") == "true";
        //        return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
        //            .SetDescription(element.GetAttribute("Description"))
        //            .Returns(expected.OuterXml);
        //    });
        //}
        //
        //[Test, Property("Priority", 1)]
        //[TestCaseSource(nameof(GetFsPathPatternTestCases))]
        //public string FsPathPatternTest(string input, bool base64Encoded)
        //{
        //    Match match = Regex.Match(input, WindowsFileUriConverter.PATTERN_ABS_FS_PATH);
        //    return UrlHelperTest.ToTestReturnValueXml(match, "FsPathPattern", base64Encoded);
        //}

        public static IEnumerable<TestCaseData> GetFormatDetectionRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FormatGuess");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFormatDetectionRegexTestCases))]
        public string FormatDetectionRegexTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriConverter.FORMAT_DETECTION_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FormatGuess", base64Encoded, "file", "d", "host", "path");
        }

        public static IEnumerable<TestCaseData> GetFileUriStrictTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Windows/FileUriStrict");
                Console.WriteLine($"Emitting {expected}");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFileUriStrictTestCases))]
        public string FileUriStrictTest(string input, bool base64Encoded)
        {
            Match match = WindowsFileUriConverter.FILE_URI_STRICT_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FileUriStrict", base64Encoded, "file", "host", "ipv4", "ipv6", "d", "dns", "path", "dir", "fileName");
        }

        public static IEnumerable<TestCaseData> GetHostNameRegexTestCases()
        {
            return FileUriConverterTest.HostTestData.Root.Elements().Select(testData =>
            {
                switch (testData.Name.LocalName)
                {
                    case "HostName":
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), testData.Value, new XAttribute("Success", true))
                                ).ToString(SaveOptions.DisableFormatting)
                            );
                    case "IPV4":
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), testData.Value, new XAttribute("Success", true)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))
                                ).ToString(SaveOptions.DisableFormatting)
                            );
                    case "IPV6":
                        if (testData.Attribute("Type").Value == "UNC")
                            return new TestCaseData(testData.Attribute("Address").Value)
                                .Returns(
                                    new XElement("HostNameRegex",
                                        new XAttribute("Success", true),
                                        new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                        new XElement("Group", new XAttribute("Name", "ipv6"), testData.Value, new XAttribute("Success", true)),
                                        new XElement("Group", new XAttribute("Name", "d"),
                                            testData.Attribute("Address").Value[testData.Value.Length..], new XAttribute("Success", true)),
                                        new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))
                                    ).ToString(SaveOptions.DisableFormatting)
                                );
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), testData.Value, new XAttribute("Success", true)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))
                                ).ToString(SaveOptions.DisableFormatting)
                            );
                    default:
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", false)
                                ).ToString(SaveOptions.DisableFormatting)
                            );
                }
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetHostNameRegexTestCases))]
        public string HostNameRegexTest(string input)
        {
            Match match = WindowsFileUriConverter.HOST_NAME_W_UNC_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "HostNameRegex", "ipv4", "ipv6", "d", "dns");
        }

        public static IEnumerable<TestCaseData> GetIsWellFormedFileUriStringTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("IsWellFormedFileUriString");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"),
                        Enum.Parse(typeof(UriKind), expected.GetAttribute("Kind")))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(XmlConvert.ToBoolean(expected.InnerText));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsWellFormedFileUriStringTestCases))]
        public bool IsWellFormedFileUriStringTest(string uriString, UriKind kind)
        {
            bool result = WindowsFileUriConverter.INSTANCE.IsWellFormedUriString(uriString, kind);
            return result;
        }

        //public static IEnumerable<TestCaseData> GetParseUriOrPathTestCases()
        //{
        //    return UrlHelperTest.GetUriTestData().Where(e => !(e.SelectSingleNode("ParseUriOrPath") is null)).Select(element =>
        //    {
        //        XmlElement expected = (XmlElement)element.SelectSingleNode("ParseUriOrPath");
        //        bool preferFsPath = XmlConvert.ToBoolean(expected.GetAttribute("PreferFsPath"));
        //        bool base64Encoded = element.GetAttribute("Base64") == "true";
        //        return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"),
        //                preferFsPath, base64Encoded)
        //            .SetDescription(element.GetAttribute("Description"))
        //            .Returns(expected.SelectSingleNode("Expected").OuterXml);
        //    });
        //}

        //[Test, Property("Priority", 1)]
        //[TestCaseSource(nameof(GetParseUriOrPathTestCases))]
        //public string ParseUriOrPathTest(string source, bool preferFsPath, bool base64Encoded)
        //{
        //    FileStringFormat result = WindowsFileUriConverter.INSTANCE.ParseUriOrPath(source, preferFsPath, out string hostName, out string path);
        //    if (base64Encoded)
        //    {
        //        UTF8Encoding encoding = new UTF8Encoding(false, true);
        //        return XmlBuilder.NewDocument("Expected")
        //            .WithAttribute("HostName", (string.IsNullOrEmpty(hostName)) ? hostName : Convert.ToBase64String(encoding.GetBytes(hostName)))
        //            .WithAttribute("Path", (string.IsNullOrEmpty(path)) ? path : Convert.ToBase64String(encoding.GetBytes(path)))
        //            .WithInnerText(result.ToString("F")).OuterXml;
        //    }
        //    return XmlBuilder.NewDocument("Expected")
        //        .WithAttribute("HostName", hostName)
        //        .WithAttribute("Path", path)
        //        .WithInnerText(result.ToString("F")).OuterXml;
        //}
        public static IEnumerable<TestCaseData> GetTrySplitFileUriStringTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("TrySplitFileUriString");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTrySplitFileUriStringTestCases))]
        public string TrySplitFileUriStringTest(string uriString, bool base64Encoded)
        {
            bool result = WindowsFileUriConverter.INSTANCE.TrySplitFileUriString(uriString, out string hostName, out string path, out string fileName, out bool isAbsolute);
            if (result)
            {
                if (base64Encoded)
                {
                    UTF8Encoding encoding = new UTF8Encoding(false, true);
                    return XmlBuilder.NewDocument("TrySplitFileUriString")
                        .WithAttribute("HostName", (string.IsNullOrEmpty(hostName)) ? hostName : Convert.ToBase64String(encoding.GetBytes(hostName)))
                        .WithAttribute("Path", (string.IsNullOrEmpty(path)) ? path : Convert.ToBase64String(encoding.GetBytes(path)))
                        .WithAttribute("FileName", (string.IsNullOrEmpty(fileName)) ? fileName : Convert.ToBase64String(encoding.GetBytes(fileName)))
                        .WithAttribute("IsAbsolute", isAbsolute)
                        .WithInnerText(result).OuterXml;
                }
                return XmlBuilder.NewDocument("TrySplitFileUriString")
                    .WithAttribute("HostName", hostName)
                    .WithAttribute("Path", path)
                    .WithAttribute("FileName", fileName)
                    .WithAttribute("IsAbsolute", isAbsolute)
                    .WithInnerText(result).OuterXml;
            }
            return XmlBuilder.NewDocument("TrySplitFileUriString").WithInnerText(result).OuterXml;
        }

        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases()
        {
            throw new NotImplementedException();
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToFileSystemPathTestCases))]
        public string ToFileSystemPathTest(string hostName, string path)
        {
            string result = WindowsFileUriConverter.INSTANCE.ToFileSystemPath(hostName, path);
            return result;
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath3TestCases()
        {
            throw new NotImplementedException();
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFromFileSystemPath3TestCases))]
        public string FromFileSystemPath3Test(string path)
        {
            string result = WindowsFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName, out string directoryName);
            XElement xElement = new XElement("FromFileSystemPath");
            if (!(result is null))
                xElement.Add(new XElement("Result", result));
            if (!(hostName is null))
                xElement.Add(new XElement("HostName", hostName));
            if (!(directoryName is null))
                xElement.Add(new XElement("DirectoryName", directoryName));
            return xElement.ToString(SaveOptions.DisableFormatting);
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath2TestCases()
        {
            throw new NotImplementedException();
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFromFileSystemPath2TestCases))]
        public string FromFileSystemPath2Test(string path)
        {
            string result = WindowsFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName);
            XElement xElement = new XElement("FromFileSystemPath");
            if (!(result is null))
                xElement.Add(new XElement("Result", result));
            if (!(hostName is null))
                xElement.Add(new XElement("HostName", hostName));
            return xElement.ToString(SaveOptions.DisableFormatting);
        }
    }
}
