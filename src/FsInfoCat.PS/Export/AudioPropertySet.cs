using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class AudioPropertySet : ExportSet.ExtendedPropertySetBase
    {
        /// <summary>
        /// Gets the Compression Method.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.Compression"/>
        public string Compression { get; set; }

        /// <summary>
        /// Indicates the average data rate in Hz for the audio file in "bits per second".
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.EncodingBitrate"/>
        public uint? EncodingBitrate { get; set; }

        /// <summary>
        /// Indicates the format of the audio file.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.Format"/>
        public string Format { get; set; }

        /// <summary>
        /// Indicates whether Bit Rate of the audio is variable
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.IsVariableBitrate"/>
        public bool? IsVariableBitrate { get; set; }

        /// <summary>
        /// Indicates the audio sample rate for the audio file in "samples per second".
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.SampleRate"/>
        public uint? SampleRate { get; set; }

        /// <summary>
        /// Indicates the audio sample size for the audio file in "bits per sample".
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.SampleSize"/>
        public uint? SampleSize { get; set; }

        /// <summary>
        /// Gets the Stream Name
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.StreamName"/>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets the Stream Number
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemAudio.StreamNumber"/>
        public ushort? StreamNumber { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static AudioPropertySet Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
