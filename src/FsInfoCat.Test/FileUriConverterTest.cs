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

        //public static IEnumerable<TestCaseData> GetDnsNameRegexTestCases()
        //{
        //    return UrlHelperTest.GetHostNameTestData().Select(element =>
        //    {
        //        XmlElement expected = (XmlElement)element.SelectSingleNode("DnsNameRegex");
        //        return new TestCaseData(element.GetAttribute("Value"))
        //            .SetDescription(element.GetAttribute("Description"))
        //            .Returns(expected.OuterXml);
        //    });
        //}

        //[Test, Property("Priority", 1)]
        //[TestCaseSource(nameof(GetDnsNameRegexTestCases))]
        //public string DnsNameRegexTest(string input)
        //{
        //    Match match = FileUriConverter.DNS_NAME_REGEX.Match(input);
        //    return UrlHelperTest.ToTestReturnValueXml(match, "DnsNameRegex");
        //}

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
