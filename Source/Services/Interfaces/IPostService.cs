using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>?> GetFeaturedPosts();
        
        Task<List<Post>?> GetLatestPosts(int pageSize, int currentPage);
        
        Task<List<Post>?> GetByTagPosts(string tag,int pageSize, int currentPage);
        
        Task<List<Post>?> GetByCategoryPosts(string categoryName,int pageSize, int currentPage);

        Task<Post?> GetBySlug(string slug);

        Task<Post?> GetById(Guid id);

        Task Save();




    }
}
