using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinalDoListAPI.Models;

namespace DoListAPI.Controllers;

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
