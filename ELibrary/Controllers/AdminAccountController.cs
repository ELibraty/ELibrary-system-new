using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Controllers
{
    public class AdminAccountController:Controller
    {
        public IActionResult Home()
        {
            //var userId = HttpContext.Session.GetString("userId");
            //ViewData["Message"] = userId;

            return View();
        }
    }
}
