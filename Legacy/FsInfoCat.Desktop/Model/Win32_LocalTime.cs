using System;
using System.Management;

namespace FsInfoCat.Desktop.Model
{
    public class Win32_LocalTime
    {
        private readonly ManagementObject _managementObject;

        public uint Day => (uint)_managementObject[nameof(Day)];

        public DayOfWeek DayOfWeek => (DayOfWeek)_managementObject[nameof(DayOfWeek)];

        public uint Hour => (uint)_managementObject[nameof(Hour)];

        public uint Milliseconds => (uint)_managementObject[nameof(Milliseconds)];

        public uint Minute => (uint)_managementObject[nameof(Minute)];

        public uint Month => (uint)_managementObject[nameof(Month)];

        public uint Quarter => (uint)_managementObject[nameof(Quarter)];

        public uint Second => (uint)_managementObject[nameof(Second)];

        public uint WeekInMonth => (uint)_managementObject[nameof(WeekInMonth)];

        public uint Year => (uint)_managementObject[nameof(Year)];

        public Win32_LocalTime(ManagementObject managementObject)
        {
            _managementObject = managementObject ?? throw new ArgumentNullException(nameof(managementObject));
        }
    }
}
