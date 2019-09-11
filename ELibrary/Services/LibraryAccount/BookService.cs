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
    public class BookService: IAddBookService
    {
        private ApplicationDbContext context;

        public BookService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateBook(string bookName, string author, string genreId, string userId)
        {
            var genreObj = this.context.Genres.FirstOrDefault(x => x.Id == genreId);
            var book = this.context.Books.FirstOrDefault(x => x.BookName == bookName&& x.UserId==userId);
            if(book==null)
            {
                var newBook = new Book()
                {
                    BookName = bookName,
                    Author = author,
                    GenreId = genreId,
                    Genre = genreObj,
                    UserId = userId
                };
                this.context.Books.Add(newBook);

                genreObj.Books.Add(newBook);
                this.context.Books.Add(newBook);

                this.context.SaveChanges();

                return book.Id;

            }
            return "Книганата същесвува в библиотеката Ви!";
           
        }

        public AllBooksViewModel GetAllBooks(string userId)
        {
            var books = context.Books.Where(x=>x.DeletedOn==null&& x.UserId==userId).Select(b => new BookViewModel()
            {
                Author = b.Author,
                BookId = b.Id,
                BookName = b.BookName,
                GenreName = b.Genre.Name
            });


            var model = new AllBooksViewModel()
            {
                Books = books
            };
            return model;


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
