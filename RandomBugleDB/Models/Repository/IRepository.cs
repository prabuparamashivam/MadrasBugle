using RandomBugleDB.Models.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RandomBugleDB.Models.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();

      
        IndexViewModel GetAllPosts(int pagenumber,string Category,string search);
      
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);

        void AddSubComment(SubComment comment);
        Task<bool> SaveChangesAsync();

    }
}
