using Microsoft.AspNetCore.Mvc;

namespace VolunteerMS.Controllers;

public abstract class BaseController : Controller
{
    protected const string SuccessMessageKey = "SuccessMessage";
    protected const string ErrorMessageKey = "ErrorMessage";

    /*
    protected int? CurrentUserId =>
        HttpContext.Session.GetInt32(SessionKeys.UserId);

    protected string? CurrentUsername =>
        HttpContext.Session.GetString(SessionKeys.Username);
    
    protected bool IsLoggedIn =>
        CurrentUserId.HasValue;
    */
    
    protected void SetSuccessMessage(string message)
    {
        TempData[SuccessMessageKey] = message;
    }

    protected void SetErrorMessage(string message)
    {
        TempData[ErrorMessageKey] = message;
    }
}