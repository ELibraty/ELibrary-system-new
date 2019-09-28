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
    public class LibraryService: ILibraryService
    {
        private ApplicationDbContext context;

        public LibraryService(ApplicationDbContext context)
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
                this.context.SaveChanges();
                string result = "Успешно добавена книганата!";
                AddMessageAtDB(userId, result);
                return result;
            }
            return "Книганата същесвува в библиотеката Ви!";           
        }

        public AllBooksViewModel DeleteBook(string userId,
            string bookName, string author, string genreId,
            string SortMethodId, string bookId,
            int currentPage, int countBookAtOnePage)
        {
            var deleteBook = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            if(deleteBook!=null)
            {
                deleteBook.DeletedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                string result = "Успешно премахната книганата!";
                AddMessageAtDB(userId, result);
            }
            return GetAllBooks(userId, bookName,author, genreId, SortMethodId, currentPage, countBookAtOnePage);
        }

        public string EditBook(string bookName, string author, string genreId, string userId, string bookId)
        {
            var genreObj = this.context.Genres.FirstOrDefault(g =>
                g.Id == genreId
                && g.DeletedOn == null);

            var book = this.context.Books.FirstOrDefault(b =>
                b.Id == bookId);

            if (book != null)
            {
                var checkDublicateBook = this.context.Books.FirstOrDefault(b =>
                     b.Id != bookId
                     && b.BookName == bookName
                     && b.Author == author);
                if(checkDublicateBook==null)
                {
                    book.BookName = bookName;
                    book.Author = author;
                    book.GenreId = genreId;
                    book.Genre = genreObj;
                    book.UserId = userId;


                    genreObj.Books.Add(book);
                    this.context.SaveChanges();
                    string result = "Успешно редактирана книганата!";
                    AddMessageAtDB(userId,  result);
                    return result;
                }
                
                return "Редакцията на книгата дублира друга книга!";

            }
            return "Книганата не същесвува в библиотеката Ви!";
        }

        public AllBooksViewModel GetAllBooks(string userId, string bookName,
            string author, string genreId,string sortMethodId,
            int currentPage, int CountBooksOfPage)
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

            if(sortMethodId== "Име на книгата я-а") books=books.OrderByDescending(b => b.BookName);
            else if (sortMethodId == "Име на автора а-я")books = books.OrderBy(b => b.Author);
            else if (sortMethodId == "Име на автора я-а") books = books.OrderByDescending(b => b.Author);
            else if (sortMethodId == "Жанр а-я") books = books.OrderBy(b => b.GenreName);
            else if (sortMethodId == "Жанр я-а") books = books.OrderByDescending(b => b.GenreName);
            else books = books.OrderBy(b => b.BookName);

            var genres = GetAllGenres().OrderByDescending(x=>x.Name).ToList();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр"
            };

            genres.Add(genre);
            genres.Reverse();
            int maxCountPage = books.Count() / CountBooksOfPage;
            if (books.Count() % CountBooksOfPage != 0) maxCountPage++;

            var viewBook = books.Skip((currentPage - 1) * CountBooksOfPage)
                                .Take(CountBooksOfPage);


            var model = new AllBooksViewModel()
            {
                Books = viewBook,
                Author = author,
                BookName = bookName,
                GenreId = genreId,
                SortMethodId = sortMethodId,
                Genres= genres,
                MaxCountPage=maxCountPage,
                CurrentPage= currentPage,
                CountBooksOfPage= CountBooksOfPage
            };
            return model;
        }

        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = this.context.Genres.Select(g => new GenreListViewModel()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
            var result = genres.OrderBy(x => x.Name).ToList();       

            return result;
        }

        public AddBookViewModel GetBookData(string bookId)
        {
            var book = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            var model = new AddBookViewModel()
            {
                Author = book.Author,
                BookName = book.BookName,
                GenreId = book.GenreId,
                Genres = GetAllGenres(),
                BookId=bookId
            };
            return model;
        }

        public string AddMessageAtDB(string userId, string textOfMessage)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);

            Message message = new Message()
            {
                UserId = userId,
                User = user,
                TextOfMessage = textOfMessage
            };

            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return message.Id;
        }

      /*  public GiveBookViewModel GetGiveBookInformation(string userId)
        {
           
        }
        */
        public AllUsersViewModel AllUsers()
        {
            var users = context.Users.Where(u=>
                    u.Type=="user")
                .Select(u => new UserViewModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserId = u.Id,
                    UserName = u.UserName
                }).ToList();

            //if (sortMethodId == "Име на книгата я-а") books = books.OrderByDescending(b => b.BookName);
            //else if (sortMethodId == "Име на автора а-я") books = books.OrderBy(b => b.Author);


            AllUsersViewModel model = new AllUsersViewModel()
            {
                Users= users
            };
            return model;
        }

        public GiveBookViewModel GetGiveBookInformation(string userId)
        {
            var allBooks = this.GetAllBooks(userId, null, null,
                 null, "Име на книгата а-я", 1, 10);
            var allUsers = AllUsers();
            var model = new GiveBookViewModel()
            {
                AllBooks= allBooks,
                AllUsers= allUsers
            };


            return model;
        }

        public GiveBookViewModel GetGiveBookInformationSearchBook(string userId, GiveBookViewModel model)
        {
            var modelBook = model.AllBooks;
            var allBooks = this.GetAllBooks(userId, modelBook.BookName, modelBook.Author,
                modelBook.GenreId, modelBook.SortMethodId, modelBook.CurrentPage, modelBook.CountBooksOfPage);
            allBooks.Author = modelBook.Author==null? "Null":modelBook.Author;
            allBooks.BookName = modelBook.BookName == null ? "Null" : modelBook.BookName;

            var allUsers = new AllUsersViewModel();// model.AllUsers;
            var returnModel = new GiveBookViewModel()
            {
                AllBooks=allBooks
            };


            return returnModel;
        }
    }
}
