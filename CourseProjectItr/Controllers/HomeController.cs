using CourseProjectItr.Areas.Identity.Data;
using CourseProjectItr.Data;
using CourseProjectItr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly CourseDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, CourseDbContext db, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            ViewBag.audioExtensions = new List<string> { ".mp3", ".wav", ".wma", ".wpl", ".mid", ".midi", ".aif", ".cda", ".mpa", ".ogg" };
            ViewBag.videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };
            ViewBag.textExtensions = new List<string> { ".doc", ".docx", ".odt", ".pdf", ".rtf", ".tex", ".txt", ".wpd", ".fb2" };

            var collections = _db.Collection.ToList();
            var files = _db.FileModel.ToList();
            int maxItems = 0;
            var countItems = 0;
            int collectionId = 0;

            foreach (var item in collections)
            {
                countItems = files.Where(x => x.CollectionId == item.Id).ToList().Count;
                if (countItems > maxItems)
                {
                    maxItems = files.Where(x => x.CollectionId == item.Id).ToList().Count;
                    collectionId = item.Id;
                    ViewBag.userName = item.OwnerEmail;
                    ViewBag.id = item.Id;
                }
            }
            

            return View(_db.FileModel.Where(x => x.CollectionId == collectionId).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
