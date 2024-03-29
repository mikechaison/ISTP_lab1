using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Newsreel.Models;
using Microsoft.EntityFrameworkCore;

namespace MVC.Newsreel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Data.Lab1dbContext _context;

    public HomeController(ILogger<HomeController> logger, Data.Lab1dbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var lab1dbContext = _context.Articles.OrderByDescending(a => a.PubDate).Take(3);
        return View(await lab1dbContext.ToListAsync());
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
