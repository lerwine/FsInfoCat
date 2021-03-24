using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FsInfoCat.Test.Helpers
{
    public static class TestResultBuilder
    {
        private const string ElementNameNull = "null";
        public static readonly XNamespace XmlSchemaInstanceNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        public static readonly XName XmlSchemaInstanceXmlns = XNamespace.Xmlns.GetName("xsi");
        public static readonly XName XmlSchemaInstanceNil = XmlSchemaInstanceNamespace.GetName("nil");

        public static XElement AppendElement(this XElement target, object content, XName elementName)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (elementName is null)
                throw new ArgumentNullException(nameof(elementName));
            if (content is null)
                target.Add(CreateNullResult(elementName));
            else
                target.Add(new XElement(elementName, content));
            return target;
        }

        public static string StringFromBase64Encoded(this XElement target)
        {
            if (target is null || target.IsEmpty)
                return null;
            string result = target.Value;
            if ((result = result.Trim()).Length == 0)
                return result;
            return Encoding.Unicode.GetString(Convert.FromBase64String(result));
        }

        public static byte[] BytesFromBase64Encoded(this XElement target)
        {
            if (target is null || target.IsEmpty)
                return null;
            string result = target.Value;
            if ((result = result.Trim()).Length == 0)
                return Array.Empty<byte>();
            return Convert.FromBase64String(result);
        }

        public static XElement AppendElementBase64Encoded(this XElement target, string content, XName elementName = null)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (content is null)
                target.Add(CreateNullResult(elementName ?? "string"));
            else
                target.Add(new XElement(elementName ?? "string", (content.Length == 0) ? "" : Convert.ToBase64String(Encoding.Unicode.GetBytes(content))));
            return target;
        }

        public static XElement AppendElementBase64Encoded(this XElement target, byte[] content, XName elementName = null)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (content is null)
                target.Add(CreateNullResult(elementName ?? "bytes"));
            else
                target.Add(new XElement(elementName ?? "bytes", (content.Length == 0) ? "" : Convert.ToBase64String(content)));
            return target;
        }

        public static XElement ApplyElementFirst(this XElement target, object content, XName elementName)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (elementName is null)
                throw new ArgumentNullException(nameof(elementName));
            if (content is null)
                target.AddFirst(CreateNullResult(elementName));
            else
                target.AddFirst(new XElement(GetResultElementName(content), content));
            return target;
        }

        public static XElement ApplyElementFirstBase64Encoded(this XElement target, string content, XName elementName = null)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (content is null)
                target.AddFirst(CreateNullResult(elementName ?? "string"));
            else
                target.AddFirst(new XElement(elementName ?? "string", (content.Length == 0) ? "" : Convert.ToBase64String(Encoding.Unicode.GetBytes(content))));
            return target;
        }

        public static XElement ApplyElementFirstBase64Encoded(this XElement target, byte[] content, XName elementName = null)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (content is null)
                target.AddFirst(CreateNullResult(elementName ?? "bytes"));
            else
                target.AddFirst(new XElement(elementName ?? "bytes", (content.Length == 0) ? "" : Convert.ToBase64String(content)));
            return target;
        }

        public static string ToTestResultString(this XElement target)
        {
            target.SetAttributeValue(XmlSchemaInstanceXmlns, XmlSchemaInstanceNamespace.NamespaceName);
            return target.ToString(SaveOptions.DisableFormatting);
        }

        public static XElement CreateNullResult(XName elementName = null) => new XElement(elementName ?? ElementNameNull, new XAttribute(XmlSchemaInstanceNil, true));

        public static XElement NormalizeXsiNs(this XElement target)
        {
            if (target is null)
                return target;
            string ns = XmlSchemaInstanceNamespace.NamespaceName;
            XAttribute[] childXsi = target.Descendants().SelectMany(e => e.Attributes(XmlSchemaInstanceXmlns).Where(a => a.Value == ns)).ToArray();
            if (childXsi.Length == 0)
                return target;
            if (target.Attribute(XmlSchemaInstanceXmlns) is null)
                target.SetAttributeValue(XmlSchemaInstanceXmlns, "http://www.w3.org/2001/XMLSchema-instance");
            foreach (XAttribute a in childXsi)
                a.Remove();
            return target;
        }

        private static XElement CreateRegexResult(XName typeName, string name, bool success, string value, int index, int length) =>
            CreateRegexResult(typeName, name, success, index, length).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, string name, bool success, int index, int length) => CreateRegexResult(typeName, name, success)
            .ApplyAttributeValue(nameof(Capture.Index), index).ApplyAttributeValue(nameof(Capture.Length), length);

        private static XElement CreateRegexResult(XName typeName, string name, string value, int index, int length) =>
            CreateRegexResult(typeName, name, index, length).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, bool success, string value, int index, int length) =>
            CreateRegexResult(typeName, success, index, length).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, string name, bool success, string value) =>
            CreateRegexResult(typeName, name, success).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, bool success, int index, int length) => CreateRegexResult(typeName, success)
            .ApplyAttributeValue(nameof(Capture.Index), index).ApplyAttributeValue(nameof(Capture.Length), length);

        private static XElement CreateRegexResult(XName typeName, string value, int index, int length) => CreateRegexResult(typeName, index, length).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, string name, bool success) => CreateRegexResult(typeName, name)
            .ApplyAttributeValue(nameof(Group.Success), success);

        private static XElement CreateRegexResult(XName typeName, string name, string value) => CreateRegexResult(typeName, name).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, bool success, string value) => CreateRegexResult(typeName, success).ApplyValue(value);

        private static XElement CreateRegexResult(XName typeName, int index, int length) =>
            new XElement(typeName, new XAttribute(nameof(Capture.Index), index), new XAttribute(nameof(Capture.Length), length));

        private static XElement CreateRegexResult(XName typeName, string name) => (name is null) ? new XElement(typeName) :
            new XElement(typeName, new XAttribute(nameof(Group.Name), name));

        private static XElement CreateRegexResult(XName typeName, bool success) => new XElement(typeName, new XAttribute(nameof(Group.Success), success));

        public static XElement CreateMatchResult(bool success) => CreateRegexResult(nameof(Match), success);

        public static XElement CreateGroupResult(bool success) => CreateRegexResult(nameof(Group), success);

        public static XElement CreateCaptureResult(string value, int index, int length) => CreateRegexResult(nameof(Capture), value, index, length);

        public static XElement CreateMatchResult(string value) => (value is null) ? CreateMatchResult(false) : CreateMatchResult(true).ApplyValue(value);

        public static IEnumerable<Exception> GetAllInnerExceptions(this Exception exception)
        {
            if (exception is null)
                yield break;
            if (!(exception.InnerException is null))
                yield return exception.InnerException;
            if (exception is AggregateException aggregateException && !(aggregateException.InnerExceptions is null))
            {
                Exception i = exception.InnerException;
                if (i is null)
                    foreach (Exception e in aggregateException.InnerExceptions)
                        yield return e;
                else
                    foreach (Exception e in aggregateException.InnerExceptions)
                    {
                        if (!ReferenceEquals(i, e))
                            yield return e;
                    }
            }
        }

        public static XName GetResultElementName(object obj)
        {
            if (obj is null)
                return ElementNameNull;
            if (obj is string)
                return "string";
            if (obj is byte)
                return "byte";
            if (obj is sbyte)
                return "sbyte";
            if (obj is short)
                return "short";
            if (obj is ushort)
                return "ushort";
            if (obj is int)
                return "int";
            if (obj is uint)
                return "uint";
            if (obj is long)
                return "long";
            if (obj is ulong)
                return "ulong";
            if (obj is float)
                return "float";
            if (obj is double)
                return "double";
            if (obj is Exception exception)
                return GetDefaultResultElementName(exception);
            return System.Xml.XmlConvert.EncodeLocalName(obj.GetType().Name);
        }

        public static XName GetDefaultFullResultElementName(this Exception exception)
        {
            if (exception is null)
                return nameof(Exception);
            if (exception is ArgumentException)
            {
                if (exception is ArgumentOutOfRangeException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(ArgumentOutOfRangeException).FullName);
                if (exception is ArgumentNullException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(ArgumentNullException).FullName);
                return System.Xml.XmlConvert.EncodeLocalName(typeof(ArgumentException).FullName);
            }
            if (exception is System.Xml.XmlException)
            {
                if (exception is System.Data.Common.DbException)
                {
                    if (exception is System.Data.SqlClient.SqlException)
                        return System.Xml.XmlConvert.EncodeLocalName(typeof(System.Data.SqlClient.SqlException).FullName);
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(System.Data.Common.DbException).FullName);
                }
                return System.Xml.XmlConvert.EncodeLocalName(typeof(System.Runtime.InteropServices.ExternalException).FullName);
            }
            if (exception is System.Xml.XmlException)
                return System.Xml.XmlConvert.EncodeLocalName(typeof(System.Xml.XmlException).FullName);
            if (exception is IOException)
            {
                if (exception is FileNotFoundException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(FileNotFoundException).FullName);
                if (exception is DirectoryNotFoundException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(DirectoryNotFoundException).FullName);
                if (exception is DriveNotFoundException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(DriveNotFoundException).FullName);
                if (exception is EndOfStreamException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(EndOfStreamException).FullName);
                if (exception is PathTooLongException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(PathTooLongException).FullName);
                return System.Xml.XmlConvert.EncodeLocalName(typeof(IOException).FullName);
            }
            if (exception is KeyNotFoundException)
                return System.Xml.XmlConvert.EncodeLocalName(typeof(KeyNotFoundException).FullName);
            if (exception is IndexOutOfRangeException)
                return System.Xml.XmlConvert.EncodeLocalName(typeof(IndexOutOfRangeException).FullName);
            if (exception is FormatException)
            {
                if (exception is UriFormatException)
                    return System.Xml.XmlConvert.EncodeLocalName(typeof(UriFormatException).FullName);
                return System.Xml.XmlConvert.EncodeLocalName(typeof(FormatException).FullName);
            }
            if (exception is InvalidOperationException)
                return System.Xml.XmlConvert.EncodeLocalName(typeof(InvalidOperationException).FullName);
            if (exception is AggregateException)
                return System.Xml.XmlConvert.EncodeLocalName(typeof(AggregateException).FullName);
            return System.Xml.XmlConvert.EncodeLocalName(typeof(Exception).FullName);
        }

        public static XName GetDefaultResultElementName(this Exception exception)
        {
            if (exception is null)
                return nameof(Exception);
            if (exception is ArgumentException)
            {
                if (exception is ArgumentOutOfRangeException)
                    return nameof(ArgumentOutOfRangeException);
                if (exception is ArgumentNullException)
                    return nameof(ArgumentNullException);
                return nameof(ArgumentException);
            }
            if (exception is System.Runtime.InteropServices.ExternalException)
            {
                if (exception is System.Data.Common.DbException)
                {
                    if (exception is System.Data.SqlClient.SqlException)
                        return nameof(System.Data.SqlClient.SqlException);
                    return nameof(System.Data.Common.DbException);
                }
                return nameof(System.Runtime.InteropServices.ExternalException);
            }
            if (exception is System.Xml.XmlException)
                return nameof(System.Xml.XmlException);
            if (exception is IOException)
            {
                if (exception is FileNotFoundException)
                    return nameof(FileNotFoundException);
                if (exception is DirectoryNotFoundException)
                    return nameof(DirectoryNotFoundException);
                if (exception is DriveNotFoundException)
                    return nameof(DriveNotFoundException);
                if (exception is EndOfStreamException)
                    return nameof(EndOfStreamException);
                if (exception is PathTooLongException)
                    return nameof(PathTooLongException);
                return nameof(IOException);
            }
            if (exception is KeyNotFoundException)
                return nameof(KeyNotFoundException);
            if (exception is IndexOutOfRangeException)
                return nameof(IndexOutOfRangeException);
            if (exception is FormatException)
            {
                if (exception is UriFormatException)
                    return nameof(UriFormatException);
                return nameof(FormatException);
            }
            if (exception is InvalidOperationException)
                return nameof(InvalidOperationException);
            if (exception is AggregateException)
                return nameof(AggregateException);
            return nameof(Exception);
        }

        /// <summary>
        /// Create result information for unit testing.
        /// </summary>
        /// <param name="exception">The target exception</param>
        /// <param name="detailLevel">Determines which properties to add to the response element.</param>
        /// <param name="elementName">The result element name. Can be <see langword="null"/> to use the default name.</param>
        /// <param name="includeMessage">Where to include the exception message.</param>
        /// <param name="innerExceptionDepth">Recursion level for inner exceptions to include.</param>
        /// <returns>The <seealso cref="XElement"/> representing the specified <paramref name="exception"/>.</returns>
        /// <remarks>Values for <paramref name="detailLevel"/>:
        /// <list type="bullet">
        /// <item><term>0</term> Adds attributes only for <seealso cref="SqlException.State"/> and <seealso cref="SqlException.Number"/>.</item>
        /// <item><term>1</term> Includes attributes for <seealso cref="SqlException.Class"/> and <seealso cref="ExternalException.ErrorCode"/>.</item>
        /// <item><term>2</term> Includes element for <seealso cref="SqlException.Procedure"/>.</item>
        /// <item><term>3</term> Includes attribute for <seealso cref="SqlException.LineNumber"/> and element for <seealso cref="SqlException.Source"/>.</item>
        /// <item><term>4 (or greater)</term> Includes attribute for <seealso cref="SqlException.ClientConnectionId"/> and elements for
        /// <seealso cref="SqlException.Server"/>.</item>
        /// </list></remarks>
        public static XElement CreateExceptionResult(this System.Data.SqlClient.SqlException exception, int detailLevel = 0, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
        {
            if (detailLevel == 0)
                return CreateExceptionResult(exception, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
            return CreateExceptionResult(exception, (x, e) => ApplyExceptionProperties(x, e, detailLevel), elementName, includeMessage, innerExceptionDepth);
        }

        /// <summary>
        /// Adds test result information to a response xml element.
        /// </summary>
        /// <param name="element">The target response xml element.</param>
        /// <param name="sqlException">The exception to apply.</param>
        /// <param name="detailLevel">Determines which properties to add to the response <paramref name="element"/>.</param>
        /// <remarks>Values for <paramref name="detailLevel"/>:
        /// <list type="bullet">
        /// <item><term>0 (or less)</term> Adds attributes only for <seealso cref="SqlException.State"/> and <seealso cref="SqlException.Number"/>.</item>
        /// <item><term>1</term> Includes attributes for <seealso cref="SqlException.Class"/> and <seealso cref="ExternalException.ErrorCode"/>.</item>
        /// <item><term>2</term> Includes element for <seealso cref="SqlException.Procedure"/>.</item>
        /// <item><term>3</term> Includes attribute for <seealso cref="SqlException.LineNumber"/> and element for <seealso cref="SqlException.Source"/>.</item>
        /// <item><term>4 (or greater)</term> Includes attribute for <seealso cref="SqlException.ClientConnectionId"/> and element for
        /// <seealso cref="SqlException.Server"/>.</item>
        /// </list></remarks>
        public static void ApplyExceptionProperties(XElement element, System.Data.SqlClient.SqlException sqlException, int detailLevel = 0)
        {
            element.SetAttributeValue(nameof(sqlException.State), sqlException.State);
            element.SetAttributeValue(nameof(sqlException.Number), sqlException.Number);
            if (detailLevel > 0)
            {
                element.SetAttributeValue(nameof(sqlException.Class), sqlException.Class);
                ApplyExceptionProperties(element, (System.Runtime.InteropServices.ExternalException)sqlException);
            }
            if (detailLevel > 2)
                element.SetAttributeValue(nameof(sqlException.LineNumber), sqlException.LineNumber);
            if (detailLevel > 3)
                element.SetAttributeValue(nameof(sqlException.ClientConnectionId), sqlException.ClientConnectionId);
            if (detailLevel > 2)
                element.Add(CreateTestResult(sqlException.Source, nameof(sqlException.Source)));
            if (detailLevel > 1)
                element.Add(CreateTestResult(sqlException.Procedure, nameof(sqlException.Procedure)));
            if (detailLevel > 3)
                element.Add(CreateTestResult(sqlException.Server, nameof(sqlException.Server)));
        }

        public static XElement CreateExceptionResult(this System.Runtime.InteropServices.ExternalException exception, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
        {
            return CreateExceptionResult(exception, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
        }

        public static void ApplyExceptionProperties(XElement element, System.Runtime.InteropServices.ExternalException externalException)
        {
            element.SetAttributeValue(nameof(externalException.ErrorCode), externalException.ErrorCode);
        }

        public static XElement CreateExceptionResult(this System.Xml.XmlException exception, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
        {
            return CreateExceptionResult(exception, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
        }

        public static void ApplyExceptionProperties(XElement element, System.Xml.XmlException xmlException)
        {
            element.SetAttributeValue(nameof(xmlException.LineNumber), xmlException.LineNumber);
            element.SetAttributeValue(nameof(xmlException.LinePosition), xmlException.LinePosition);
            element.AppendElement(xmlException.Source, nameof(xmlException.Source));
        }

        public static XElement CreateExceptionResult(this System.IO.FileNotFoundException exception, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
        {
            return CreateExceptionResult(exception, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
        }

        public static void ApplyExceptionProperties(XElement element, System.IO.FileNotFoundException fileNotFoundException)
        {
            element.AppendElement(fileNotFoundException.FileName, nameof(fileNotFoundException.FileName));
        }

        public static XElement CreateExceptionResult(this ArgumentException exception, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
        {
            return CreateExceptionResult(exception, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
        }

        public static void ApplyExceptionProperties(XElement element, ArgumentException argumentException)
        {
            if (!(argumentException.ParamName is null))
                element.SetAttributeValue(nameof(argumentException.ParamName), argumentException.ParamName);
        }

        /// <summary>
        /// Creates an <seealso cref="XElement"/> object named after an <seealso cref="Exception"/> type for use with data driven unit testing.
        /// </summary>
        /// <param name="exception">The target exception object.</param>
        /// <returns>A new <seealso cref="XElement"/> with the name based off of the exception type.</returns>
        public static XElement CreateExceptionResultElement(this Exception exception, XName elementName = null)
        {
            if (exception is null)
                return CreateNullResult(elementName ?? nameof(Exception));
            XElement result = new XElement(elementName ?? exception.GetDefaultResultElementName());
            return result;
        }

        public static XElement CreateExceptionResult<E>(this E exception, Action<XElement, E> setResultContent, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0)
            where E : Exception
        {
            if (exception is null)
                return CreateNullResult(elementName ?? nameof(Exception));

            XElement result = (elementName is null) ? CreateExceptionResultElement(exception) : new XElement(elementName);
            if (includeMessage)
                result.Add((exception.Message is null) ? new XElement(nameof(exception.Message)) : new XElement(nameof(exception.Message), exception.Message));
            setResultContent?.Invoke(result, exception);
            if (innerExceptionDepth > 0)
            {
                elementName = nameof(exception.InnerException);
                if (--innerExceptionDepth == 0)
                    foreach (Exception i in exception.GetAllInnerExceptions())
                        result.Add(new XElement(elementName, new XAttribute(nameof(Type.Name), i.GetDefaultResultElementName())));
                else
                    foreach (Exception i in exception.GetAllInnerExceptions())
                    {
                        XElement e = i.CreateExceptionResult(elementName, false, innerExceptionDepth, true);
                        e.SetAttributeValue(nameof(Type.Name), i.GetDefaultResultElementName());
                        result.Add(e);
                    }
            }
            return result;
        }

        public static XElement CreateExceptionResult(this Exception exception, XName elementName = null, bool includeMessage = false, int innerExceptionDepth = 0,
            bool doNotAddAdditionalProperties = false)
        {
            if (exception is null)
                return CreateNullResult(elementName ?? nameof(Exception));
            if (!doNotAddAdditionalProperties)
            {
                if (exception is ArgumentException argumentException)
                    return CreateExceptionResult(argumentException, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
                else if (exception is System.IO.FileNotFoundException fileNotFoundException)
                    return CreateExceptionResult(fileNotFoundException, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
                else if (exception is System.Xml.XmlException xmlException)
                    return CreateExceptionResult(xmlException, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
                else if (exception is System.Data.SqlClient.SqlException sqlException)
                    return CreateExceptionResult(sqlException, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
                else if (exception is System.Runtime.InteropServices.ExternalException externalException)
                    return CreateExceptionResult(externalException, ApplyExceptionProperties, elementName, includeMessage, innerExceptionDepth);
            }
            return CreateExceptionResult(exception, null, elementName, includeMessage, innerExceptionDepth);
        }

        public static XElement CreateTestResult(string value, XName elementName = null)
        {
            if (value is null)
                return CreateNullResult(elementName ?? nameof(value));
            return new XElement(elementName ?? nameof(value), value);
        }

        public static XElement CreateTestResult(this Capture capture, bool includePosition = false, XName elementName = null)
        {
            if (capture is null)
                return CreateNullResult(elementName ?? nameof(Capture));
            if (capture is Group group)
            {
                if (group.Success)
                {
                    if (includePosition)
                        return CreateRegexResult(elementName ?? ((group is Match) ? nameof(Match) : nameof(Group)), true, group.Value, group.Index, group.Length);
                    return CreateRegexResult(elementName ?? ((group is Match) ? nameof(Match) : nameof(Group)), true, group.Value);
                }
                return CreateRegexResult(elementName ?? ((group is Match) ? nameof(Match) : nameof(Group)), false);
            }
            if (includePosition)
                return CreateRegexResult(elementName ?? nameof(Capture), true, capture.Value, capture.Index, capture.Length);
            return CreateRegexResult(elementName ?? nameof(Capture), true, capture.Value);
        }

        public static XElement CreateTestResultWithGroups(this Match match, bool excludeValue, bool includePosition, XName elementName, params string[] explicitGroupNames)
        {
            if (match is null)
                return CreateNullResult(elementName ?? nameof(Match));
            if (match.Success)
            {
                if (excludeValue)
                {
                    if (includePosition)
                        return CreateRegexResult(elementName ?? nameof(Match), true, match.Index, match.Length).Append(explicitGroupNames.Select(n =>
                            match.Groups[n]).Where(g => !(g is null)).Select(g => (g.Success) ?
                            CreateRegexResult(elementName ?? nameof(Group), g.Name, true, g.Index, g.Length) : CreateRegexResult(elementName ?? nameof(Group), g.Name, false)).ToArray());
                    return CreateRegexResult(elementName ?? nameof(Match), true).Append(explicitGroupNames.Select(n =>
                        match.Groups[n]).Where(g => !(g is null)).Select(g => (g.Success) ?
                        CreateRegexResult(elementName ?? nameof(Group), g.Name, true) : CreateRegexResult(elementName ?? nameof(Group), g.Name, false)).ToArray());
                }
                if (includePosition)
                    return CreateRegexResult(elementName ?? nameof(Match), true, match.Index, match.Length).Append(explicitGroupNames.Select(n =>
                        match.Groups[n]).Where(g => !(g is null)).Select(g => (g.Success) ?
                        CreateRegexResult(elementName ?? nameof(Group), g.Name, true, g.Value, g.Index, g.Length) : CreateRegexResult(elementName ?? nameof(Group), g.Name, false)).ToArray());
                return CreateRegexResult(elementName ?? nameof(Match), true).Append(explicitGroupNames.Select(n =>
                    match.Groups[n]).Where(g => !(g is null)).Select(g => (g.Success) ?
                    CreateRegexResult(elementName ?? nameof(Group), g.Name, true, g.Value) : CreateRegexResult(elementName ?? nameof(Group), g.Name, false)).ToArray());
            }
            return CreateRegexResult(elementName ?? nameof(Match), false);
        }

        public static XElement CreateTestResultWithGroups(this Match match, bool excludeValue, bool includePosition, params string[] explicitGroupNames) =>
            CreateTestResultWithGroups(match, excludeValue, includePosition, null, explicitGroupNames);

        public static XElement CreateTestResultWithGroups(this Match match, bool excludeValue, XName elementName, params string[] explicitGroupNames) =>
            CreateTestResultWithGroups(match, excludeValue, false, elementName, explicitGroupNames);

        public static XElement CreateTestResultWithGroups(this Match match, bool excludeValue, params string[] explicitGroupNames) =>
            CreateTestResultWithGroups(match, excludeValue, false, explicitGroupNames);

        public static XElement CreateTestResultWithGroups(this Match match, XName elementName, params string[] explicitGroupNames) =>
            CreateTestResultWithGroups(match, false, elementName, explicitGroupNames);

        public static XElement CreateTestResultWithGroups(this Match match, params string[] explicitGroupNames) =>
            CreateTestResultWithGroups(match, false, explicitGroupNames);

        public static XElement AppendMatchGroupResult(this XElement target, string groupName, bool success) =>
            target.Append(CreateRegexResult(nameof(Group), groupName, success));

        public static XElement AppendMatchGroupResult(this XElement target, string groupName, string value) =>
            target.Append((value is null) ? CreateRegexResult(nameof(Group), groupName, false) : CreateRegexResult(nameof(Group), groupName, true).ApplyValue(value));

        public static XElement AppendMatchGroupResult(this XElement target, Group group, bool excludeValue = false, bool includePosition = false, XName elementName = null) =>
            target.Append((group is null) ? CreateNullResult(elementName ?? nameof(Group)) : (excludeValue ?
                (includePosition ?
                    CreateRegexResult(elementName ?? nameof(Group), group.Name, group.Success, group.Index, group.Length) :
                    CreateRegexResult(elementName ?? nameof(Group), group.Name, group.Success)) :
                (includePosition ?
                    CreateRegexResult(elementName ?? nameof(Group), group.Name, group.Success, group.Value, group.Index, group.Length) :
                    CreateRegexResult(elementName ?? nameof(Group), group.Name, group.Success, group.Value))));

        public static XElement AppendMatchGroupResult(this XElement target, Group group, bool excludeValue = false, bool includePosition = false) =>
            AppendMatchGroupResult(target, group, excludeValue, includePosition, null);

        public static XElement AppendMatchGroupFailed(this XElement target, string groupName) => target.AppendMatchGroupResult(groupName, false);

    }
}
