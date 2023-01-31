using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomBugle.ViewModels;
using RandomBugleDB.Models;
using RandomBugleDB.Models.FileManager;
using RandomBugleDB.Models.Repository;
using System.Threading.Tasks;

namespace RandomBugle.Controllers
{
    [Authorize (Roles ="Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _filemanager;

        public PanelController(IRepository repo,IFileManager fileManager)
        {
            _repo = repo;
            _filemanager = fileManager;
        }
        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Title,
                    CurrentImage = post.Image,
                    Description =post.Description,
                    Category =post.Category,
                    Tags = post.Tags
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Description = vm.Description,
                Category = vm.Category,
                Tags = vm.Tags,
                //Image = await _filemanager.SaveImage(vm.Image)

            };
            if (vm.Image == null)
                post.Image = vm.CurrentImage;
            else
                if (!string.IsNullOrEmpty(vm.CurrentImage))
                _filemanager.RemoveImage(vm.CurrentImage);

                post.Image = await _filemanager.SaveImage(vm.Image);

            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);
        }


        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
}
