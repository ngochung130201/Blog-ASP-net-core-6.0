using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Helpper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList.Core;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Posts
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
        }

        // GET: Admin/Posts/Details/5
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

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categorys, "CategoryId", "TenDanhMuc");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,NamePost,Avatar,ContentSort,Description,CategoryId,CreatedAt,UpdateAt")] Posts posts, Microsoft.AspNetCore.Http.IFormFile avatar)
        {
            if (ModelState.IsValid)
            {
                if (avatar != null)
                {
                    string extension = Path.GetExtension(avatar.FileName);
                    string image = Utilities.SEOUrl(posts.NamePost) + extension;
                    posts.Avatar = await Utilities.UploadFile(avatar, @"post", image.ToLower());
                }
                posts.CreatedAt = DateTime.Now;
                posts.UpdateAt = DateTime.Now;
                _context.Add(posts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categorys, "CategoryId", "TenDanhMuc", posts.CategoryId);
            return View(posts);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.FindAsync(id);
            if (posts == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categorys, "CategoryId", "TenDanhMuc", posts.CategoryId);
            return View(posts);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,NamePost,Avatar,ContentSort,Description,CategoryId,CreatedAt,UpdateAt")] Posts posts, Microsoft.AspNetCore.Http.IFormFile avatar)
        {
            if (id != posts.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (avatar != null)
                    {
                        string extension = Path.GetExtension(avatar.FileName);
                        string image = Utilities.SEOUrl(posts.NamePost) + extension;
                        posts.Avatar = await Utilities.UploadFile(avatar, @"post", image.ToLower());
                    }
                    posts.CreatedAt = DateTime.Now;
                    posts.UpdateAt = DateTime.UtcNow;
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.PostId))
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
            ViewData["DanhMuc"] = new SelectList(_context.Categorys, "CategoryId", "TenDanhMuc", posts.CategoryId);
            return View(posts);
        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var posts = await _context.Posts.FindAsync(id);
            if (posts != null)
            {
                _context.Posts.Remove(posts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostsExists(int id)
        {
          return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
