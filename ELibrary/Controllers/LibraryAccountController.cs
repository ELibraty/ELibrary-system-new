using ELibrary.Services.Contracts.LibraryAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELibrary.Models.ViewModels.LibraryAccount;
using ELibrary.Models;

namespace ELibrary.Controllers
{
    public class LibraryAccountController:Controller
    {
        private ILibraryService libraryService;
        private string userId;

        public LibraryAccountController(
            ILibraryService addBookService)
        {
            this.libraryService = addBookService;
        }

        //Home Page
        [Authorize]
        public IActionResult Index()
        {

            StarUp();
            return View();
        }

        public void StarUp()
        {
            userId = HttpContext.Session.GetString("userId");
            ViewBag.UserType = "libary";
            ViewBag.userId = userId;

        }



        //AddBook Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            StarUp();

            var allGenres = this.libraryService.GetAllGenres();
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
            StarUp();         
            ViewData["message"] =  this.libraryService.CreateBook(bookName, author, genreId, userId);
            var allGenres = this.libraryService.GetAllGenres();            
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
            StarUp();
            int currentPage = 1;
            var model = libraryService.GetAllBooks(userId, null,
                null, null, "Име на книгата а-я", currentPage, 10);
            ViewBag.model.Author = model.Author;
            
            return View(model);
        }

        //AllBooks Page - search books
        [Authorize]
        [HttpPost]        
        public IActionResult AllBooksSearch(string bookName, string author, string genreId,string SortMethodId,int currentPage, int CountBooksOfPage)
        {
            StarUp();
            //int countBooksOfPage = 1;
            var model = libraryService.GetAllBooks(userId,
                bookName, author, genreId, SortMethodId, currentPage, CountBooksOfPage);

            var allGenres = this.libraryService.GetAllGenres();
            return View("AllBooks",model);
        }


        //AllBooks Page - search books
        [Authorize]
        [HttpPost]
        public IActionResult ChangePage(string bookName, string author,
            string genreId, string SortMethodId, int id, int CountBooksOfPage)
        {
            StarUp();
            //int countBooksOfPage = 1;
            var model = libraryService.GetAllBooks(userId,
                bookName, author, genreId, SortMethodId, id, CountBooksOfPage);

            var allGenres = this.libraryService.GetAllGenres();
            return View("AllBooks", model);
        }

        //AllBooks Page - Delete book
        [Authorize]
        [HttpPost]
        public IActionResult DeleteBook(string bookName,
            string author, string genreId, string SortMethodId,string id)
        {
            StarUp();
            ViewData["message"] = "Успешно премахната книга";
            int currentPage = 1;
            var model = libraryService.DeleteBook(userId, bookName,
                author, genreId, SortMethodId,id, currentPage, 10); 
            var allGenres = this.libraryService.GetAllGenres();
            return View("AllBooks", model);
        }

        //AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBookAllBook(string id)
        {
            StarUp();
            var model = this.libraryService.GetBookData(id);
            HttpContext.Session.SetString("editBookId", id);
            return View("EditBook", model);
        }


        //AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBook(AddBookViewModel model)
        {
            StarUp();
            var bookId = HttpContext.Session.GetString("editBookId");

            ViewData["message"] = this.libraryService.EditBook(
                model.BookName, model.Author, model.GenreId, userId, bookId);
            var allGenres = this.libraryService.GetAllGenres();
            var viewModel = new AddBookViewModel()
            {
                Genres = allGenres,
            };
            return View(viewModel);
        }



    }
}
