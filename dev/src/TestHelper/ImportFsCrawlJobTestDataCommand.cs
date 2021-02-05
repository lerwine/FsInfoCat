using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using Microsoft.PowerShell.Commands;

namespace TestHelper
{
    [Cmdlet(VerbsData.Import,"FsCrawlJobTestData")]
    [OutputType(typeof(FsCrawlJobTestData))]
    public class ImportFsCrawlJobTestDataCommand : PSCmdlet
    {
        public const string PARAMETER_SET_NAME_PATH = "Path";
        public const string PARAMETER_SET_NAME_LITERALPATH = "LiteralPath";
        public const string PARAMETER_SET_NAME_XML = "XML";
        public const string ERROR_ID_PATH_NOT_FOUND = "PathNotFound";
        public const string ERROR_ID_NOT_SUPPORTED = "NotSupported";
        public const string ERROR_ID_PROVIDER_ERROR = "ProviderError";
        public const string ERROR_ID_OPEN_ERROR = "OpenError";
        public const string ERROR_ID_READ_ERROR = "ReadError";
        public const string ERROR_ID_CLOSE_ERROR = "CloseError";
        private const string ACTIVITY_NAME = "Import FSCrawlJobTestData";
        private const string STATUS_DESCRIPTION_RESOLVING_PATHS = "";
        private const string STATUS_DESCRIPTION_OPENING_ITEM = "";
        private const string STATUS_DESCRIPTION_READING_ITEM = "";
        private XmlSerializer _serializer;
        private bool _isLiteralPaths;
        private bool _isXml;
        private string[] _fileSystemProviders = new string[0];
        List<FileToProcess> _filesToProcess = new List<FileToProcess>();

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_PATH)]
        [Alias("FullName")]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_LITERALPATH)]
        [Alias("PSPath")]
        [ValidateNotNullOrEmpty]

        public string[] LiteralPath { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = PARAMETER_SET_NAME_XML)]
        [Alias("DocumentElement")]
        public XmlNode XML { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_PATH)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_LITERALPATH)]
        public SwitchParameter Force { get; set; }

        protected override void BeginProcessing()
        {
            Type t = typeof(FileSystemProvider);
            _fileSystemProviders = SessionState.Provider.GetAll().Where(p => t.IsAssignableFrom(p.ImplementingType)).Select(p => p.Name).ToArray();
            _serializer = new XmlSerializer(typeof(FsCrawlJobTestData));
            _isLiteralPaths = String.Equals(ParameterSetName, PARAMETER_SET_NAME_LITERALPATH, StringComparison.InvariantCultureIgnoreCase);
            _isXml = !_isLiteralPaths && String.Equals(ParameterSetName, PARAMETER_SET_NAME_XML, StringComparison.InvariantCultureIgnoreCase);
        }

        private FileToProcess ToFileToProcess(string path)
        {
            try
            {
                Collection<PathInfo> pathInfoCollection = SessionState.Path.GetResolvedPSPathFromPSPath(path);
                if (pathInfoCollection.Count == 1)
                {
                    PathInfo pathInfo = pathInfoCollection[0];
                    if (null != pathInfo.Provider && _fileSystemProviders.Contains(pathInfo.Provider.Name, StringComparer.InvariantCultureIgnoreCase))
                        try
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(pathInfo.ProviderPath);
                            if (fileInfo.Exists)
                                return new FileToProcess(fileInfo, (pathInfo.Provider is null) ? "" : pathInfo.Provider.Name, pathInfo.Path);
                        }
                        catch { /* Ignore */ }
                    return new FileToProcess(pathInfo);
                }
            }
            catch { /* Ignore */ }
            try
            {
                Collection<string> providerPath = GetResolvedProviderPathFromPSPath(path, out ProviderInfo provider);
                if (providerPath.Count == 1)
                    return new FileToProcess(path, providerPath[0], (provider is null) ? "" : provider.Name);
            }
            catch { /* Ignore */ }

            return new FileToProcess(path, GetUnresolvedProviderPathFromPSPath(path), "");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            if (_isXml)
            {
                return;
            }
            if (_isLiteralPaths)
            {
                foreach (string path in LiteralPath)
                {
                    WriteProgress(new ProgressRecord(0, ACTIVITY_NAME, STATUS_DESCRIPTION_RESOLVING_PATHS));
                    try
                    {
                        _filesToProcess.Add(ToFileToProcess(path));
                    }
                    catch (NotSupportedException exc)
                    {
                        ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_NOT_SUPPORTED, ErrorCategory.NotImplemented, path);
                        errorRecord.ErrorDetails = new ErrorDetails("Path references an unsupported provider type: " + path);
                        ThrowTerminatingError(errorRecord);
                    }
                    catch (Exception exc)
                    {
                        ErrorRecord errorRecord;
                        if (exc is ItemNotFoundException || exc is DriveNotFoundException || exc is ProviderNotFoundException)
                        {
                            errorRecord = new ErrorRecord(exc, ERROR_ID_PATH_NOT_FOUND, ErrorCategory.ObjectNotFound, path);
                            errorRecord.ErrorDetails = new ErrorDetails("Item referenced by path could not be found: " + path);
                        }
                        else
                        {
                            errorRecord = new ErrorRecord(exc, ERROR_ID_PROVIDER_ERROR, ErrorCategory.InvalidOperation, path);
                            if (string.IsNullOrWhiteSpace(exc.Message))
                                errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to resolve path \"" + path + "\".");
                            else
                                errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to resolve path \"" + path + "\": " + exc.Message);
                        }
                        ThrowTerminatingError(errorRecord);
                    }
                }
            }
            else
            {
                foreach (string path in Path)
                {
                    WriteProgress(new ProgressRecord(0, ACTIVITY_NAME, STATUS_DESCRIPTION_RESOLVING_PATHS));
                    try
                    {
                        foreach (PathInfo pathInfo in SessionState.Path.GetResolvedPSPathFromPSPath(path))
                        {
                            if (null != pathInfo.Provider && _fileSystemProviders.Contains(pathInfo.Provider.Name, StringComparer.InvariantCultureIgnoreCase))
                            {
                                try
                                {
                                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(pathInfo.ProviderPath);
                                    if (fileInfo.Exists)
                                    {
                                        _filesToProcess.Add(new FileToProcess(fileInfo, pathInfo.Provider.Name, pathInfo.Path));
                                        continue;
                                    }
                                }
                                catch { /* Okay to ignore */ }
                            }
                            _filesToProcess.Add(new FileToProcess(pathInfo));
                        }
                    }
                    catch (NotSupportedException exc)
                    {
                        ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_NOT_SUPPORTED, ErrorCategory.NotImplemented, path);
                        errorRecord.ErrorDetails = new ErrorDetails("Path references an unsupported provider type: " + path);
                        ThrowTerminatingError(errorRecord);
                    }
                    catch (ItemNotFoundException exc)
                    {
                        try
                        {
                            _filesToProcess.Add(ToFileToProcess(path));
                        }
                        catch
                        {
                            ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_PATH_NOT_FOUND, ErrorCategory.ObjectNotFound, path);
                            errorRecord.ErrorDetails = new ErrorDetails("Item referenced by path could not be found: " + path);
                            ThrowTerminatingError(errorRecord);
                        }
                    }
                    catch (Exception exc)
                    {
                        ErrorRecord errorRecord;
                        if (exc is DriveNotFoundException || exc is ProviderNotFoundException)
                        {
                            errorRecord = new ErrorRecord(exc, ERROR_ID_PATH_NOT_FOUND, ErrorCategory.ObjectNotFound, path);
                            errorRecord.ErrorDetails = new ErrorDetails("Item referenced by path could not be found: " + path);
                        }
                        else
                        {
                            errorRecord = new ErrorRecord(exc, ERROR_ID_PROVIDER_ERROR, ErrorCategory.InvalidOperation, path);
                            if (string.IsNullOrWhiteSpace(exc.Message))
                                errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to resolve path \"" + path + "\".");
                            else
                                errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to resolve path \"" + path + "\": " + exc.Message);
                        }
                        ThrowTerminatingError(errorRecord);
                    }
                }
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            int totalCount = _filesToProcess.Count;
            if (totalCount == 0)
                return;
            try
            {
                WriteProgress(new ProgressRecord(0, ACTIVITY_NAME, STATUS_DESCRIPTION_OPENING_ITEM));

                for (int index = 0; index < _filesToProcess.Count; index++)
                {
                    WriteProgress(new ProgressRecord(0, ACTIVITY_NAME, STATUS_DESCRIPTION_OPENING_ITEM) { CurrentOperation = _filesToProcess[index].ProviderPath, PercentComplete = (index * 100) / totalCount });
                    FileToProcess fileToProcess = _filesToProcess[index];
                    if (fileToProcess.FileInfo is null)
                        ProcessOtherProviderPath(_filesToProcess[index]);
                    else
                        ProcessFileItem(_filesToProcess[index].FileInfo);
                }
            }
            finally
            {
                WriteProgress(new ProgressRecord(0, ACTIVITY_NAME, "Completed") { RecordType = ProgressRecordType.Completed });
            }
        }

        private void ProcessFileItem(System.IO.FileInfo fileInfo)
        {
            XmlReader reader;
            try
            {
                reader = XmlReader.Create(fileInfo.FullName);
            }
            catch (Exception exc)
            {
                ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_OPEN_ERROR, ErrorCategory.InvalidOperation, fileInfo.FullName);
                if (string.IsNullOrWhiteSpace(exc.Message))
                    errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while opening \"" + fileInfo.FullName + "\".");
                else
                    errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while opening \"" + fileInfo.FullName + "\": " + exc.Message);
                ThrowTerminatingError(errorRecord);
                return;
            }
            using (reader)
            {
                try
                {
                    WriteObject((FsCrawlJobTestData)_serializer.Deserialize(reader));
                }
                catch (Exception exc)
                {
                    ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_READ_ERROR, ErrorCategory.ReadError, fileInfo.FullName);
                    if (string.IsNullOrWhiteSpace(exc.Message))
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while reading \"" + fileInfo.FullName + "\".");
                    else
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while reading \"" + fileInfo.FullName + "\": " + exc.Message);
                    ThrowTerminatingError(errorRecord);
                }
            }
        }

        private void ProcessOtherProviderPath(FileToProcess fileToProcess)
        {
            Collection<IContentReader> readerCollection;
            try
            {
                readerCollection = SessionState.InvokeProvider.Content.GetReader(fileToProcess.ProviderPath);
            }
            catch (NotSupportedException exc)
            {
                ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_OPEN_ERROR, ErrorCategory.NotImplemented, fileToProcess.MSHPath);
                errorRecord.ErrorDetails = new ErrorDetails("Path references an unsupported provider type: " + fileToProcess.MSHPath);
                ThrowTerminatingError(errorRecord);
                return;
            }
            catch (Exception exc)
            {
                ErrorRecord errorRecord;
                if (exc is ItemNotFoundException || exc is DriveNotFoundException || exc is ProviderNotFoundException)
                {
                    errorRecord = new ErrorRecord(exc, ERROR_ID_OPEN_ERROR, ErrorCategory.ObjectNotFound, fileToProcess.MSHPath);
                    errorRecord.ErrorDetails = new ErrorDetails("Item referenced by path could not be found: " + fileToProcess.ProviderPath);
                }
                else
                {
                    errorRecord = new ErrorRecord(exc, ERROR_ID_OPEN_ERROR, ErrorCategory.InvalidOperation, fileToProcess.MSHPath);
                    if (string.IsNullOrWhiteSpace(exc.Message))
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to open file \"" + fileToProcess.ProviderPath + "\".");
                    else
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while trying to open file \"" + fileToProcess.ProviderPath + "\": " + exc.Message);
                }
                ThrowTerminatingError(errorRecord);
                return;
            }

            try
            {
                while (readerCollection.Count > 0)
                {
                    IContentReader r = readerCollection[0];
                    try
                    {
                        readerCollection.RemoveAt(0);
                        using (System.IO.TextReader reader = ConvertToTextReader(r))
                            ImportFromTextReader(reader, fileToProcess.ProviderPath);
                    }
                    finally
                    {
                        try { r.Dispose(); } catch { /* Okay to ignore */ }
                    }
                }
            }
            finally
            {
                foreach (IContentReader r in readerCollection)
                    try { r.Dispose(); } catch { /* okay to ignore */ }
            }
        }

        private void ImportFromTextReader(System.IO.TextReader textReader, string path)
        {
            XmlReader xmlReader;
            try
            {
                xmlReader = XmlReader.Create(textReader);
            }
            catch (Exception exc)
            {
                try { textReader.Dispose(); } catch { /* Already handling an exception */ }
                ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_OPEN_ERROR, ErrorCategory.OpenError, path);
                if (string.IsNullOrWhiteSpace(exc.Message))
                    errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while opening \"" + path + "\".");
                else
                    errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while opening \"" + path + "\": " + exc.Message);
                ThrowTerminatingError(errorRecord);
                return;
            }
            using (xmlReader)
            {
                try
                {
                    WriteObject((FsCrawlJobTestData)_serializer.Deserialize(xmlReader));
                }
                catch (Exception exc)
                {
                    ErrorRecord errorRecord = new ErrorRecord(exc, ERROR_ID_READ_ERROR, ErrorCategory.ReadError, path);
                    if (string.IsNullOrWhiteSpace(exc.Message))
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while reading \"" + path + "\".");
                    else
                        errorRecord.ErrorDetails = new ErrorDetails("Unexpected error while reading \"" + path + "\": " + exc.Message);
                    ThrowTerminatingError(errorRecord);
                }
            }
        }

        private System.IO.TextReader ConvertToTextReader(IContentReader reader)
        {
            IList block = reader.Read(1L);
            if (block is null || block.Count == 0 || block[0] is null)
                return new System.IO.StringReader("");
            object o = block[0];
            if (o is PSObject)
                o = ((PSObject)o).BaseObject;
            if (o is char || o is char[] || (o is IEnumerable<char> && !(o is string)))
                return ReadAsChars(block, reader);
            if (o is byte || o is byte[] || o is IEnumerable<byte>)
                return ReadAsBytes(block, reader);
            return ReadAsStrings(block, reader);
        }

        private System.IO.TextReader ReadAsStrings(IList block, IContentReader reader)
        {
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                do
                {
                    if (block is string[])
                    {
                        foreach (string line in (string[])block)
                            sw.WriteLine(line);
                    }
                    else
                    {
                        foreach (object obj in block)
                        {
                            if (obj is null)
                                continue;
                            if (obj is PSObject)
                            {
                                object o = ((PSObject)obj).BaseObject;
                                if (o is string)
                                    sw.WriteLine((string)o);
                                else
                                    sw.WriteLine(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                            }
                            else if (obj is string)
                                sw.WriteLine((string)obj);
                            else
                                sw.WriteLine(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                        }
                    }
                } while (null != (block = reader.Read(1024L)) && block.Count > 0);
                sw.Flush();
                return new System.IO.StringReader(sw.ToString());
            }
        }

        private System.IO.TextReader ReadAsChars(IList block, IContentReader reader)
        {
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                do
                {
                    if (block is char[])
                    {
                        char[] buffer = (char[])block;
                        sw.Write(buffer, 0, buffer.Length);
                    }
                    else if (block is IEnumerable<char>)
                    {
                        char[] buffer = ((IEnumerable<char>)block).ToArray();
                        sw.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        foreach (object obj in block)
                        {
                            if (obj is null)
                                continue;
                            if (obj is PSObject)
                            {
                                object o = ((PSObject)obj).BaseObject;
                                if (o is char)
                                    sw.Write((char)o);
                                else if (o is string)
                                    sw.Write((string)o);
                                else if (o is char[])
                                {
                                    char[] buffer = (char[])o;
                                    if (buffer.Length > 0)
                                        sw.Write(buffer, 0, buffer.Length);
                                }
                                if (o is IEnumerable<char>)
                                {
                                    char[] buffer = ((IEnumerable<char>)o).ToArray();
                                    if (buffer.Length > 0)
                                        sw.Write(buffer, 0, buffer.Length);
                                }
                                else
                                    sw.Write(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                            }
                            else if (obj is char)
                                sw.Write((char)obj);
                            else if (obj is string)
                                sw.Write((string)obj);
                            else if (obj is char[])
                            {
                                char[] buffer = (char[])obj;
                                if (buffer.Length > 0)
                                    sw.Write(buffer, 0, buffer.Length);
                            }
                            if (obj is IEnumerable<char>)
                            {
                                char[] buffer = ((IEnumerable<char>)obj).ToArray();
                                if (buffer.Length > 0)
                                    sw.Write(buffer, 0, buffer.Length);
                            }
                            else
                                sw.Write(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                        }
                    }
                } while (null != (block = reader.Read(32768L)) && block.Count > 0);
                sw.Flush();
                return new System.IO.StringReader(sw.ToString());
            }
        }

        private System.IO.TextReader ReadAsBytes(IList block, IContentReader reader)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            try
            {
                do
                {
                    if (block is byte[])
                    {
                        byte[] buffer = (byte[])block;
                        memoryStream.Write(buffer, 0, buffer.Length);
                    }
                    else if (block is IEnumerable<byte>)
                    {
                        byte[] buffer = ((IEnumerable<byte>)block).ToArray();
                        memoryStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(memoryStream, new UTF8Encoding(false, false), 32768, true))
                        {
                            foreach (object obj in block)
                            {
                                if (obj is null)
                                    continue;
                                if (obj is PSObject)
                                {
                                    object o = ((PSObject)obj).BaseObject;
                                    if (o is byte)
                                        sw.Write((byte)o);
                                    else if (o is byte[])
                                    {
                                        byte[] buffer = (byte[])o;
                                        if (buffer.Length > 0)
                                            memoryStream.Write(buffer, 0, buffer.Length);
                                    }
                                    if (o is IEnumerable<byte>)
                                    {
                                        byte[] buffer = ((IEnumerable<byte>)o).ToArray();
                                        if (buffer.Length > 0)
                                            memoryStream.Write(buffer, 0, buffer.Length);
                                    }
                                    else if (o is string)
                                        sw.Write((string)o);
                                    else
                                        sw.Write(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                                }
                                else if (obj is byte)
                                    sw.Write((byte)obj);
                                else if (obj is byte[])
                                {
                                    byte[] buffer = (byte[])obj;
                                    if (buffer.Length > 0)
                                        memoryStream.Write(buffer, 0, buffer.Length);
                                }
                                if (obj is IEnumerable<byte>)
                                {
                                    byte[] buffer = ((IEnumerable<byte>)obj).ToArray();
                                    if (buffer.Length > 0)
                                        memoryStream.Write(buffer, 0, buffer.Length);
                                }
                                else if (obj is string)
                                    sw.Write((string)obj);
                                else
                                    sw.Write(((LanguagePrimitives.TryConvertTo<string>(obj, out string result))) ? result : obj.ToString());
                            }
                        }
                    }
                } while (null != (block = reader.Read(32768L)) && block.Count > 0);
                memoryStream.Seek(0L, System.IO.SeekOrigin.Begin);
                return new System.IO.StreamReader(memoryStream);
            }
            catch
            {
                memoryStream.Dispose();
                throw;
            }
        }

        class FileToProcess
        {
            internal System.IO.FileInfo FileInfo { get; }
            internal string ProviderPath { get; }
            internal string MSHPath { get; }
            internal string ProviderName { get; }

            internal FileToProcess(System.IO.FileInfo fileInfo, string providerName, string mshPath)
            {
                FileInfo = null;
                ProviderPath = fileInfo.FullName;
                MSHPath = mshPath;
                ProviderName = (providerName is null) ? "" : providerName;
            }

            internal FileToProcess(PathInfo pathInfo)
            {
                FileInfo = null;
                ProviderPath = pathInfo.ProviderPath;
                MSHPath = pathInfo.Path;
                ProviderName = (pathInfo.Provider is null || pathInfo.Provider.Name is null) ? "" : pathInfo.Provider.Name;
            }

            internal FileToProcess(string path, string providerPath, string providerName)
            {
                FileInfo = null;
                ProviderPath = providerPath;
                MSHPath = path;
                ProviderName = providerName;
            }
        }

        // private Collection<PathInfo> ResolvePath(string path)
        // {
        //     try
        //     {
        //         return SessionState.Path.GetResolvedPSPathFromPSPath(path);
        //     }
        //     catch (ItemNotFoundException)
        //     {
        //         try
        //         {
        //             Collection<PathInfo> result = new Collection<PathInfo>();
        //             result.Add(ResolveLiteralPath(path));
        //             return result;
        //         }
        //         catch { /* Ignoring so we can re-throw the original exception */ }
        //         throw;
        //     }
        // }

        // private PathInfo ResolveLiteralPath(string path)
        // {
        //     try
        //     {
        //         return new ResolvedPathInfo(SessionState.Path.GetUnresolvedProviderPathFromPSPath(path, out ProviderInfo provider, out PSDriveInfo drive), provider, drive);
        //     }
        //     catch (ItemNotFoundException)
        //     {
        //         try { return ResolveLiteralPath(path); } catch { /* Ignoring so we can re-throw the original exception */ }
        //         throw;
        //     }
        // }

        // /// <summary>
        // /// A struct to hold the path information and the content readers/writers
        // /// for an item.
        // /// </summary>
        // internal readonly struct ContentHolder
        // {
        //     internal ContentHolder(
        //         PathInfo pathInfo,
        //         IContentReader reader,
        //         IContentWriter writer)
        //     {
        //         if (pathInfo == null)
        //         {
        //             throw PSTraceSource.NewArgumentNullException(nameof(pathInfo));
        //         }

        //         PathInfo = pathInfo;
        //         Reader = reader;
        //         Writer = writer;
        //     }

        //     internal PathInfo PathInfo { get; }

        //     internal IContentReader Reader { get; }

        //     internal IContentWriter Writer { get; }
        // }

        // /// <summary>
        // /// Gets the IContentReaders for the current path(s)
        // /// </summary>
        // /// <returns>
        // /// An array of IContentReaders for the current path(s)
        // /// </returns>
        // internal List<ContentHolder> GetContentReaders(
        //     string[] readerPaths,
        //     CmdletProviderContext currentCommandContext)
        // {
        //     // Resolve all the paths into PathInfo objects

        //     Collection<PathInfo> pathInfos = ResolvePaths(readerPaths, false, true, currentCommandContext);

        //     // Create the results array

        //     List<ContentHolder> results = new List<ContentHolder>();

        //     foreach (PathInfo pathInfo in pathInfos)
        //     {
        //         // For each path, get the content writer

        //         Collection<IContentReader> readers = null;

        //         try
        //         {
        //             string pathToProcess = WildcardPattern.Escape(pathInfo.Path);

        //             if (currentCommandContext.SuppressWildcardExpansion)
        //             {
        //                 pathToProcess = pathInfo.Path;
        //             }

        //             readers =
        //                 InvokeProvider.Content.GetReader(pathToProcess, currentCommandContext);
        //         }
        //         catch (PSNotSupportedException notSupported)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     notSupported.ErrorRecord,
        //                     notSupported));
        //             continue;
        //         }
        //         catch (DriveNotFoundException driveNotFound)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     driveNotFound.ErrorRecord,
        //                     driveNotFound));
        //             continue;
        //         }
        //         catch (ProviderNotFoundException providerNotFound)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     providerNotFound.ErrorRecord,
        //                     providerNotFound));
        //             continue;
        //         }
        //         catch (ItemNotFoundException pathNotFound)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     pathNotFound.ErrorRecord,
        //                     pathNotFound));
        //             continue;
        //         }

        //         if (readers != null && readers.Count > 0)
        //         {
        //             if (readers.Count == 1 && readers[0] != null)
        //             {
        //                 ContentHolder holder =
        //                     new(pathInfo, readers[0], null);

        //                 results.Add(holder);
        //             }
        //         }
        //     }

        //     return results;
        // }

        // /// <summary>
        // /// Resolves the specified paths to PathInfo objects.
        // /// </summary>
        // /// <param name="pathsToResolve">
        // /// The paths to be resolved. Each path may contain glob characters.
        // /// </param>
        // /// <param name="allowNonexistingPaths">
        // /// If true, resolves the path even if it doesn't exist.
        // /// </param>
        // /// <param name="allowEmptyResult">
        // /// If true, allows a wildcard that returns no results.
        // /// </param>
        // /// <param name="currentCommandContext">
        // /// The context under which the command is running.
        // /// </param>
        // /// <returns>
        // /// An array of PathInfo objects that are the resolved paths for the
        // /// <paramref name="pathsToResolve"/> parameter.
        // /// </returns>
        // internal Collection<PathInfo> ResolvePaths(
        //     string[] pathsToResolve,
        //     bool allowNonexistingPaths,
        //     bool allowEmptyResult,
        //     CmdletProviderContext currentCommandContext)
        // {
        //     Collection<PathInfo> results = new();

        //     foreach (string path in pathsToResolve)
        //     {
        //         bool pathNotFound = false;
        //         bool filtersHidPath = false;

        //         ErrorRecord pathNotFoundErrorRecord = null;

        //         try
        //         {
        //             // First resolve each of the paths
        //             Collection<PathInfo> pathInfos =
        //                 SessionState.Path.GetResolvedPSPathFromPSPath(
        //                     path,
        //                     currentCommandContext);

        //             if (pathInfos.Count == 0)
        //             {
        //                 pathNotFound = true;

        //                 // If the item simply did not exist,
        //                 // we would have got an ItemNotFoundException.
        //                 // If we get here, it's because the filters
        //                 // excluded the file.
        //                 if (!currentCommandContext.SuppressWildcardExpansion)
        //                 {
        //                     filtersHidPath = true;
        //                 }
        //             }

        //             foreach (PathInfo pathInfo in pathInfos)
        //             {
        //                 results.Add(pathInfo);
        //             }
        //         }
        //         catch (PSNotSupportedException notSupported)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     notSupported.ErrorRecord,
        //                     notSupported));
        //         }
        //         catch (DriveNotFoundException driveNotFound)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     driveNotFound.ErrorRecord,
        //                     driveNotFound));
        //         }
        //         catch (ProviderNotFoundException providerNotFound)
        //         {
        //             WriteError(
        //                 new ErrorRecord(
        //                     providerNotFound.ErrorRecord,
        //                     providerNotFound));
        //         }
        //         catch (ItemNotFoundException pathNotFoundException)
        //         {
        //             pathNotFound = true;
        //             pathNotFoundErrorRecord = new ErrorRecord(pathNotFoundException.ErrorRecord, pathNotFoundException);
        //         }

        //         if (pathNotFound)
        //         {
        //             if (allowNonexistingPaths &&
        //                 (!filtersHidPath) &&
        //                 (currentCommandContext.SuppressWildcardExpansion ||
        //                 (!WildcardPattern.ContainsWildcardCharacters(path))))
        //             {
        //                 ProviderInfo provider = null;
        //                 PSDriveInfo drive = null;
        //                 string unresolvedPath =
        //                     SessionState.Path.GetUnresolvedProviderPathFromPSPath(
        //                         path,
        //                         currentCommandContext,
        //                         out provider,
        //                         out drive);

        //                 PathInfo pathInfo =
        //                     new(
        //                         drive,
        //                         provider,
        //                         unresolvedPath,
        //                         SessionState);
        //                 results.Add(pathInfo);
        //             }
        //             else
        //             {
        //                 if (pathNotFoundErrorRecord == null)
        //                 {
        //                     // Detect if the path resolution failed to resolve to a file.
        //                     string error = StringUtil.Format(NavigationResources.ItemNotFound, Path);
        //                     Exception e = new(error);

        //                     pathNotFoundErrorRecord = new ErrorRecord(
        //                         e,
        //                         "ItemNotFound",
        //                         ErrorCategory.ObjectNotFound,
        //                         Path);
        //                 }

        //                 WriteError(pathNotFoundErrorRecord);
        //             }
        //         }
        //     }

        //     return results;
        // }

        // /// <summary>
        // /// Closes the content readers and writers in the content holder array.
        // /// </summary>
        // internal void CloseContent(List<ContentHolder> contentHolders, bool disposing)
        // {
        //     if (contentHolders == null)
        //     {
        //         throw PSTraceSource.NewArgumentNullException(nameof(contentHolders));
        //     }

        //     foreach (ContentHolder holder in contentHolders)
        //     {
        //         try
        //         {
        //             if (holder.Writer != null)
        //             {
        //                 holder.Writer.Close();
        //             }
        //         }
        //         catch (Exception e) // Catch-all OK. 3rd party callout
        //         {
        //             // Catch all the exceptions caused by closing the writer
        //             // and write out an error.

        //             ProviderInvocationException providerException =
        //                 new(
        //                     "ProviderContentCloseError",
        //                     SessionStateStrings.ProviderContentCloseError,
        //                     holder.PathInfo.Provider,
        //                     holder.PathInfo.Path,
        //                     e);

        //             // Log a provider health event

        //             MshLog.LogProviderHealthEvent(
        //                 this.Context,
        //                 holder.PathInfo.Provider.Name,
        //                 providerException,
        //                 Severity.Warning);

        //             if (!disposing)
        //             {
        //                 WriteError(
        //                     new ErrorRecord(
        //                         providerException.ErrorRecord,
        //                         providerException));
        //             }
        //         }

        //         try
        //         {
        //             if (holder.Reader != null)
        //             {
        //                 holder.Reader.Close();
        //             }
        //         }
        //         catch (Exception e) // Catch-all OK. 3rd party callout
        //         {
        //             // Catch all the exceptions caused by closing the writer
        //             // and write out an error.

        //             ProviderInvocationException providerException =
        //                 new(
        //                     "ProviderContentCloseError",
        //                     SessionStateStrings.ProviderContentCloseError,
        //                     holder.PathInfo.Provider,
        //                     holder.PathInfo.Path,
        //                     e);

        //             // Log a provider health event

        //             MshLog.LogProviderHealthEvent(
        //                 this.Context,
        //                 holder.PathInfo.Provider.Name,
        //                 providerException,
        //                 Severity.Warning);

        //             if (!disposing)
        //             {
        //                 WriteError(
        //                     new ErrorRecord(
        //                         providerException.ErrorRecord,
        //                         providerException));
        //             }
        //         }
        //     }
        // }


        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing

    }
}
