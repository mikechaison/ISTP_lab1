using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;

namespace MVC.Newsreel.Controllers_
{
    public class ArticleRequestController : Controller
    {
        private readonly Lab1dbContext _context;

        public ArticleRequestController(Lab1dbContext context)
        {
            _context = context;
        }

        // GET: ArticleRequest
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.ArticleRequests.Include(a => a.Article).Include(a => a.ArticleDraft).Include(a => a.Author);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: ArticleRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests
                .Include(a => a.Article)
                .Include(a => a.ArticleDraft)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }

            return View(articleRequest);
        }

        // GET: ArticleRequest/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title");
            ViewData["ArticleDraftId"] = new SelectList(_context.ArticleDrafts, "ArticleDraftId", "Title");
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        // POST: ArticleRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleRequestId,AuthorId,ArticleDraftId,ArticleId,Status")] ArticleRequest articleRequest)
        {
            if (ModelState.IsValid)
            {
                articleRequest.Status="Checking";
                _context.Add(articleRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(_context.ArticleDrafts, "ArticleDraftId", "ArticleDraftId", articleRequest.ArticleDraftId);
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", articleRequest.AuthorId);
            return View(articleRequest);
        }

        // GET: ArticleRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests.FindAsync(id);
            if (articleRequest == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(_context.ArticleDrafts, "ArticleDraftId", "Title", articleRequest.ArticleDraftId);
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name", articleRequest.AuthorId);
            return View(articleRequest);
        }

        // POST: ArticleRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleRequestId,AuthorId,ArticleDraftId,ArticleId,Status")] ArticleRequest articleRequest)
        {
            if (id != articleRequest.ArticleRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleRequest);
                    if (articleRequest.Status=="Approved")
                    {
                        var articleDraft = await _context.ArticleDrafts
                            .Include(a => a.SuggestedCategory)
                            .Include(a => a.Author)
                            .FirstOrDefaultAsync(m => m.ArticleDraftId == articleRequest.ArticleDraftId);
                        Article article = new Article
                        {
                            Title = articleDraft.Title,
                            Text = articleDraft.Text,
                            AuthorId = articleDraft.AuthorId,
                            Author = articleDraft.Author,
                            CategoryId = articleDraft.SuggestedCategoryId,
                            Category = articleDraft.SuggestedCategory,
                            PubDate = DateTime.UtcNow,
                            Image = articleDraft.Image,
                            ImageFile = articleDraft.ImageFile
                        };
                        _context.Add(article);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleRequestExists(articleRequest.ArticleRequestId))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(_context.ArticleDrafts, "ArticleDraftId", "ArticleDraftId", articleRequest.ArticleDraftId);
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", articleRequest.AuthorId);
            return View(articleRequest);
        }

        // GET: ArticleRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests
                .Include(a => a.Article)
                .Include(a => a.ArticleDraft)
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }

            return View(articleRequest);
        }

        // POST: ArticleRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleRequest = await _context.ArticleRequests.FindAsync(id);
            if (articleRequest != null)
            {
                _context.ArticleRequests.Remove(articleRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleRequestExists(int id)
        {
            return _context.ArticleRequests.Any(e => e.ArticleRequestId == id);
        }
    }
}
