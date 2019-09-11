﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ELibrary.Models;
using ELibrary.Services;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using ELibrary.Models.ViewModels;
using System;
using ELibrary.Models.AccountViewModels;
using ELibrary.Data;
using Microsoft.AspNetCore.Http;

namespace ELibrary.Controllers
{
    public class HomeController : Controller
    {

     
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private ApplicationDbContext _context;


        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel indexModel, string returnUrl = null)
        {
            ViewBag.UserType = "guest";

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var registerModel = indexModel.RegisterViewModel;
                var loginModel = indexModel.LoginViewModel;
                if (loginModel != null)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var userName = this._context.Users.FirstOrDefault(x => x.Email == loginModel.Email).UserName;
                    if(userName != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            userName,
                            loginModel.Password,
                            loginModel.RememberMe,
                            lockoutOnFailure: false);
                       
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            var userId = this._context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Id;
                            var type = this._context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Type;

                            return RedirectToLocal(userId, type, returnUrl);
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, loginModel.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToAction(nameof(Lockout));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Невалиден Email или парола!");
                            return View(indexModel);
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Невалиден Email или парола!");
                    return View(indexModel);
                }
                else
                {
                    //asp -for= "RegisterViewModel.Type"
                    var type = registerModel.Type;
                    var user = new ApplicationUser
                    {
                        Email = registerModel.Email,
                        UserName = registerModel.UserName,
                        Type = type
                    };

                    var result = await _userManager.CreateAsync(user, registerModel.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(registerModel.Email, callbackUrl);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created a new account with password.");

                        var userId = this._context.Users.FirstOrDefault(x => x.Email == registerModel.Email).Id;
                        return RedirectToLocal(userId, type, returnUrl);
                    }
                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(indexModel);
        }

       


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {

            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

               


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

       

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal( string userId,string type, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                HttpContext.Session.SetString("userId", userId);
                if(type=="admin") return RedirectToAction(nameof(AdminAccountController.Home), "AdminAccount");
                else if (type == "library") return RedirectToAction(nameof(LibraryAccountController.Home), "LibraryAccount");
                return RedirectToAction(nameof(UserAccountController.Home), "UserAccount");
            }
        }
        #endregion
    }
}