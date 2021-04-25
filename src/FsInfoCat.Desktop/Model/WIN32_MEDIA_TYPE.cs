using System.ComponentModel;

namespace FsInfoCat.Desktop
{
    public enum WIN32_MEDIA_TYPE
    {
        /// <summary>
        /// Format is unknown
        /// </summary>
        [Description("Format is unknown")]
        Unknown = 0,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 1.2 MB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 1.2 MB - 512 bytes/sector")]
        Floppy5_25Inch1_2MB = 1,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 1.44 MB -512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 1.44 MB -512 bytes/sector")]
        Floppy3_5Inch1_44MB = 2,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 2.88 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 2.88 MB - 512 bytes/sector")]
        Floppy3_5Inch2_88MB = 3,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 20.8 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 20.8 MB - 512 bytes/sector")]
        Floppy3_5Inch20_8MB = 4,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 720 KB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 720 KB - 512 bytes/sector")]
        Floppy3_5Inch720KB = 5,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 360 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 360 KB - 512 bytes/sector")]
        Floppy5_25Inch360KB = 6,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 320 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 320 KB - 512 bytes/sector")]
        Floppy5_25Inch320KB512 = 7,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 320 KB - 1024 bytes/sector
        [Description("5 1/4-Inch Floppy Disk - 320 KB - 1024 bytes/sector")]
        Floppy5_25Inch320KB1024 = 8,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 180 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 180 KB - 512 bytes/sector")]
        Floppy5_25Inch180KB = 9,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 160 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 160 KB - 512 bytes/sector")]
        Floppy5_25Inch160KB = 10,

        /// <summary>
        /// Removable media other than floppy
        /// </summary>
        [Description("Removable media other than floppy")]
        OtherRemovable = 11,

        /// <summary>
        /// Fixed hard disk media
        /// </summary>
        [Description("Fixed hard disk media")]
        Fixed = 12,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 120 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 120 MB - 512 bytes/sector")]
        Floppy3_5Inch120MB = 13,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 640 KB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 640 KB - 512 bytes/sector")]
        Floppy3_5Inch640KB = 14,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 640 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 640 KB - 512 bytes/sector")]
        Floppy5_25Inch640KB = 15,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 720 KB - 512 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 720 KB - 512 bytes/sector")]
        Floppy5_25Inch720KB = 16,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 1.2 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 1.2 MB - 512 bytes/sector")]
        Floppy3_5Inch1_23MB512 = 17,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector")]
        Floppy3_5Inch1_23MB1024 = 18,

        /// <summary>
        /// 5 1/4-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector
        /// </summary>
        [Description("5 1/4-Inch Floppy Disk - 1.23 MB - 1024 bytes/sector")]
        Floppy5_25Inch1_23MB = 19,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 128 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 128 MB - 512 bytes/sector")]
        Floppy3_5Inch128MB = 20,

        /// <summary>
        /// 3 1/2-Inch Floppy Disk - 230 MB - 512 bytes/sector
        /// </summary>
        [Description("3 1/2-Inch Floppy Disk - 230 MB - 512 bytes/sector")]
        Floppy3_5Inch230MB = 21,

        /// <summary>
        /// 8-Inch Floppy Disk - 256 KB - 128 bytes/sector
        /// </summary>
        [Description("8-Inch Floppy Disk - 256 KB - 128 bytes/sector")]
        Floppy8Inch = 22
    }
}
