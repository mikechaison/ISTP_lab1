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
    public class ArticleDraftController : Controller
    {
        private readonly Lab1dbContext _context;

        public ArticleDraftController(Lab1dbContext context)
        {
            _context = context;
        }

        // GET: ArticleDraft
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.ArticleDrafts.Include(a => a.Author).Include(a => a.SuggestedCategory);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: ArticleDraft/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleDraft = await _context.ArticleDrafts
                .Include(a => a.Author)
                .Include(a => a.SuggestedCategory)
                .FirstOrDefaultAsync(m => m.ArticleDraftId == id);
            if (articleDraft == null)
            {
                return NotFound();
            }

            return View(articleDraft);
        }

        // GET: ArticleDraft/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name");
            ViewData["SuggestedCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: ArticleDraft/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleDraftId,Title,AuthorId,SuggestedCategoryId,Text")] ArticleDraft articleDraft)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleDraft);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", articleDraft.AuthorId);
            ViewData["SuggestedCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", articleDraft.SuggestedCategoryId);
            return View(articleDraft);
        }

        // GET: ArticleDraft/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleDraft = await _context.ArticleDrafts.FindAsync(id);
            if (articleDraft == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "Name", articleDraft.AuthorId);
            ViewData["SuggestedCategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", articleDraft.SuggestedCategoryId);
            return View(articleDraft);
        }

        // POST: ArticleDraft/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleDraftId,Title,AuthorId,SuggestedCategoryId,Text")] ArticleDraft articleDraft)
        {
            if (id != articleDraft.ArticleDraftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleDraft);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleDraftExists(articleDraft.ArticleDraftId))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "UserId", "UserId", articleDraft.AuthorId);
            ViewData["SuggestedCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", articleDraft.SuggestedCategoryId);
            return View(articleDraft);
        }

        // GET: ArticleDraft/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleDraft = await _context.ArticleDrafts
                .Include(a => a.Author)
                .Include(a => a.SuggestedCategory)
                .FirstOrDefaultAsync(m => m.ArticleDraftId == id);
            if (articleDraft == null)
            {
                return NotFound();
            }

            return View(articleDraft);
        }

        // POST: ArticleDraft/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleDraft = await _context.ArticleDrafts.FindAsync(id);
            if (articleDraft != null)
            {
                _context.ArticleDrafts.Remove(articleDraft);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleDraftExists(int id)
        {
            return _context.ArticleDrafts.Any(e => e.ArticleDraftId == id);
        }
    }
}