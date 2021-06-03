using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public interface IExtendedProperties : IDbEntity
    {
        Guid Id { get; set; }
        ushort Width { get; set; }
        ushort Height { get; set; }
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
