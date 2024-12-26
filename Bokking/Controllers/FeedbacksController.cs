using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bokking.Models;

namespace Bokking.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly ModelContext _context;

        public FeedbacksController(ModelContext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index()
        {
              return _context.Feedbacks != null ? 
                          View(await _context.Feedbacks.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Feedbacks'  is null.");
        }

        // GET: Feedbacks/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Guestname,Guestemail,Guestmessage,Isapproved")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Guestname,Guestemail,Guestmessage,Isapproved")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'ModelContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(decimal id)
        {
          return (_context.Feedbacks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult ContactUs()
        {
            ViewBag.hotelid = _context.Hotels.ToList();
            ViewBag.feedAprove=_context.Feedbacks.Where(x=>x.Isapproved=="true").ToList();
            ViewBag.testAprove =_context.Testimonials.Where(x=> x.Isapproved=="true").ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs([Bind("Guestname,Guestemail,Guestmessage,Isapproved")] Feedback feedback ,int hotel)
        {
            var users = _context.UserLogins.SingleOrDefault(x => x.Email == feedback.Guestemail);
            if (users != null)
            {
                var cust = _context.Customers.SingleOrDefault(x => x.Id == users.Customerid);
                if (cust != null && feedback != null)
                {
                    Testimonial testimonial1 = new Testimonial
                    {
                        Userid = cust.Id,
                        Isapproved = "false",
                        Message = feedback.Guestmessage,
                        Hotelid = hotel,    
                    };

                    _context.Add(testimonial1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ThankYou));
                }
            }
            else if (feedback != null)
            {
                feedback.Isapproved = "false";
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThankYou));
            }

            return View();
        }


      
        [HttpPost]        
        
        public IActionResult FeedAprove2( Testimonial testimonial,int id)
        {
            var aprove = _context.Testimonials.Where(x => x.Id == id);
            testimonial.Isapproved = "true";
            _context.Add(testimonial);
          
            return View();
        }

        public IActionResult FeedAprove(Feedback feedback, int id)
        {
            var aprove = _context.Feedbacks.Where(x => x.Id == id);
            feedback.Isapproved = "true";
            _context.Feedbacks.Add(feedback);

            return View();
        }

        public IActionResult ThankYou()
        {
            return View();
        }





    }
}
