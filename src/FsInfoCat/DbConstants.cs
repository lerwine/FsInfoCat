namespace FsInfoCat
{
    public static class DbConstants
    {
        public const int DbColMaxLen_SimpleName = 256;
        public const int DbColMaxLen_LongName = 1024;
        public const int DbColMaxLen_ShortName = 128;
        public const int DbColMaxLen_Identifier = 1024;
        public const int DbColMaxLen_FileName = 1024;
        public const uint DbColDefaultValue_MaxNameLength = 255;
        public const ushort DbColMinValue_MaxTotalItems = 2;
        public const ushort DbColDefaultValue_MaxRecursionDepth = 256;
        public const long DbColMinValue_TTL = 60L;
        public const long DbColMinValue_RescheduleInterval = 900L;
    }

}

