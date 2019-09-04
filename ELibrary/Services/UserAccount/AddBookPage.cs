using ELibrary.Data;
using ELibrary.Models;
using ELibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Services.UserAccount
{
    public class AddBookPage : IAddBookPage
    {
        private ApplicationDbContext context;       

        public AddBookPage(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateBook(string bookName, string author, string genre, ApplicationUser user)
        {
            var genreObj = this.context.Genres.FirstOrDefault(x => x.Name == genre);

            var book = new Book()
            {
                BookName = bookName,
                Author = author,
                GenreId = genreObj.Id,
                Genre = genreObj,
                UserId = user.Id
            };
            this.context.Books.Add(book);

            genreObj.Books.Add(book);
            this.context.Books.Add(book);

            this.context.SaveChanges();

            return book.Id;
        }
    }
}
