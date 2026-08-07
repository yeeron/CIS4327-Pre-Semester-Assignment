using Microsoft.AspNetCore.Mvc;

namespace VolunteerMS.Controllers;
public class DashboardController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}