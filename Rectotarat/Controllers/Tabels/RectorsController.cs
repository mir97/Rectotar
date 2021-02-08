using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Rectotarat.Models;
using Rectotarat.ViewModels;
using Rectotarat.Data;

namespace GomelRectorCouncil.Areas.Admin.Controllers
{

    [Authorize(Roles = "admin")]
    public class RectorsController : Controller
    {
        private readonly RectoratContext _context;
        private IHostingEnvironment _environment;
        private IConfiguration _iconfiguration;
        private RectorExternalFile _externalFile;

        public RectorsController(RectoratContext context, IHostingEnvironment environment, IConfiguration iconfiguration)
        {
            _context = context;
            _environment = environment;
            _iconfiguration = iconfiguration;
            _externalFile = new RectorExternalFile(_environment, _iconfiguration);
        }


        // GET: Rectors

        public IActionResult Index(string UniversityName, string LastName, string Email, string RegistrationNumber, string DocumentName, int page = 1)
        {

            int pageSize = 5;




            IQueryable<Rector> source = _context.Rectors.Include(r => r.University);



            if (LastName != null)
            {
                source = source.Where(x => x.LastName.Contains(LastName));
            }


            if (Email != null)
            {
                source = source.Where(x => x.Email.Contains(Email));
            }


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                FilterViewModel = new FilterViewModel( UniversityName, LastName, Email, RegistrationNumber, DocumentName),
                Rectors = items
            };

            return View(ivm);

        }

        // GET: Rectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rector = await _context.Rectors
                .Include(r => r.University)
                .SingleOrDefaultAsync(m => m.RectorId == id);
            if (rector == null)
            {
                return NotFound();
            }

            return View(rector);
        }

        // GET: Rectors/Create
        public IActionResult Create()
        {
            EditRectorViewModel rectorView = new EditRectorViewModel()
            {
                Universities = _context.Universitys,
                Rectors = _context.Rectors
            };
            SelectList listFreeUniversities = rectorView.ListUniversities;

            if (listFreeUniversities.Count() == 0)
            {
                string message = "Нет университетов не занятых ректорами!";
                return View("Message", message);
            };

            ViewData["UniversityId"] = listFreeUniversities;

            return View();
        }

        // POST: Rectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rector rector, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                var rectorWithPhoto = await _externalFile.UploadRectorWithPhoto(rector, upload);
                _context.Add(rectorWithPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            EditRectorViewModel rectorView = new EditRectorViewModel()
            {
                CurrentRector = rector,
                Universities = _context.Universitys,
                Rectors = _context.Rectors
            };
            ViewData["UniversityId"] = rectorView.ListUniversities;
            return View(rector);
        }

        // GET: Rectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rector = await _context.Rectors.SingleOrDefaultAsync(m => m.RectorId == id);
            if (rector == null)
            {
                return NotFound();
            }
            EditRectorViewModel rectorView = new EditRectorViewModel()
            {
                CurrentRector = rector,
                Universities = _context.Universitys,
                Rectors = _context.Rectors
            };

            return View(rectorView);
        }

        // POST: Rectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Rector CurrentRector, IFormFile upload)
        {
            Rector rector = CurrentRector;
            if (ModelState.IsValid)
            {
                try
                {
                    rector = await _externalFile.UploadRectorWithPhoto(CurrentRector, upload);
                    _context.Update(rector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RectorExists(rector.RectorId))
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

            EditRectorViewModel rectorView = new EditRectorViewModel()
            {
                CurrentRector = rector,
                Universities = _context.Universitys,
                Rectors = _context.Rectors
            };
            return View(rectorView);
        }

        // GET: Rectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rector = await _context.Rectors
                .Include(r => r.University)
                .SingleOrDefaultAsync(m => m.RectorId == id);
            if (rector == null)
            {
                return NotFound();
            }

            return View(rector);
        }

        // POST: Rectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RectorId)
        {
            var rector = await _context.Rectors.SingleOrDefaultAsync(m => m.RectorId == RectorId);
            string fullFileName = _environment.WebRootPath + rector.Photo;
            _context.Rectors.Remove(rector);
            await _context.SaveChangesAsync();

            //Удаление файла фотографии
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

        private bool RectorExists(int id)
        {
            return _context.Rectors.Any(e => e.RectorId == id);
        }

    }
}
