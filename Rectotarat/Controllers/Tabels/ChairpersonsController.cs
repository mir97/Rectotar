using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Microsoft.AspNetCore.Authorization;
using Rectotarat.ViewModels;

namespace Rectotarat.Controllers.Tabels
{
    [Authorize(Roles = "admin")]
    public class ChairpersonsController : Controller
    {
        private readonly RectoratContext _context;

        public ChairpersonsController(RectoratContext context)
        {
            _context = context;
        }

        // GET: Chairpersons
        public IActionResult Index(int page = 1)
        {


            int pageSize = 5;


            IQueryable<Chairperson> source = _context.Chairpersons.Include(c => c.Rector);


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageView = new PageViewModel(count, page, pageSize);


            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                Chairpersons = items
            };

            return View(ivm);
        }

        // GET: Chairpersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chairperson = await _context.Chairpersons
                .Include(c => c.Rector)
                .SingleOrDefaultAsync(m => m.ChairpersonId == id);
            if (chairperson == null)
            {
                return NotFound();
            }

            return View(chairperson);
        }

        // GET: Chairpersons/Create
        public IActionResult Create()
        {
            ViewData["RectorId"] = new SelectList(_context.Rectors, "RectorId", "FullName");
            return View();
        }

        // POST: Chairpersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChairpersonId,StartDate,StopDate,RectorId")] Chairperson chairperson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chairperson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RectorId"] = new SelectList(_context.Rectors, "RectorId", "FullName", chairperson.RectorId);
            return View(chairperson);
        }

        // GET: Chairpersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chairperson = await _context.Chairpersons.SingleOrDefaultAsync(m => m.ChairpersonId == id);
            if (chairperson == null)
            {
                return NotFound();
            }
            ViewData["RectorId"] = new SelectList(_context.Rectors, "RectorId", "FullName", chairperson.RectorId);
            return View(chairperson);
        }

        // POST: Chairpersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChairpersonId,StartDate,StopDate,RectorId")] Chairperson chairperson)
        {
            if (id != chairperson.ChairpersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chairperson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChairpersonExists(chairperson.ChairpersonId))
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
            ViewData["RectorId"] = new SelectList(_context.Rectors, "RectorId", "FullName", chairperson.RectorId);
            return View(chairperson);
        }

        // GET: Chairpersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chairperson = await _context.Chairpersons
                .Include(c => c.Rector)
                .SingleOrDefaultAsync(m => m.ChairpersonId == id);
            if (chairperson == null)
            {
                return NotFound();
            }

            return View(chairperson);
        }

        // POST: Chairpersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chairperson = await _context.Chairpersons.SingleOrDefaultAsync(m => m.ChairpersonId == id);
            _context.Chairpersons.Remove(chairperson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChairpersonExists(int id)
        {
            return _context.Chairpersons.Any(e => e.ChairpersonId == id);
        }
    }
}
