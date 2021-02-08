using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Rectotarat.Data;
using Rectotarat.Models;
using Rectotarat.ViewModels;

namespace GomelRectorCouncil.Areas.Admin.Controllers
{

    [Authorize(Roles = "admin")]
    public class UniversitiesController : Controller
    {
        private readonly RectoratContext _context;
        private IHostingEnvironment _environment;
        private IConfiguration _iconfiguration;
        private UniversityExternalFile _externalFile;
        public UniversitiesController(RectoratContext context, IHostingEnvironment environment, IConfiguration iconfiguration)
        {
            _context = context;
            _environment = environment;
            _iconfiguration = iconfiguration;
            _externalFile = new UniversityExternalFile(_environment, _iconfiguration);

        }

        public IActionResult Index(string UniversityName, string LastName, string Email, string RegistrationNumber, string DocumentName, int page = 1, SortState sortOrder = SortState.UniversityNameAsc)
        {

            int pageSize = 5;

            ViewData["NameUniversity"] = sortOrder == SortState.UniversityNameAsc ? SortState.UniversityNameDesc : SortState.UniversityNameAsc;

            IQueryable<University> source = _context.Universitys;

            if (UniversityName != null)
            {
                source = source.Where(x => x.UniversityName.Contains(UniversityName));
            }


         

            switch (sortOrder)
            {

                case SortState.UniversityNameDesc:
                    source = source.OrderByDescending(x => x.UniversityName);
                    break;
                case SortState.UniversityNameAsc:
                    source = source.OrderBy(x => x.UniversityName);
                    break;
            }


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                FilterViewModel = new FilterViewModel(UniversityName, LastName, Email, RegistrationNumber, DocumentName),
                Universitys = items
            };
            return View(ivm);

        }

        // GET: Universities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.Universitys
                .SingleOrDefaultAsync(m => m.UniversityId == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // GET: Universities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniversityId,UniversityName,Address,Website,Logo")] University university, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                var universityWithLogo = await _externalFile.UploadUniversityWithLogo(university, upload);
                _context.Add(universityWithLogo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(university);
        }

        // GET: Universities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.Universitys.SingleOrDefaultAsync(m => m.UniversityId == id);
            if (university == null)
            {
                return NotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(University university, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    university = await _externalFile.UploadUniversityWithLogo(university, upload);
                    _context.Update(university);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversityExists(university.UniversityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(university);
        }

        // GET: Universities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.Universitys
                .SingleOrDefaultAsync(m => m.UniversityId == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int UniversityId)
        {

            var university = await _context.Universitys.SingleOrDefaultAsync(m => m.UniversityId == UniversityId);
            string fullFileName = _environment.WebRootPath + university.Logo;
            _context.Universitys.Remove(university);
            await _context.SaveChangesAsync();
            //Удаление фотографии
            try
            {
                System.IO.File.Delete(fullFileName);
                return RedirectToAction("Index");
            }
            catch (IOException deleteError)
            {
                return View("Message", deleteError.Message);
            }
        }

        private bool UniversityExists(int id)
        {
            return _context.Universitys.Any(e => e.UniversityId == id);
        }

    }
}
