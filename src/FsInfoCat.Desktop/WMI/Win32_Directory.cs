using FsInfoCat.Local;
using System;
using System.Management;

namespace FsInfoCat.Desktop.WMI
{
    public class Win32_Directory : IVolumeDirectory
    {
        internal Win32_Directory(ManagementObject obj)
        {
            Name = obj.GetString(nameof(Name));
            Description = obj.GetString(nameof(Description));
            Caption = obj.GetString(nameof(Caption));
            Status = obj.GetString(nameof(Status));
            CSName = obj.GetString(nameof(CSName));
            Drive = obj.GetString(nameof(Drive));
            EightDotThreeFileName = obj.GetString(nameof(EightDotThreeFileName));
            FileName = obj.GetString(nameof(FileName));
            Path = obj.GetString(nameof(Path));
            FSName = obj.GetString(nameof(FSName));
            CreationDate = obj.GetDateTimeOpt(nameof(CreationDate));
            LastModified = obj.GetDateTimeOpt(nameof(LastModified));
            Archive = obj.GetBooleanOpt(nameof(Archive));
            Hidden = obj.GetBooleanOpt(nameof(Hidden));
            Readable = obj.GetBoolean(nameof(Readable));
            System = obj.GetBooleanOpt(nameof(System));
            Writeable = obj.GetBooleanOpt(nameof(Writeable));
        }

        public string Name { get; }

        public string Description { get; }

        public string Caption { get; }

        public string Status { get; }

        public string CSName { get; }

        public string Drive { get; }

        public string EightDotThreeFileName { get; }

        public string FileName { get; }

        public string Path { get; }

        public string FSName { get; }

        public DateTime? CreationDate { get; }

        public DateTime? LastModified { get; }

        public bool? Archive { get; }

        public bool? Hidden { get; }

        public bool Readable { get; }

        public bool? System { get; }

        public bool? Writeable { get; }
    }
}
