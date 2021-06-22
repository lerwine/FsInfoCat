using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum DeviceOperationalState
    {
        [Description("Other")]
        Other = 1,


        [Description("Unknown")]
        Unknown = 2,


        [Description("Enabled")]
        Enabled = 3,


        [Description("Disabled")]
        Disabled = 4,


        [Description("Not Applicable")]
        NotApplicable = 5,
    }
}
