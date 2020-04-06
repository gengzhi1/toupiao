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
    public class ZVoteItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZVoteItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: zvote/ZVoteItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZVoteItem.ToListAsync());
        }

        // GET: zvote/ZVoteItem/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVoteItem = await _context.ZVoteItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zVoteItem == null)
            {
                return NotFound();
            }

            return View(zVoteItem);
        }

        // GET: zvote/ZVoteItem/Create/Id
        public IActionResult Create(ZVote zVote)
        {
            Console.WriteLine(zVote);
            var _zVoteItem = new ZVoteItem()
            {
                ForZVote = zVote
            };
            return View(_zVoteItem);
        }

        // POST: zvote/ZVoteItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemSource,Description")] ZVoteItem zVoteItem)
        {
            if (ModelState.IsValid)
            {
                zVoteItem.Id = Guid.NewGuid();
                _context.Add(zVoteItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zVoteItem);
        }

        // GET: zvote/ZVoteItem/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVoteItem = await _context.ZVoteItem.FindAsync(id);
            if (zVoteItem == null)
            {
                return NotFound();
            }
            return View(zVoteItem);
        }

        // POST: zvote/ZVoteItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ItemSource,Description")] ZVoteItem zVoteItem)
        {
            if (id != zVoteItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zVoteItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZVoteItemExists(zVoteItem.Id))
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
            return View(zVoteItem);
        }

        // GET: zvote/ZVoteItem/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zVoteItem = await _context.ZVoteItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zVoteItem == null)
            {
                return NotFound();
            }

            return View(zVoteItem);
        }

        // POST: zvote/ZVoteItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var zVoteItem = await _context.ZVoteItem.FindAsync(id);
            _context.ZVoteItem.Remove(zVoteItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZVoteItemExists(Guid id)
        {
            return _context.ZVoteItem.Any(e => e.Id == id);
        }
    }
}
