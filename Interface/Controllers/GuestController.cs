using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index","Post");
        }
    }
}
