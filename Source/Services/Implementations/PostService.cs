using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;
using Source.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly ApplicationContext context;

        public  PostService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<List<Post>?> GetByCategoryPosts(string categoryName, int pageSize, int currentPage)
        {
           return await context.Posts.Include(p => p.User).Include(p => p.Tags).Include(p => p.Category)
                                                 .Where(p => p.Category.Name == categoryName)
                                                 .OrderByDescending(p => p.CreationDate)
                                                .Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Post?> GetById(Guid id)
        {
            return await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> GetBySlug(string slug)
        {
            return await context.Posts.Include(p => p.User).Include(p => p.Tags)
                                          .Include(p => p.Comments).ThenInclude(c => c.User)
                                          .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<List<Post>?> GetByTagPosts(string tag, int pageSize, int currentPage)
        {
            return await context.Posts.Include(p => p.User).Include(p => p.Tags)
                                                 .Where(p => p.Tags.Any(t => t.Name == tag))
                                                 .OrderByDescending(p => p.CreationDate)
                                                .Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<Post>?> GetFeaturedPosts()
        {
            return await context.Posts.AsNoTracking().Include(p => p.User).Include(p => p.Tags).OrderByDescending(p => p.Reacts).Take(3).ToListAsync();
        }

        public async Task<List<Post>?> GetLatestPosts(int pageSize, int currentPage)
        {
            return await context.Posts.Include(p => p.User).Include(p => p.Tags).OrderByDescending(p => p.CreationDate)
                                        .Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
