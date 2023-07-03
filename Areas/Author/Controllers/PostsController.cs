using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using personalblog.DataContext;
using System.Security.Claims;
using personalblog.Models;
using personalblog.Models.ViewModels;

namespace personalblog.Areas.Author.Controllers
{
    [Area("Author")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext _context)
        {
            this._context = _context;
        }

        private async Task<SelectList> GenerateList()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return new SelectList(await _context.Categories
                                    .Where(c => c.AuthorId == userid)
                                    .OrderBy(c => c.Categoryname).ToListAsync(), "Cateid", "Categoryname");

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
           .Include(e => e.Posts)
           .Where(e => e.Posts.AuthorId == userid)
           .Count();
        }


        // GET: Posts

        public async Task<IActionResult> Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PopulateViewData(userid);
            return View(await _context.Posts
            .Include(p => p.Category)
            .Where(p => p.AuthorId == userid)
            .ToListAsync());
        }
        [HttpPost]
        public IActionResult Publish(Guid Id)
        {
            var post = _context.Posts.Find(Id);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }
            post.Status = (byte)PostStatus.Published;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public IActionResult Unpublish(Guid Id)
        {
            var post = _context.Posts.Find(Id);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }
            post.Status = (byte)PostStatus.Unpublished;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


        // GET: Posts/Details/5


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Comment)
             .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(new PostCommentVM()
            {
                Post = post,
                PostId = post.Id,

            });
        }
        [HttpPost]
        [ActionName("details")]
        [Area("Author")]
        public async Task<IActionResult> CreateComment(PostCommentVM model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Postid = model.PostId,
                    CommentText = model.CommentText,
                    Name = "Testing",
                    CommentDate = DateTime.UtcNow
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), nameof(Details), comment.Postid);
                // return CreatedAtRoute("Author", comment);

            }
            return View(nameof(Index));

        }



        // GET: Posts/Create

        public async Task<IActionResult> Create()
        {
            ViewBag.Categoryid = await GenerateList();
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Route("{action=Index", Name = "Author")]
        [Area("Author")]
        public async Task<IActionResult> Create([Bind("Id,Title,Categoryid,Subtitle,Body")] PostVM post)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                var data = new Post()
                {
                    Id = Guid.NewGuid(),
                    Title = post.Title,
                    Categoryid = post.Categoryid,
                    Subtitle = post.Subtitle,
                    Body = post.Body,
                    AuthorId = userid,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Add(data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categoryid = GenerateList();
            return View(post);
        }

        // GET: Posts/Edit/5

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && p.AuthorId == userid);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categoryid = await GenerateList();

            return View(new PostVM() { Id = post.Id, Title = post.Title, Categoryid = post.Categoryid, Subtitle = post.Subtitle, Body = post.Body });
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //XSS
        [ValidateAntiForgeryToken]
        [Area("Author")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Subtitle,Body,Categoryid")] PostVM post)
        {
            if (ModelState.IsValid)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var data = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && p.AuthorId == userid);
                data.Title = post.Title;
                data.Subtitle = post.Subtitle;
                data.Body = post.Body;
                data.Categoryid = post.Categoryid;
                // _context.Update(post);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categoryid = GenerateList();
            return View(post);

            // if (id != post.Id)
            // {
            //     return NotFound();
            // }


            // if (ModelState.IsValid)
            // {
            //     var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //     var data = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && p.AuthorId == userid);
            //     if (data == null)
            //     {
            //         return NotFound();
            //     }

            //     try
            //     {
            //         data.Title = post.Title;
            //         data.Subtitle = post.Subtitle;
            //         data.Body = post.Body;
            //         data.Categoryid = post.Categoryid;
            //         // _context.Update(post);

            //         await _context.SaveChangesAsync();
            //     }
            //     catch (DbUpdateConcurrencyException)
            //     {
            //         if (!PostExists(post.Id))
            //         {
            //             return NotFound();
            //         }
            //         else
            //         {
            //             throw;
            //         }
            //     }

            //     return RedirectToAction(nameof(Index));
            // }
            // ViewBag.Categoryid = GenerateList();
            // return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }



}
