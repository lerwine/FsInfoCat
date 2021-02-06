using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        #region static fields

        private static readonly string _hostName;
        private static readonly string _systemDrivePath;
        private static readonly string _systemDriveUrl;
        private static readonly string _ipV2Address;
        private static readonly string _ipV6Address;

        static VolumeIdentifierTest()
        {
            _hostName = Environment.MachineName.ToLower();
            _systemDrivePath = Path.GetPathRoot(Environment.SystemDirectory);
            _systemDriveUrl = new Uri(_systemDrivePath, UriKind.Absolute).AbsoluteUri;
            IPAddress[] addressList = Dns.GetHostEntry(_hostName).AddressList;
            _ipV2Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.ToString()).DefaultIfEmpty("127.0.0.1").First();
            _ipV6Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6)
                .Select(a => a.ToString()).DefaultIfEmpty("::1").First().Split('%')[0];
        }

        #endregion

        public static IEnumerable<TestCaseData> GetGuidToIdentifierStringTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.UUID)
                    .SetName($"{t.Description} GuidToIdentifierStringTest({t.UUID})")
                .Returns(t.StringValue));

        public static IEnumerable<TestCaseData> GetSerialNumberToIdentifierStringTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"{t.Description} SerialNumberToIdentifierStringTest({t.SerialNumber}, null)")
                .Returns(t.StringValue))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select(t => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} SerialNumberToIdentifierStringTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(t.StringValue)));

        public static IEnumerable<TestCaseData> GetGuidToUrnTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.UUID)
                .SetName($"{t.Description} GuidToUrnTest({t.UUID})")
                .Returns(t.Url.AbsoluteUri));

        public static IEnumerable<TestCaseData> GetSerialNumberToUrnTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"{t.Description} SerialNumberToUrnTest({t.SerialNumber})")
                .Returns(t.Url.AbsoluteUri))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select(t => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} SerialNumberToUrnTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(t.Url.AbsoluteUri)));

        public static IEnumerable<TestCaseData> GetGuidParameterTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t =>
                new TestCaseData(t.UUID)
                    .SetName($"{t.Description} FromGuidValueConstructorTest({t.UUID})")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidUUIDUriParameterTestCases() => UUIDTestValues.GetTestValues(true)
            .Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidUUIDUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new UuidValues(t.UUID, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberParameterTestCases() =>
            SerialNumberTestValues.GetTestValues(false, false).Select(t =>
                new TestCaseData(t.SerialNumber)
                    .SetName($"{t.Description} FromSerialNumberValueConstructorTest({t.SerialNumber})")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberAndOrdinalParametersTestCases() => SerialNumberOrdinalTestValues.GetTestValues(false)
            .Select(t =>
                new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} FromSerialNumberAndOrdinalValuesConstructorTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidSerialNumberUriParameterTestCases() => SerialNumberTestValues.GetTestValues(true, true)
            .Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidSerialNumberAndOrdinalUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new SnIdValues(t.SerialNumber, null, t.StringValue, t.Url.AbsoluteUri))
            ).Concat(SerialNumberOrdinalTestValues.GetTestValues(true).Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidSerialNumberAndOrdinalUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            ));

        public static IEnumerable<TestCaseData> GetValidFilePathParameterTestCases()
        {
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare";
            string expected = $@"{path}\";
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare/";
            yield return new TestCaseData(path)
                .SetName($"URN with fully qualified machine name: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\$Admin";
            expected = $@"{path}\";
            url = $"file://{_hostName}/$Admin/";
            yield return new TestCaseData(path)
                .SetName($"URN with machine name only: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\Admin$\";
            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them";
            expected = $@"{path}\";
            url = $"file://{_ipV2Address}/Us&Them/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV2 address: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done";
            expected = $@"{path}\";
            url = $"file://[{_ipV6Address}]/100%25%20Done/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV6 address: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));
        }

        public static IEnumerable<TestCaseData> GetValidFileUrlParameterTestCases()
        {
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare";
            string expected = $"{url}/";
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare\";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            string noTrailingSlash = url;
            url = $"{noTrailingSlash}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty query: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty query: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, empty query and empty fragment:0 FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash, empty query and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_hostName}/$Admin";
            expected = $"{url}/";
            path = $@"\\{_hostName}\$Admin\";
            yield return new TestCaseData(url)
                .SetName($"URN with machine name only: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_ipV2Address}/Us&Them";
            expected = $"{url}/";
            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV2 address: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://[{_ipV6Address}]/100%25%20Done";
            expected = $"{url}/";
            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV6 address: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));
        }
    }
}
