using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Rectotarat.ViewModels;

namespace Rectotarat.Controllers.Tabels
{
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {
        private readonly RectoratContext _context;

        public NewsController(RectoratContext context)
        {
            _context = context;
        }

        // GET: News
        public IActionResult Index(int page=1)
        {
            IQueryable<News> source = _context.Newss.Include(n => n.Univercity);
            int pageSize = 5;
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                Newss = items
            };

            return View(ivm);

        }

        // GET: News/Details/5
       
        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Newsid,UnivercityId,Header,Message,PublicDate")] News news)
        {
            if (ModelState.IsValid)
            {
                news.PublicDate = DateTime.Now;
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", news.UnivercityId);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.Newss.SingleOrDefaultAsync(m => m.Newsid == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", news.UnivercityId);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Newsid,UnivercityId,Header,Message,PublicDate")] News news)
        {
            if (id != news.Newsid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Newsid))
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
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", news.UnivercityId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.Newss
                .Include(n => n.Univercity)
                .SingleOrDefaultAsync(m => m.Newsid == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.Newss.SingleOrDefaultAsync(m => m.Newsid == id);
            _context.Newss.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.Newss.Any(e => e.Newsid == id);
        }
    }
}
