using System;

namespace FsInfoCat.Desktop
{
    [Flags]
    public enum Win32_AccessMask : uint
    {
        /// <summary>
        /// No rights granted.
        /// </summary>
        NONE = 0b0_0000_0000_0000_0000_0000_0000,

        /// <summary>
        /// Grants the right to read data from the file or to list the contents of the directory.
        /// </summary>
        FILE_READ_DATA = 0b0_0000_0000_0000_0000_0000_0001,

        /// <summary>
        /// Grants the right to list the contents of the directory or to read data from the file.
        /// </summary>
        FILE_LIST_DIRECTORY = 0b0_0000_0000_0000_0000_0000_0001,

        /// <summary>
        /// Grants the right to write data to the file or to create a file in the directory.
        /// </summary>
        FILE_WRITE_DATA = 0b0_0000_0000_0000_0000_0000_0010,

        /// <summary>
        /// Grants the right to create a file in the directory or to write data to the file.
        /// </summary>
        FILE_ADD_FILE = 0b0_0000_0000_0000_0000_0000_0010,

        /// <summary>
        /// Grants the right to append data to the file or to create a subdirectory.
        /// </summary>
        FILE_APPEND_DATA = 0b0_0000_0000_0000_0000_0000_0100,

        /// <summary>
        /// Grants the right to create a subdirectory or to append data to the file.
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0b0_0000_0000_0000_0000_0000_0100,

        /// <summary>
        /// Grants the right to read extended attributes.
        /// </summary>
        FILE_READ_EA = 0b0_0000_0000_0000_0000_0000_1000,

        /// <summary>
        /// Grants the right to write extended attributes.
        /// </summary>
        FILE_WRITE_EA = 0b0_0000_0000_0000_0000_0001_0000,

        /// <summary>
        /// Grants the right to execute a file or to traverse the subdirectory.
        /// </summary>
        FILE_EXECUTE = 0b0_0000_0000_0000_0000_0010_0000,

        /// <summary>
        /// Grants the right to traverse the subdirectory or to execute a file.
        /// </summary>
        FILE_TRAVERSE = 0b0_0000000_0000_0000_0_0010_0000,

        /// <summary>
        /// Grants the right to delete a directory and all the files it contains (its children), even if the files are read-only.
        /// </summary>
        FILE_DELETE_CHILD = 0b0_0000_0000_0000_0000_0100_0000,

        /// <summary>
        /// Grants the right to read file attributes.
        /// </summary>
        FILE_READ_ATTRIBUTES = 0b0_0000_0000_0000_0000_1000_0000,

        /// <summary>
        /// Grants the right to change file attributes.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0b0_0000_0000_0000_0001_0000_0000,

        /// <summary>
        /// Grants the right to delete the object.
        /// </summary>
        DELETE = 0b0_0000_0001_0000_0000_0000_0000,

        /// <summary>
        /// Grants the right to read the information in the security descriptor for the object, not including the information in the SACL.
        /// </summary>
        READ_CONTROL = 0b0_0000_0010_0000_0000_0000_0000,

        /// <summary>
        /// Grants the right to modify the DACL in the object security descriptor for the object.
        /// </summary>
        WRITE_DAC = 0b0_0000_0100_0000_0000_0000_0000,

        /// <summary>
        /// Grants the right to change the owner in the security descriptor for the object.
        /// </summary>
        WRITE_OWNER = 0b0_0000_1000_0000_0000_0000_0000,

        /// <summary>
        /// Grants the right to use the object for synchronization.
        /// </summary>
        /// <remarks>This enables a process to wait until the object is in signaled state. Some object types do not support this access right.</remarks>
        SYNCHRONIZE = 0b0_0001_0000_0000_0000_0000_0000,

        /// <summary>
        /// Controls the ability to get or set the SACL in an object's security descriptor.
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0b1_0001_1111_0000_0001_1111_1111
    }
}
