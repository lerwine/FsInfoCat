namespace FsInfoCat
{
    public enum VolumeStatus : byte
    {
        Unknown = 0,
        Controlled = 1,
        Uncontrolled = 2,
        Offline = 3,
        Relinquished = 4,
        Destroyed = 5
    }
}
