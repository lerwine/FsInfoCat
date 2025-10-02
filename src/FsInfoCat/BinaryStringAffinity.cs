namespace FsInfoCat;

/// <summary>
/// Determines the order in which binary string representations are attempted to be parsed.
/// </summary>
public enum BinaryStringAffinity
{
    /// <summary>
    /// Parses all strings as UUID where possible, attempting BinHex next if not parsable as UUID.
    /// </summary>
    UUID_BinHex_B64,

    /// <summary>
    /// Parses all strings as UUID where possible, attempting Base64 next if not parsable as UUID.
    /// </summary>
    UUID_B64_BinHex,

    /// <summary>
    /// Parses all strings as Base64 where possible, attempting UUID next if not parsable as Base64.
    /// </summary>
    B64_UUID_BinHex,
    ///
    /// <summary>
    /// Parses all strings as Base64 where possible, attempting BinHex next if not parsable as Base64.
    /// </summary>
    B64_BinHex_UUID,

    /// <summary>
    /// Parses all strings as BinHex where possible, attempting Base64 next if not parsable as BinHex.
    /// </summary>
    BinHex_B64_UUID,
    ///
    /// <summary>
    /// Parses all strings as BinHex where possible, attempting UUID next if not parsable as BinHex.
    /// </summary>
    BinHex_UUID_B64
}
