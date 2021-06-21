using System;
using System.Collections.Generic;

namespace FsInfoCat.PS.Export
{
    public class MusicProperties : ExportSet.ExtendedPropertiesBase
    {
        /// <summary>
        /// Gets the Album Artist
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.AlbumArtist"/>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// Gets the Album Title
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.AlbumTitle"/>
        public string AlbumTitle { get; set; }

        /// <summary>
        /// Gets the Artist
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.Artist"/>
        public string[] Artist { get; set; }

        /// <summary>
        /// Gets the Composer
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.Composer"/>
        public string[] Composer { get; set; }

        /// <summary>
        /// Gets the Conductor
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.Conductor"/>
        public string[] Conductor { get; set; }

        /// <summary>
        /// This property returns the best representation of Album Artist for a given music file based upon AlbumArtist, ContributingArtist and compilation info.
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.DisplayArtist"/>
        public string DisplayArtist { get; set; }

        /// <summary>
        /// Gets the Genre
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.Genre"/>
        public string[] Genre { get; set; }

        /// <summary>
        /// Gets the Part of Set
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.PartOfSet"/>
        public string PartOfSet { get; set; }

        /// <summary>
        /// Gets the Period
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.Period"/>
        public string Period { get; set; }

        /// <summary>
        /// Gets the Track Number
        /// </summary>
        /// <seealso cref="Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties.PropertySystemMusic.TrackNumber"/>
        public uint? TrackNumber { get; set; }

        public IEnumerable<File> GetFiles() => throw new NotImplementedException();

        internal static MusicProperties Create(System.IO.FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
