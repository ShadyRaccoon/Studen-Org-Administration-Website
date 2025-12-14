using Microsoft.AspNetCore.Mvc;

namespace StudentOrg_A4_Website.Controllers
{
    public class StaticContentController : Controller
    {
        public IActionResult Departments()
        {
            return View();
        }
    }
}
