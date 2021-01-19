using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CourseProjectItr.Areas.Identity.Data;
using CourseProjectItr.Data;
using CourseProjectItr.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectItr.Controllers
{
    public class CollectionsController : Controller
    {
        public readonly CourseDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly Account account = new Account(
            "dnntqrull",
            "841957784659873",
            "7QkkV8FwsWEBz5f8eGowbjfDj6k"
            );
        private readonly Cloudinary cloudinary = new Cloudinary(account);

        public CollectionsController(CourseDbContext db , UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserCollectionsList(string name)
        {
            var collections = await _db.Collection.Where(x => x.OwnerEmail == name).ToListAsync();
            List<Collection> userName = new List<Collection>();
            Collection col = new Collection
            {
                OwnerEmail = name
            };
            userName.Add(col);
            if (collections.Count == 0)
                return View(userName);

            return View(await _db.Collection.Where(x => x.OwnerEmail == name).ToListAsync());
        }

        public IActionResult CollectionItems(int id)
        {
            ViewBag.imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            ViewBag.audioExtensions = new List<string> { ".mp3", ".wav", ".wma", ".wpl", ".mid", ".midi", ".aif", ".cda", ".mpa", ".ogg" };
            ViewBag.videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };
            ViewBag.textExtensions = new List<string> { ".doc", ".docx", ".odt", ".pdf", ".rtf", ".tex", ".txt", ".wpd", ".fb2" };
            return View(_db.FileModel.Where(x => x.CollectionId == id));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, IFormFile file)
        {
            var collection = _db.Collection.Find(id);
            FileModel fileModel = new FileModel();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            fileModel.FilePath = file.FileName;
            fileModel.CollectionId = id;
            if (collection.Files == null)
                collection.Files = new List<FileModel>();
            collection.Files.Add(fileModel);
            if (ModelState.IsValid)
            {
                _db.Add(fileModel);
                _db.Update(collection);
                await _db.SaveChangesAsync();
                var name = _db.Collection.First(x => x.Id == id).OwnerEmail;
                return RedirectToAction("UserCollectionsList", new { name });
            }
            return View("Add");
        }

        public IActionResult Create(string userName)
        {
            ViewBag.userName = userName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Collection collection, IFormFile file, string userEmail)
        {
            if (file == null || file.Length == 0)
            {
                return Content("File(s) not selected");
            }
            collection.OwnerEmail = userEmail;
            collection.Files = new List<FileModel>();
            FileModel fileModel = new FileModel();
            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            fileModel.FilePath = file.FileName;
            fileModel.CollectionId = collection.Id;
            collection.Files.Add(fileModel);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"wwwroot/files/" + file.FileName)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            collection.Avatar = uploadResult.Url.ToString();

            if (ModelState.IsValid)
            {
                _db.Add(fileModel);
                _db.Add(collection);
                await _db.SaveChangesAsync();
                var name = _db.Collection.First(x => x.Id == collection.Id).OwnerEmail;
                return RedirectToAction("UserCollectionsList", new { name });
            }
            return View("Create");
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.FileModel.FindAsync(id);
            var name = _db.Collection.First(x => x.Id == item.CollectionId).OwnerEmail;
            _db.FileModel.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("UserCollectionsList", new { name });
        }

        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _db.Collection.FindAsync(id);
            var name = collection.OwnerEmail;
            var items = await _db.FileModel.Where(x => x.CollectionId == id).ToListAsync();
            foreach (var item in items)
            {
                _db.Remove(item);
            }

            var deletionParams = new DeletionParams(Path.GetFileNameWithoutExtension(collection.Avatar));
            var deletionResult = cloudinary.Destroy(deletionParams);

            _db.Remove(collection);
            await _db.SaveChangesAsync();
            return RedirectToAction("UserCollectionsList", new { name });
        }
    }
}
