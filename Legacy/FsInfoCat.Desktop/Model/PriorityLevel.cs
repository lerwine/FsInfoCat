namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.PriorityLevel")]
    public enum PriorityLevel : byte
    {
        Deferred = 0,
        Critical = 1,
        High = 2,
        Normal = 3,
        Low = 4,
    }
}
