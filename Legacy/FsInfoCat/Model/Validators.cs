using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{

    public static class Validators
    {
        public static bool IsValidFileLength(long length, ValidationContext context, out ValidationResult result)
        {
            if (length < 0L)
            {
                result = new ValidationResult(ModelResources.ErrorMessage_FileLengthNegative);
                return false;
            }

            result = null;
            return true;
        }

        public static bool IsValidMD5Hash(byte[] data, ValidationContext context, out ValidationResult result)
        {
            if (data is null || data.Length == 0 || data.Length == UInt128.ByteSize)
            {

                result = null;
                return true;
            }
            result = new ValidationResult($"MD5 hash values should be {UInt128.ByteSize} bytes long.");
            return false;
        }
    }
}
