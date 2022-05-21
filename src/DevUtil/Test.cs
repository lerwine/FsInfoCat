using FsInfoCat;
using FsInfoCat.Collections;
using FsInfoCat.Model;
using FsInfoCat.Local.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DevUtil
{
    public class ILocalFileAccessError
    {
        public ILocalFile Target { get; }
        public ErrorCode ErrorCode { get; }
        public String Message { get; }
        public String Details { get; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ILocalFileAccessError error &&
                EqualityComparer<ILocalFile>.Default.Equals(Target, error.Target) &&
                ErrorCode == error.ErrorCode &&
                Message == error.Message &&
                Details == error.Details &&
                CreatedOn == error.CreatedOn &&
                ModifiedOn == error.ModifiedOn;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Target, ErrorCode, Message, Details, CreatedOn, ModifiedOn);
        }
    }
}
