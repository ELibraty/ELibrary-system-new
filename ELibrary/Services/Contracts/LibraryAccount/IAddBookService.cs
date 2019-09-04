using ELibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Services.Contracts.LibraryAccount
{
    public interface IAddBookService
    {
        string CreateBook(string bookName, string author, string genre, ApplicationUser user);
    }
}
