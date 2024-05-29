using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Docteur.Models;

namespace Docteur.Controllers;
y b
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        CaracteristiqueCorps[] parametres = new CaracteristiqueCorps().Find(null, null).OfType<CaracteristiqueCorps>().ToArray();
        return View(parametres);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
