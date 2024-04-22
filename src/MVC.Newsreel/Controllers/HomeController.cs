using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Newsreel.Models;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Services;
using MVC.Newsreel.Data;

namespace MVC.Newsreel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Lab1dbContext _context;
    private readonly IEmailService _emailService;

    public HomeController(ILogger<HomeController> logger, 
                        Lab1dbContext context,
                        IEmailService emailService)
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var lab1dbContext = _context.Articles.OrderByDescending(a => a.PubDate).Take(3);

        /*UserEmailOptions options = new UserEmailOptions
        {
            ToEmails = new List<string>() { "minko.vad005@gmail.com" },
            PlaceHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}", "VADIM")
            }
        };

        await _emailService.SendTestEmail(options);*/

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
