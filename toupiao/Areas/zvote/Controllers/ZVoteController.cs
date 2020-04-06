using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;
using toupiao.Data;

namespace toupiao.Areas.zvote.Controllers
{
    [Area("zvote")]
    [Authorize]
    public class ZVoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ZVoteController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: zvote/ZVote
        public async Task<IActionResult> Index()
        {
            var _user = await _userManager.GetUserAsync(User);
            
            var _zVote = await _context.ZVote.Where(p=>p.Submitter == _user)
                    .ToListAsync();

            _zVote.First().ItemTypes = ZVoteMix.GetItemTypes();

            return View( _zVote );
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
        [ActionName("Create")]
        public IActionResult XinJian()
        {
            var _zvote = new ZVote()
            {
                ItemTypes = ZVoteMix.GetItemTypes(),
                DOStart = DateTimeOffset.Now
            };
            TempData[nameof(_zvote.DOStart)] = _zvote.DOStart?
                .ToUnixTimeMilliseconds().ToString();

            return View( _zvote );
        }

        // POST: zvote/ZVote/Create
        // To protect from overposting attacks, enable the specific properties 
        //  you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,DOEnd,DOStart,IsSaveOnly,ItemType")] 
            ZVote zVote
            )
        {

            if (ModelState.IsValid)
            {

                zVote.Submitter = await _userManager.GetUserAsync(User);
                zVote.DOCreating = DateTimeOffset.Now;
                zVote.Id = Guid.NewGuid();

                _context.Add(zVote);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(ZVoteItemController.Create),
                    nameof(ZVoteItem),
                    zVote.Id );

            }

            zVote.ItemTypes = ZVoteMix.GetItemTypes();
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
