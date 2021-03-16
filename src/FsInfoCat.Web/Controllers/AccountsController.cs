using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FsInfoCat.Models;
using FsInfoCat.Models.DB;
using FsInfoCat.Web.Data;
using FsInfoCat.Models.Accounts;
using FsInfoCat.Util;

namespace FsInfoCat.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly FsInfoDataContext _context;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(FsInfoDataContext context, ILogger<AccountsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Accounts
        [Authorize(Roles = ModelHelper.ROLE_NAME_VIEWER)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Account.ToListAsync());
        }

        // GET: Accounts/Details/5
        [Authorize(Roles = ModelHelper.ROLE_NAME_VIEWER)]
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

        // GET: Accounts/Create
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
        public IActionResult Create()
        {
            return View(new EditAccount(null) { ChangePassword = true });
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
        public async Task<IActionResult> Create([Bind("DisplayName,LoginName,ChangePassword,Password,Confirm,Role,Notes")] EditAccount editAccount)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account(editAccount);
                Guid createdBy = account.CreatedBy = account.ModifiedBy = new Guid(User.Identity.Name);
                account.CreatedOn = account.ModifiedOn = DateTime.Now;
                account.AccountID = Guid.NewGuid();
                _context.Add(account);
                UserCredential userCredential = new UserCredential
                {
                    AccountID = account.AccountID,
                    CreatedBy = createdBy,
                    CreatedOn = account.CreatedOn,
                    ModifiedBy = createdBy,
                    ModifiedOn = account.CreatedOn
                };
                userCredential.SetPasswordHash(PwHash.Create(editAccount.Password));
                _context.Add(userCredential);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editAccount);
        }

        // GET: Accounts/Edit/5
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
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
            return View(new EditAccount(account));
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
        public async Task<IActionResult> Edit(Guid id, [Bind("AccountID,DisplayName,LoginName,ChangePassword,Password,Confirm,Role,Notes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] EditAccount editAccount)
        {
            if (id != editAccount.AccountID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Account account = new Account(editAccount);
                    account.ModifiedOn = DateTime.Now;
                    account.ModifiedBy = new Guid(User.Identity.Name);
                    if (editAccount.ChangePassword)
                    {
                        UserCredential userCredential = await ViewModelHelper.LookUp(_context.UserCredential, id);
                        Guid updatedBy = new Guid(User.Identity.Name);
                        if (userCredential is null)
                        {
                            userCredential = new UserCredential
                            {
                                AccountID = id,
                                CreatedBy = updatedBy,
                                CreatedOn = DateTime.Now,
                                ModifiedBy = updatedBy
                            };
                            userCredential.ModifiedOn = userCredential.CreatedOn;
                            userCredential.SetPasswordHash(PwHash.Create(editAccount.Password));
                            _context.Add(userCredential);
                        }
                        else
                        {
                            userCredential.ModifiedOn = DateTime.Now;
                            userCredential.ModifiedBy = updatedBy;
                            userCredential.SetPasswordHash(PwHash.Create(editAccount.Password));
                            _context.Update(userCredential);
                        }
                    }
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (AccountExists(id))
                        throw;
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editAccount);
        }

        // GET: Accounts/Delete/5
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
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

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ModelHelper.ROLE_NAME_APP_ADMIN)]
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

        private bool CredentialExists(Guid id)
        {
            return _context.UserCredential.Any(e => e.AccountID == id);
        }

        // /Accounts/Login
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
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ErrorMessage"] = "";
                ViewData["ShowError"] = false;
                return View(request);
            }

            ActionResult<RequestResponse<Account>> result = await FsInfoCat.Web.API.AuthController.Login(request, _context, HttpContext, _logger);
            if (result.Value.Success)
            {
                ViewData["ErrorMessage"] = "";
                ViewData["ReturnUrl"] = returnUrl;
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

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await FsInfoCat.Web.API.AuthController.Logout(User, HttpContext, _logger);
            ViewData["ErrorMessage"] = "";
            ViewData["ReturnUrl"] = "";
            ViewData["ShowError"] = false;
            return Redirect("/Accounts/Login");
        }
    }
}
