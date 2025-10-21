using Interface.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using source;
using Source.Models;

namespace Interface.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationContext context;

        public CommentController(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> AddAsync(AddCommentViewModel model)
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                Content = model.CommentText,
                PostId = model.PostId,
                UserId = model.UserId,
            };

            await context.AddAsync(comment);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new {Slug = model.PostSlug});
        }
    }
}
