using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.Data.SqlClient;
using FsInfoCat.Models.Accounts;
using FsInfoCat.Models.HostDevices;
using FsInfoCat.Util;

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

        public static async Task<ActionResult<RequestResponse<Account>>> Login(UserLoginRequest request, FsInfoDataContext dbContext, HttpContext httpContext, ILogger logger)
        {
            Account user;
            try
            {
                user = dbContext.Account.Include<Account, UserCredential>(t => t.UserCredential).FirstOrDefault(u => u.LoginName == request.LoginName);
            }
            catch (SqlException exc)
            {
                string message = SqlHelper.GetErrorMessages(exc);
                // TODO: Log exception
                return new RequestResponse<Account>(null, message);
            }
            catch (Exception exc)
            {
                // TODO: Log exception
                return new RequestResponse<Account>(null, (string.IsNullOrWhiteSpace(exc.Message)) ?
                    "An unexpected " + exc.GetType().Name + " occurred while accessing the database." :
                    "An unexpected error occurred while accessing the database: " + exc.Message);
            }
            if (user is null || user.UserCredential is null || !CheckPwHash(user.UserCredential.HashString, request.Password))
                return new RequestResponse<Account>(null, "Invalid username or password");

            if (user.Role == UserRole.None)
                return new RequestResponse<Account>(null, "Your account has been disabled");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountID.ToString("n")),
                new Claim(ClaimTypes.Name, (string.IsNullOrWhiteSpace(user.DisplayName)) ? user.LoginName : user.DisplayName)
            };
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(r => r != UserRole.None && r <= user.Role))
                claims.Add(new Claim(ClaimTypes.Role, role.ToString("F")));
#warning Need to find some other way to retrieve device reg for local host
            // HostDeviceRegRequest deviceReg = HostDeviceRegRequest.CreateForLocal();
            // HostDevice host = await ViewModelHelper.LookUp(dbContext.HostDevice, deviceReg.MachineName, deviceReg.MachineIdentifer);
            // if (null != host && host.AllowCrawl)
            // {
            //     if (user.Role >= UserRole.Crawler)
            //         claims.Add(new Claim(ClaimTypes.Role, ModelHelper.Role_Name_Host_Contrib));
            //     else
            //     {
            //         HostContributor c = await ViewModelHelper.Lookup(dbContext.HostContributor, user.AccountID, host.HostDeviceID);
            //         if (null != c)
            //             claims.Add(new Claim(ClaimTypes.Role, ModelHelper.Role_Name_Host_Contrib));
            //     }
            // }
            ClaimsPrincipal cp = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", ClaimTypes.NameIdentifier, ClaimTypes.Role));
            await httpContext.SignInAsync(cp);
            return new RequestResponse<Account>(new Account(user));
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
        public async Task<ActionResult<RequestResponse<Account>>> Login(UserLoginRequest request)
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

        // TODO: Replace this with using PwHash directly
        public static bool CheckPwHash(string pwHash, string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return string.IsNullOrWhiteSpace(pwHash);
            PwHash? hash;
            try { hash = PwHash.Import(pwHash); } catch { hash = null; }
            return hash.HasValue && hash.Value.Test(rawPw);
        }
    }
}
