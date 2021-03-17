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
    public class LinuxFileUriConverterTest
    {
        public static IEnumerable<TestCaseData> GetFsPathRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Linux/FsPathRegex");
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
            Match match = LinuxFileUriConverter.FS_PATH_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FsPathRegex", base64Encoded, "root", "host", "path");
        }

        public static IEnumerable<TestCaseData> GetFormatDetectionRegexTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Linux/FormatGuess");
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
            Match match = LinuxFileUriConverter.FORMAT_DETECTION_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FormatGuess", base64Encoded, "file", "d", "host", "path");
        }

        public static IEnumerable<TestCaseData> GetFileUriStrictTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Linux/FileUriStrict");
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
            Match match = LinuxFileUriConverter.FILE_URI_STRICT_REGEX.Match(input);
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
            Match match = LinuxFileUriConverter.HOST_NAME_REGEX.Match(input);
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
            bool result = LinuxFileUriConverter.INSTANCE.IsWellFormedUriString(uriString, kind);
            return result;
        }

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
            bool result = LinuxFileUriConverter.INSTANCE.TrySplitFileUriString(uriString, out string hostName, out string path, out string fileName, out bool isAbsolute);
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
            string result = LinuxFileUriConverter.INSTANCE.ToFileSystemPath(hostName, path);
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
            string result = LinuxFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName, out string directoryName);
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
            string result = LinuxFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName);
            XElement xElement = new XElement("FromFileSystemPath");
            if (!(result is null))
                xElement.Add(new XElement("Result", result));
            if (!(hostName is null))
                xElement.Add(new XElement("HostName", hostName));
            return xElement.ToString(SaveOptions.DisableFormatting);
        }
    }
}
