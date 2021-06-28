using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class AudioPropertySet : LocalDbEntity, ILocalAudioPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _compression;
        private readonly IPropertyChangeTracker<uint?> _encodingBitrate;
        private readonly IPropertyChangeTracker<string> _format;
        private readonly IPropertyChangeTracker<bool?> _isVariableBitrate;
        private readonly IPropertyChangeTracker<uint?> _sampleRate;
        private readonly IPropertyChangeTracker<uint?> _sampleSize;
        private readonly IPropertyChangeTracker<string> _streamName;
        private readonly IPropertyChangeTracker<ushort?> _streamNumber;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string Compression { get => _compression.GetValue(); set => _compression.SetValue(value); }
        public uint? EncodingBitrate { get => _encodingBitrate.GetValue(); set => _encodingBitrate.SetValue(value); }
        public string Format { get => _format.GetValue(); set => _format.SetValue(value); }
        public bool? IsVariableBitrate { get => _isVariableBitrate.GetValue(); set => _isVariableBitrate.SetValue(value); }
        public uint? SampleRate { get => _sampleRate.GetValue(); set => _sampleRate.SetValue(value); }
        public uint? SampleSize { get => _sampleSize.GetValue(); set => _sampleSize.SetValue(value); }
        public string StreamName { get => _streamName.GetValue(); set => _streamName.SetValue(value); }
        public ushort? StreamNumber { get => _streamNumber.GetValue(); set => _streamNumber.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public AudioPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _compression = AddChangeTracker<string>(nameof(Compression), null);
            _encodingBitrate = AddChangeTracker<uint?>(nameof(EncodingBitrate), null);
            _format = AddChangeTracker<string>(nameof(Format), null);
            _isVariableBitrate = AddChangeTracker<bool?>(nameof(IsVariableBitrate), null);
            _sampleRate = AddChangeTracker<uint?>(nameof(SampleRate), null);
            _sampleSize = AddChangeTracker<uint?>(nameof(SampleSize), null);
            _streamName = AddChangeTracker<string>(nameof(StreamName), null);
            _streamNumber = AddChangeTracker<ushort?>(nameof(StreamNumber), null);
        }
    }
}
