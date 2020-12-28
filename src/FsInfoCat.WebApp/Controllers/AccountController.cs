using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;
using FsInfoCat.Models.DB;

namespace FsInfoCat.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly FsInfoCat.WebApp.Data.FsInfoDataContext _context;

        public AccountController(FsInfoCat.WebApp.Data.FsInfoDataContext context)
        {
            _context = context;
        }


        // POST: api/Account/login
        [HttpPost("login")]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<AppUser>>> Login(UserLoginRequest userLogin)
        {
            Account user = _context.Account.FirstOrDefault(u => u.LoginName == userLogin.LoginName);
            if (null == user || !CheckPwHash(user.PwHash, userLogin.Password))
                return new RequestResponse<AppUser>(null, "Invalid username or password");

            RequestResponse<AppUser> result = new RequestResponse<AppUser>(new AppUser(user), (user.Role != UserRole.None) ? "" : "User account is inactive");
            if (result.Success)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AccountID.ToString("n")),
                    new Claim(ClaimTypes.Name, (string.IsNullOrWhiteSpace(user.DisplayName)) ? user.LoginName : user.DisplayName)
                };
                foreach (UserRole role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(r => r != UserRole.None && r <= user.Role))
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString("F")));
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", ClaimTypes.NameIdentifier, ClaimTypes.Role)));
            }
            return result;
        }

        // GET: api/Account/logout
        [HttpGet("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        public static string GetPwHash(string pw)
        {
            if (string.IsNullOrEmpty(pw))
                return "";
            byte[] bytes = Encoding.ASCII.GetBytes(pw);
            byte[] salt = new byte[8];
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
                provider.GetBytes(salt);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in salt)
                sb.Append(b.ToString("x2"));
            using (SHA256 sha = SHA256.Create())
            {
                sha.ComputeHash(salt.Concat(bytes).ToArray());
                foreach (byte b in sha.Hash)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public const int Hash_Bytes_Length = 64;
        public const int Salt_Bytes_Length = 8;

        public static string ToPwHash(string rawPw, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(rawPw))
                return "";
            if (null == salt)
            {
                salt = new byte[Salt_Bytes_Length];
                using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                    cryptoServiceProvider.GetBytes(salt);
            }
            else if (salt.Length != Salt_Bytes_Length)
                throw new ArgumentException("Invalid salt length", "salt");
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(salt.Concat(Encoding.ASCII.GetBytes(rawPw)).ToArray());
                return Convert.ToBase64String(sha.Hash.Concat(salt).ToArray());
            }
        }

        public static bool CheckPwHash(string pwHash, string rawPw)
        {
            if (string.IsNullOrEmpty(pwHash))
                return string.IsNullOrEmpty(rawPw);
            if (pwHash.Length != Account.Encoded_Pw_Hash_Length || string.IsNullOrEmpty(rawPw))
                return false;
            byte[] hash;
            try
            {
                if ((hash = Convert.FromBase64String(pwHash)).Length != (Salt_Bytes_Length + Hash_Bytes_Length))
                    return false;
            }
            catch
            {
                return false;
            }
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(Encoding.ASCII.GetBytes(rawPw).Concat(hash.Skip(Hash_Bytes_Length)).ToArray());
                for (int i = 0; i < Hash_Bytes_Length; i++)
                {
                    if (sha.Hash[i] != hash[i])
                        return false;
                }
            }
            return true;
        }

        // POST: api/Account/create
        [HttpPost("create")]
        // [Authorize(Roles = AppUser.Role_Name_Admin)]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<RequestResponse<Account>>> Create(string userName, string password, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult(new RequestResponse<Account>(null, "Invalid credentials"));
            Account user = _context.Account.FirstOrDefault(u => string.Equals(u.LoginName, userName, StringComparison.InvariantCultureIgnoreCase));
            RequestResponse<Account> result = new RequestResponse<Account>(user, (null != user) ? "A user with that login name already exists" : "");
            if (result.Success)
            {
                result.Result = new Account(userName, ToPwHash(password), role, new Guid(User.Identity.Name));
                await _context.Account.AddAsync(result.Result);
                await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
