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
    public class DislikeController : Controller
    {
        private readonly Lab1dbContext _context;

        public DislikeController(Lab1dbContext context)
        {
            _context = context;
        }

        // GET: Dislike
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.Dislikes.Include(d => d.Article).Include(d => d.Comment).Include(d => d.User);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: Dislike/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes
                .Include(d => d.Article)
                .Include(d => d.Comment)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DislikeId == id);
            if (dislike == null)
            {
                return NotFound();
            }

            return View(dislike);
        }

        // GET: Dislike/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId");
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Dislike/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DislikeId,UserId,ArticleId,CommentId")] Dislike dislike)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dislike);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", dislike.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId", dislike.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dislike.UserId);
            return View(dislike);
        }

        // GET: Dislike/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes.FindAsync(id);
            if (dislike == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", dislike.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId", dislike.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dislike.UserId);
            return View(dislike);
        }

        // POST: Dislike/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DislikeId,UserId,ArticleId,CommentId")] Dislike dislike)
        {
            if (id != dislike.DislikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dislike);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DislikeExists(dislike.DislikeId))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", dislike.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId", dislike.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", dislike.UserId);
            return View(dislike);
        }

        // GET: Dislike/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes
                .Include(d => d.Article)
                .Include(d => d.Comment)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DislikeId == id);
            if (dislike == null)
            {
                return NotFound();
            }

            return View(dislike);
        }

        // POST: Dislike/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dislike = await _context.Dislikes.FindAsync(id);
            if (dislike != null)
            {
                _context.Dislikes.Remove(dislike);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DislikeExists(int id)
        {
            return _context.Dislikes.Any(e => e.DislikeId == id);
        }
    }
}
