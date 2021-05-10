using FsInfoCat.Models.Volumes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Util
{
    public class FileUriKey<TVolume> : IFileUriKey<TVolume>
        where TVolume : class, IVolumeSetItem
    {
        public FileUriKey(TVolume volume, FileUri uri)
        {
            if (volume is null)
                throw new ArgumentNullException(nameof(volume));
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));
            if (!uri.IsUriOf(volume))
                throw new ArgumentOutOfRangeException(nameof(uri));
            Uri = uri;
            Volume = volume;
        }

        public TVolume Volume { get; }

        public FileUri Uri { get; }

        IVolumeSetItem IFileUriKey.Volume => Volume;

        public bool Equals(IFileUriKey other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!ReferenceEquals(Volume, other.Volume))
                return false;
            FileUri uriB = other.Uri;
            if (uriB is null)
                return false;
            if (ReferenceEquals(Uri, uriB))
                return true;
            int count = Uri.PathSegmentCount;
            if (uriB.PathSegmentCount != count)
                return false;
            IFileUriKey mountPointParentUri = Volume.MountPointParentUri;
            IVolumeSetItem parentVol;
            if (mountPointParentUri is null || (parentVol = mountPointParentUri.Volume) is null)
                return Uri.GetPathSegments().SequenceEqual(Uri.GetPathSegments(), Volume.GetNameComparer());
            int countV = Volume.RootUri.PathSegmentCount;
            FileUri uriA = Uri;
            if (count > countV)
            {
                IEqualityComparer<string> nameComparer = Volume.GetNameComparer();
                do
                {
                    if (!nameComparer.Equals(uriA.Name, uriB.Name))
                        return false;
                    uriA = uriA.Parent;
                    uriB = uriB.Parent;
                } while (--count > countV);
            }
            return parentVol.GetNameComparer().Equals(uriA.Name, uriB.Name) && uriB.IsUriOf(parentVol);
        }
    }
}
