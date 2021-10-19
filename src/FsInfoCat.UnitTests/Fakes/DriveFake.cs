using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FsInfoCat.UnitTests.Fakes
{
    internal sealed class DriveFake : IDisposable
    {
        private static readonly object _syncRoot = new();
        private static DriveFake _lastSubstituteDrive;
        private DriveFake _previous;
        private DriveFake _next;

        public static string ProjectDirectory { get; private set; }

        internal char Letter { get; }

        internal DriveInfo Info { get; }

        private DriveFake(char driveLetter)
        {
            Letter = driveLetter;
            Info = new DriveInfo(new string(new char[] { driveLetter }));
            if ((_previous = _lastSubstituteDrive) is not null)
                _previous._next = this;
            _lastSubstituteDrive = this;
        }

        internal static void AssemblyInit(TestContext context)
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            DirectoryInfo directoryInfo = new(context.DeploymentDirectory);
            while ((directoryInfo = directoryInfo.Parent) is not null)
            {
                if (directoryInfo.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    ProjectDirectory = directoryInfo.FullName;
                    break;
                }
            }
        }

        internal static void AssemblyCleanup() => DisposeAll();

        internal static DriveFake Create(string sourceArchiveFileName)
        {
            lock (_syncRoot)
            {
                if (ProjectDirectory is null)
                    throw new AssertInconclusiveException($"Could not find parent directory with the same name as the test assembly ({Assembly.GetExecutingAssembly().GetName().Name}). Ensure that the subdirectory for the current test project is named accordingly.");
                char[] usedDrives = DriveInfo.GetDrives().Select(d => d.Name).Where(n => n.Length > 1 && n[1] == ':').Select(n => char.ToUpper(n[0])).ToArray();
                char driveLetter = 'D';
                while (usedDrives.Contains(driveLetter))
                    driveLetter++;
                if (driveLetter > 'Z')
                    throw new AssertInconclusiveException("Could not find available drive letter for temporary substitute drive");
                string destinationDirectoryName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
                if (!Directory.Exists(destinationDirectoryName))
                    throw new AssertInconclusiveException($"Failed to extract \"{sourceArchiveFileName}\" to \"{destinationDirectoryName}\".");
                try
                {
                    Console.WriteLine($"Creating substitute drive {driveLetter}: Path=\"{destinationDirectoryName}\"");
                    using Process process = Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "cmd",
                        Arguments = $"SUBST {driveLetter}: \"{destinationDirectoryName}\""
                    });
                    process.WaitForExit();
                    if (!Directory.Exists($"{driveLetter}:\\"))
                        throw new AssertInconclusiveException($"Failed to substitute drive {driveLetter}: from \"{destinationDirectoryName}\".");

                    return new DriveFake(driveLetter);
                }
                catch
                {
                    Directory.Delete(destinationDirectoryName, true);
                    throw;
                }
            }
        }

        internal static void DisposeAll()
        {
            DriveFake substituteDrive = _lastSubstituteDrive;
            if (substituteDrive is not null)
                _lastSubstituteDrive.PrivateDisposeAll();
        }

        private void PrivateDisposeAll()
        {
            DriveFake substituteDrive = _previous;
            try { Dispose(); }
            finally
            {
                if (substituteDrive is not null)
                    substituteDrive.PrivateDisposeAll();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            lock (_syncRoot)
            {
                if (_previous is null)
                {
                    if (_next is null)
                    {
                        if (!ReferenceEquals(_lastSubstituteDrive, this))
                            return;
                        _lastSubstituteDrive = null;
                    }
                    _next = _next._previous = null;
                }
                else if ((_previous._next = _next) is null)
                    _previous = _previous._next = null;
                else
                {
                    _next._previous = _previous;
                    _previous = _next = null;
                }
                if (Directory.Exists(Info.RootDirectory.FullName))
                {
                    Console.WriteLine($"Removing substituted drive {Letter}:");
                    using Process process = Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "cmd",
                        Arguments = $"SUBST /D {Letter}:"
                    });
                    process.WaitForExit();
                }
            }
        }
    }
}
