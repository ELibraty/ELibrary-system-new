using ELibrary.Data;
using ELibrary.Models;
using ELibrary.Models.ViewModels.LibraryAccount.AddBookPageViewModelFolder;
using ELibrary.Services.Contracts.LibraryAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Services.LibraryAccount
{
    public class AddBookService: IAddBookService
    {
        private ApplicationDbContext context;

        public AddBookService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateBook(string bookName, string author, string genreId, ApplicationUser user)
        {
            var genreObj = this.context.Genres.FirstOrDefault(x => x.Name == genreId);

            var book = new Book()
            {
                BookName = bookName,
                Author = author,
                GenreId = genreId,
                Genre = genreObj,
                UserId = user.Id
            };
            this.context.Books.Add(book);

            genreObj.Books.Add(book);
            this.context.Books.Add(book);

            this.context.SaveChanges();

            return book.Id;
        }

        public List<GenreListViewModel> GetAllGenres()
        {
            return this.context.Genres.Select(c => new GenreListViewModel()
            {
                Id =c.Id,
                Name =c.Name
            }).ToList();
        }
    }
}
