using _19033684_Kumar_Pulami.Models.DatabaseModel.Login;
using _19033684_Kumar_Pulami.Models.ViewModel.Login;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace _19033684_Kumar_Pulami.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public IActionResult Login(LoginView loginCredential)
        {
            LoginDB loginDB = new();

            DataTable data = loginDB.Login(loginCredential);
            if (data.Rows.Count == 1)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.error = "Invalid, Please Try Again!";
                return View("Login");
            }
        }
    }
}
