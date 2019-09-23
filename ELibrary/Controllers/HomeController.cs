using System.Linq;
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
        private readonly ApplicationDbContext _context;


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
        public IActionResult Index(string returnUrl)
        {
            

            if (ViewBag.userId != null)
            {
                var userId = HttpContext.Session.GetString("userId");
                var type = this._context.Users.FirstOrDefault(x=>x.Id == userId).Type;
                ViewBag.LoginErr = "";
                ViewBag.RegisterErr= "";
                return RedirectToLocal(userId, type, returnUrl);
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel indexModel, string returnUrl = null)
        {
            ViewBag.UserType = "guest";
            if (ModelState.IsValid)
            {
                var registerModel = indexModel.RegisterViewModel;
                var loginModel = indexModel.LoginViewModel;
                if (loginModel != null)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    ViewData["ReturnUrl"] = returnUrl;

                    return await Login(indexModel, returnUrl);
                }
                else
                {
                    ViewData["ReturnUrl"] = returnUrl;

                    
                    return await Register(indexModel, returnUrl);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(indexModel);
        }




        public async Task<IActionResult> Login(IndexViewModel indexModel, string returnUrl = null)
        {
            LoginViewModel loginModel = indexModel.LoginViewModel;

                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            if (this._context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null) != null)
            {
                var userName = this._context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null).UserName;

                var result = await _signInManager.PasswordSignInAsync(
                    userName,
                    loginModel.Password,
                    loginModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Успешно влизане!");
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
            }
            ViewBag.LoginErr = "Невалиден Email или парола!";

            return View(indexModel);               
            
        }

        
        public async Task<IActionResult> Register(IndexViewModel indexModel, string returnUrl = null)
        {
            ViewBag.UserType = "guest";
            ViewData["ReturnUrl"] = returnUrl;
            var registerModel = indexModel.RegisterViewModel;
            if (ModelState.IsValid)
            {
                var userChack = this._context.Users.FirstOrDefault(u => u.Email == registerModel.Email);
                if (userChack == null)
                {
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
                        _logger.LogInformation("Успешно регистриран потребител!");
                       


                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(registerModel.Email, callbackUrl);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("Успешно регистриран потребител!");

                        var userId = this._context.Users.FirstOrDefault(x => x.Email == registerModel.Email).Id;
                        Message message = new Message()
                        {
                            UserId = userId,
                            User = user,
                            TextOfMessage = "Успешно регистриран потребител!"
                        };

                        this._context.Messages.Add(message);
                        this._context.SaveChanges();

                        return RedirectToLocal(userId, type, returnUrl);
                    }
                }
                else
                {
                    ViewBag.RegisterErr = "Email адреса е зает!";
                }
            }

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
                if(type=="admin") return RedirectToAction(nameof(AdminAccountController.Index), "AdminAccount");
                else if (type == "library") return RedirectToAction(nameof(LibraryAccountController.Index), "LibraryAccount");
                return RedirectToAction(nameof(UserAccountController.Index), "UserAccount");
            }
        }
        #endregion
    }
}