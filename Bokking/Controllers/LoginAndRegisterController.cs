using Bokking.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bokking.Controllers
{
    public class LoginAndRegisterController : Controller
    {
            private readonly ModelContext _context;
            private readonly IWebHostEnvironment _environment;

            public LoginAndRegisterController(ModelContext context, IWebHostEnvironment environment)
            {
                _context = context;
                _environment = environment;
            }
            public IActionResult Register()
            {
               return View();
            }

           [HttpPost]
           [ValidateAntiForgeryToken]

          public async Task<IActionResult> Register([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Customer customer, String Fname, String Lname,  String UserName, String email, String password)
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
                    HttpContext.Session.SetString("Image", customer.ImagePath);
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();


                UserLogin userLogin = new UserLogin();
                userLogin.Fname = Fname;
                userLogin.Lname = Lname;
                userLogin.Username = UserName;
                userLogin.Email = email;
                userLogin.Password = password;
                userLogin.Role = 2;
                userLogin.Customerid=customer.Id;

                _context.Add(userLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");

            }
            return View(customer);
          }

         public IActionResult Login()
         {
            return View();
         }

        [HttpPost]
        public async Task<IActionResult>Login([Bind("Username,Password")] UserLogin userLogin)
        {
            var auth = _context.UserLogins.Where(x => x.Username == userLogin.Username && x.Password == userLogin.Password).SingleOrDefault();
            if (auth!=null)
            {
                 HttpContext.Session.SetString("fname", auth.Fname);
                 HttpContext.Session.SetString("Lname", auth.Lname);
                 
                switch (auth.Role)
                {
                    case 1:
                        HttpContext.Session.SetString("AdminName",userLogin.Username);
                        HttpContext.Session.SetInt32("AdminId",(int)userLogin.Id);  
                        return RedirectToAction("Index", "Admin");

                    case 2:
						HttpContext.Session.SetString("UserName", userLogin.Username);
                      
                        return RedirectToAction("Index", "Home");



                }


            }
            return View();

        }

    }
}
