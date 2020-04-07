using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using toupiao.Controllers;
using toupiao.Data;

namespace toupiao.Areas.Admin.Controllers
{

    [Area("Admin")]

    [Authorize(Roles = "ADMIN")]
    public class ZlUserRoleController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string AdminRoleId;

        public ZlUserRoleController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;



            AdminRoleId = _context.Roles.Where(p => p.Name == "ADMIN")
                .Select(p => p.Id).FirstOrDefaultAsync()
                .GetAwaiter().GetResult();

;            
        }


        // GET: ZlUserRole
        
        [Authorize(Roles ="ADMIN")]
        public async Task<ActionResult> IndexAsync(
            int pageNumber =1,
            int pageSize = 10)
        {
            var users = await PaginatedList<IdentityUser>.CreateAsync(
                _context.Users.AsNoTracking(),
                pageNumber ,
                pageSize ,
                ViewData);
            var admins = await _context.UserRoles.Where(
                p => p.RoleId == AdminRoleId).ToListAsync();
            ViewData[nameof(admins)] = admins;

            return View(users);
        }

        // GET: ZlUserRole/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZlUserRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZlUserRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: ZlUserRole/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZlUserRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: ZlUserRole/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AsAdminAsync( Guid Id )
        {
            var _id = Id.ToString();
            var user = await _context.Users.FirstOrDefaultAsync(
                p=>p.Id == _id);

            if (await _userManager.IsInRoleAsync(user, "ADMIN"))
            {
                _context.UserRoles.RemoveRange(
                    _context.UserRoles.Where(
                        p => p.UserId == _id && p.RoleId == AdminRoleId));
                await _context.SaveChangesAsync();
                return Ok(0);
            }

            await _userManager.AddToRolesAsync(user, new[] { "ADMIN" });

            await _context.SaveChangesAsync();

            Console.WriteLine($"user: {user.Email} is ADMIN now");

            return Ok(1);
        }

        // POST: ZlUserRole/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}