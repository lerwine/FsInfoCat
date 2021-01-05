using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.HostDevices;

namespace FsInfoCat.PS
{
    public class FsCrawlJob : Job
    {
        private string _statusMessage = "Initializing";
        public override string StatusMessage => _statusMessage;

        // TODO: Implement this
        public override bool HasMoreData => throw new System.NotImplementedException();

        // TODO: Implement this
        public override string Location => throw new System.NotImplementedException();

        public int MaxDepth { get; }

        public ReadOnlyCollection<string> RootPaths { get; }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public CancellationToken CancellationToken { get; }

        private readonly Task<FsHost> _task;

        public FsCrawlJob(int maxDepth, params string[] rootPath) : this(null, maxDepth) { }

        public FsCrawlJob(string friendlyName, int maxDepth, params string[] rootPath)
            : base(null, friendlyName)
        {
            MaxDepth = (maxDepth < 0) ? int.MaxValue : maxDepth;
            RootPaths = new ReadOnlyCollection<string>(rootPath);
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken = _cancellationTokenSource.Token;
            _task = Task.Factory.StartNew<FsHost>(Run, new object[] { (maxDepth < 0) ? 0 : maxDepth, rootPath, new Action<string>(s => _statusMessage = (null == s) ? "" : s.Trim()), _cancellationTokenSource.Token }, _cancellationTokenSource.Token);
        }

        public static FsHost Run(object args)
        {
            object[] argArray = (object[])args;
            int maxDepth = (int)argArray[0];
            string[] rootPath = (string[])argArray[1];
            Action<string> setStatusMessage = (Action<string>)argArray[2];
            CancellationToken token = (CancellationToken)argArray[3];
            FsHost result = new FsHost { MachineName = Environment.MachineName };
            if (token.IsCancellationRequested)
                return result;
            string machineIdentifier;
            try { machineIdentifier = HostDeviceRegRequest.GetLocalMachineIdentifier(); }
            catch (Exception exc)
            {
                result.Errors.Add(new CrawlError(exc, "Getting machine identifier"));
                return result;
            }
            for (int i = 0; i < rootPath.Length; i++)
            {
                if (token.IsCancellationRequested)
                    break;
                DirectoryInfo startDi;
                FsRoot fsRoot;
                IFsDirectory parentBranch;
                string path = rootPath[i];
                try
                {
                    if (null == path)
                        throw new Exception("Null path at position " + i.ToString());
                    if (path.Trim().Length == 0)
                        throw new Exception("Empty path at position " + i.ToString());
                }
                catch (Exception exc)
                {
                    result.Errors.Add(new CrawlError(exc, "Getting file system root at position " + i.ToString()));
                    continue;
                }
                try
                {
                    try { startDi = new DirectoryInfo(path); }
                    catch (Exception e) { throw new Exception("Invalid path at position " + i.ToString() + ": " + path, e); }
                    if (!startDi.Exists)
                        throw new Exception("Path not found at position " + i.ToString() + ": " + path);
                    fsRoot = FsDirectory.GetRoot(result, startDi, out parentBranch);
                }
                catch (Exception exc)
                {
                    result.Errors.Add(new CrawlError(exc, "Getting file system root at position " + i.ToString() + ": " + path));
                    continue;
                }
                parentBranch.ChildNodes.Clear();
                Run(startDi, parentBranch, fsRoot, (maxDepth < 1) ? 0 : maxDepth, setStatusMessage, token);
            }
            return result;
        }

        public static void Run(DirectoryInfo directory, IFsDirectory container, IEqualityComparer<IFsChildNode> comparer, int maxDepth, Action<string> setStatusMessage, CancellationToken cancellationToken)
        {
            FileInfo[] filesArray;
            if (cancellationToken.IsCancellationRequested)
                return;
            try { setStatusMessage("Crawling " + directory.FullName); }
            catch (Exception exc)
            {
                container.Errors.Add(new CrawlError(exc, "Getting directory full name"));
                return;
            }
            try
            {
                if (null == (filesArray = directory.GetFiles()))
                    filesArray = new FileInfo[0];
            }
            catch (Exception exc)
            {
                container.Errors.Add(new CrawlError(exc, "Enumerating files"));
                filesArray = new FileInfo[0];
            }
            for (int i = 0; i < filesArray.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                FsFile.Import(filesArray[i], i, container);
            }
            if (--maxDepth < 0 || cancellationToken.IsCancellationRequested)
                return;
            DirectoryInfo[] directoriesArray;
            try
            {
                if (null == (directoriesArray = directory.GetDirectories()) || directoriesArray.Length == 0 || cancellationToken.IsCancellationRequested)
                    return;
            }
            catch (Exception exc)
            {
                container.Errors.Add(new CrawlError(exc, "Enumerating files"));
                return;
            }
            var subDirs = directoriesArray.Select((d, i) => new
            {
                FS = (cancellationToken.IsCancellationRequested) ? null : FsDirectory.Import(d, i, container),
                DI = d
            }).Where(d => null != d.FS).ToArray();
            foreach (var d in subDirs)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
                Run(d.DI, d.FS, comparer, maxDepth, setStatusMessage, cancellationToken);
            }
        }

        public override void StopJob()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                    _cancellationTokenSource.Cancel(true);
                _cancellationTokenSource.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
