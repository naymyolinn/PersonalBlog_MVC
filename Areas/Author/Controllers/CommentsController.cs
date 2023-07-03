using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using personalblog.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;


namespace personalblog.Areas.Author.Controllers
{
    [Authorize]
    [Area("Author")]
    public class CommentsController : Controller
    {
        private readonly BlogContext _context;


        public CommentsController(BlogContext _context)
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
           .Include(e => e.Posts)
           .Where(e => e.Posts.AuthorId == userid)
           .Count();
        }

        public async Task<IActionResult> Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PopulateViewData(userid);
            var data = await _context
            .Comments
            .Include(p => p.Posts)
            .Where(p => p.Posts.AuthorId == userid)
            .ToListAsync();
            return View(data);
        }

    }

}