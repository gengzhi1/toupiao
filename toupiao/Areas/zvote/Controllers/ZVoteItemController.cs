using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;
using toupiao.Controllers;
using toupiao.Data;

namespace toupiao.Areas.zvote.Controllers
{
    [Area("zvote")]
    public class ZVoteItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        private readonly UserManager<IdentityUser> _userManager;

        public ZVoteItemController(
            IWebHostEnvironment env,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: zvote/ZVoteItem
        public async Task<IActionResult> Index(Guid Id)
        {

            var _user = await _userManager.GetUserAsync(User);
            var _zVoteItems = await _context.ZVoteItem
                .Where(p=>p.ZVoteId == Id).ToListAsync();
            _zVoteItems.FirstOrDefault().ZVote = await _context.ZVote
                .FirstOrDefaultAsync(p => p.Id == Id);

            ViewData[nameof(ZVoteItem.ZVoteId)] = Id;
            ViewData[nameof(ZUserVote)] = await _context.ZUserVote
                .Where(p => p.Voter == _user).ToListAsync();

            return View(_zVoteItems);

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
        public async Task<IActionResult> CreateAsync(Guid Id)
        {
            TempData[nameof(ZVoteItem.ZVoteId)] = Id;
            var _zVoteItem = new ZVoteItem()
            {
                ZVote = await _context.ZVote.FirstOrDefaultAsync(
                    p => p.Id == Id)
            };
            return View( _zVoteItem );
        }

        // POST: zvote/ZVoteItem/Create
        // To protect from overposting attacks, enable the specific properties 
        // you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Content,Description,ImageFile")] 
            ZVoteItem zVoteItem)
        {
            if (ModelState.IsValid)
            {

                if (zVoteItem.ImageFile != null)
                {
                    var fileName = await ControllerMix.SaveFormFileAsync(
                        zVoteItem.ImageFile, _env, ModelState,
                        nameof(zVoteItem.ImageFile)) ?? "";
                    if (fileName.Length < 1)
                        return View(zVoteItem);

                    zVoteItem.ImageFileName = fileName;
                }

                var zVoteId =Guid.Parse( TempData[nameof(ZVoteItem.ZVoteId)]
                    .ToString() );

                zVoteItem.Id = Guid.NewGuid();
                zVoteItem.ZVoteId = zVoteId;

                _context.Add(zVoteItem);
                await _context.SaveChangesAsync();

                return RedirectToAction( 
                    nameof(Index), 
                    new { Id =zVoteId });
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
        // To protect from overposting attacks, enable the specific properties
        // you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id, 
            [Bind("Id,Content,Description")] 
            ZVoteItem zVoteItem)
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


        // GET: zvote/ZVoteItem/UserVote/5
        public async Task<IActionResult> UserVoteAsync(Guid Id)
        {
            var voter = await _userManager.GetUserAsync(User);
            if ( !await _context.ZVoteItem.AnyAsync(p => p.Id == Id))
                return BadRequest();
            var zUserVotes = await _context.ZUserVote.Where(
                p => p.Voter == voter && p.ZVoteItemId == Id)
                .ToListAsync();

            if (zUserVotes.Count()>0)
            {
                _context.ZUserVote.RemoveRange( zUserVotes);
                await _context.SaveChangesAsync();
                return Ok(0);
            }

            await _context.ZUserVote.AddAsync(
                new ZUserVote()
                {
                    Voter = voter,
                    ZVoteItemId = Id,
                    Id = Guid.NewGuid()
                });

            await _context.SaveChangesAsync();
            return Ok(1);
        }

        private bool ZVoteItemExists(Guid id)
        {
            return _context.ZVoteItem.Any(e => e.Id == id);
        }
    }
}
