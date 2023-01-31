using Microsoft.AspNetCore.Mvc;
using RandomBugle.ViewModels;
using RandomBugleDB.Models;
using RandomBugleDB.Models.Comments;
using RandomBugleDB.Models.FileManager;
using RandomBugleDB.Models.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomBugle.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager filemanager)
        {
            _repo = repo;
            _fileManager = filemanager;
        
        }


        //public IActionResult Index(string category) => View (string.IsNullOrEmpty(category) ?
        //    _repo.GetAllPosts() : 
        //    _repo.GetAllPosts(category));

        public IActionResult Index(int pagenumber, string category, string search)
        {
            if (pagenumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1, category });

            //var vm = new IndexViewModel
            //{
            //    PageNumber = pagenumber,
            //    Posts = string.IsNullOrEmpty(category) ?
            //    _repo.GetAllPosts() :
            //    _repo.GetAllPosts(category)
            //};

            var vm = _repo.GetAllPosts(pagenumber , category,search);
            return  View(vm);
        }


        public IActionResult Post(int id) =>
            View(_repo.GetPost(id));

        [HttpGet("/Image/{image}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image) => 
            new FileStreamResult(
                _fileManager.ImageStream(image),
                $"image/{image.Substring(image.LastIndexOf('.') + 1)}");

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return  RedirectToAction("Post",new { id = vm.PostId });

            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });
                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,

                };
                _repo.AddSubComment(comment);
            }
            
            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }


        //public HomeController(IRepository repo,IFileManager filemanager)
        //{
        //    _repo = repo;
        //    _fileManager = filemanager;
        //}

        //public IActionResult Index(string category)
        //{
        //    var posts = string.IsNullOrEmpty(category)? _repo.GetAllPosts() : _repo.GetAllPosts(category);
        //    return View(posts);
        //}

        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);
        //    return View(post);
        //}
        //[HttpGet("/Image/{image}")]
        //public IActionResult Image(string image)
        //{
        //    var mime = image.Substring(image.LastIndexOf('.') + 1);
        //    return new FileStreamResult(_fileManager.ImageStream(image),$"image/{mime}");
        //}
    }
}
