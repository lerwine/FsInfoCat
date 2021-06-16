using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IExtendedProperties : IDbEntity
    {
        Guid Id { get; set; }
        string Kind { get; set; }
        /*
         * What:
         // string Title { get; 
         // string Subject { get; }
         // string StreamName { get; } // PropertySystemAudio
         // string AlbumTitle { get; } // PropertySystemMusic
         // string StreamName { get; } // PropertySystemVideo
        
         // string Subtitle { get; } // PropertySystemMedia
         // string RevisionNumber { get; } // PropertySystemDocument
         // string Version { get; } // PropertySystemDocument
         // string FileVersion { get; }
         // string ImageID { get; } // PropertySystemImage
        
         // ushort? StreamNumber { get; } // PropertySystemAudio
         // ushort? StreamNumber { get; } // PropertySystemVideo
         // uint? TrackNumber { get; } // PropertySystemMusic

         // string ApplicationName { get; }
         // string ProductName { get; } // PropertySystemSoftware

         // string[] Author { get; }
         // string[] ItemAuthors { get; }
         // string[] Contributor { get; } // PropertySystemDocument
         // string LastAuthor { get; } // PropertySystemDocument
         // string Manager { get; } // PropertySystemDocument
         // string[] Producer { get; } // PropertySystemMedia
         // string[] Writer { get; } // PropertySystemMedia
         // string Publisher { get; } // PropertySystemMedia
         // string DisplayArtist { get; } // PropertySystemMusic
         // string[] Artist { get; } // PropertySystemMusic
         // string[] Composer { get; } // PropertySystemMusic
         // string[] Conductor { get; } // PropertySystemMusic
         // string[] Director { get; } // PropertySystemVideo

         // string Company { get; 
         // string Division { get; } // PropertySystemDocument
        
         // string[] Event { get; } // PropertySystemPhoto
         // string[] PeopleNames { get; } // PropertySystemPhoto

         // string Copyright { get; }

         // int? PerceivedType { get; }
         // string ContentType { get; }

         // uint? EncodingBitrate { get; } // PropertySystemAudio
         // uint? EncodingBitrate { get; } // PropertySystemVideo
         // bool? IsVariableBitrate { get; } // PropertySystemAudio
         // uint? BitDepth { get; } // PropertySystemImage

         // uint? SampleRate { get; } // PropertySystemAudio
         // uint? SampleSize { get;  // PropertySystemAudio
         // double? HorizontalResolution { get; } // PropertySystemImage
         // double? VerticalResolution { get; } // PropertySystemImage
         // uint? FrameRate { get; } // PropertySystemVideo
        
         // string Language { get; }

         // string Format { get; } // PropertySystemAudio

         // System.DateTime? DateCreated { get; } // PropertySystemDocument
         // uint? Year { get; } // PropertySystemMedia
         // System.DateTime? DateTaken { get; } // PropertySystemPhoto
         * 
         // uint? HorizontalSize { get; } // PropertySystemImage
         // uint? VerticalSize { get; } // PropertySystemImage
         // uint? FrameHeight { get; } // PropertySystemVideo
         // uint? FrameWidth { get; } // PropertySystemVideo
         * 
         // ulong? Duration { get; } // PropertySystemMedia
         // uint? FrameCount { get; } // PropertySystemMedia

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
        IEnumerable<IFile> Files { get; }
    }
}
