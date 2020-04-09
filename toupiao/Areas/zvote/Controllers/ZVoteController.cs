using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ZVoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        
        // 每24小时 每用户 每投票项 最 多投票数
        private readonly int MaxVoteTimesPer24 = 3;

        public ZVoteController(
            IWebHostEnvironment env
,            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: zvote/ZVote
        public async Task<IActionResult> Index(
            int pageNumber=1,
            int pageSize=10,
            int ViewAll = 1,
            string kw="")
        {

            var _user = await _userManager.GetUserAsync(User);

            var _zVote = await PaginatedList<ZVote>.CreateAsync(
                _context.ZVote.Where(
                    p=>
                        (ViewAll == 1?true:(p.Submitter == User.Identity)) && 
                        (kw.Length<1?
                            true:
                            (p.Submitter.UserName.Contains(kw) ||
                            p.Title.Contains(kw)))   )
                .OrderByDescending(p=>p.DOCreating)
                .AsNoTracking(), 
                pageNumber , 
                pageSize,
                ViewData);

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

            var _user = await _userManager.GetUserAsync(User);

            if ( !zVote.IsLegal && zVote.Submitter !=_user )
            {
                return NotFound();
            }

            var zUserVotes = await _context.ZUserVote.Where(
                p => p.ZVoteId == id)
                .ToListAsync();


            ViewData[nameof(zUserVotes)] = zUserVotes;
            ViewData["userId"] = _user?.Id;
            ViewData[nameof(MaxVoteTimesPer24)] = MaxVoteTimesPer24;

            return View(zVote);
        }

        // GET: zvote/ZVote/Create
        [ActionName("Create")]
        public IActionResult XinJian()
        {
            var _zvote = new ZVote()
            {
                DOStart = DateTimeOffset.Now
            };

            ViewData["IsEdit"] = false;
            return View( _zvote );
        }

        // POST: zvote/ZVote/Create
        // To protect from overposting attacks, enable the specific properties 
        //  you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,DOEnd,DOStart,CoverImage,"+
            "XuanxiangA,"+
            "XuanxiangB,"+
            "XuanxiangC,"+
            "XuanxiangD")] 
            ZVote zVote
            )
        {

            if (ModelState.IsValid)
            {
                if (zVote.Title == null || zVote.Title?.Length < 1)
                {
                    ModelState.AddModelError(
                        nameof(zVote.Title),
                    "请添加一个标题!!");
                    return View(zVote);
                }
                if (zVote.Description == null || zVote.Description?.Length < 1)
                {
                    ModelState.AddModelError(
                        nameof(zVote.Description),
                    "请添加一个描述!!"); 
                    return View(zVote);
                }
                if (zVote.XuanxiangA == null || zVote.XuanxiangA?.Length < 1)
                {
                    ModelState.AddModelError(
                        nameof(zVote.XuanxiangA),
                    "选项A和选项B要填写!");
                    return View(zVote);
                }
                if (zVote.XuanxiangB== null || zVote.XuanxiangB?.Length < 1)
                {
                    ModelState.AddModelError(
                        nameof(zVote.XuanxiangB),
                    "选项A和选项B要填写!");
                    return View(zVote);
                }

                zVote.Submitter = await _userManager.GetUserAsync(User);
                zVote.DOCreating = DateTimeOffset.Now;
                zVote.Id = Guid.NewGuid();

                if(zVote.CoverImage!=null)
                {
                    zVote.CoverPath = await ControllerMix.SaveFormFileAsync(
                        zVote.CoverImage, _env, ModelState,
                        nameof(zVote.CoverImage));
                }

                _context.Add(zVote);
                await _context.SaveChangesAsync();
                ViewData["IsEdit"] = false;
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
            var _user = await _userManager.GetUserAsync(User);

            var zVote = await _context.ZVote.FirstOrDefaultAsync(
                p=>p.Id == id && p.Submitter == _user );


            if (zVote == null || zVote.Submitter!=_user)
            {
                return NotFound();
            }

            ViewData["IsEdit"] = true;

            return View(nameof(Create),zVote);
        }

        // POST: zvote/ZVote/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,Title,Description,DOEnd,DOStart,CoverImage,"+
            "XuanxiangA,"+
            "XuanxiangB,"+
            "XuanxiangC,"+
            "XuanxiangD")] 
            ZVote zVote)
        {
            if (id != zVote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (zVote.CoverImage != null)
                    {
                        zVote.CoverPath = await ControllerMix.SaveFormFileAsync(
                            zVote.CoverImage, _env, ModelState,
                            nameof(zVote.CoverImage));
                    }

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

            var _user = await _userManager.GetUserAsync(User);

            if (zVote == null || zVote.Submitter!=_user)
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


        public async Task<IActionResult> VoteNowAsync(Guid Id, String Item)
        {

            var _user = await _userManager.GetUserAsync(User);

            var zVote = await _context.ZVote.FindAsync(Id);

            // 不合法
            if (!zVote.IsLegal && zVote.Submitter != _user)
            {
                return NotFound();
            }

            var _userVote = await _context.ZUserVote.Where(
                 p => p.ZVoteId == Id && p.VoteItem == Item
                        && p.VoterId == _user.Id).ToListAsync();

            if( _userVote.Count() > MaxVoteTimesPer24 )return Ok(0);


            var Des = await _context.ZUserVote.Where(
                    p => p.VoterId == _user.Id && p.ZVoteId == Id
                ).OrderByDescending(p => p.DOVoting)
                .Take(MaxVoteTimesPer24).ToListAsync();

            if (Des.Count() == MaxVoteTimesPer24)
            {


                // 最近的第二次投票时间
                var DODe2 = Des.Take(MaxVoteTimesPer24 - 1)
                    .Select(p => p.DOVoting).Last();

                if ((DateTimeOffset.Now - DODe2).Hours < 24)
                {
                    return Ok(3);
                }

            }

            await _context.ZUserVote.AddAsync(new ZUserVote { 
                VoteItem = Item,
                VoterId = _user.Id,
                ZVoteId = Id
            });

            await _context.SaveChangesAsync();
            return Ok(1);
        }
        private bool ZVoteExists(Guid id)
        {
            return _context.ZVote.Any(e => e.Id == id);
        }
    }
}
