using Interface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using source;
using Source.Models;

namespace Interface.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationContext context;

        public CommentController(ApplicationContext context)
        {
            this.context = context;
        }

        public ActionResult Index()
        {
            return View();
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
       
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
