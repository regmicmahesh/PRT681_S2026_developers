using Microsoft.AspNetCore.Mvc;

namespace week1HelloWorldMVC.Controllers;

public class HelloWorldController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Welcome(string name = "Student", int numTimes = 1)
    {
        ViewData["Message"] = $"Hello, {name}!";
        ViewData["NumTimes"] = Math.Clamp(numTimes, 1, 10);
        return View();
    }
}
