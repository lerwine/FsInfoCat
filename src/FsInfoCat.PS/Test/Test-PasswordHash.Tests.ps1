BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "Test-PasswordHash -RawPwd '<Password>' -EncodedPwHash '<EncodedPwHash>'" -ForEach @(
    @{
        Password = 'FirstT3s!P@$S\/\/0rd';
        EncodedPwHash = 'QzY2tTqGtvLCBZuVNUAWlPGSTe9qJ4Vy7j3g4+M/osXqg01rr1ZLZZJHX3/esp7usc3dcb4efvYVqCRABPSardx4qT/hHCSP';
        ExpectedResult = True;
    },
    @{
        Password = 'FirstT3s!P@$S\/\/0rd';
        EncodedPwHash = 'NkULLrCsaU3QbLpIvlF2twCdLUS3nKvVAIOV9upRHaWNlTIwZkE7tqIognuK1v3yqs37SHJIRKuyCf1nVhiZtJ3VHnJG6ZAp';
        ExpectedResult = False;
    },
    @{
        Password = 'FirstT3s!P@$S\/\/0rd';
        EncodedPwHash = 'N7uuRU6dWa3z4O7DxslzRvTf/1n5HCnsfgK0fZXZ+t8oboxVP7kK57ycnMkHtQ69Rkx5jGWEAyh66/DdWINgaGOEmXn7EepX';
        ExpectedResult = False;
    },
    @{
        Password = '1@3$5^7*9)';
        EncodedPwHash = 'QzY2tTqGtvLCBZuVNUAWlPGSTe9qJ4Vy7j3g4+M/osXqg01rr1ZLZZJHX3/esp7usc3dcb4efvYVqCRABPSardx4qT/hHCSP';
        ExpectedResult = False;
    },
    @{
        Password = '1@3$5^7*9)';
        EncodedPwHash = 'NkULLrCsaU3QbLpIvlF2twCdLUS3nKvVAIOV9upRHaWNlTIwZkE7tqIognuK1v3yqs37SHJIRKuyCf1nVhiZtJ3VHnJG6ZAp';
        ExpectedResult = True;
    },
    @{
        Password = '1@3$5^7*9)';
        EncodedPwHash = 'N7uuRU6dWa3z4O7DxslzRvTf/1n5HCnsfgK0fZXZ+t8oboxVP7kK57ycnMkHtQ69Rkx5jGWEAyh66/DdWINgaGOEmXn7EepX';
        ExpectedResult = False;
    },
    @{
        Password = ' ';
        EncodedPwHash = 'QzY2tTqGtvLCBZuVNUAWlPGSTe9qJ4Vy7j3g4+M/osXqg01rr1ZLZZJHX3/esp7usc3dcb4efvYVqCRABPSardx4qT/hHCSP';
        ExpectedResult = False;
    },
    @{
        Password = ' ';
        EncodedPwHash = 'NkULLrCsaU3QbLpIvlF2twCdLUS3nKvVAIOV9upRHaWNlTIwZkE7tqIognuK1v3yqs37SHJIRKuyCf1nVhiZtJ3VHnJG6ZAp';
        ExpectedResult = False;
    },
    @{
        Password = ' ';
        EncodedPwHash = 'N7uuRU6dWa3z4O7DxslzRvTf/1n5HCnsfgK0fZXZ+t8oboxVP7kK57ycnMkHtQ69Rkx5jGWEAyh66/DdWINgaGOEmXn7EepX';
        ExpectedResult = True;
    }
) {
    It "Returns <Base64String>"  {
        $ActualResult = Test-PasswordHash -RawPwd $Password -EncodedPwHash $EncodedPwHash;
        $ActualResult | Should -Be $ExpectedResult;
    }
}
