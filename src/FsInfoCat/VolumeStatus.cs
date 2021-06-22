namespace FsInfoCat
{
    public enum VolumeStatus : byte
    {
        Unknown = 0,
        Controlled = 1,
        AccessError = 2,
        Uncontrolled = 3,
        Offline = 4,
        Relinquished = 5,
        Destroyed = 6
    }
}
