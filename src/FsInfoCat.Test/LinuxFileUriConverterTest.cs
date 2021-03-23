using FsInfoCat.Test.Helpers;
using FsInfoCat.Test.FileUriConverterTestHelpers;
using FsInfoCat.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class LinuxFileUriConverterTest
    {
        private static ILogger<FileUriConverterTest> _logger;
        private static FilePathTestData _testItems;

        [SetUp()]
        public static void Init()
        {
            _logger = TestLogger.Create<FileUriConverterTest>();

            try
            {
                _testItems = FilePathTestData.Load();
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, "Failed to load test items");
                throw;
            }
        }

        public static IEnumerable<TestCaseData> GetIsWellFormedFileUriStringTestCases()
        {
            _tes
            return FileUriConverterTest.FilePathTestDataXML.LinuxElements().AbsoluteUrlElements().Select(element =>
                new TestCaseData(element.InputString(), UriKind.Absolute)
                    .Returns(element.IsWellFormed() && element.IsFileScheme())
            ).Concat(
                FileUriConverterTest.FilePathTestDataXML.LinuxElements().AbsoluteUrlElements().Select(element =>
                    new TestCaseData(element.InputString(), UriKind.Relative)
                        .Returns(false)
                )
            ).Concat(
                FileUriConverterTest.FilePathTestDataXML.LinuxElements().RelativeUrlElements().Select(element =>
                    new TestCaseData(element.InputString(), UriKind.Relative)
                        .Returns(element.IsWellFormed())
                )
            ).Concat(
                FileUriConverterTest.FilePathTestDataXML.LinuxElements().RelativeUrlElements().Select(element =>
                    new TestCaseData(element.InputString(), UriKind.Absolute)
                        .Returns(false)
                )
            );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsWellFormedFileUriStringTestCases))]
        public bool IsWellFormedFileUriStringTest(string uriString, UriKind kind)
        {
            bool returnValue = LinuxFileUriConverter.INSTANCE.IsWellFormedUriString(uriString, kind);
            return returnValue;
        }

        public static IEnumerable<TestCaseData> GetTrySplitFileUriStringTestCases()
        {
            // TODO: Get test cases from XML
            yield return new TestCaseData("file://mysite/My%20Documents/MyFile%231.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.TrySplitFileUriString))
                        .AppendElement("mysite", "hostName")
                        .AppendElement("/My%20Documents", "path")
                        .AppendElement("MyFile%231.txt", "fileName")
                        .AppendElement(true, "isAbsolute")
                        .AppendElement(true, "returnValue").ToTestResultString()
                );
            yield return new TestCaseData("/My Documents/MyFile#1.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.TrySplitFileUriString))
                        .AppendElement("", "hostName")
                        .AppendElement("/My%20Documents", "path")
                        .AppendElement("MyFile%231.txt", "fileName")
                        .AppendElement(true, "isAbsolute")
                        .AppendElement(true, "returnValue").ToTestResultString()
                );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTrySplitFileUriStringTestCases))]
        public string TrySplitFileUriStringTest(string uriString)
        {
            bool returnValue = LinuxFileUriConverter.INSTANCE.TrySplitFileUriString(uriString, out string hostName, out string path, out string fileName, out bool isAbsolute);
            if (returnValue)
                return new XElement(nameof(LinuxFileUriConverter.TrySplitFileUriString))
                    .AppendElement(hostName, nameof(hostName))
                    .AppendElement(path, nameof(path))
                    .AppendElement(fileName, nameof(fileName))
                    .AppendElement(isAbsolute, nameof(isAbsolute))
                    .AppendElement(returnValue, nameof(returnValue)).ToTestResultString();
            return new XElement(nameof(LinuxFileUriConverter.TrySplitFileUriString))
                .AppendElement(returnValue, nameof(returnValue)).ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases()
        {
            // TODO: Get test cases from XML
            yield return new TestCaseData("mysite", "My%20Documents")
                .Returns(TestResultBuilder.CreateTestResult("//mysite/My Documents").ToTestResultString());
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToFileSystemPathTestCases))]
        public string ToFileSystemPathTest(string hostName, string path)
        {
            string returnValue = LinuxFileUriConverter.INSTANCE.ToFileSystemPath(hostName, path);
            return TestResultBuilder.CreateTestResult(returnValue).ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath3TestCases()
        {
            // TODO: Get test cases from XML
            yield return new TestCaseData("//mysite/My Documents/MyFile#1.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                        .AppendElement("MyFile%231.txt", "returnValue")
                        .AppendElement("mysite", "hostName")
                        .AppendElement("/My%20Documents", "directoryName").ToTestResultString()
                );
            yield return new TestCaseData("/My Documents/MyFile#1.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                        .AppendElement("MyFile%231.txt", "returnValue")
                        .AppendElement("", "hostName")
                        .AppendElement("/My%20Documents", "directoryName").ToTestResultString()
                );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFromFileSystemPath3TestCases))]
        public string FromFileSystemPath3Test(string path)
        {
            string returnValue = LinuxFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName, out string directoryName);
            return new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                .AppendElement(returnValue, nameof(returnValue))
                .AppendElement(hostName, nameof(hostName))
                .AppendElement(directoryName, nameof(directoryName)).ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath2TestCases()
        {
            // TODO: Get test cases from XML
            yield return new TestCaseData("//mysite/My Documents/MyFile#1.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                        .AppendElement("/My%20Documents/MyFile%231.txt", "returnValue")
                        .AppendElement("mysite", "hostName").ToTestResultString()
                );
            yield return new TestCaseData("/My Documents/MyFile#1.txt")
                .Returns(
                    new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                        .AppendElement("/My%20Documents/MyFile%231.txt", "returnValue")
                        .AppendElement("", "hostName").ToTestResultString()
                );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFromFileSystemPath2TestCases))]
        public string FromFileSystemPath2Test(string path)
        {
            string returnValue = LinuxFileUriConverter.INSTANCE.FromFileSystemPath(path, out string hostName);
            return new XElement(nameof(LinuxFileUriConverter.FromFileSystemPath))
                .AppendElement(returnValue, nameof(returnValue))
                .AppendElement(hostName, nameof(hostName)).ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath1TestCases()
        {
            // TODO: Get test cases from XML
            yield return new TestCaseData("//mysite/My Documents/MyFile#1.txt")
                .Returns(TestResultBuilder.CreateTestResult("file://mysite/My%20Documents/MyFile%231.txt").ToTestResultString());
            yield return new TestCaseData("/My Documents/MyFile#1.txt")
                .Returns(TestResultBuilder.CreateTestResult("file:///My%20Documents/MyFile%231.txt").ToTestResultString());
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetFromFileSystemPath1TestCases))]
        public string FromFileSystemPath1Test(string path)
        {
            string returnValue;
            try { returnValue = LinuxFileUriConverter.INSTANCE.FromFileSystemPath(path); }
            catch (ArgumentOutOfRangeException exc)
            {
                return exc.CreateExceptionResult().ToTestResultString();
            }
            return TestResultBuilder.CreateTestResult(returnValue).ToTestResultString();
        }
    }
}
