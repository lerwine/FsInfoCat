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
                if ((volumeRegistration = ((obj is PSObject psObj) ? psObj.BaseObject : obj) as VolumeInfoRegistration) is null)
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

        /// <summary>
        /// Resolves FileSystem provider path from a path string that may include wildcard characters.
        /// </summary>
        /// <param name="path">Path string that may contain wildcard characters.</param>
        /// <returns>FileSystem provider path strings that match the specified <paramref name="path"/>.</returns>
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

        /// <summary>
        /// This gets called whenever an invalid path string is enountered.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="ResolveDirectoryFromLiteralPath(IEnumerable{string})"/>, <see cref="ResolveDirectoryFromWcPath(string)"/> and <see cref="TryResolveDirectoryFromLiteralPath(string, out string)"/>
        /// when trying to resolve a path that is not supported by the local filesystem.</remarks>
        protected abstract void OnProviderNotSupportedException(string path, Exception exc);

        /// <summary>
        /// This gets called whenever a non-existent-path is encountered.
        /// </summary>
        /// <param name="path">the non-existent filesystem path.</param>
        /// <param name="exc">The exception that was thrown or <see langword="null"/> if existence validation failed without an exception being thrown.</param>
        /// <remarks>This gets called by <see cref="ResolveDirectoryFromLiteralPath(IEnumerable{string})"/>, <see cref="ResolveDirectoryFromWcPath(string)"/> and <see cref="TryResolveDirectoryFromLiteralPath(string, out string)"/>
        /// when trying to resolve a path that does not exist.</remarks>
        protected abstract void OnItemNotFoundException(string path, ItemNotFoundException exc);

        /// <summary>
        /// This gets called when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.
        /// </summary>
        /// <param name="path">The path string that could not be resolved.</param>
        /// <param name="exc">The exeption that was thrown.</param>
        /// <remarks>This gets called by <see cref="ResolveDirectoryFromWcPath(string)"/> when an unexpected exception is thrown while trying to resolve a wildcard-supported path string.</remarks>
        protected abstract void OnResolveError(string path, Exception exc);

        /// <summary>
        /// This gets called whenever a path is encountered with refers to a file rather than a subdirectory.
        /// </summary>
        /// <param name="path">The path to a file.</param>
        /// <remarks>This gets called by <see cref="ResolveDirectoryFromLiteralPath(IEnumerable{string})"/>, <see cref="ResolveDirectoryFromWcPath(string)"/> and <see cref="TryResolveDirectoryFromLiteralPath(string, out string)"/>
        /// when a path was successfully resolved, but it did not refer to a subdirectory.</remarks>
        protected abstract void OnPathIsFileError(string providerPath);
    }
}
