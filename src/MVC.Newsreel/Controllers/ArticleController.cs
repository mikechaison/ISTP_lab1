using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;
using System;

namespace MVC.Newsreel.Controllers_
{
    public class ArticleController : Controller
    {
        private readonly Lab1dbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(Lab1dbContext context,
                                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.Articles.Include(a => a.Author).Include(a => a.Category);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .Include(a => a.Comments).ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            article.Likes=_context.Likes.Where(a => a.ArticleId == article.ArticleId).Where(a => !a.IsDis).Count();
            article.Dislikes=_context.Likes.Where(a => a.ArticleId == article.ArticleId).Where(a => a.IsDis).Count();
            foreach (var item in article.Comments)
            {
                item.Likes=_context.Likes.Where(a => a.CommentId == item.CommentId).Where(a => !a.IsDis).Count();
                item.Dislikes=_context.Likes.Where(a => a.CommentId == item.CommentId).Where(a => a.IsDis).Count();
            }
            if (article == null)
            {
                return NotFound();
            }

            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title");
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");

            return View(article);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Title,CategoryId,AuthorId,Text,Likes,Dislikes,PubDate,ImageFile")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (article.ImageFile != null)
                {
                    string folder = "static/images/Article/";
                    folder += Guid.NewGuid().ToString() + "_" + article.ImageFile.FileName ;
                    article.Image = "/"+folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    await article.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                article.PubDate=DateTime.UtcNow;
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else{
                Console.WriteLine("aboba");
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", article.CategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,CategoryId,AuthorId,Text,Likes,Dislikes,PubDate,ImageFile")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (article.ImageFile != null)
                {
                    string folder = "static/images/Article/";
                    folder += Guid.NewGuid().ToString() + "_" + article.ImageFile.FileName ;
                    article.Image = "/"+folder;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                    await article.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                try
                {
                    article.PubDate=DateTime.UtcNow;
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", article.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", article.CategoryId);
            return View(article);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
