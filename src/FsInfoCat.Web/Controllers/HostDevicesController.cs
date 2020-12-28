using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models.DB;
using FsInfoCat.Web.Data;

namespace FsInfoCat.Web.Controllers
{
    public class HostDevicesController : Controller
    {
        private readonly FsInfoDataContext _context;

        public HostDevicesController(FsInfoDataContext context)
        {
            _context = context;
        }

        // GET: HostDevices
        public async Task<IActionResult> Index()
        {
            return View(await _context.HostDevice.ToListAsync());
        }

        // GET: HostDevices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostDevice = await _context.HostDevice
                .FirstOrDefaultAsync(m => m.HostID == id);
            if (hostDevice == null)
            {
                return NotFound();
            }

            return View(hostDevice);
        }

        // GET: HostDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HostDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HostID,DisplayName,MachineIdentifer,MachineName,IsWindows,IsInactive,Notes,CreatedOn,ModifiedOn")] HostDevice hostDevice)
        {
            if (ModelState.IsValid)
            {
                hostDevice.HostID = Guid.NewGuid();
                _context.Add(hostDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hostDevice);
        }

        // GET: HostDevices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostDevice = await _context.HostDevice.FindAsync(id);
            if (hostDevice == null)
            {
                return NotFound();
            }
            return View(hostDevice);
        }

        // POST: HostDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("HostID,DisplayName,MachineIdentifer,MachineName,IsWindows,IsInactive,Notes,CreatedOn,ModifiedOn")] HostDevice hostDevice)
        {
            if (id != hostDevice.HostID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hostDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HostDeviceExists(hostDevice.HostID))
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
            return View(hostDevice);
        }

        // GET: HostDevices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hostDevice = await _context.HostDevice
                .FirstOrDefaultAsync(m => m.HostID == id);
            if (hostDevice == null)
            {
                return NotFound();
            }

            return View(hostDevice);
        }

        // POST: HostDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var hostDevice = await _context.HostDevice.FindAsync(id);
            _context.HostDevice.Remove(hostDevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HostDeviceExists(Guid id)
        {
            return _context.HostDevice.Any(e => e.HostID == id);
        }
    }
}
