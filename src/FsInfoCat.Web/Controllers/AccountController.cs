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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FsInfoCat.Models;
using FsInfoCat.Web.Data;

namespace FsInfoCat.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly FsInfoDataContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(FsInfoDataContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.Account.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PwHash,AccountID,DisplayName,LoginName,Role,Notes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.AccountID = Guid.NewGuid();
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PwHash,AccountID,DisplayName,LoginName,Role,Notes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Account account)
        {
            if (id != account.AccountID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.AccountID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(Guid id)
        {
            return _context.Account.Any(e => e.AccountID == id);
        }

        // /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["ErrorMessage"] = "";
            ViewData["ShowError"] = false;
            return View();
        }

        [HttpPost, ActionName("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequest request, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(request);

            ActionResult<RequestResponse<AppUser>> result = await FsInfoCat.Web.API.AuthController.Login(request, _context, HttpContext, _logger);
            if (result.Value.Success)
            {
                ViewData["ErrorMessage"] = "";
                ViewData["ShowError"] = false;
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");
            }
            ViewData["ErrorMessage"] = result.Value.Message;
            ViewData["ShowError"] = true;
            ViewData["ReturnUrl"] = returnUrl;
            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await FsInfoCat.Web.API.AuthController.Logout(User, HttpContext, _logger);
            return Redirect("/Account/Login");
        }
    }
}
