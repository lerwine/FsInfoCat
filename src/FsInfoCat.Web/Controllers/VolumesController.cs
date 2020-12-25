using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;
using FsInfoCat.Web.Data;

namespace FsInfoCat.Web.Controllers
{
    public class VolumesController : Controller
    {
        private readonly FsInfoDataContext _context;

        public VolumesController(FsInfoDataContext context)
        {
            _context = context;
        }

        // GET: Volumes
        public async Task<IActionResult> Index()
        {
            var fsInfoDataContext = _context.Volume.Include(v => v.Host);
            return View(await fsInfoDataContext.ToListAsync());
        }

        // GET: Volumes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volume = await _context.Volume
                .Include(v => v.Host)
                .FirstOrDefaultAsync(m => m.VolumeID == id);
            if (volume == null)
            {
                return NotFound();
            }

            return View(volume);
        }

        // GET: Volumes/Create
        public IActionResult Create()
        {
            ViewData["HostID"] = new SelectList(_context.HostDevice, "HostID", "MachineName");
            return View();
        }

        // POST: Volumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolumeID,HostID,DisplayName,RootPathName,FileSystemName,VolumeName,SerialNumber,MaxNameLength,Flags,IsInactive,Notes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Volume volume)
        {
            if (ModelState.IsValid)
            {
                volume.VolumeID = Guid.NewGuid();
                _context.Add(volume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HostID"] = new SelectList(_context.HostDevice, "HostID", "MachineName", volume.HostID);
            return View(volume);
        }

        // GET: Volumes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volume = await _context.Volume.FindAsync(id);
            if (volume == null)
            {
                return NotFound();
            }
            ViewData["HostID"] = new SelectList(_context.HostDevice, "HostID", "MachineName", volume.HostID);
            return View(volume);
        }

        // POST: Volumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VolumeID,HostID,DisplayName,RootPathName,FileSystemName,VolumeName,SerialNumber,MaxNameLength,Flags,IsInactive,Notes,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Volume volume)
        {
            if (id != volume.VolumeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolumeExists(volume.VolumeID))
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
            ViewData["HostID"] = new SelectList(_context.HostDevice, "HostID", "MachineName", volume.HostID);
            return View(volume);
        }

        // GET: Volumes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volume = await _context.Volume
                .Include(v => v.Host)
                .FirstOrDefaultAsync(m => m.VolumeID == id);
            if (volume == null)
            {
                return NotFound();
            }

            return View(volume);
        }

        // POST: Volumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var volume = await _context.Volume.FindAsync(id);
            _context.Volume.Remove(volume);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolumeExists(Guid id)
        {
            return _context.Volume.Any(e => e.VolumeID == id);
        }
    }
}
