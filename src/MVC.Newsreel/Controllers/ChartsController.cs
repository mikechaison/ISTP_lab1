using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MVC.Newsreel.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController : ControllerBase
{
    private record CountByCategoryName(string Category, int Count);
    private record CountByAuthorName(string Author, int Count);
    private readonly Lab1dbContext lab1DbContext;
    public ChartsController(Lab1dbContext lab1DbContext)
    {
        this.lab1DbContext = lab1DbContext;
    }

    [HttpGet("countByCategory")]
    public async Task<JsonResult> GetCountByCategoryAsync(CancellationToken cancellationToken)
    {
        var responseItems = await lab1DbContext
        .Articles
        .GroupBy(article => article.Category.Name)
        .Select(group => new
        CountByCategoryName(group.Key.ToString(), group.Count()))
        .ToListAsync(cancellationToken);
        return new JsonResult(responseItems);
    }

    [HttpGet("countByAuthor")]
    public async Task<JsonResult> GetCountByAuthorAsync(CancellationToken cancellationToken)
    {
        var responseItems = await lab1DbContext
        .Articles
        .GroupBy(article => article.Author.Name)
        .Select(group => new
        CountByAuthorName(group.Key.ToString(), group.Count()))
        .ToListAsync(cancellationToken);
        return new JsonResult(responseItems);
    }
}