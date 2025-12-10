using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentOrg_A4_Website.Controllers
{
    [Authorize(Roles = "Admin,Bureau")]
    public class ControlPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}