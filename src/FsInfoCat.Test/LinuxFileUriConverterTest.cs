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
            return FileUriConverterTest.FilePathTestData.Root.Elements().Select(testDataElement =>
            {
                XElement fileSystemElement = testDataElement.Elements("Linux").Elements("FileSystem").FirstOrDefault(); ;
                if (fileSystemElement is null)
                    return new TestCaseData(testDataElement.Attribute("InputString").Value)
                        .Returns(new XElement("FsPathRegex", new XAttribute("Success", false)).ToString(SaveOptions.DisableFormatting));
                bool base64Encoded = testDataElement.Attributes("Base64").Any(a => XmlConvert.ToBoolean(a.Value));
                // TODO: Handle base-64
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(testDataElement.Attribute("InputString").Value)) :
                        testDataElement.Attribute("InputString").Value, base64Encoded)
                    .Returns(new XElement("FsPathRegex", new XAttribute("Success", true),
                        fileSystemElement.Elements("Host").Attributes("Match").Select(a => new XElement("Group",
                                new XAttribute("Name", "host"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "host"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Host").Where(e => e.Attribute("Type").Value == "IPV4")
                            .Select(a => new XElement("Group",
                                new XAttribute("Name", "ipv4"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Host").Where(e => e.Attribute("Type").Value == "IPV6" || e.Attribute("Type").Value == "IPV6.UNC")
                            .Select(a => new XElement("Group",
                                new XAttribute("Name", "ipv6"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Host").Where(e => e.Attribute("Type").Value == "IPV6.UNC").Attributes("Match")
                            .Select(a => new XElement("Group",
                                new XAttribute("Name", "unc"), new XAttribute("Success", true), a.Value.Substring(a.Value.IndexOf('.') + 1)))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "unc"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Host").Where(e => e.Attribute("Type").Value == "DNS")
                            .Select(a => new XElement("Group",
                                new XAttribute("Name", "dns"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Path").Attributes("Match").Select(a => new XElement("Group",
                                new XAttribute("Name", "path"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "path"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Path").Elements("Directory").Select(a => new XElement("Group",
                                new XAttribute("Name", "dir"), new XAttribute("Success", true), a.Value))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "dir"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Path").Select(a => new XElement("Group",
                                new XAttribute("Name", "root"), new XAttribute("Success", true)))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "root"), new XAttribute("Success", false))).First(),
                        fileSystemElement.Elements("Path").Elements("FileName").Select(a => new XElement("Group",
                                new XAttribute("Name", "fileName"), new XAttribute("Success", true)))
                            .DefaultIfEmpty(new XElement("Group", new XAttribute("Name", "fileName"), new XAttribute("Success", false))).First()
                    ).ToString(SaveOptions.DisableFormatting));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFsPathRegexTestCases))]
        public string FsPathRegexTest(string input, bool base64Encoded)
        {
            Match match = LinuxFileUriConverter.FS_HOST_DIR_AND_FILE_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "FsPathRegex", base64Encoded, "host", "ipv4", "ipv6", "unc", "dns", "path", "dir", "root", "fileName");
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
            Match match = LinuxFileUriConverter.URI_HOST_DIR_AND_FILE_STRICT_REGEX.Match(input);
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
            Match match = LinuxFileUriConverter.HOST_NAME_OR_ADDRESS_FOR_URI_REGEX.Match(input);
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
            // TODO: Implement GetToFileSystemPathTestCases
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
            // TODO: Implement GetFromFileSystemPath3TestCases
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
            // TODO: Implement GetFromFileSystemPath2TestCases
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
