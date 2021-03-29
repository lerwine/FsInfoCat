using FsInfoCat.Models.Volumes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.PS.Commands
{
    public abstract class FsVolumeInfoCommand : PSCmdlet
    {
        private const string VAR_NAME_FS_VOLUME_INFO_REGISTRATION = "__FsVolumeInfoRegistration";
        private const string EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR = "Unable to create module-dependent __FsVolumeInfoRegistration variable";

        protected static VolumeInfoRegistration GetVolumeRegistration(SessionState sessionState)
        {
            PSVariable psVariable = sessionState.PSVariable.Get(VAR_NAME_FS_VOLUME_INFO_REGISTRATION);
            VolumeInfoRegistration volumeRegistration;
            if (psVariable is null || !psVariable.Options.HasFlag(ScopedItemOptions.AllScope))
            {
                volumeRegistration = new VolumeInfoRegistration();
                sessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else if (!psVariable.Options.HasFlag(ScopedItemOptions.Constant))
            {
                sessionState.PSVariable.Remove(psVariable);
                volumeRegistration = new VolumeInfoRegistration();
                sessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else
            {
                object obj = psVariable.Value;
                if (obj is null)
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is null.");
                if (null == (volumeRegistration = ((obj is PSObject psObj) ? psObj.BaseObject : obj) as VolumeInfoRegistration))
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is incorrect type.");
            }
            return volumeRegistration;
        }

        internal static IEnumerable<IVolumeInfo> GetVolumeInfos(SessionState sessionState) => GetVolumeRegistration(sessionState).Cast<IVolumeInfo>();

        protected IEnumerable<IVolumeInfo> GetVolumeInfos() => GetVolumeInfos(SessionState);

        protected IEnumerable<string> ResolveDirectoryFromLiteralPath(IEnumerable<string> literalPaths)
        {
            foreach (string lPath in literalPaths)
            {
                if (TryResolveDirectoryFromLiteralPath(lPath, out string providerPath))
                    yield return providerPath;
            }
        }

        private bool TryResolveDirectoryFromLiteralPath(string path, out string providerPath)
        {
            WriteDebug($"Processing LiteralPath: \"{path}\"");
            if (string.IsNullOrWhiteSpace(path))
            {
                providerPath = null;
                return false;
            }
            try
            {
                providerPath = GetUnresolvedProviderPathFromPSPath(path);
                WriteDebug($"Unresolved provider path from LiteralPath: \"{providerPath}\"");
                if (!Directory.Exists(providerPath))
                    providerPath = null;
            }
            catch (ItemNotFoundException nfExc)
            {
                OnItemNotFoundException(path, nfExc);
                providerPath = null;
                return false;
            }
            catch (Exception exc)
            {
                OnProviderNotSupportedException(path, exc);
                providerPath = null;
                return false;
            }
            if (providerPath is null)
            {
                if (File.Exists(path))
                    OnPathIsFileError(path);
                else
                    OnItemNotFoundException(path, null);
                return false;
            }
            return true;
        }

        protected IEnumerable<string> ResolveDirectoryFromWcPath(string path)
        {
            WriteDebug($"Resolving Path: \"{path}\"");
            if (string.IsNullOrWhiteSpace(path))
                yield break;

            Collection<PathInfo> resolvedPaths;
            try
            {
                resolvedPaths = SessionState.Path.GetResolvedPSPathFromPSPath(path);
                WriteDebug($"Resolved {resolvedPaths.Count} paths: \"{string.Join("\", \"", resolvedPaths.Select(p => p.ProviderPath))}\"");
            }
            catch (NotSupportedException nsExc)
            {
                OnProviderNotSupportedException(path, nsExc);
                yield break;
            }
            catch (ItemNotFoundException nfExc)
            {
                OnItemNotFoundException(path, nfExc);
                yield break;
            }
            catch (Exception exc)
            {
                OnResolveError(path, exc);
                yield break;
            }

            foreach (PathInfo pathInfo in resolvedPaths)
            {
                WriteDebug($"Processing resolved path: \"{pathInfo.ProviderPath}\"");
                string p;
                try
                {
                    p = (Directory.Exists(pathInfo.ProviderPath)) ? pathInfo.ProviderPath : null;
                }
                catch (Exception exc)
                {
                    OnProviderNotSupportedException(path, exc);
                    continue;
                }
                if (p is null)
                {
                    if (File.Exists(pathInfo.ProviderPath))
                        OnPathIsFileError(pathInfo.ProviderPath);
                    else
                        OnItemNotFoundException(path, null);
                }
                else
                    yield return p;
            }
        }

        protected abstract void OnProviderNotSupportedException(string path, Exception exc);

        protected abstract void OnItemNotFoundException(string path, ItemNotFoundException exc);

        protected abstract void OnResolveError(string path, Exception exc);

        protected abstract void OnPathIsFileError(string providerPath);
    }
}
