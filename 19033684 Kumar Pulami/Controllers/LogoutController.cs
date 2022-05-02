using Microsoft.AspNetCore.Mvc;

namespace _19033684_Kumar_Pulami.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            return RedirectToActionPermanent("Login", "Login");
        }
    }
}
