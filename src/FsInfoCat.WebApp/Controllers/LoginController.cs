using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;

namespace FsInfoCat.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly FsInfoCat.WebApp.Data.FsInfoDataContext _context;

        public LoginController(FsInfoCat.WebApp.Data.FsInfoDataContext context)
        {
            _context = context;
        }

        // GET: api/Login/admin/{password}
        [HttpGet("{userName}/{password}")]
        public async Task<ActionResult<RegisteredUser>> Login(string userName, string password)
        {
            RegisteredUser user = _context.RegisteredUser.FirstOrDefault(u => u.LoginName == userName);
            if (null == user || user.IsInactive || !CheckPwHash(user.PwHash, password))
                return null;
            var claims = new List<Claim>
            {
                new Claim("user", user.LoginName),
                new Claim("role", user.Role)
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
            return user;
        }

        // GET: api/Login
        [HttpGet]
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

        public static bool CheckPwHash(string pwHash, string rawPw)
        {
            if (string.IsNullOrEmpty(pwHash))
                return string.IsNullOrEmpty(rawPw);
            if (pwHash.Length != 80 || string.IsNullOrEmpty(rawPw))
                return false;
            byte[] salt = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                if (int.TryParse(pwHash.Substring(i << 1, 2), NumberStyles.HexNumber, null, out int s))
                    salt[i] = (byte)s;
                else
                    return false;
            }
            byte[] hash = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                if (int.TryParse(pwHash.Substring(16 + (i << 1), 2), NumberStyles.HexNumber, null, out int s))
                    hash[i] = (byte)s;
                else
                    return false;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(rawPw);
            using (SHA256 sha = SHA256.Create())
            {
                sha.ComputeHash(salt.Concat(bytes).ToArray());
                for (int i = 0; i < 32; i++)
                {
                    if (sha.Hash[i] != hash[i])
                        return false;
                }
            }
            return true;
        }

        // POST: api/Login/create
        [HttpPost("create")]
        public async Task<ActionResult<RegisteredUser>> Create(string userName, string password, bool isAdmin)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || null == User || null == User.Identity || !User.Identity.IsAuthenticated || User.Claims.First(c => c.Subject.Name == "role").Value != "Admin")
                return await Task.FromResult((RegisteredUser)null);

            RegisteredUser user = _context.RegisteredUser.FirstOrDefault(u => string.Equals(u.LoginName, userName, StringComparison.InvariantCultureIgnoreCase));
            if (null != user)
                return null;
            user = new RegisteredUser();
            user.CreatedBy = user.ModifiedBy = User.Claims.First(c => c.Subject.Name == "user").Value;
            user.CreatedOn = user.ModifiedOn = DateTime.Now;
            user.Role = (isAdmin) ? "Admin" : "Member";
            user.PwHash = GetPwHash(password);
            await _context.RegisteredUser.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
