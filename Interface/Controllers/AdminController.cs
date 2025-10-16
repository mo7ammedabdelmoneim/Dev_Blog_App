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
        


    }
}
