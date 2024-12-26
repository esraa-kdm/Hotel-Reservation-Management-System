using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bokking.Models;
using Microsoft.Extensions.Hosting;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Bokking.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public CustomersController(ModelContext context , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment; 

        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                if (customer.ImageFile != null)
                {
                    String wwwrootPath = _environment.WebRootPath;
                    String FileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    String path = Path.Combine(wwwrootPath + "/Image/", FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }
                    customer.ImagePath = FileName;
                }

                    _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Fname,Lname,ImagePath,ImageFile")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ModelContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(decimal id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult EditProfile(int id)
        {
            ViewBag.fname = HttpContext.Session.GetString("fname");
            ViewBag.lname = HttpContext.Session.GetString("Lname");
            if (id == null || _context.Customers == null)
            {
                return View("Create");
            }

            var customer = _context.Customers.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (customer == null)
            {
                return View("Index");
            }
            return View(); // تمرير الكائن إلى طريقة العرض
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(int id, [Bind("Fname,Lname,ImagePath,ImageFile")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (customer.ImageFile != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Image/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }
                    customer.ImagePath = fileName;
                }

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");

            }

            return View("Create"); // تمرير الكائن إلى طريقة العرض
        }




        public IActionResult CustomerProfile(int id)
        {
            ViewBag.fname = HttpContext.Session.GetString("fname");
            ViewBag.lname = HttpContext.Session.GetString("Lname");
            if (id == null || _context.Customers == null)
            {
                return View("Create");
            }

            var customer = _context.Customers.Where(x=>x.Id ==id).SingleOrDefaultAsync();
            if (customer == null)
            {
                return View("Index");
            }
            return View(); // تمرير الكائن إلى طريقة العرض
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>CustomerProfile(int id, [Bind("Fname,Lname,ImagePath,ImageFile")] Customer customer)
        {   
            if (id != customer.Id)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                if (customer.ImageFile != null)
                {
                    string wwwrootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    string path = Path.Combine(wwwrootPath + "/Image/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }
                    customer.ImagePath = fileName;
                }

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");

            }

            return View("Create"); // تمرير الكائن إلى طريقة العرض
        }







    }
}
