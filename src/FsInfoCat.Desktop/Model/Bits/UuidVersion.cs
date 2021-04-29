namespace FsInfoCat.Desktop.Model.Bits
{
    public enum UuidVersion : byte
    {
        Unknown = 0b0000_0000,
        TimeBased = 0b0000_0001,
        DceSecurity = 0b0000_0010,
        NameBasedMD5 = 0b0000_0011,
        Random = 0b0000_0100,
        NameBasedSHA1 = 0b0000_0101,
        Other6 = 0b0000_0110,
        Other7 = 0b0000_0111,
    }
}
