using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;
using toupiao.Data;

namespace toupiao.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "ADMIN")]
    public class ZVotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZVotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ZVotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZVote.ToListAsync());
        }

        // GET: Admin/ZVotes/Details/5
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

        // GET: Admin/ZVotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ZVotes/Create
        // To protect from overposting attacks, enable the specific properties you
        // want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,DOCreating,DOStart,DOEnd,IsSaveOnly,Description,"+
            "XuanxiangA,XuanxiangB,XuanxiangC,XuanxiangD,IsLegal")] ZVote zVote)
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

        // GET: Admin/ZVotes/Edit/5
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

        // POST: Admin/ZVotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, 
            [Bind("Id,IsLegal")] ZVote zVote)
        {
            if (id != zVote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _zVote = await _context.ZVote.FindAsync(id);
                    _zVote.IsLegal = zVote.IsLegal;
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

        // GET: Admin/ZVotes/Delete/5
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

        // POST: Admin/ZVotes/Delete/5
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
