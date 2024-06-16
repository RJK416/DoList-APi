using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinalToDoAPI.Models;

namespace FinalToDoAPI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }

    public IActionResult Privacy()
    {
        return this.View();
    }

}
