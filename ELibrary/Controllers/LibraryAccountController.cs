using ELibrary.Services.Contracts.LibraryAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELibrary.Models.ViewModels.LibraryAccount;

namespace ELibrary.Controllers
{
    public class LibraryAccountController:Controller
    {
        private IAddBookService addBookService;

        public LibraryAccountController(
            IAddBookService addBookService)
        {
            this.addBookService = addBookService;
        }

        [Authorize]
        public IActionResult Home()
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";

            var allGenres = this.addBookService.GetAllGenres();
            var viewModel = new AddBookViewModel()
            {
                Genres = allGenres,
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddBook(string bookName,string author,string genreId)
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";
        
            bool flagAddBook = false;



            this.addBookService.CreateBook(bookName, author, genreId, userId);

            var allGenres = this.addBookService.GetAllGenres();
            var viewModel = new AddBookViewModel()
            {
                Genres = allGenres,
            };


            return View();
        }


    }
}
