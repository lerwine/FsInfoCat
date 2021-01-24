BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "ConvertTo-PasswordHash -RawPwd '<Password>'" -ForEach @(
    @{
        Password = 'FirstT3s!P@$S\/\/0rd';
        SaltBits = ([System.UInt64]::Parse('8DFE482564140EA1', [System.Globalization.NumberStyles]::HexNumber));
        HashBits000_03f = ([System.UInt64]::Parse('8DFE482564140EA1', [System.Globalization.NumberStyles]::HexNumber));
        HashBits040_07f = ([System.UInt64]::Parse('95D7139923D11D36', [System.Globalization.NumberStyles]::HexNumber));
        HashBits080_0bf = ([System.UInt64]::Parse('0F011BAE6B6C82F7', [System.Globalization.NumberStyles]::HexNumber));
        HashBits0c0_0ff = ([System.UInt64]::Parse('6C7AA4A845C59E97', [System.Globalization.NumberStyles]::HexNumber));
        HashBits100_13f = ([System.UInt64]::Parse('3744F252821E9366', [System.Globalization.NumberStyles]::HexNumber));
        HashBits140_17f = ([System.UInt64]::Parse('2ACED7805BF6E81E', [System.Globalization.NumberStyles]::HexNumber));
        HashBits180_1bf = ([System.UInt64]::Parse('FBF6241405EAB187', [System.Globalization.NumberStyles]::HexNumber));
        HashBits1c0_1ff = ([System.UInt64]::Parse('8CABD0D5A6C374B1', [System.Globalization.NumberStyles]::HexNumber));
        HashB64 = 'oQ4UZCVI/o02HdEjmRPXlfeCbGuuGwEPl57FRaikemxmkx6CUvJENx7o9luA184qh7HqBRQk9vuxdMOm1dCrjA==';
        ToString = 'oQ4UZCVI/o02HdEjmRPXlfeCbGuuGwEPl57FRaikemxmkx6CUvJENx7o9luA184qh7HqBRQk9vuxdMOm1dCrjIw6HMH0yxMf';
    },
    @{
        Password = '1@3$5^7*9)';
        SaltBits = ([System.UInt64]::Parse('C822DC405F1658A2', [System.Globalization.NumberStyles]::HexNumber));
        HashBits000_03f = ([System.UInt64]::Parse('C822DC405F1658A2', [System.Globalization.NumberStyles]::HexNumber));
        HashBits040_07f = ([System.UInt64]::Parse('7EC4851ACAC83BE1', [System.Globalization.NumberStyles]::HexNumber));
        HashBits080_0bf = ([System.UInt64]::Parse('A16A0F003CFF7163', [System.Globalization.NumberStyles]::HexNumber));
        HashBits0c0_0ff = ([System.UInt64]::Parse('E5BFBF48E025877D', [System.Globalization.NumberStyles]::HexNumber));
        HashBits100_13f = ([System.UInt64]::Parse('19C888606FC3DC0B', [System.Globalization.NumberStyles]::HexNumber));
        HashBits140_17f = ([System.UInt64]::Parse('F9525A1C7D1BF260', [System.Globalization.NumberStyles]::HexNumber));
        HashBits180_1bf = ([System.UInt64]::Parse('221DF17BFA3E7E47', [System.Globalization.NumberStyles]::HexNumber));
        HashBits1c0_1ff = ([System.UInt64]::Parse('332B8E95AEA92FB7', [System.Globalization.NumberStyles]::HexNumber));
        HashB64 = 'olgWX0DcIsjhO8jKGoXEfmNx/zwAD2qhfYcl4Ei/v+UL3MNvYIjIGWDyG30cWlL5R34++nvxHSK3L6mulY4rMw==';
        ToString = 'olgWX0DcIsjhO8jKGoXEfmNx/zwAD2qhfYcl4Ei/v+UL3MNvYIjIGWDyG30cWlL5R34++nvxHSK3L6mulY4rM+dIocc72Lyw';
    },
    @{
        Password = ' ';
        SaltBits = ([System.UInt64]::Parse('5F9E5EFB87CEAB29', [System.Globalization.NumberStyles]::HexNumber));
        HashBits000_03f = ([System.UInt64]::Parse('5F9E5EFB87CEAB29', [System.Globalization.NumberStyles]::HexNumber));
        HashBits040_07f = ([System.UInt64]::Parse('217342E0C2BF2635', [System.Globalization.NumberStyles]::HexNumber));
        HashBits080_0bf = ([System.UInt64]::Parse('BD74A07400B3CF22', [System.Globalization.NumberStyles]::HexNumber));
        HashBits0c0_0ff = ([System.UInt64]::Parse('C81E13F51B240C5B', [System.Globalization.NumberStyles]::HexNumber));
        HashBits100_13f = ([System.UInt64]::Parse('B3B1E8E721E900E1', [System.Globalization.NumberStyles]::HexNumber));
        HashBits140_17f = ([System.UInt64]::Parse('B89162CEB44C5831', [System.Globalization.NumberStyles]::HexNumber));
        HashBits180_1bf = ([System.UInt64]::Parse('7176967DF6A44D9E', [System.Globalization.NumberStyles]::HexNumber));
        HashBits1c0_1ff = ([System.UInt64]::Parse('D991EB8AAF6359CA', [System.Globalization.NumberStyles]::HexNumber));
        HashB64 = 'KavOh/tenl81Jr/C4EJzISLPswB0oHS9WwwkG/UTHsjhAOkh5+ixszFYTLTOYpG4nk2k9n2WdnHKWWOviuuR2Q==';
        ToString = 'KavOh/tenl81Jr/C4EJzISLPswB0oHS9WwwkG/UTHsjhAOkh5+ixszFYTLTOYpG4nk2k9n2WdnHKWWOviuuR2WEQqayuzCXQ';
    }
) {
    It "Returns <Base64String>"  {
        $PwHash = ConvertTo-PasswordHash -RawPwd $Password;
        $PwHash.ToString() | Should -Be $Base64String;
        $PwHash.HashBits000_03f | Should -Be $HashBits000_03f;
        $PwHash.HashBits040_07f | Should -Be $HashBits040_07f;
        $PwHash.HashBits080_0bf | Should -Be $HashBits080_0bf;
        $PwHash.HashBits0c0_0ff | Should -Be $HashBits0c0_0ff;
        $PwHash.HashBits100_13f | Should -Be $HashBits100_13f;
        $PwHash.HashBits140_17f | Should -Be $HashBits140_17f;
        $PwHash.HashBits180_1bf | Should -Be $HashBits180_1bf;
        $PwHash.HashBits1c0_1ff | Should -Be $HashBits1c0_1ff;
        $Value = [System.BitConverter]::ToUInt64($PwHash.GetSaltBytes(), 0);
        $Value | Should -Be $SaltBits;
        $B64 = [System.Convert]::ToBase64String($PwHash.GetSha512HashBytes());
        $B64 | Should -Be $HashB64;
        $B64 = [System.Convert]::ToBase64String($PwHash.GetHashAndSaltBytes());
        $B64 | Should -Be $ToString;
    }
}
