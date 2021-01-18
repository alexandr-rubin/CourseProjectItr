using CourseProjectItr.Areas.Identity.Data;
using CourseProjectItr.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Controllers
{
    public class UserProfilePageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CourseDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserProfilePageController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            CourseDbContext db, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _db = db;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> UserProfilePage(string name)
        {
            ViewBag.userName = name;
            ViewBag.avatar = await _db.FileModel.ToListAsync();
            return View(await _db.Collection.Where(x => x.OwnerEmail == name).ToListAsync());
        }

        public IActionResult CollectionItems(int id)
        {
            return RedirectToAction("CollectionItems", "Collections", new { id = id });
        }

        public IActionResult DeleteCollection(int id)
        {
            return RedirectToAction("DeleteCollection", "Collections", new { id = id });
        }

        public IActionResult CreateCollection(string userName)
        {
            return RedirectToAction("Create", "Collections", new { userName = userName });
        }
    }
}
