using Microsoft.EntityFrameworkCore;
using RandomBugle.Helpers;
using RandomBugleDB.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBugleDB.Models.Repository
{
    public class Repository : IRepository
    {
        private RandomBugleDBContext _ctx;
        public Repository(RandomBugleDBContext ctx)
        {
            _ctx = ctx;

        }
        public  void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
           
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

       
        public IndexViewModel GetAllPosts(int pageNumber,string category,string search)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };
            //return _ctx.Posts
            //    .Where(post => InCategory(post))
            //    .ToList();
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Posts.AsNoTracking().AsQueryable();
               
         
            if (!string.IsNullOrEmpty(category))
                query= query.Where(x => InCategory(x));

            //if (!String.IsNullOrEmpty(category))
            //    query = query.Where(x => x.Title.Contains(search)  
            //                            || x.Body.Contains(search) 
            //                            || x.Description.Contains(search));
            if (!String.IsNullOrEmpty(search))
                query = query.Where(x => EF.Functions.Like(x.Title,$"%{search}%")
                                        || EF.Functions.Like(x.Body, $"%{search}%")
                                        || EF.Functions.Like(x.Description, $"%{search}%"));

            int postsCount = query.Count();
            int pageCount = (int)Math.Ceiling((double)postsCount / pageSize);
            return new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount  = pageCount,
                NextPage = postsCount > skipAmount + pageSize,
                Pages = PageHelpers.PageNumbers(pageNumber, pageCount).ToList(),
                Category = category,
                Search = search,
                Posts = query
                 .Skip(skipAmount)
                .Take(pageSize)
                .ToList()
            };


            
        }


        //public List<Post> GetAllPosts(string category)
        //{
        //    //Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };
        //    //return _ctx.Posts
        //    //    .Where(post => InCategory(post))
        //    //    .ToList();


        //    return _ctx.Posts
        //           .Where(post => post.Category.ToLower().Equals(category.ToLower()))
        //           .ToList();
        //}

        

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.MainComments)
                .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        //public Post GetPost(int id)
        //{
        //    return _ctx.Posts.FirstOrDefault(p => p.Id == id);
        //}
        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }
        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

       

        
     
        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
    
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
            }

    //public IndexViewModel GetAllPosts(int pagenumber)
    //{
    //    int pageSize = 5;
    //    int skipAmount = pageSize * (pagenumber - 1);
    //    int postsCount = _ctx.Posts.Count();


    //    return new IndexViewModel
    //    {
    //        PageNumber = pagenumber,
    //        NextPage = pageSize > skipAmount +pageSize,
    //         Posts =  _ctx.Posts
    //        .Skip(pageSize * (pagenumber - 1))
    //        .Take(pageSize)
    //        .ToList()
    //          };

    //}

}

