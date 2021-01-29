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

        public CollectionsController(CourseDbContext db, UserManager<ApplicationUser> userManager)
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

        public async Task<IActionResult> CollectionItems(int id, string searchText)
        {
            var collection = await _db.Collection.FindAsync(id);
            ViewBag.collection = collection;
            ViewBag.id = collection.Id;
            ViewBag.userName = collection.OwnerEmail;
            ViewBag.imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            ViewBag.audioExtensions = new List<string> { ".mp3", ".wav", ".wma", ".wpl", ".mid", ".midi", ".aif", ".cda", ".mpa", ".ogg" };
            ViewBag.videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };
            ViewBag.textExtensions = new List<string> { ".doc", ".docx", ".odt", ".pdf", ".rtf", ".tex", ".txt", ".wpd", ".fb2" };
            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    var result = _db.FileModel.Where(x => x.CollectionId == id && EF.Functions.Contains(x.Tags, searchText)).ToList();
            //    return View(result);
            //}
            return View(_db.FileModel.Where(x => x.CollectionId == id));
        }

        public async Task<IActionResult> Add(int id)
        {
            var collection = await _db.Collection.FindAsync(id);
            ViewBag.collection = collection;
            if (User.Identity.Name == collection.OwnerEmail || User.IsInRole("Admin"))
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, IFormFile file, FileModel fileModel)
        {
            var collection = await _db.Collection.FindAsync(id);
            
            fileModel.CollectionId = id;

            collection.Files.Add(fileModel);

            fileModel.FilePath = UploadFile(file);
            fileModel.Id = 0;

            if (ModelState.IsValid)
            {
                _db.Add(fileModel);
                _db.Update(collection);
                await _db.SaveChangesAsync();
                return RedirectToAction("CollectionItems", new { collection.Id });
            }
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string comment, int id)
        {
            var file = await _db.FileModel.FindAsync(id);

            Comment com = new Comment
            {
                CommentAuthor = User.Identity.Name,
                UserComment = comment,
                FileModelId = id
            };

            file.Comments.Add(com);

            com.Id = 0;

            _db.Add(com);
            _db.Update(file);
            await _db.SaveChangesAsync();

            return RedirectToAction("Item", new { id });

        }

        public IActionResult Create(string userName)
        {
            ViewBag.userName = userName;
            if (User.Identity.Name == userName || User.IsInRole("Admin"))
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Collection collection, IFormFile file, string userEmail)
        {
            collection.OwnerEmail = userEmail;

            collection.Avatar = UploadFile(file);

            if (ModelState.IsValid)
            {
                _db.Add(collection);
                await _db.SaveChangesAsync();
                var name = _db.Collection.First(x => x.Id == collection.Id).OwnerEmail;
                return RedirectToAction("UserCollectionsList", new { name });
            }
            return View("Create");
        }

        public string UploadFile(IFormFile file)
        {
            var imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            var videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            if (imageExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(@"wwwroot/files/" + file.FileName)
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                System.IO.File.Delete("wwwroot/files/" + file.FileName);
                return uploadResult.Url.ToString();
            }
            else if (videoExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(@"wwwroot/files/" + file.FileName)
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                System.IO.File.Delete("wwwroot/files/" + file.FileName);
                return uploadResult.Url.ToString();
            }
            else
            {
                return file.FileName;
            }
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.FileModel.FindAsync(id);
            var collection = _db.Collection.First(x => x.Id == item.CollectionId);

            DeleteFile(item.FilePath);

            _db.FileModel.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("CollectionItems", new { collection.Id });
        }

        public void DeleteFile(string filePath)
        {
            var imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            var videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };
            if (imageExtensions.Contains(Path.GetExtension(filePath)))
            {
                var deletionParams = new DeletionParams(Path.GetFileNameWithoutExtension(filePath));
                var deletionResult = cloudinary.Destroy(deletionParams);
            }
            else if (videoExtensions.Contains(Path.GetExtension(filePath)))
            {
                var deletionParams = new DeletionParams(Path.GetFileNameWithoutExtension(filePath))
                {
                    ResourceType = ResourceType.Video
                };
                var deletionResult = cloudinary.Destroy(deletionParams);
            }
            else
            {
                System.IO.File.Delete("wwwroot/files/" + filePath);
            }
        }

        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _db.Collection.FindAsync(id);
            var items = await _db.FileModel.Where(x => x.CollectionId == id).ToListAsync();

            var deletionParams = new DeletionParams(Path.GetFileNameWithoutExtension(collection.Avatar));
            var deletionResult = cloudinary.Destroy(deletionParams);

            foreach (var item in items)
            {
                await DeleteItem(item.Id);
            }

            _db.Remove(collection);
            await _db.SaveChangesAsync();
            return RedirectToAction("UserCollectionsList", new { name = collection.OwnerEmail });
        }

        public async Task<IActionResult> EditCollection(int id)
        {
            var collection = await _db.Collection.FindAsync(id);
            if (User.Identity.Name == collection.OwnerEmail || User.IsInRole("Admin"))
                return View(collection);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditCollection(Collection collection, IFormFile file)
        {
            if (file != null)
            {
                DeleteFile(collection.Avatar);
                collection.Avatar = UploadFile(file);
            }

            _db.Collection.Update(collection);
            await _db.SaveChangesAsync();

            return RedirectToAction("UserCollectionsList", new { name = collection.OwnerEmail });
        }

        [HttpPost]
        public async Task<IActionResult> EditItem (FileModel fileModel)
        {
            _db.FileModel.Update(fileModel);
            await _db.SaveChangesAsync();
            return RedirectToAction("Item", new { fileModel.Id });
        }

        public IActionResult Item(int id)
        {
            ViewBag.imageExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png" };
            ViewBag.audioExtensions = new List<string> { ".mp3", ".wav", ".wma", ".wpl", ".mid", ".midi", ".aif", ".cda", ".mpa", ".ogg" };
            ViewBag.videoExtensions = new List<string> { ".avi", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".wmd" };
            ViewBag.textExtensions = new List<string> { ".doc", ".docx", ".odt", ".pdf", ".rtf", ".tex", ".txt", ".wpd", ".fb2" };
            ViewBag.comments = _db.Comment.Where(x => x.FileModelId == id).ToList();
            var fileModel = _db.FileModel.Find(id);
            ViewBag.userName = _db.Collection.Find(fileModel.CollectionId).OwnerEmail;
            return View(fileModel);
        }

        public async Task<IActionResult> Like(int id)
        {
            var file = _db.FileModel.Find(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var likes = _db.FileModel.Include(x => x.Likes).ToList();
            if (!likes.Find(x => x.Id == id).Likes.Contains(user))
            {
                file.Likes.Add(user);
                file.LikesNumber++;
            }
            else
            {
                file.Likes.Remove(user);
                file.LikesNumber--;
            }

            await _db.SaveChangesAsync();

            return Ok(file.LikesNumber);
        }
    }
}
