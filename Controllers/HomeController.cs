using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using personalblog.Models;
using personalblog.DataContext;
using Microsoft.EntityFrameworkCore;

namespace personalblog.Controllers
{
    public class HomeController : Controller
    {

        private readonly BlogContext _context;



        public HomeController(BlogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //single or multi
            //multi filter with published only

            return View(await _context.Posts
            .Include(p => p.Comment)
            .Where(p => p.Status == (byte)PostStatus.Published)
            .ToListAsync());
        }

        public IActionResult View(Guid Id)
        {

            var post = _context.Posts
            .Include(p => p.Author)
            .FirstOrDefault(p =>
             p.Status == (byte)PostStatus.Published
             && p.Id == Id);

            if (post == null)
            {

                return RedirectToAction(nameof(Index));
            }

            return View(post);

        }



        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Post()
        {
            return View();

        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Catalog() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
