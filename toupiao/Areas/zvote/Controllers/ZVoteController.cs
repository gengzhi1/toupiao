using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;
using toupiao.Data;

namespace toupiao.Areas.zvote.Controllers
{
    [Area("zvote")]
    public class ZVoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZVoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: zvote/ZVote
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZVote.ToListAsync());
        }

        // GET: zvote/ZVote/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVote = await _context.ZVote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zVote == null)
            {
                return NotFound();
            }

            return View(zVote);
        }

        // GET: zvote/ZVote/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: zvote/ZVote/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DOCreating,DOEnd,IsSaveOnly,ItemType")] ZVote zVote)
        {
            if (ModelState.IsValid)
            {
                zVote.Id = Guid.NewGuid();
                _context.Add(zVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zVote);
        }

        // GET: zvote/ZVote/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVote = await _context.ZVote.FindAsync(id);
            if (zVote == null)
            {
                return NotFound();
            }
            return View(zVote);
        }

        // POST: zvote/ZVote/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,DOCreating,DOEnd,IsSaveOnly,ItemType")] ZVote zVote)
        {
            if (id != zVote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZVoteExists(zVote.Id))
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
            return View(zVote);
        }

        // GET: zvote/ZVote/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVote = await _context.ZVote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zVote == null)
            {
                return NotFound();
            }

            return View(zVote);
        }

        // POST: zvote/ZVote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var zVote = await _context.ZVote.FindAsync(id);
            _context.ZVote.Remove(zVote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZVoteExists(Guid id)
        {
            return _context.ZVote.Any(e => e.Id == id);
        }
    }
}
