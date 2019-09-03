using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Controllers
{
    public class UserAccountController : Controller
    {
        public IActionResult About()
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewData["Message"] = userId;

            return View();
        }


    }
}
