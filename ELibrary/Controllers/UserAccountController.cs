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
            //ViewData["Message"] = "Your application description page.";

            return View();
        }


    }
}
