using Microsoft.AspNetCore.Mvc;
using siguria.Models;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using System.Linq;
using siguria.Data;
using reCAPTCHA.AspNetCore;

namespace siguria.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRecaptchaService _recaptchaService;

        public AccountController(AppDbContext context, IRecaptchaService recaptchaService)
        {
            _context = context;
            _recaptchaService = recaptchaService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            // Kontrollo validimin automatik te modelit
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email;
            var password = model.Password;

            
            email = HtmlEncoder.Default.Encode(email);
            password = HtmlEncoder.Default.Encode(password);

            if (email.Contains("'") || email.Contains("--"))
            {
                ViewBag.Error = "Email jo i vlefshëm.";
                return View(model);
            }

            // kontrollo nese perdoruesi ekziston
            User? existingUser = _context.Users.FirstOrDefault(u => u.Username == email);
            if (existingUser != null)
            {
                ViewBag.Error = "Ky email është regjistruar më parë.";
                return View(model);
            }

            // Krijo dhe ruaj perdoruesin
            var user = new User { Username = email, Password = password };
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var recaptcha = await _recaptchaService.Validate(Request);
            if (!recaptcha.success)
            {
                ViewBag.Error = "Verifikimi i reCAPTCHA dështoi. Ju lutemi provoni përsëri.";
                return View();
            }

            var encodedEmail = HtmlEncoder.Default.Encode(email);
            var encodedPassword = HtmlEncoder.Default.Encode(password);

            var user = _context.Users.FirstOrDefault(u => u.Username == encodedEmail && u.Password == encodedPassword);
            if (user != null)
            {
                HttpContext.Session.SetString("user", user.Username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Kredencialet janë të pasakta.";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}