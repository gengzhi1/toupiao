﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using toupiao.Areas.zvote.Models;
using toupiao.Controllers;
using toupiao.Data;

namespace toupiao.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "ADMIN")]
    public class ZVotesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public ZVotesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/ZVotes
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            // key word 关键词搜索
            string kw="")
        {

            //关键词搜索代码（标题和创建者）
            var _zVotes = await PaginatedList<ZVote>.CreateAsync(
                // _context 数据库上下文
                _context.ZVote.Where(
                    p =>
                        kw.Length < 1 ? true : (p.Title.Contains(kw) ||
                        p.Submitter.UserName.Contains(kw)))

                .OrderByDescending(p => p.DOCreating)
                // 纯查询
                .AsNoTracking(),
                pageNumber,
                pageSize,
                ViewData);

            return View(_zVotes);
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
        public async Task<IActionResult> DeleteNoConfirmed(Guid id)
        {
            var _user = await _userManager.GetUserAsync(User);
            if( !await _userManager.IsInRoleAsync(_user, "ADMIN") )
            {
                return BadRequest();
            }

            var zVote = await _context.ZVote.FindAsync(id);
            _context.ZVote.Remove(zVote);
            await _context.SaveChangesAsync();
            return Ok(1);
        }

        private bool ZVoteExists(Guid id)
        {
            return _context.ZVote.Any(e => e.Id == id);
        }
    }
}
