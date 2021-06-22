using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    [Obsolete("Use other I*Properties interfaces")]
    public interface IExtendedProperties : IDbEntity
    {
        Guid Id { get; set; }
        string Kind { get; set; }
        /*
         * What:
         // string Title { get; 
         // string Subject { get; set; }
         // string StreamName { get; set; } // PropertySystemAudio
         // string AlbumTitle { get; set; } // PropertySystemMusic
         // string StreamName { get; set; } // PropertySystemVideo
        
         // string Subtitle { get; set; } // PropertySystemMedia
         // string RevisionNumber { get; set; } // PropertySystemDocument
         // string Version { get; set; } // PropertySystemDocument
         // string FileVersion { get; set; }
         // string ImageID { get; set; } // PropertySystemImage
        
         // ushort? StreamNumber { get; set; } // PropertySystemAudio
         // ushort? StreamNumber { get; set; } // PropertySystemVideo
         // uint? TrackNumber { get; set; } // PropertySystemMusic

         // string ApplicationName { get; set; }
         // string ProductName { get; set; } // PropertySystemSoftware

         // string[] Author { get; set; }
         // string[] ItemAuthors { get; set; }
         // string[] Contributor { get; set; } // PropertySystemDocument
         // string LastAuthor { get; set; } // PropertySystemDocument
         // string Manager { get; set; } // PropertySystemDocument
         // string[] Producer { get; set; } // PropertySystemMedia
         // string[] Writer { get; set; } // PropertySystemMedia
         // string Publisher { get; set; } // PropertySystemMedia
         // string DisplayArtist { get; set; } // PropertySystemMusic
         // string[] Artist { get; set; } // PropertySystemMusic
         // string[] Composer { get; set; } // PropertySystemMusic
         // string[] Conductor { get; set; } // PropertySystemMusic
         // string[] Director { get; set; } // PropertySystemVideo

         // string Company { get; 
         // string Division { get; set; } // PropertySystemDocument
        
         // string[] Event { get; set; } // PropertySystemPhoto
         // string[] PeopleNames { get; set; } // PropertySystemPhoto

         // string Copyright { get; set; }

         // int? PerceivedType { get; set; }
         // string ContentType { get; set; }

         // uint? EncodingBitrate { get; set; } // PropertySystemAudio
         // uint? EncodingBitrate { get; set; } // PropertySystemVideo
         // bool? IsVariableBitrate { get; set; } // PropertySystemAudio
         // uint? BitDepth { get; set; } // PropertySystemImage

         // uint? SampleRate { get; set; } // PropertySystemAudio
         // uint? SampleSize { get;  // PropertySystemAudio
         // double? HorizontalResolution { get; set; } // PropertySystemImage
         // double? VerticalResolution { get; set; } // PropertySystemImage
         // uint? FrameRate { get; set; } // PropertySystemVideo
        
         // string Language { get; set; }

         // string Format { get; set; } // PropertySystemAudio

         // System.DateTime? DateCreated { get; set; } // PropertySystemDocument
         // uint? Year { get; set; } // PropertySystemMedia
         // System.DateTime? DateTaken { get; set; } // PropertySystemPhoto
         * 
         // uint? HorizontalSize { get; set; } // PropertySystemImage
         // uint? VerticalSize { get; set; } // PropertySystemImage
         // uint? FrameHeight { get; set; } // PropertySystemVideo
         // uint? FrameWidth { get; set; } // PropertySystemVideo
         * 
         // ulong? Duration { get; set; } // PropertySystemMedia
         // uint? FrameCount { get; set; } // PropertySystemMedia

         * 
         * 
         */
        // TODO: Change to uint
        ushort? Width { get; set; }
        // TODO: Change to uint
        ushort? Height { get; set; }
        ulong? Duration { get; set; }
        uint? FrameCount { get; set; }
        uint? TrackNumber { get; set; }
        uint? Bitrate { get; set; }
        uint? FrameRate { get; set; }
        ushort? SamplesPerPixel { get; set; }
        uint? PixelPerUnitX { get; set; }
        uint? PixelPerUnitY { get; set; }
        ushort? Compression { get; set; }
        uint? XResNumerator { get; set; }
        uint? XResDenominator { get; set; }
        uint? YResNumerator { get; set; }
        uint? YResDenominator { get; set; }
        ushort? ResolutionXUnit { get; set; }
        ushort? ResolutionYUnit { get; set; }
        ushort? JPEGProc { get; set; }
        ushort? JPEGQuality { get; set; }
        DateTime? DateTime { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Copyright { get; set; }
        string SoftwareUsed { get; set; }
        string Artist { get; set; }
        string HostComputer { get; set; }
        IEnumerable<IFile> Files { get; set; }
    }
}
