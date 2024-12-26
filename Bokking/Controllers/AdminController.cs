using Bokking.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.IO;


namespace Bokking.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        public AdminController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString("AdminName");
            ViewBag.Id = HttpContext.Session.GetInt32("AdminId");
            ViewBag.NumRoomAva = _context.Rooms.Count(x => x.Availability == "available");
            ViewBag.NumRoomNotAva = _context.Rooms.Count(x => x.Availability == "NotAvailable");
            ViewBag.NumOfHotels = _context.Hotels.Count();
            ViewBag.NumOfCustomer = _context.Customers.Count();
            ViewBag.feedPanding = _context.Feedbacks.Take(5).ToList();
            ViewBag.testPanding = _context.Testimonials.ToList();
            ViewBag.log = _context.UserLogins.Take(3).ToList();

            return View();
        }

        public IActionResult Approve(int id)
        {
            var feed = _context.Feedbacks.Where(x => x.Id == id).SingleOrDefault();
            if (feed != null)
            {
                feed.Isapproved = "true";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Approve2(int id)
        {
            var testi = _context.Testimonials.Where(x => x.Id == id).SingleOrDefault();
            if (testi != null)
            {
                testi.Isapproved = "true";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Information()
        {
            var customers = _context.Customers.ToList();
            var rooms = _context.Rooms.ToList();
            var hotels = _context.Hotels.ToList();
            var reservation = _context.Reservations.ToList();
            var userlogin = _context.UserLogins.ToList();
            var Services = _context.Services.ToList();

            var result = from ul in userlogin
                         join c in customers on ul.Customerid equals c.Id
                         join r in reservation on c.Id equals r.Userid
                         join ro in rooms on r.Roomid equals ro.Id
                         join h in hotels on r.Hotelid equals h.Id
                         join s in Services on r.Serviceid equals s.Id

                         select new JoinTable
                         {
                             userLogin = ul,
                             customer = c,
                             reservation = r,
                             room = ro,
                             hotel = h,
                             service = s,

                         };


            return View(result);

        }
        public IActionResult Search()
        {
            var modelContext = _context.Reservations.ToList();
            return View(modelContext);
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var modelContext = _context.Reservations.AsQueryable();

            if (startDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.CheckIn >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.CheckOut <= endDate.Value);
            }

            var result = modelContext.ToList();
            ViewBag.Reservations = result;
            return View(result);
        }
        public IActionResult report(int? year, int? month)
        {
            year ??= DateTime.Now.Year;
            month ??= null;

            decimal totalRevenue;
            decimal totalServiceCosts;

            if (month.HasValue)
            {
                totalRevenue = TotalRevenueForMonth(year.Value, month.Value);
                totalServiceCosts = ServiceCostsForMonth(year.Value, month.Value);
            }
            else
            {
                totalRevenue = TotalRevenueForYear(year.Value);
                totalServiceCosts = ServiceCostsForYear(year.Value);
            }



            decimal netProfit = totalRevenue - totalServiceCosts;
            decimal loss = totalServiceCosts > totalRevenue ? totalServiceCosts - totalRevenue : 0;

            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalServiceCosts = totalServiceCosts;
            ViewBag.NetProfit = netProfit > 0 ? netProfit : 0;
            ViewBag.Loss = loss > 0 ? loss : 0;

            return View();
        }

        private decimal TotalRevenueForYear(int year)
        {
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = new DateTime(year, 12, 31, 23, 59, 59);
            return _context.Reservations
                .Where(r =>
                            r.CheckIn.Value >= startOfYear &&
                            r.CheckIn.Value <= endOfYear)
                .Sum(r => r.TotalPrice) ?? 0;
        }

        private decimal TotalRevenueForMonth(int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

            return _context.Reservations
                .Where(r =>
                            r.CheckIn.Value >= startOfMonth &&
                            r.CheckIn.Value <= endOfMonth)
                .Sum(r => r.TotalPrice) ?? 0;
        }

        private decimal ServiceCostsForYear(int year)
        {
            var serviceCostPercentage = 0.20m;
            var monthlyServerCost = 2000;
            var monthlyMaintenanceCost = 1000;

            var totalRevenue = TotalRevenueForYear(year);
            var serviceCost = totalRevenue * serviceCostPercentage;
            var totalServiceCosts = serviceCost + (monthlyServerCost + monthlyMaintenanceCost) * 12;

            return totalServiceCosts;
        }

        private decimal ServiceCostsForMonth(int year, int month)
        {
            var serviceCostPercentage = 0.20m;
            var monthlyServerCost = 2000;
            var monthlyMaintenanceCost = 1000;

            var totalRevenue = TotalRevenueForMonth(year, month);
            var serviceCost = totalRevenue * serviceCostPercentage;
            var totalServiceCosts = serviceCost + monthlyServerCost + monthlyMaintenanceCost;

            return totalServiceCosts;
        }


















    }

}


 




   



