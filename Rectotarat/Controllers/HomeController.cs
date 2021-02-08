using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rectotarat.Models;
using Rectotarat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Rectotarat.Controllers
{
    public class HomeController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        private readonly RectoratContext _context;

        public HomeController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, RectoratContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            
            string adminEmail = "admin@gmail.com";
            string password = "_Aa1997";

            string userEmail = "user@gmail.com";
            string userpassword = "_Aa1996";



            if (await _roleManager.FindByNameAsync("admin") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await _roleManager.FindByNameAsync("user") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }


            User user = new User { Email = adminEmail, UserName = adminEmail , UniversityId=1,RegistrationDate = DateTime.Now };
            var result = await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "admin");



            User user1 = new User { Email = userEmail, UserName = userEmail, UniversityId = 1, RegistrationDate = DateTime.Now };
            var result1 = await _userManager.CreateAsync(user1, userpassword);

            await _userManager.AddToRoleAsync(user1, "user");
            return View(await _context.Newss.Include(t =>t.Univercity).ToListAsync());
        }


        public IActionResult About()
        {
            /*
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel achievementsViewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Achievements = achievements.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
\            };
*/
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
