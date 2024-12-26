using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class BanksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
