using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FsInfoCat.Models;

namespace FsInfoCat.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public const int Hash_Bytes_Length = 64;
        public const int Salt_Bytes_Length = 8;
        private readonly FsInfoCat.Web.Data.FsInfoDataContext _context;
        private readonly ILogger<HomeController> _logger;

        public AccountController(FsInfoCat.Web.Data.FsInfoDataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            RegisteredUser user;
            try
            {
                user = _context.RegisteredUser.FirstOrDefault(u => u.LoginName == userName);
            }
            catch (Exception exc)
            {
                // TODO: Log exception
                // TODO: Present error
                ViewData["ErrorMessage"] = (string.IsNullOrWhiteSpace(exc.Message)) ? "Unexpected " + exc.GetType().Name : exc.Message;
                return View();
            }
            if (null == user || !CheckPwHash(user.PwHash, password))
                return View();

            if (user.Role == UserRole.None)
                return View();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString("n")),
                new Claim(ClaimTypes.Name, (string.IsNullOrWhiteSpace(user.DisplayName)) ? user.LoginName : user.DisplayName)
            };
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(r => r != UserRole.None && r <= user.Role))
                claims.Add(new Claim(ClaimTypes.Role, role.ToString("F")));
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", ClaimTypes.NameIdentifier, ClaimTypes.Role)));
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return Redirect("/");
        }

        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
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
            if (pwHash.Length != RegisteredUser.Encoded_Pw_Hash_Length || string.IsNullOrEmpty(rawPw))
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
