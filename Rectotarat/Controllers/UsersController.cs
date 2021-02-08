using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Microsoft.AspNetCore.Identity;
using Rectotarat.ViewModels;
using Microsoft.AspNetCore.Authorization;

using MimeKit;
using MailKit.Net.Smtp;

namespace Rectotarat.Controllers
{

    public class UsersController : Controller
    {


        public string   emailL;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        private readonly RectoratContext _context;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, RectoratContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id);
            var universities = _context.Universitys;

            List<User> userViewModel = new List<User>();

            string uname = "";
            string urole = "";
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count() > 0)
                {
                    urole = userRoles[0] ?? "";
                }
                var universityName = (from un in universities
                                      where (un.UniversityId == user.UniversityId)
                                      select un.UniversityName);
                if (universityName.Count() > 0)
                {
                    uname = universityName.FirstOrDefault().ToString() ?? "";
                }
                userViewModel.Add(
                    new User
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        RegistrationDate = user.RegistrationDate,
                        UniversityName = uname,
                        RoleName = urole

                    });

            }

            return View(userViewModel);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();

            ViewData["UniversityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName");
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View();

        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {

            var allRoles = _roleManager.Roles.ToList();

            ViewData["UniversityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName");
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");


            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    RegistrationDate = DateTime.Now,
                    UniversityId = model.UniversityId
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
         
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            string userRole = "";
            if (userRoles.Count() > 0)
            {
                userRole = userRoles[0] ?? "";
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                UniversityId = user.UniversityId,
                UserRole = userRole
            };
            ViewData["UniversityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", model.UniversityId);
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name", model.UserRole);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {

                    var oldRoles = await _userManager.GetRolesAsync(user);

                    if (oldRoles.Count() > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                    }

                    var newRole = model.UserRole;
                    if (newRole.Count() > 0)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.UniversityId = model.UniversityId;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin,user")]
        [HttpPost]
        public async Task<IActionResult> Main(User userdetalis)
        {

            IdentityResult x = await _userManager.UpdateAsync(userdetalis);

            if (x.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(userdetalis);
        }
        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public IActionResult Main()
        {

            var userid = _userManager.GetUserId(HttpContext.User);
            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                User user = _userManager.FindByIdAsync(userid).Result;
                return View(user);
            }
        }
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sendemail(string email,string text,string Headtext)
        {


            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(email,Headtext, text);
            List<string> years = _context.User.Select(t => t.Email).ToList();


            IndexViewModel inj = new IndexViewModel
            {
                Listemail = new SelectList(years.Distinct())
            };


            return View(inj);


        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Sendemail()
        {

            List<string> years = _context.User.Select(t => t.Email).ToList();


            IndexViewModel inj = new IndexViewModel
            {
                Listemail = new SelectList(years.Distinct())
            };

            return View(inj);

            


        }
        

    }

}
