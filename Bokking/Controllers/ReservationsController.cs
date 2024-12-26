using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bokking.Models;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.AspNetCore.Http;

namespace Bokking.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reservations.Include(r => r.Service).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["Serviceid"] = new SelectList(_context.Services, "Id", "Id");
            ViewData["Userid"] = new SelectList(_context.Customers, "Id", "Id");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckIn,CheckOut,TotalPrice,PayementStatus,Userid,Hotelid,Roomid,Serviceid")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Serviceid"] = new SelectList(_context.Services, "Id", "Id", reservation.Serviceid);
            ViewData["Userid"] = new SelectList(_context.Customers, "Id", "Id", reservation.Userid);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Serviceid"] = new SelectList(_context.Services, "Id", "Id", reservation.Serviceid);
            ViewData["Userid"] = new SelectList(_context.Customers, "Id", "Id", reservation.Userid);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CheckIn,CheckOut,TotalPrice,PayementStatus,Userid,Hotelid,Roomid,Serviceid")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Serviceid"] = new SelectList(_context.Services, "Id", "Id", reservation.Serviceid);
            ViewData["Userid"] = new SelectList(_context.Customers, "Id", "Id", reservation.Userid);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ModelContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(decimal id)
        {
          return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public IActionResult Reserve(int id)
        {
           
            ViewBag.roomess =_context.Rooms.Where(x=>x.Id == id).SingleOrDefault();
            ViewBag.events=_context.Services.ToList();  

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Reserve(int id,[Bind("Id,CheckIn,CheckOut,TotalPrice,PayementStatus,Hotelid,Roomid,Serviceid")] Reservation reservation ,Room room ,UserLogin userLogin ,Customer customer)
        {
            var user = _context.UserLogins.Where(x => x.Fname == userLogin.Fname && x.Lname == userLogin.Lname && x.Email == userLogin.Email).SingleOrDefault();

            if (reservation != null)
            {
                if(user != null)
                {
                    reservation.Roomid = id;
                    var rooms = _context.Rooms.Where(x => x.Id == reservation.Roomid).SingleOrDefault();
                    TimeSpan duration = (TimeSpan)(reservation.CheckOut - reservation.CheckIn);
                    decimal daysNum = (decimal)duration.TotalDays;
                    if (rooms != null)
                    {  
                        if(rooms.Availability== "available")
                        {
                            reservation.Userid = user.Id;
                            customer.Id = user.Id;
                            customer.Fname = user.Fname;
                            customer.Lname = user.Lname;
                            reservation.TotalPrice = daysNum * rooms.Pricepernight;
                            reservation.Hotelid = rooms.HotelId;
                            reservation.PayementStatus = "Pandding";

                            _context.Add(reservation);
                             _context.SaveChangesAsync();
                            HttpContext.Session.SetInt32("ReservationId", (int)reservation.Id);
                            return RedirectToAction("ReservationService", "ReservationServices");
                        }
                        else
                        {
                            ViewBag.error = "3";
                            return RedirectToAction("Blank", "ReservationServices");
                        }

                    }

                }


            }

            return View();
        }
        public IActionResult Reserve2(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Reserve2(int id, [Bind("Id,CheckIn,CheckOut,TotalPrice,PayementStatus,Hotelid,Roomid,Serviceid")] Reservation reservation, Room room, UserLogin userLogin, Customer customer)
        {
            var user = _context.UserLogins.Where(x => x.Fname == userLogin.Fname && x.Lname == userLogin.Lname && x.Email == userLogin.Email).SingleOrDefault();
            if (reservation != null)
            {   
                if (user != null)
                {
                    reservation.Userid = user.Id;
                    customer.Id = user.Id;
                    customer.Fname = user.Fname;
                    customer.Lname = user.Lname;
                    reservation.Roomid = 21;
                    reservation.Serviceid = id;
                    var res = _context.Services.Where(x => x.Id == reservation.Serviceid).SingleOrDefault();

                    if (res != null)
                    {
                        reservation.Hotelid = res.HotelId;
                        TimeSpan duration = (TimeSpan)(reservation.CheckOut - reservation.CheckIn);
                        decimal daysNum = (decimal)duration.TotalDays;
                        reservation.TotalPrice = daysNum * res.Price;
                        reservation.PayementStatus = "Pandding";

                        _context.Add(reservation);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetInt32("ReservationId", (int)reservation.Id);
                        return RedirectToAction("ReservationService", "ReservationServices");

                    }

                }    




            }
                return View();
        }














    }
}
