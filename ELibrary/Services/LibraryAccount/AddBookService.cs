using ELibrary.Data;
using ELibrary.Models;
using ELibrary.Models.ViewModels.LibraryAccount;
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

        public string CreateBook(string bookName, string author, string genreId, string userId)
        {
            var genreObj = this.context.Genres.FirstOrDefault(x => x.Id == genreId);

            var book = new Book()
            {
                BookName = bookName,
                Author = author,
                GenreId = genreId,
                Genre = genreObj,
                UserId = userId
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
