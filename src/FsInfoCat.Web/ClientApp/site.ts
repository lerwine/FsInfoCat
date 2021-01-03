type GUIDBytes = Uint8Array & { length: 16 };

interface RequestResponse<T>
{
    readonly Result: T;
    readonly Success: boolean;
    readonly Message: string;
}

enum UserRole {
    None = 0,
    Viewer = 1,
    User = 2,
    Crawler = 3,
    Admin = 4
}

interface IModficationAuditable {
    CreatedOn: Date;
    CreatedBy: GUIDBytes;
    Creator?: Account;
    ModifiedOn: Date;
    ModifiedBy: GUIDBytes;
    Modifier?: Account;
}

interface ILogin extends IModficationAuditable {
    LoginName: string;
}

interface IUserCredentials extends ILogin {
    Password: string;
}

type UserLoginRequest = IUserCredentials;

interface IAccount extends ILogin, IModficationAuditable {
    AccountID: GUIDBytes;
    DisplayName: string;
    Role: UserRole;
    Notes: string;
}

interface PwHash {
    readonly HashBits0: number;
    readonly HashBits1: number;
    readonly HashBits2: number;
    readonly HashBits3: number;
    readonly HashBits4: number;
    readonly HashBits5: number;
    readonly HashBits6: number;
    readonly HashBits7: number;
    readonly SaltBits: number;
}

interface UserCredential extends IModficationAuditable {
    AccountID: GUIDBytes;
    HashString: string;
    readonly PasswordHash?: PwHash;
}

interface Account extends IAccount {
    UserCredential?: UserCredential;
}
