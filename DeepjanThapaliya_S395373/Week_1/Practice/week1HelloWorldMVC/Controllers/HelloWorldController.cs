using Microsoft.AspNetCore.Mvc;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Controllers;

public class HelloWorldController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Welcome()
    {
        return View();
    }

    public IActionResult Profile()
    {
        var student = new StudentProfile
        {
            StudentId = "S395373",
            FullName = "Deepjan Thapaliya",
            UnitName = "Software Engineering Practice",
            CurrentWeek = 1,
            IsEnrolled = true
        };

        return View(student);
    }
}