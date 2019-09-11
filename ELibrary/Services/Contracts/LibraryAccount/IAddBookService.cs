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

        string CreateBook(string bookName, string author, string genreId, string userId);
    }
}