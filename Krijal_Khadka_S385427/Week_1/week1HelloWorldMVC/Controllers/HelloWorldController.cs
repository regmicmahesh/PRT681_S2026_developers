using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
 
namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
     
     public IActionResult Index()
{
    return View();
}

     
    public string Welcome(string name, int id = 1)
{
    return HtmlEncoder.Default.Encode(
        $"Hello {name}, ID: {id}"
    );
}
        
      
    }
}