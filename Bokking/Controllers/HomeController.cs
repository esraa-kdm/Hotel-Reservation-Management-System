using Bokking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;

namespace Bokking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Hotels = _context.Hotels.ToList();
            ViewBag.Rooms = _context.Rooms.ToList();
            ViewBag.Events=_context.Services.ToList();
            ViewBag.FeedAprove  =_context.Feedbacks.Where(x=>x.Isapproved=="true").ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AboutUs()
        {
            ViewBag.Rooms = _context.Rooms.ToList();
            return View();
        }

        public IActionResult GetRooms(int id)
        {
            var room= _context.Rooms.Where(x=>x.HotelId ==id).ToList();
            return View(room);
   
        }

        public IActionResult Rooms()
        {
            ViewBag.room = _context.Rooms.ToList();
          return View(); 
        }


    }
}
