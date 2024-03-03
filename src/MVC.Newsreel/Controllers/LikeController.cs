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
    public class LikeController : Controller
    {
        private readonly Lab1dbContext _context;

        public LikeController(Lab1dbContext context)
        {
            _context = context;
        }

        // GET: Like
        public async Task<IActionResult> Index()
        {
            var lab1dbContext = _context.Likes.Include(l => l.Article).Include(l => l.Comment).Include(l => l.User);
            return View(await lab1dbContext.ToListAsync());
        }

        // GET: Like/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Article)
                .Include(l => l.Comment)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Like/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title");
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Text");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        // POST: Like/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikeId,UserId,ArticleId,CommentId")] Like like)
        {
            if (ModelState.IsValid)
            {
                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", like.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId", like.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", like.UserId);
            return View(like);
        }

        // GET: Like/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "Title", like.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Text", like.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", like.UserId);
            return View(like);
        }

        // POST: Like/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikeId,UserId,ArticleId,CommentId")] Like like)
        {
            if (id != like.LikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.LikeId))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", like.ArticleId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "CommentId", like.CommentId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", like.UserId);
            return View(like);
        }

        // GET: Like/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Article)
                .Include(l => l.Comment)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Like/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.LikeId == id);
        }
    }
}
