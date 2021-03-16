using FsInfoCat.Test.Helpers;
using FsInfoCat.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class FileUriConverterTest
    {
        public static IEnumerable<TestCaseData> GetUriPathSegmentRegexTestCases()
        {
            return (new string[][]
            {
                new string[] { "" },
                new string[] { "/" },
                new string[] { "/", "Test" },
                new string[] { "/", "Test/" },
                new string[] { "/", "Test/", "Two" },
                new string[] { "/", "Test/", "Two/" },
                new string[] { "Test" },
                new string[] { "Test/" },
                new string[] { "Test/", "Two" },
                new string[] { "Test/", "Two/" }
            }).Select(expected => new TestCaseData(string.Join("", expected))
                .SetDescription($"\"{string.Join("\" + \"", expected)}\"")
                .Returns(expected));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetUriPathSegmentRegexTestCases))]
        public string[] UriPathSegmentRegexTest(string input)
        {
            MatchCollection matches = FileUriConverter.URI_PATH_SEGMENT_REGEX.Matches(input);
            return matches.Select(m => m.Value).ToArray();
        }

        public static IEnumerable<TestCaseData> GetPatternHostNameTestCases()
        {
            return UrlHelperTest.GetHostNameTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("PatternHostName");
                return new TestCaseData(element.GetAttribute("Value"))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternHostNameTestCases))]
        public string PatternHostNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_HOST_NAME);
            return UrlHelperTest.ToTestReturnValueXml(match, "PatternHostName");
        }

        //public static IEnumerable<TestCaseData> GetIpv2AddressRegexTestCases()
        //{
        //    return UrlHelperTest.GetHostNameTestData().Select(element =>
        //    {
        //        XmlElement expected = (XmlElement)element.SelectSingleNode("Ipv2AddressRegex");
        //        return new TestCaseData(element.GetAttribute("Value"))
        //            .SetDescription(element.GetAttribute("Description"))
        //            .Returns(expected.OuterXml);
        //    });
        //}

        //[Test, Property("Priority", 1)]
        //[TestCaseSource(nameof(GetIpv2AddressRegexTestCases))]
        //public string Ipv2AddressRegexTest(string input)
        //{
        //    Match match = FileUriConverter.IPV2_ADDRESS_REGEX.Match(input);
        //    return UrlHelperTest.ToTestReturnValueXml(match, "Ipv2AddressRegex");
        //}

        public static IEnumerable<TestCaseData> GetIpv6AddressRegexTestCases()
        {
            return UrlHelperTest.GetHostNameTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("Ipv6AddressRegex");
                return new TestCaseData(element.GetAttribute("Value"))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIpv6AddressRegexTestCases))]
        public string Ipv6AddressRegexTest(string input)
        {
            Match match = FileUriConverter.IPV6_ADDRESS_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "Ipv6AddressRegex", "ipv6");
        }

        public static IEnumerable<TestCaseData> GetPatternDnsNameTestCases()
        {
            return UrlHelperTest.GetHostNameTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("PatternDnsName");
                return new TestCaseData(element.GetAttribute("Value"))
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternDnsNameTestCases))]
        public string PatternDnsNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_DNS_NAME);
            return UrlHelperTest.ToTestReturnValueXml(match, "PatternDnsName");
        }

        public static IEnumerable<TestCaseData> GetTryParseFileUriStringTestCases()
        {
            return UrlHelperTest.GetUriTestData().Select(element =>
            {
                XmlElement expected = (XmlElement)element.SelectSingleNode("TryParseFileUriString");
                bool base64Encoded = element.GetAttribute("Base64") == "true";
                return new TestCaseData((base64Encoded) ? Encoding.UTF8.GetString(Convert.FromBase64String(element.GetAttribute("Value"))) : element.GetAttribute("Value"), base64Encoded)
                    .SetDescription(element.GetAttribute("Description"))
                    .Returns(expected.OuterXml);
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryParseFileUriStringTestCases))]
        public string TryParseFileUriStringTest(string uriString, bool base64Encoded)
        {
            bool result = FileUriConverter.TrySplitFileUriString(uriString, out string hostName, out string absolutePath, out int leafIndex);
            if (base64Encoded)
            {
                UTF8Encoding encoding = new UTF8Encoding(false, true);
                return XmlBuilder.NewDocument("TryParseFileUriString")
                    .WithAttribute("HostName", (string.IsNullOrEmpty(hostName)) ? hostName : Convert.ToBase64String(encoding.GetBytes(hostName)))
                    .WithAttribute("AbsolutePath", (string.IsNullOrEmpty(absolutePath)) ? absolutePath : Convert.ToBase64String(encoding.GetBytes(absolutePath)))
                    .WithAttribute("LeafIndex", leafIndex)
                    .WithInnerText(result).OuterXml;
            }
            return XmlBuilder.NewDocument("TryParseFileUriString")
                .WithAttribute("HostName", hostName)
                .WithAttribute("AbsolutePath", absolutePath)
                .WithAttribute("LeafIndex", leafIndex)
                .WithInnerText(result).OuterXml;
        }
    }
}
