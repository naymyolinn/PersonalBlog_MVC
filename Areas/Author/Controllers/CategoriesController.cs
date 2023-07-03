using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personalblog.DataContext;
using personalblog.Models;
using personalblog.Models.ViewModels;

namespace personalblog.Areas.Author.Controllers
{
    [Area("Author")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly BlogContext _context;

        public CategoriesController(BlogContext _context)
        {
            this._context = _context;
        }

        private void PopulateViewData(string userid)
        {
            ViewData["PostCount"] = _context.Posts
                      .Where(p => p.AuthorId == userid)
                      .Count();
            ViewData["categoryCount"] = _context.Categories
            .Where(c => c.AuthorId == userid)
            .Count();
            ViewData["commentCount"] = _context.Comments
            .Include(cm => cm.Posts)
            .Where(cm => cm.Posts.AuthorId == userid)
            .Count();
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PopulateViewData(userid);
            return View(await _context.Categories
            .Where(m => m.AuthorId == userid)
            .ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cateid == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cateid,Categoryname")] CategoryVM category)
        {
            if (ModelState.IsValid)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var data = new Category()
                {
                    Categoryname = category.Categoryname,
                    AuthorId = userid
                };
                _context.Add(data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var category = await _context.Categories.SingleOrDefaultAsync(m => m.Cateid == id
             && m.AuthorId == userid);
            if (category == null)
            {
                return NotFound();
            }
            return View(new CategoryVM()
            {
                Cateid = category.Cateid,
                Categoryname = category.Categoryname
            });
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cateid,Categoryname")] CategoryVM category)
        {
            if (id != category.Cateid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var data = await _context.Categories.SingleOrDefaultAsync(m => m.Cateid == id
                     && m.AuthorId == userid);
                    if (data == null)
                    {
                        return NotFound();
                    }
                    data.Categoryname = category.Categoryname;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Cateid))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Cateid == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Cateid == id);
        }
    }
}
