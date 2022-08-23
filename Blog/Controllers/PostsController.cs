using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using PagedList.Core;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? page,string searchString)
        {
           
            try
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                    var pageSize = 6;
                    var lspPost1 = _context.Posts
                      .AsNoTracking()
                      .Include(x => x.Category).Where(c => c.ContentSort!.Contains(searchString));
                    // tim kiem thieu ten bai viet
                    var lspPost2 = _context.Posts
                      .AsNoTracking()
                      .Include(x => x.Category).Where(c => c.NamePost.ToString()!.Contains(searchString));
                    // tim kiem thieu ten bai viet
                    if (lspPost1 == null)
                    {
                        var lspPost = lspPost2;
                        PagedList<Posts> models = new PagedList<Posts>(lspPost, pageNumber, pageSize);
                        ViewBag.CurrentPage = pageNumber;
                        ViewBag.Search = searchString;
                        return View(models);
                    }
                    else
                    {
                        var lspPost = lspPost1;
                         PagedList<Posts> models = new PagedList<Posts>(lspPost, pageNumber, pageSize);
                        ViewBag.CurrentPage = pageNumber;
                        ViewBag.Search = searchString;
                        return View(models);
                    }

                  
                   
                }
               else
                {
                    var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                    var pageSize = 6;
                    var lspPost = _context.Posts
                      .AsNoTracking()
                      .Include(x => x.Category);
                    PagedList<Posts> models = new PagedList<Posts>(lspPost, pageNumber, pageSize);

                    ViewBag.CurrentPage = pageNumber;
                    return View(models);

                }
            }
            catch
            {
                //return RedirectToAction("Index", "Posts");
                return NotFound();

            }
            //var applicationDbContext = _context.Posts.Include(p => p.Category);
           // return View(await applicationDbContext.ToListAsync());
        }

       
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // GET: Posts/Create
       
    }
}
