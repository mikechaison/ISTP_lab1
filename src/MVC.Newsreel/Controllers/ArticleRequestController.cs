using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Newsreel.Data;

namespace MVC.Newsreel.Controllers_
{
    public class ArticleRequestController : Controller
    {
        private readonly Lab1dbContext _context;
        private readonly UserManager<User> _userManager;

        public ArticleRequestController(Lab1dbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var userName = _userManager.GetUserName(User);
            var userArticles = _context.Articles
                                .Include(a => a.Author)
                                .Where(a => a.Author.UserName == userName);
            var userArticleDrafts = _context.ArticleDrafts
                                .Include(a => a.Author)
                                .Where(a => a.Author.UserName == userName);
            ViewData["ArticleId"] = new SelectList(userArticles, "ArticleId", "Title");
            ViewData["ArticleDraftId"] = new SelectList(userArticleDrafts, "ArticleDraftId", "Title");
            return View();
        }

        // POST: ArticleRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleRequestId,AuthorId,ArticleDraftId,ArticleId,Status")] ArticleRequest articleRequest)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                articleRequest.Status="Checking";
                articleRequest.AuthorId = user.Id;
                _context.Add(articleRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userId = user.Id;
            var userArticles = _context.Articles.
                                Where(a => a.AuthorId == userId);
            var userArticleDrafts = _context.ArticleDrafts.
                                Where(a => a.AuthorId == userId);
            ViewData["ArticleId"] = new SelectList(userArticles, "ArticleId", "ArticleId", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(userArticleDrafts, "ArticleDraftId", "ArticleDraftId", articleRequest.ArticleDraftId);
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
            var userArticles = _context.Articles.
                                Where(a => a.AuthorId == articleRequest.AuthorId);
            var userArticleDrafts = _context.ArticleDrafts.
                                Where(a => a.AuthorId == articleRequest.AuthorId);
            ViewData["ArticleId"] = new SelectList(userArticles, "ArticleId", "Title", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(userArticleDrafts, "ArticleDraftId", "Title", articleRequest.ArticleDraftId);
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
            var userArticles = _context.Articles.
                                Where(a => a.AuthorId == articleRequest.AuthorId);
            var userArticleDrafts = _context.ArticleDrafts.
                                Where(a => a.AuthorId == articleRequest.AuthorId);
            ViewData["ArticleId"] = new SelectList(userArticles, "ArticleId", "ArticleId", articleRequest.ArticleId);
            ViewData["ArticleDraftId"] = new SelectList(userArticleDrafts, "ArticleDraftId", "ArticleDraftId", articleRequest.ArticleDraftId);
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
