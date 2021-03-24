using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        #region static fields

        private static readonly string _hostName;
        private static readonly string _systemDrivePath;
        private static readonly string _systemDriveUrl;
        private static readonly string _ipV4Address;
        private static readonly string _ipV6Address;

        static VolumeIdentifierTest()
        {
            _hostName = Environment.MachineName.ToLower();
            _systemDrivePath = Path.GetPathRoot(Environment.SystemDirectory);
            _systemDriveUrl = new Uri(_systemDrivePath, UriKind.Absolute).AbsoluteUri;
            IPAddress[] addressList = Dns.GetHostEntry(_hostName).AddressList;
            _ipV4Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.ToString()).DefaultIfEmpty("127.0.0.1").First();
            _ipV6Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6)
                .Select(a => a.ToString()).DefaultIfEmpty("::1").First().Split('%')[0];
        }

        #endregion

        public static IEnumerable<TestCaseData> GetGuidToIdentifierStringTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.UUID)
                    .SetArgDisplayNames("guid")
                    .SetDescription($"{i}. {t.UUID} {t.Description}")
                .Returns(t.StringValue[5..]));

        public static IEnumerable<TestCaseData> GetSerialNumberToIdentifierStringTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetDescription($"{i}. {t.SerialNumber} {t.Description}")
                .Returns(t.StringValue[10..]))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select((t, i) => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetDescription($"{i}. ({t.SerialNumber}, {t.Ordinal}) {t.Description}")
                    .Returns(t.StringValue[10..])));

        public static IEnumerable<TestCaseData> GetGuidToUrnTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.UUID)
                    .SetArgDisplayNames("guid")
                .SetDescription($"{i}. {t.UUID} {t.Description}")
                .Returns(t.Url.AbsoluteUri));

        public static IEnumerable<TestCaseData> GetSerialNumberToUrnTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetDescription($"{i}. {t.SerialNumber}) {t.Description}")
                .Returns(t.Url.AbsoluteUri))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select((t, i) => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetDescription($"{i}. ({t.SerialNumber}, {t.Ordinal}) {t.Description}")
                    .Returns(t.Url.AbsoluteUri)));

        public static IEnumerable<TestCaseData> GetGuidParameterTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) =>
                new TestCaseData(t.UUID)
                    .SetArgDisplayNames("guid")
                    //.SetDescription($"{i}. {t.UUID} {t.Description}")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidUUIDUriParameterTestCases() => UUIDTestValues.GetTestValues(true)
            .Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetArgDisplayNames("uri")
                    //.SetDescription($"{i}. \"{t.UrnParam}\" {t.Description}")
                    .Returns(new UuidValues(t.UUID, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberParameterTestCases() =>
            SerialNumberTestValues.GetTestValues(false, false).Select((t, i) =>
                new TestCaseData(t.SerialNumber)
                    .SetDescription($"{i}. {t.SerialNumber} {t.Description}")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberAndOrdinalParametersTestCases() => SerialNumberOrdinalTestValues.GetTestValues(false)
            .Select((t, i) =>
                new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetArgDisplayNames("serialNumber", "ordinal")
                    .SetDescription($"{i}. ({t.SerialNumber}, {t.Ordinal}) // {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidSerialNumberUriParameterTestCases() => SerialNumberTestValues.GetTestValues(true, true)
            .Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetDescription($"{i}. \"{t.UrnParam}\" {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, null, t.StringValue, t.Url.AbsoluteUri))
            ).Concat(SerialNumberOrdinalTestValues.GetTestValues(true).Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetDescription($"{i}. \"{t.UrnParam}\" // {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            ));

        public static IEnumerable<TestCaseData> GetValidFilePathParameterTestCases()
        {
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare";
            string expected = $@"{path}\";
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare/";
            yield return new TestCaseData(path)
                .SetDescription($"\"{path}\"")
                .Returns(new IdValues(expected, url));

            yield return new TestCaseData(expected)
                .SetDescription($" \"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\$Admin";
            expected = $@"{path}\";
            url = $"file://{_hostName}/$Admin/";
            yield return new TestCaseData(path)
                .SetDescription($"\"{path}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\Admin$\";
            expected = $@"{path}\";
            url = $"file://{_hostName}/Admin$/";
            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV4Address}\Us&Them";
            expected = $@"{path}\";
            url = $"file://{_ipV4Address}/Us&Them/";
            yield return new TestCaseData(path)
                .SetDescription($"\"{path}\"")
                .Returns(new IdValues(expected, url));

            expected = $@"\\{_ipV4Address}\Us&Them\";
            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done";
            expected = $@"{path}\";
            url = $"file://[{_ipV6Address}]/100%25%20Done/";
            yield return new TestCaseData(path)
                .SetDescription($"\"{path}\"")
                .Returns(new IdValues(expected, url));

            expected = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(expected, url));
        }

        public static IEnumerable<TestCaseData> GetValidFileUrlParameterTestCases()
        {
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare";
            string expected = $"{url}/";
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare\";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(path, expected));

            string noTrailingSlash = url;
            url = $"{noTrailingSlash}?";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}#";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{expected}#";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}?#";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?#";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_hostName}/$Admin";
            expected = $"{url}/";
            path = $@"\\{_hostName}\$Admin\";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(path, expected));

            url = $"file://{_ipV4Address}/Us&Them";
            expected = $"{url}/";
            path = $@"\\{_ipV4Address}\Us&Them\";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(path, expected));

            url = $"file://[{_ipV6Address}]/100%25%20Done";
            expected = $"{url}/";
            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(url)
                .SetDescription($"\"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetDescription($"\"{expected}\"")
                .Returns(new IdValues(path, expected));
        }
    }
}
