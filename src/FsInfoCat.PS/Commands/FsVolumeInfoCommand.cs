using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Management.Automation;
using System.Linq;
using FsInfoCat.Models.DB;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;

namespace FsInfoCat.PS.Commands
{
    public abstract class FsVolumeInfoCommand : PSCmdlet
    {
        private const string VAR_NAME_FS_VOLUME_INFO_REGISTRATION = "__FsVolumeInfoRegistration";
        private const string EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR = "Unable to create module-dependent __FsVolumeInfoRegistration variable";

        protected Collection<PSObject> GetVolumeRegistration()
        {
            PSVariable psVariable = SessionState.PSVariable.Get(VAR_NAME_FS_VOLUME_INFO_REGISTRATION);
            VolumeRegistration volumeRegistration;
            if (psVariable is null || !psVariable.Options.HasFlag(ScopedItemOptions.AllScope))
            {
                volumeRegistration = new VolumeRegistration(new Collection<PSObject>());
                SessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else if (!psVariable.Options.HasFlag(ScopedItemOptions.Constant))
            {
                SessionState.PSVariable.Remove(psVariable);
                volumeRegistration = new VolumeRegistration(new Collection<PSObject>());
                SessionState.PSVariable.Set(new PSVariable(VAR_NAME_FS_VOLUME_INFO_REGISTRATION, volumeRegistration, ScopedItemOptions.Constant | ScopedItemOptions.AllScope));
            }
            else
            {
                object obj = psVariable.Value;
                if (obj is null)
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is null.");
                if (null == (volumeRegistration = ((obj is PSObject) ? ((PSObject)obj).BaseObject : obj) as VolumeRegistration))
                    throw new PSInvalidOperationException(EXCEPTION_MESSAGE_REGISTRATION_VAR_ERROR + ": Existing variable is incorrect type.");
            }
            return volumeRegistration.BackingCollection;
        }

        protected IEnumerable<IVolumeInfo> GetVolumeInfos()
        {
            return GetVolumeRegistration().Select(o => (IVolumeInfo)o.BaseObject);
        }

        protected class RegisteredVolumeInfo : IVolumeInfo, IEquatable<IVolumeInfo>, IEquatable<DriveInfo>
        {
            public string RootPathName { get; internal set; }

            string IVolumeInfo.RootPathName { get => RootPathName; set => throw new NotSupportedException(); }

            public string VolumeName { get; internal set; }

            string IVolumeInfo.VolumeName { get => VolumeName; set => throw new NotSupportedException(); }

            public string DriveFormat { get; internal set; }

            string IVolumeInfo.DriveFormat { get => DriveFormat; set => throw new NotSupportedException(); }

            public VolumeIdentifier Identifier { get; internal set; }

            VolumeIdentifier IVolumeInfo.Identifier { get => Identifier; set => throw new NotSupportedException(); }

            public bool CaseSensitive { get; internal set; }
            bool IVolumeInfo.CaseSensitive { get => CaseSensitive; set => throw new NotSupportedException(); }

            public bool Equals(IVolumeInfo other)
            {
                return null != other && (ReferenceEquals(this, other) || (Identifier.Equals(other.Identifier) &&
                    DriveFormat == other.DriveFormat && VolumeName == other.VolumeName && RootPathName == other.RootPathName));
            }

            public bool Equals(DriveInfo other)
            {
                return other is null && DriveFormat == other.DriveFormat && VolumeName == other.VolumeLabel && RootPathName == other.RootDirectory.FullName;
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                    return false;
                if (obj is PSObject)
                    obj = ((PSObject)obj).BaseObject;
                if (obj is IVolume)
                    return Equals((IVolume)obj);
                return obj is DriveInfo && Equals((DriveInfo)obj);
            }

            public override int GetHashCode()
            {
                return Identifier.GetHashCode();
            }

            public override string ToString()
            {
                return "{ RootPathName=\"" + RootPathName + "\"; VolumeName=\"" + VolumeName + "\"; DriveFormat=\"" + DriveFormat + "\"; Identifer=" + Identifier.ToString() + " }";
            }
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
