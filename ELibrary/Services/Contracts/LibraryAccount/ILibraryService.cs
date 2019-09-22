﻿using ELibrary.Models;
using ELibrary.Models.ViewModels.LibraryAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Services.Contracts.LibraryAccount
{
    public interface IAddBookService
    {
        List<GenreListViewModel> GetAllGenres();

        string CreateBook(string bookName, string author,
            string genreId, string userId);

        AllBooksViewModel GetAllBooks(string userId, string bookName,
            string author, string genreId, string sortMethodId,
            int currentPage, int countBookAtOnePage);

        AllBooksViewModel DeleteBook(string userId, string bookName,
            string author, string genreId, string SortMethodId, string bookId,
            int currentPage, int countBookAtOnePage);

        AddBookViewModel GetBookData(string bookId);

        string EditBook(string bookName, string author,
            string genreId, string userId, string bookId);


        string GiveBook(string userId, GiveBookViewModel model);
    }
}