using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FsInfoCat.UnitTests
{
    public static class TestHelper
    {
        //private static Task _ensureLocalDbTestDataTask;
        //private static SubstituteDrive _lastSubstituteDrive;
        internal const string TestCategory_LocalDb = "LocalDb";
        internal const string TestProperty_Description = "Description";

        internal static Local.FileSystem GetVFatFileSystem(Local.LocalDbContext dbContext)
        {
            Guid id = Guid.Parse("{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}");
            return dbContext.FileSystems.ToList().FirstOrDefault(fs => fs.Id.Equals(id));
        }

        internal class ApplicationNavigationFakeOld : IApplicationNavigation
        {
            private readonly Stack<(Uri Source, object Content, object NavigationState, bool? SandboxExternalContent)> _backwardHistory = new();
            private readonly Stack<(Uri Source, object Content, object NavigationState, bool? SandboxExternalContent)> _forwardHistory = new();

            public IEnumerable<(Uri Source, object Content, object NavigationState, bool? SandboxExternalContent)> GetBackwardHistory() => _backwardHistory.AsEnumerable();

            public IEnumerable<(Uri Source, object Content, object NavigationState, bool? SandboxExternalContent)> GetForwardHistory() => _forwardHistory.AsEnumerable();

            public CancellationToken CancellationToken { get; set; }

            public Uri Source { get; private set; }

            public Uri CurrentSource { get; private set; }

            public object Content { get; private set; }

            public object NavigationState { get; private set; }

            public bool? SandboxExternalContent { get; private set; }

            public bool CanGoForward => _forwardHistory.Count > 0;

            public bool CanGoBack => _backwardHistory.Count > 0;

            public bool GoBack()
            {
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    if (_backwardHistory.TryPop(out (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) prev))
                    {
                        if (Source is not null || Content is not null)
                            _forwardHistory.Push((Source, Content, NavigationState, SandboxExternalContent));
                        (CurrentSource, _, NavigationState, SandboxExternalContent) = prev;
                        Thread.Sleep(100);
                        token.ThrowIfCancellationRequested();
                        return true;
                    }
                    return false;
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        if (task.Result)
                        {
                            Source = CurrentSource;
                            return true;
                        }
                    }
                    else
                    {
                        (Source, Content, NavigationState, SandboxExternalContent) = current;
                        _backwardHistory.Push((Source, Content, NavigationState, SandboxExternalContent));
                        if (Source is not null || Content is not null)
                            _ = _forwardHistory.Pop();
                    }
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool GoForward()
            {
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    if (_forwardHistory.TryPop(out (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) next))
                    {
                        if (Source is not null || Content is not null)
                            _backwardHistory.Push((Source, Content, NavigationState, SandboxExternalContent));
                        (CurrentSource, _, NavigationState, SandboxExternalContent) = next;
                        Thread.Sleep(100);
                        token.ThrowIfCancellationRequested();
                        return true;
                    }
                    return false;
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        if (task.Result)
                        {
                            Source = CurrentSource;
                            return true;
                        }
                    }
                    else
                    {
                        (Source, Content, NavigationState, SandboxExternalContent) = current;
                        _forwardHistory.Push((Source, Content, NavigationState, SandboxExternalContent));
                        if (Source is not null || Content is not null)
                            _ = _backwardHistory.Pop();
                    }
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool Navigate(Uri source, object navigationState, bool sandboxExternalContent)
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    CurrentSource = source;
                    NavigationState = navigationState;
                    SandboxExternalContent = sandboxExternalContent;
                    Thread.Sleep(100);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        _backwardHistory.Push(current);
                        _forwardHistory.Clear();
                        Source = source;
                        return true;
                    }
                    (Source, Content, NavigationState, SandboxExternalContent) = current;
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool Navigate(Uri source, object navigationState)
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    CurrentSource = source;
                    NavigationState = navigationState;
                    SandboxExternalContent = null;
                    Thread.Sleep(100);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        _backwardHistory.Push(current);
                        _forwardHistory.Clear();
                        Source = source;
                        return true;
                    }
                    (Source, Content, NavigationState, SandboxExternalContent) = current;
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool Navigate(Uri source)
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    CurrentSource = source;
                    NavigationState = null;
                    SandboxExternalContent = null;
                    Thread.Sleep(100);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        _backwardHistory.Push(current);
                        _forwardHistory.Clear();
                        Source = source;
                        return true;
                    }
                    (Source, Content, NavigationState, SandboxExternalContent) = current;
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool Navigate(object root, object navigationState)
            {
                if (root is null)
                    throw new ArgumentNullException(nameof(root));
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    CurrentSource = null;
                    Content = root;
                    NavigationState = navigationState;
                    SandboxExternalContent = null;
                    Thread.Sleep(100);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        _backwardHistory.Push(current);
                        _forwardHistory.Clear();
                        Source = null;
                        return true;
                    }
                    (Source, Content, NavigationState, SandboxExternalContent) = current;
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public bool Navigate(object root)
            {
                if (root is null)
                    throw new ArgumentNullException(nameof(root));
                CancellationToken token = CancellationToken;
                Monitor.Enter(_backwardHistory);
                (Uri Source, object Content, object NavigationState, bool? SandboxExternalContent) current = (Source, Content, NavigationState, SandboxExternalContent);
                Task<bool> result = Task.Factory.StartNew(() =>
                {
                    token.ThrowIfCancellationRequested();
                    CurrentSource = null;
                    Content = root;
                    NavigationState = null;
                    SandboxExternalContent = null;
                    Thread.Sleep(100);
                    token.ThrowIfCancellationRequested();
                }, token).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        _backwardHistory.Push(current);
                        _forwardHistory.Clear();
                        Source = null;
                        return true;
                    }
                    (Source, Content, NavigationState, SandboxExternalContent) = current;
                    return false;
                });
                _ = result.ContinueWith(task => Monitor.Exit(_backwardHistory));
                return result.Result;
            }

            public void Refresh() { }
        }
        //internal sealed class SubstituteDrive : IDisposable
        //{
        //    private readonly string _destinationDirectoryName;
        //    private SubstituteDrive _previous;
        //    private SubstituteDrive _next;

        //    internal char Letter { get; }

        //    internal DriveInfo Info { get; }

        //    private SubstituteDrive(char driveLetter)
        //    {
        //        Letter = driveLetter;
        //        Info = new DriveInfo(new string(new char[] { driveLetter }));
        //        if ((_previous = _lastSubstituteDrive) is not null)
        //            _previous._next = this;
        //        _lastSubstituteDrive = this;
        //    }

        //    internal static SubstituteDrive Create(string sourceArchiveFileName)
        //    {
        //        lock (_syncRoot)
        //        {
        //            if (ProjectDirectory is null)
        //                throw new AssertInconclusiveException($"Could not find parent directory with the same name as the test assembly ({Assembly.GetExecutingAssembly().GetName().Name}). Ensure that the subdirectory for the current test project is named accordingly.");
        //            char[] usedDrives = DriveInfo.GetDrives().Select(d => d.Name).Where(n => n.Length > 1 && n[1] == ':').Select(n => char.ToUpper(n[0])).ToArray();
        //            char driveLetter = 'D';
        //            while (usedDrives.Contains(driveLetter))
        //                driveLetter++;
        //            if (driveLetter > 'Z')
        //                throw new AssertInconclusiveException("Could not find available drive letter for temporary substitute drive");
        //            string destinationDirectoryName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        //            ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        //            if (!Directory.Exists(destinationDirectoryName))
        //                throw new AssertInconclusiveException($"Failed to extract \"{sourceArchiveFileName}\" to \"{destinationDirectoryName}\".");
        //            try
        //            {
        //                Console.WriteLine($"Creating substitute drive {driveLetter}: Path=\"{destinationDirectoryName}\"");
        //                using Process process = Process.Start(new ProcessStartInfo()
        //                {
        //                    UseShellExecute = false,
        //                    CreateNoWindow = true,
        //                    FileName = "cmd",
        //                    Arguments = $"SUBST {driveLetter}: \"{destinationDirectoryName}\""
        //                });
        //                process.WaitForExit();
        //                if (!Directory.Exists($"{driveLetter}:\\"))
        //                    throw new AssertInconclusiveException($"Failed to substitute drive {driveLetter}: from \"{destinationDirectoryName}\".");

        //                return new SubstituteDrive(driveLetter);
        //            }
        //            catch
        //            {
        //                Directory.Delete(destinationDirectoryName, true);
        //                throw;
        //            }
        //        }
        //    }

        //    internal static void DisposeAll()
        //    {
        //        SubstituteDrive substituteDrive = _lastSubstituteDrive;
        //        if (substituteDrive is not null)
        //            _lastSubstituteDrive.PrivateDisposeAll();
        //    }

        //    private void PrivateDisposeAll()
        //    {
        //        SubstituteDrive substituteDrive = _previous;
        //        try { Dispose(); }
        //        finally
        //        {
        //            if (substituteDrive is not null)
        //                substituteDrive.PrivateDisposeAll();
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        GC.SuppressFinalize(this);
        //        lock (_syncRoot)
        //        {
        //            if (_previous is null)
        //            {
        //                if (_next is null)
        //                {
        //                    if (!ReferenceEquals(_lastSubstituteDrive, this))
        //                        return;
        //                    _lastSubstituteDrive = null;
        //                }
        //                _next = _next._previous = null;
        //            }
        //            else if ((_previous._next = _next) is null)
        //                _previous = _previous._next = null;
        //            else
        //            {
        //                _next._previous = _previous;
        //                _previous = _next = null;
        //            }
        //            if (Directory.Exists(Info.RootDirectory.FullName))
        //            {
        //                Console.WriteLine($"Removing substituted drive {Letter}:");
        //                using Process process = Process.Start(new ProcessStartInfo()
        //                {
        //                    UseShellExecute = false,
        //                    CreateNoWindow = true,
        //                    FileName = "cmd",
        //                    Arguments = $"SUBST /D {Letter}:"
        //                });
        //                process.WaitForExit();
        //            }
        //        }
        //    }
        //}

        //private static bool CheckProperty<T>([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<XAttribute, T> ifPresent, out T result)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //    {
        //        result = default;
        //        return false;
        //    }
        //    result = ifPresent(attribute);
        //    return true;
        //}

        //private static T CheckProperty<T>([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<XAttribute, T> ifPresent, [DisallowNull] Func<T> ifNotPresent)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //        return ifNotPresent();
        //    return ifPresent(attribute);
        //}

        //private static T CheckProperty<T>([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<XAttribute, T> ifPresent, T ifNotPresent)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //        return ifNotPresent;
        //    return ifPresent(attribute);
        //}

        //private static void EnsureStringProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<string> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => a.Value, out string result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static string EnsureStringProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, string ifNotPresent) => CheckProperty(element, attributeName, a => a.Value, ifNotPresent);

        //private static void EnsureBooleanProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<bool> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToBoolean(a.Value), out bool result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static bool EnsureBooleanProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, bool ifNotPresent) => CheckProperty(element, attributeName, a => XmlConvert.ToBoolean(a.Value), ifNotPresent);

        //private static void EnsureBooleanProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<bool?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToBoolean(a.Value) : null, out bool? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static bool? EnsureBooleanProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, bool? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToBoolean(a.Value) : null, ifNotPresent);

        //private static void EnsureInt32Property([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<int> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToInt32(a.Value), out int result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static int EnsureInt32Property([DisallowNull] XElement element, [DisallowNull] XName attributeName, int ifNotPresent) => CheckProperty(element, attributeName, a => XmlConvert.ToInt32(a.Value), ifNotPresent);

        //private static void EnsureInt32Property([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<int?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToInt32(a.Value) : null, out int? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static int? EnsureInt32Property([DisallowNull] XElement element, [DisallowNull] XName attributeName, int? ifNotPresent) => CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToInt32(a.Value) : null, ifNotPresent);

        //private static void EnsureDriveTypeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<DriveType> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (DriveType)Enum.Parse(typeof(DriveType), a.Value), out DriveType result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DriveType EnsureDriveTypeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, DriveType ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (DriveType)Enum.Parse(typeof(DriveType), a.Value), ifNotPresent);

        //private static void EnsureDriveTypeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<DriveType?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? (DriveType?)Enum.Parse(typeof(DriveType), a.Value) : null, out DriveType? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DriveType? EnsureDriveTypeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, DriveType? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? (DriveType?)Enum.Parse(typeof(DriveType), a.Value) : null, ifNotPresent);

        //private static void EnsureGuidProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<Guid?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? Guid.Parse(a.Value) : null, out Guid? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static Guid? EnsureGuidProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, Guid? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? Guid.Parse(a.Value) : null, ifNotPresent);

        //private static void EnsureDateTimeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<DateTime> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local), out DateTime result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DateTime EnsureDateTimeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, DateTime ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local), ifNotPresent);

        //private static void EnsureDateTimeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, [DisallowNull] Action<DateTime?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local) : null, out DateTime? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DateTime? EnsureDateTimeProperty([DisallowNull] XElement element, [DisallowNull] XName attributeName, DateTime? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local) : null, ifNotPresent);

        //private static void EnsureDbEntityProperties([DisallowNull] XElement element, DbEntity entity)
        //{
        //    EnsureDateTimeProperty(element, nameof(DbEntity.CreatedOn), dateTime => entity.CreatedOn = dateTime);
        //    EnsureDateTimeProperty(element, nameof(DbEntity.ModifiedOn), dateTime => entity.ModifiedOn = dateTime);
        //}

        //private static void EnsureLocalDbEntityProperties([DisallowNull] XElement element, Local.LocalDbEntity entity)
        //{
        //    entity.UpstreamId = EnsureGuidProperty(element, nameof(Local.LocalDbEntity.UpstreamId), null);
        //    entity.LastSynchronizedOn = EnsureDateTimeProperty(element, nameof(Local.LocalDbEntity.LastSynchronizedOn), null);
        //    EnsureDbEntityProperties(element, entity);
        //}

        //internal static Task CheckLocalDbTestData()
        //{
        //    lock (_syncRoot)
        //    {
        //        if (_ensureLocalDbTestDataTask is null)
        //        {
        //            if (Services.ServiceProvider is null)
        //                throw new InvalidOperationException("Service provider not configured");
        //            _ensureLocalDbTestDataTask = Task.Factory.StartNew(() =>
        //            {
        //                try { EnsureLocalDbTestData(); }
        //                catch (AssertInconclusiveException) { throw; }
        //                catch (Exception exception)
        //                {
        //                    throw new AssertInconclusiveException("Error ensuring test data integrity", exception);
        //                }
        //            });
        //        }
        //    }
        //    return _ensureLocalDbTestDataTask;
        //}

        //private static void EnsureLocalDbTestData()
        //{
        //    using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
        //    XDocument document = XDocument.Parse(Properties.Resources.TestData);
        //    Guid[] ids = document.Root.Elements(nameof(Local.FileSystem)).Select(e => EnsureLocalFileSystem(dbContext, e).Id).ToArray();
        //    var fileSystems = dbContext.FileSystems.AsEnumerable().Where(f => !ids.Contains(f.Id)).ToArray();
        //    if (fileSystems.Length > 0)
        //    {
        //        foreach (var f in fileSystems)
        //            dbContext.ForceDeleteFileSystem(f);
        //        dbContext.SaveChanges();
        //    }
        //    ids = document.Root.Elements(nameof(Local.BinaryProperties)).Select(e => EnsureLocalBinaryProperties(dbContext, e).Id).ToArray();
        //    var binaryPropertiess = dbContext.BinaryProperties.AsEnumerable().Where(c => !ids.Contains(c.Id)).ToArray();
        //    if (binaryPropertiess.Length > 0)
        //    {
        //        foreach (var c in binaryPropertiess)
        //            dbContext.ForceDeleteBinaryProperties(c);
        //        dbContext.SaveChanges();
        //    }
        //}

        //private static Local.BinaryProperties EnsureLocalBinaryProperties(Local.LocalDbContext dbContext, XElement element)
        //{
        //    string n = nameof(Local.BinaryProperties.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.BinaryProperties.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.FileSystem EnsureLocalFileSystem(Local.LocalDbContext dbContext, XElement element)
        //{
        //    string n = nameof(Local.FileSystem.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.FileSystems.Find(id);
        //    if (target is null)
        //    {
        //        StringBuilder sql = new StringBuilder("INSERT INTO ").Append(nameof(Local.LocalDbContext.FileSystems)).Append(" (\"Id\"");
        //        var values = element.Attributes().Where(a => a.Name != n).Select((a, i) =>
        //        {
        //            switch (a.Name.LocalName)
        //            {
        //                case nameof(Local.FileSystem.CaseSensitiveSearch):
        //                case nameof(Local.FileSystem.ReadOnly):
        //                case nameof(Local.FileSystem.IsInactive):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)XmlConvert.ToBoolean(a.Value) };
        //                case nameof(Local.FileSystem.MaxNameLength):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)XmlConvert.ToInt32(a.Value) };
        //                case nameof(Local.FileSystem.DefaultDriveType):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = Enum.ToObject(typeof(DriveType), Enum.Parse(typeof(DriveType), a.Value)) };
        //                case nameof(Local.FileSystem.DisplayName):
        //                case nameof(Local.FileSystem.Notes):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)a.Value };
        //                default:
        //                    throw new NotSupportedException($"Attribute {a.Name} not supported for element {a.Parent.Name}");
        //            }
        //        }).ToArray();
        //        foreach (var v in values)
        //            sql.Append(", \"").Append(v.LocalName).Append("\"");
        //        sql.Append(") VALUES ({0}");
        //        foreach (var v in values)
        //            sql.Append(", {").Append(v.Index).Append('}');
        //        dbContext.Database.ExecuteSqlRaw(sql.Append(')').ToString(), values.Select(v => v.Value));
        //        if ((target = dbContext.Find<Local.FileSystem>(id)) is null)
        //            throw new AssertInconclusiveException($"Failed to insert new {nameof(Local.FileSystem)} with Id {id}");
        //    }
        //    else
        //    {
        //        EnsureStringProperty(element, nameof(Local.FileSystem.DisplayName), text => target.DisplayName = text);
        //        target.CaseSensitiveSearch = EnsureBooleanProperty(element, nameof(Local.FileSystem.CaseSensitiveSearch), false);
        //        target.ReadOnly = EnsureBooleanProperty(element, nameof(Local.FileSystem.ReadOnly), false);
        //        target.MaxNameLength = EnsureInt32Property(element, nameof(Local.FileSystem.MaxNameLength), DbConstants.DbColDefaultValue_MaxNameLength);
        //        target.Notes = EnsureStringProperty(element, nameof(Local.FileSystem.Notes), "");
        //        target.IsInactive = EnsureBooleanProperty(element, nameof(Local.FileSystem.IsInactive), false);
        //        EnsureLocalDbEntityProperties(element, target);
        //        Local.Volume[] localVolumes = element.Elements(nameof(Local.Volume)).Select(e => EnsureLocalVolume(dbContext, target, e)).ToArray();
        //    }
        //    return target;
        //}

        //private static Local.Volume EnsureLocalVolume(Local.LocalDbContext dbContext, Local.FileSystem fileSystem, XElement element)
        //{
        //    string n = nameof(Local.Volume.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Volumes.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.Subdirectory EnsureLocalSubdirectory(Local.LocalDbContext dbContext, Local.Subdirectory parent, XElement element)
        //{
        //    string n = nameof(Local.Subdirectory.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Subdirectories.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.DbFile EnsureLocalFile(Local.LocalDbContext dbContext, Local.Subdirectory parent, XElement element)
        //{
        //    string n = nameof(Local.DbFile.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Files.Find(id);
        //    throw new NotImplementedException();
        //}
    }
}
