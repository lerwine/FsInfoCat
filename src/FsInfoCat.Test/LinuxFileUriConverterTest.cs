using FsInfoCat.Test.FileUriConverterTestHelpers;
using FsInfoCat.Test.Helpers;
using FsInfoCat.Util;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class LinuxFileUriConverterTest
    {
        private static readonly ILogger<FileUriConverterTest> _logger;
        private static readonly FilePathTestData _testItems;

        static LinuxFileUriConverterTest()
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
            return _testItems.Items.Select(i => i.Linux.AbsoluteUrl).Where(u => !(u is null)).Select(u =>
                new TestCaseData(u.Owner.Owner.InputString, UriKind.Absolute)
                    .Returns(!(u.LocalPath is null) && u.LocalPath.IsAbsolute && u.IsWellFormed && u.IsFileScheme())
            ).Concat(
                _testItems.Items.Select(i => i.Linux.AbsoluteUrl).Where(u => !(u is null)).Select(u =>
                    new TestCaseData(u.Owner.Owner.InputString, UriKind.Relative)
                        .Returns(!(u.LocalPath is null || u.LocalPath.IsAbsolute) && u.IsWellFormed && u.IsFileScheme())
                )
            ).Concat(
                _testItems.Items.Select(i => i.Linux.RelativeUrl).Where(u => !(u is null)).Select(u =>
                    new TestCaseData(u.Owner.Owner.InputString, UriKind.Relative)
                    .Returns(!(u.LocalPath is null) && u.IsWellFormed)
                )
            ).Concat(
                _testItems.Items.Select(i => i.Linux.RelativeUrl).Where(u => !(u is null)).Select(u =>
                    new TestCaseData(u.Owner.Owner.InputString, UriKind.Absolute)
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
            // DEFERRED: Get test cases from XML
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
                        .AppendElement(false, "isAbsolute")
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

        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases() => _testItems.Items.Select(testDataItem => testDataItem.Linux.AbsoluteUrl)
            .Where(u => !(u is null || u.LocalPath is null) && u.IsFileScheme()).Select(u =>
                new TestCaseData(u.Owner.Owner.InputString).Returns(TestResultBuilder.CreateTestResult(u.LocalPath.Path).ToTestResultString()))
                .Concat(_testItems.Items.Select(testDataItem => testDataItem.Linux.RelativeUrl).Where(u => !(u is null || u.LocalPath is null)).Select(u =>
                new TestCaseData(u.Owner.Owner.InputString).Returns(TestResultBuilder.CreateTestResult(u.LocalPath.Path).ToTestResultString())));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToFileSystemPathTestCases))]
        public string ToFileSystemPathTest(string fileUriString)
        {
            string returnValue = LinuxFileUriConverter.INSTANCE.ToFileSystemPath(fileUriString);
            return TestResultBuilder.CreateTestResult(returnValue).ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetFromFileSystemPath3TestCases()
        {
            // DEFERRED: Get test cases from XML
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
            // DEFERRED: Get test cases from XML
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
            // DEFERRED: Get test cases from XML
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