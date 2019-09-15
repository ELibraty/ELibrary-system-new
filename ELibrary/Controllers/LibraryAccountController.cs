﻿using ELibrary.Services.Contracts.LibraryAccount;
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
        private IAddBookService addBookService;
        private string userId;

        public LibraryAccountController(
            IAddBookService addBookService)
        {
            this.addBookService = addBookService;
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
        }

        //AddBook Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            StarUp();

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
            StarUp();         

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
            StarUp();
            ViewData["AddBook"] = "home userId="+ userId;
            var model = addBookService.GetAllBooks(userId, null, null, null, "Име на книгата а-я");
            var allGenres = this.addBookService.GetAllGenres();
            return View(model);
        }

        //AllBooks Page - search books
        [Authorize]
        [HttpPost]        
        public IActionResult AllBooksSearch(string bookName, string author, string genreId,string SortMethodId)
        {
            StarUp();
            var model = addBookService.GetAllBooks(userId, bookName, author, genreId, SortMethodId);
            var allGenres = this.addBookService.GetAllGenres();
            return View("AllBooks",model);
        }

        //AllBooks Page - Delete book
        [Authorize]
        [HttpPost]
        public IActionResult DeleteBook(string bookName, string author, string genreId, string SortMethodId,string id)
        {
            StarUp();
            ViewData["AddBook"] = "Успешно премахната книга";
            var model =  addBookService.DeleteBook(userId, bookName, author, genreId, SortMethodId,id); 
            var allGenres = this.addBookService.GetAllGenres();
            return View("AllBooks", model);
        }

      

    }
}
