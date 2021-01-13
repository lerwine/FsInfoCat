BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "ConvertTo-PasswordHash -RawPwd '<Password>'" -ForEach @(
    @{
        Password = 'FirstT3s!P@$S\/\/0rd';
        SaltBits = ([System.UInt64]0x8DFE482564140EA1);
        HashBits000_03f = ([System.UInt64]0x8DFE482564140EA1);
        HashBits040_07f = ([System.UInt64]0x95D7139923D11D36);
        HashBits080_0bf = ([System.UInt64]0x0F011BAE6B6C82F7);
        HashBits0c0_0ff = ([System.UInt64]0x6C7AA4A845C59E97);
        HashBits100_13f = ([System.UInt64]0x3744F252821E9366);
        HashBits140_17f = ([System.UInt64]0x2ACED7805BF6E81E);
        HashBits180_1bf = ([System.UInt64]0xFBF6241405EAB187);
        HashBits1c0_1ff = ([System.UInt64]0x8CABD0D5A6C374B1);
        HashB64 = 'oQ4UZCVI/o02HdEjmRPXlfeCbGuuGwEPl57FRaikemxmkx6CUvJENx7o9luA184qh7HqBRQk9vuxdMOm1dCrjA==';
        ToString = 'oQ4UZCVI/o02HdEjmRPXlfeCbGuuGwEPl57FRaikemxmkx6CUvJENx7o9luA184qh7HqBRQk9vuxdMOm1dCrjIw6HMH0yxMf';
    },
    @{
        Password = '1@3$5^7*9)';
        SaltBits = ([System.UInt64]0xC822DC405F1658A2);
        HashBits000_03f = ([System.UInt64]0xC822DC405F1658A2);
        HashBits040_07f = ([System.UInt64]0x7EC4851ACAC83BE1);
        HashBits080_0bf = ([System.UInt64]0xA16A0F003CFF7163);
        HashBits0c0_0ff = ([System.UInt64]0xE5BFBF48E025877D);
        HashBits100_13f = ([System.UInt64]0x19C888606FC3DC0B);
        HashBits140_17f = ([System.UInt64]0xF9525A1C7D1BF260);
        HashBits180_1bf = ([System.UInt64]0x221DF17BFA3E7E47);
        HashBits1c0_1ff = ([System.UInt64]0x332B8E95AEA92FB7);
        HashB64 = 'olgWX0DcIsjhO8jKGoXEfmNx/zwAD2qhfYcl4Ei/v+UL3MNvYIjIGWDyG30cWlL5R34++nvxHSK3L6mulY4rMw==';
        ToString = 'olgWX0DcIsjhO8jKGoXEfmNx/zwAD2qhfYcl4Ei/v+UL3MNvYIjIGWDyG30cWlL5R34++nvxHSK3L6mulY4rM+dIocc72Lyw';
    },
    @{
        Password = ' ';
        SaltBits = ([System.UInt64]0x5F9E5EFB87CEAB29);
        HashBits000_03f = ([System.UInt64]0x5F9E5EFB87CEAB29);
        HashBits040_07f = ([System.UInt64]0x217342E0C2BF2635);
        HashBits080_0bf = ([System.UInt64]0xBD74A07400B3CF22);
        HashBits0c0_0ff = ([System.UInt64]0xC81E13F51B240C5B);
        HashBits100_13f = ([System.UInt64]0xB3B1E8E721E900E1);
        HashBits140_17f = ([System.UInt64]0xB89162CEB44C5831);
        HashBits180_1bf = ([System.UInt64]0x7176967DF6A44D9E);
        HashBits1c0_1ff = ([System.UInt64]0xD991EB8AAF6359CA);
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
