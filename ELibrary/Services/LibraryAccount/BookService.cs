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
            var genreObj = this.context.Genres.FirstOrDefault(g =>
                g.Id == genreId
                && g.DeletedOn==null);

            var book = this.context.Books.FirstOrDefault(b => 
                b.BookName == bookName
                && b.Author == author
                && b.UserId==userId 
                && b.DeletedOn==null);

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
                return "Успешно добавена книганата!";
            }
            return "Книганата същесвува в библиотеката Ви!";           
        }

       

        public AllBooksViewModel GetAllBooks(string userId, string bookName,
            string author, string genreId,string SortMethodId)
        {
            var books = context.Books.Where(b =>
                b.DeletedOn == null
                && b.UserId == userId)
                .Select(b => new BookViewModel()
                {
                    Author = b.Author,
                    BookId = b.Id,
                    BookName = b.BookName,
                    GenreName = b.Genre.Name,
                    GenreId= b.GenreId
                });

            if(bookName!= null)
            {
                books = books.Where(b => b.BookName.Contains(bookName));
            }

            if (author != null)
            {
                books = books.Where(b => b.Author.Contains(author));
            }

            if (genreId != null)
            {
                books = books.Where(b => b.GenreId==genreId);
            }

            if(SortMethodId== "Име на книгата я-а") books=books.OrderByDescending(b => b.BookName);
            else if (SortMethodId == "Име на автора а-я")books = books.OrderBy(b => b.Author);
            else if (SortMethodId == "Име на автора я-а") books = books.OrderByDescending(b => b.Author);
            else if (SortMethodId == "Жанр а-я") books = books.OrderBy(b => b.GenreName);
            else if (SortMethodId == "Жанр я-а") books = books.OrderByDescending(b => b.GenreName);
            else books = books.OrderBy(b => b.BookName);


            var model = new AllBooksViewModel()
            {
                Books = books,
                Author = author,
                BookName = bookName,
                GenreId = genreId,
                SortMethodId = SortMethodId,
            };
            return model;
        }

        public List<GenreListViewModel> GetAllGenres()
        {
            /* var genres = this.context.Genres.OrderBy(g => g.Name).Select(g => new GenreListViewModel()
             {
                 Id = g.Id,
                 Name = g.Name
             }).Reverse().ToList();*/

            var genres = this.context.Genres.Select(g => new GenreListViewModel()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();

            var ganre = new GenreListViewModel()
            {
                Id = "",
                Name = "Изберете жанр"
            };

            genres.Add(ganre);    
            var result= genres.Select(g => new GenreListViewModel()
            {
                Id = g.Id,
                Name = g.Name
            }).Reverse().ToList();
            return result;
        }
    }
}
