using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using source;

namespace Interface.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext context;

        public AdminController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult PostIndex()
        {
            return View(context.Posts.Include(p=>p.Tags).Include(p=>p.Category).Include(p=>p.User).ToList());
        }


    }
}
