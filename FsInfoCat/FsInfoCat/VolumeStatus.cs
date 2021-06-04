namespace FsInfoCat
{
    public enum VolumeStatus : byte
    {
        Unknown = 0,
        Controlled = 1,
#warning Add value for AccessError
        Uncontrolled = 2,
        Offline = 3,
        Relinquished = 4,
        Destroyed = 5
    }
}
