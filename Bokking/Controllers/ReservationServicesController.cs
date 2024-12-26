using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bokking.Models;
using System.Collections.Immutable;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Hosting;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.IO;
using System.Linq;



namespace Bokking.Controllers
{
    public class ReservationServicesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        
        



        public ReservationServicesController(ModelContext context, IConfiguration configuration )
        {
            _context = context;
            _configuration = configuration;
          
          
           
        }

        // GET: ReservationServices
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.ReservationServices.Include(r => r.Card).Include(r => r.Reservation);
            return View(await modelContext.ToListAsync());
        }

        // GET: ReservationServices/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.ReservationServices == null)
            {
                return NotFound();
            }

            var reservationService = await _context.ReservationServices
                .Include(r => r.Card)
                .Include(r => r.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationService == null)
            {
                return NotFound();
            }

            return View(reservationService);
        }

        // GET: ReservationServices/Create
        public IActionResult Create()
        {
            ViewData["Cardid"] = new SelectList(_context.Banks, "Cardid", "Cardid");
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Id", "Id");
            return View();
        }

        // POST: ReservationServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentMethod,Amount,Cardid,InvoiceSent,Reservationid")] ReservationService reservationService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cardid"] = new SelectList(_context.Banks, "Cardid", "Cardid", reservationService.Cardid);
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Id", "Id", reservationService.Reservationid);
            return View(reservationService);
        }

        // GET: ReservationServices/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.ReservationServices == null)
            {
                return NotFound();
            }

            var reservationService = await _context.ReservationServices.FindAsync(id);
            if (reservationService == null)
            {
                return NotFound();
            }
            ViewData["Cardid"] = new SelectList(_context.Banks, "Cardid", "Cardid", reservationService.Cardid);
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Id", "Id", reservationService.Reservationid);
            return View(reservationService);
        }

        // POST: ReservationServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,PaymentMethod,Amount,Cardid,InvoiceSent,Reservationid")] ReservationService reservationService)
        {
            if (id != reservationService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationServiceExists(reservationService.Id))
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
            ViewData["Cardid"] = new SelectList(_context.Banks, "Cardid", "Cardid", reservationService.Cardid);
            ViewData["Reservationid"] = new SelectList(_context.Reservations, "Id", "Id", reservationService.Reservationid);
            return View(reservationService);
        }

        // GET: ReservationServices/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.ReservationServices == null)
            {
                return NotFound();
            }

            var reservationService = await _context.ReservationServices
                .Include(r => r.Card)
                .Include(r => r.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationService == null)
            {
                return NotFound();
            }

            return View(reservationService);
        }

        // POST: ReservationServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.ReservationServices == null)
            {
                return Problem("Entity set 'ModelContext.ReservationServices'  is null.");
            }
            var reservationService = await _context.ReservationServices.FindAsync(id);
            if (reservationService != null)
            {
                _context.ReservationServices.Remove(reservationService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationServiceExists(decimal id)
        {
          return (_context.ReservationServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult ReservationService()
        {
 
            return View();
        }
       


        [HttpPost]
        public async Task<IActionResult> ReservationService([Bind("Id,PaymentMethod,Amount,Cardid,InvoiceSent,Reservationid")] ReservationService reservationService ,Reservation reservation ,Bank bank,string Email,string Subject,string Body)
        {    reservationService.Reservationid= HttpContext.Session.GetInt32("ReservationId");
             
            if (reservationService != null)
            {
                var check = _context.Reservations.Where(x=>x.Id == reservationService.Reservationid).SingleOrDefault();
                reservationService.Amount = check.TotalPrice;
                var ban = _context.Banks.Where(x => x.Cardid == reservationService.Cardid).SingleOrDefault();
                if (ban !=null)
                {

                    if ( reservationService.Amount>ban.Balance)
                    {
                        ViewBag.error ="1";
                        return RedirectToAction("Blank");
                        
                    }
                    else
                    {
                        ban.Balance = ban.Balance - reservationService.Amount;
                        reservationService.InvoiceSent = "true";
                        check.PayementStatus = "Completed";
                        _context.Add(reservationService);
                        await _context.SaveChangesAsync();
                        byte[] pdfBytes =GeneratePdfReport((int)check.Id);
                        using var stream = new MemoryStream(pdfBytes);
                        var emailSettings = _configuration.GetSection("EmailSettings");

                        var smtpClient = new SmtpClient("smtp.office365.com",587)
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("esraakdm@outlook.com", "esraa2021901110"),
                            EnableSsl = true,
                        };

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress("esraakdm@outlook.com"),
                            Subject = "Welcome to Our Service",
                            Body = "Thank you for registering to our Hotel",
                            IsBodyHtml = true,
                        };

                        mailMessage.To.Add("esraakdm@outlook.com");
                        mailMessage.Attachments.Add(new Attachment(stream, "invoice.pdf"));
                        return RedirectToAction("Index", "Home");


                      





                    }

                }
                else
                {
                    ViewBag.error2 ="2";
                    return RedirectToAction("Blank");
                    
                }
            }
            return View();
        }
        public byte[] GeneratePdfReport(int id)
        {
            var reservation = _context.Reservations.Where(x => x.Id == id).FirstOrDefault();

            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);


                        var table = new Table(7);
                        table.AddCell("Hotel Name");
                        table.AddCell("Room Type");
                        table.AddCell("Price");
                        table.AddCell("Check-In");
                        table.AddCell("Check-Out");
                        table.AddCell("Benefit");

                        table.AddCell(reservation.Userid.ToString());
                        table.AddCell(reservation.Hotelid.ToString());
                        table.AddCell(reservation.Roomid.ToString());
                        table.AddCell(reservation.TotalPrice.ToString());
                        table.AddCell(reservation.CheckIn.ToString());
                        table.AddCell(reservation.CheckOut.ToString());


                        document.Add(table);
                    }
                }


                return memoryStream.ToArray();
            }
        }







        public IActionResult Blank()
        {
            return View();  
        }


    }
}
