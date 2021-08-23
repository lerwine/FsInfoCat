using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
    public class SharedTagDefinition : SharedTagDefinitionRow, ILocalSharedTagDefinition
    {
        private HashSet<SharedFileTag> _fileTags = new();
        private HashSet<SharedSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<SharedVolumeTag> _volumeTags = new();

        public HashSet<SharedFileTag> FileTags
        {
            get => _fileTags;
            set => CheckHashSetChanged(_fileTags, value, h => _fileTags = h);
        }

        public HashSet<SharedSubdirectoryTag> SubdirectoryTags
        {
            get => _subdirectoryTags;
            set => CheckHashSetChanged(_subdirectoryTags, value, h => _subdirectoryTags = h);
        }

        public HashSet<SharedVolumeTag> VolumeTags
        {
            get => _volumeTags;
            set => CheckHashSetChanged(_volumeTags, value, h => _volumeTags = h);
        }

        IEnumerable<ILocalFileTag> ILocalTagDefinition.FileTags => FileTags.Cast<ILocalFileTag>();

        IEnumerable<ISharedFileTag> ISharedTagDefinition.FileTags => FileTags.Cast<ISharedFileTag>();

        IEnumerable<IFileTag> ITagDefinition.FileTags => FileTags.Cast<IFileTag>();

        IEnumerable<ILocalSharedFileTag> ILocalSharedTagDefinition.FileTags => FileTags.Cast<ILocalSharedFileTag>();

        IEnumerable<ILocalSubdirectoryTag> ILocalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSubdirectoryTag>();

        IEnumerable<ISharedSubdirectoryTag> ISharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISharedSubdirectoryTag>();

        IEnumerable<ISubdirectoryTag> ITagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISubdirectoryTag>();

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalSharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSharedSubdirectoryTag>();

        IEnumerable<ILocalVolumeTag> ILocalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalVolumeTag>();

        IEnumerable<ISharedVolumeTag> ISharedTagDefinition.VolumeTags => VolumeTags.Cast<ISharedVolumeTag>();

        IEnumerable<IVolumeTag> ITagDefinition.VolumeTags => VolumeTags.Cast<IVolumeTag>();

        IEnumerable<ILocalSharedVolumeTag> ILocalSharedTagDefinition.VolumeTags => VolumeTags.Cast<ILocalSharedVolumeTag>();
    }
}
