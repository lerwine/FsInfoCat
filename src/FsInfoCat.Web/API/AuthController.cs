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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;
using FsInfoCat.Models.DB;
using FsInfoCat.Web.Data;

namespace FsInfoCat.Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        public const int Hash_Bytes_Length = 64;
        public const int Salt_Bytes_Length = 8;
        private readonly FsInfoDataContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(FsInfoDataContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public static async Task<ActionResult<RequestResponse<AppUser>>> Login(UserLoginRequest request, FsInfoDataContext dbContext, HttpContext httpContext, ILogger logger)
        {
            Account user;
            try
            {
                user = dbContext.Account.FirstOrDefault(u => u.LoginName == request.LoginName);
            }
            catch (Exception exc)
            {
                // TODO: Log exception
                return new RequestResponse<AppUser>(null, (string.IsNullOrWhiteSpace(exc.Message)) ?
                    "An unexpected " + exc.GetType().Name + " occurred while accessing the database." :
                    "An unexpected error occurred while accessing the database: " + exc.Message);
            }
            if (null == user || !CheckPwHash(user.PwHash, request.Password))
                return new RequestResponse<AppUser>(null, "Invalid username or password");

            if (user.Role == UserRole.None)
                return new RequestResponse<AppUser>(null, "Your account has been disabled");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountID.ToString("n")),
                new Claim(ClaimTypes.Name, (string.IsNullOrWhiteSpace(user.DisplayName)) ? user.LoginName : user.DisplayName)
            };
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(r => r != UserRole.None && r <= user.Role))
                claims.Add(new Claim(ClaimTypes.Role, role.ToString("F")));
            await httpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", ClaimTypes.NameIdentifier, ClaimTypes.Role)));
            return new RequestResponse<AppUser>(new AppUser(user));
        }

        public static async Task<ActionResult<RequestResponse<bool>>> Logout(ClaimsPrincipal user, HttpContext httpContext, ILogger logger)
        {
            if (null != user && null != user.Identity && user.Identity.IsAuthenticated)
            {
                await httpContext.SignOutAsync();
                return new RequestResponse<bool>(true);
            }
            return new RequestResponse<bool>(false);
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RequestResponse<AppUser>>> Login(UserLoginRequest request)
        {
            return await Login(request, _context, HttpContext, _logger);
        }

        // POST: /api/Auth/Logout
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RequestResponse<bool>>> Logout()
        {
            return await Logout(User, HttpContext, _logger);
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
    }
}
