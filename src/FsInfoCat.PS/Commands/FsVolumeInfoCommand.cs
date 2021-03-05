using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
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

        protected static Collection<PSObject> GetVolumeRegistration(SessionState sessionState)
        {
            PSVariable psVariable = sessionState.PSVariable.Get(VAR_NAME_FS_VOLUME_INFO_REGISTRATION);
            VolumeRegistration volumeRegistration;
            if (psVariable is null || !psVariable.Options.HasFlag(ScopedItemOptions.AllScope))
            {
                volumeRegistration = new VolumeRegistration(new Collection<PSObject>());
                sessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else if (!psVariable.Options.HasFlag(ScopedItemOptions.Constant))
            {
                sessionState.PSVariable.Remove(psVariable);
                volumeRegistration = new VolumeRegistration(new Collection<PSObject>());
                sessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else
            {
                object obj = psVariable.Value;
                if (obj is null)
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is null.");
                if (null == (volumeRegistration = ((obj is PSObject psObj) ? psObj.BaseObject : obj) as VolumeRegistration))
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is incorrect type.");
            }
            return volumeRegistration.BackingCollection;
        }

        internal Collection<PSObject> GetVolumeRegistration() => GetVolumeRegistration(SessionState);

        internal static IEnumerable<IVolumeInfo> GetVolumeInfos(SessionState sessionState) => GetVolumeRegistration(sessionState).Select(o => (IVolumeInfo)o.BaseObject);

        protected IEnumerable<IVolumeInfo> GetVolumeInfos() => GetVolumeInfos(SessionState);

        protected class RegisteredVolumeInfo : IVolumeInfo, IEquatable<IVolumeInfo>
        {
            private bool _caseSensitive;
            private StringComparer _pathComparer;
            private PSObject _parent;
            private Collection<PSObject> _nested = new Collection<PSObject>();

            public FileUri RootUri { get; internal set; }

            FileUri IVolumeInfo.RootUri { get => RootUri; set => throw new NotSupportedException(); }

            string IVolumeInfo.RootPathName => RootUri.ToLocalPath();

            public string VolumeName { get; internal set; }

            string IVolumeInfo.VolumeName { get => VolumeName; set => throw new NotSupportedException(); }

            public string DriveFormat { get; internal set; }

            string IVolumeInfo.DriveFormat { get => DriveFormat; set => throw new NotSupportedException(); }

            public VolumeIdentifier Identifier { get; internal set; }

            VolumeIdentifier IVolumeInfo.Identifier { get => Identifier; set => throw new NotSupportedException(); }

            public bool CaseSensitive
            {
                get => _caseSensitive;
                internal set
                {
                    if (_caseSensitive == value)
                        return;
                    _caseSensitive = value;
                    _pathComparer = null;
                }
            }

            bool IVolumeInfo.CaseSensitive { get => CaseSensitive; set => throw new NotSupportedException(); }

            public IEqualityComparer<string> PathComparer
            {
                get
                {
                    StringComparer comparer = _pathComparer;
                    if (comparer is null)
                        _pathComparer = comparer = _caseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
                    return comparer;
                }
            }

            public bool Equals(IVolumeInfo other) => null != other && (ReferenceEquals(this, other) || (Identifier.Equals(other.Identifier) &&
                RootUri.Equals(other.RootUri, (CaseSensitive || !other.CaseSensitive) ? PathComparer : other.PathComparer)));

            public override bool Equals(object obj)
            {
                if (obj is null)
                    return false;
                if (obj is PSObject psObj)
                    obj = psObj.BaseObject;
                if (obj is IVolume v)
                    return Equals(v);
                return obj is DriveInfo d && Equals(d);
            }

            public override int GetHashCode() => Identifier.GetHashCode();

            public override string ToString() => $"{{ RootUri=\"{RootUri}\"; VolumeName=\"{VolumeName}\"; DriveFormat=\"{DriveFormat}\"; Identifer={Identifier}; CaseSensitive = {CaseSensitive} }}";
        }

        protected class VolumeRegistration : ReadOnlyCollection<PSObject>
        {
            internal Collection<PSObject> BackingCollection { get; }
            internal VolumeRegistration(Collection<PSObject> backingCollection) : base(backingCollection)
            {
                BackingCollection = backingCollection;
            }
        }

    }
}
