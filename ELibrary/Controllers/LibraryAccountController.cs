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

        //Home Page
        [Authorize]
        public IActionResult Home()
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";

            return View();
        }

        //AddBook Page - view
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

        //AddBook Page - add new book
        [Authorize]
        [HttpPost]
        public IActionResult AddBook(string bookName,string author,string genreId)
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";
            ViewData["AddBook"] =  this.addBookService.CreateBook(bookName, author, genreId, userId);
            var allGenres = this.addBookService.GetAllGenres();            
            var viewModel = new AddBookViewModel()
            {
                Genres = allGenres,
            };
            return View(viewModel);
        }

        //AllBooks Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AllBooks()
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";

            var model = addBookService.GetAllBooks(userId, null, null, null, "Име на книгата а-я");
            var allGenres = this.addBookService.GetAllGenres();
            model.Genres = allGenres;
            return View(model);
        }

        //AllBooks Page - search book
        [Authorize]
        [HttpPost]
        public IActionResult AllBooks(string bookName, string author, string genreId,string SortMethodId)
        {
            var userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";

            var model = addBookService.GetAllBooks(userId, bookName, author, genreId, SortMethodId);
            var allGenres = this.addBookService.GetAllGenres();
            model.Genres = allGenres;
            return View(model);
        }





    }
}
