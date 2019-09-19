using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount
{
    public class AllBooksViewModel
    {
        public AllBooksViewModel()
        {
            this.SortMethods = new List<string>();
            this.SortMethods.Add("Име на книгата а-я");
            this.SortMethods.Add("Име на книгата я-а");

            this.SortMethods.Add("Име на автора а-я");
            this.SortMethods.Add("Име на автора я-а");

            this.SortMethods.Add("Жанр а-я");
            this.SortMethods.Add("Жанр я-а");
        }       

        public string BookName { get; set; }
        
        public string Author { get; set; }
        
        public string GenreId { get; set; }

        public string SortMethodId { get; set; }

        public List<string> SortMethods { get; set; }

        public List<GenreListViewModel> Genres { get; set; }

        public IEnumerable<BookViewModel> Books { get; set; }

        public int CurrentPage { get; set; }

    }
}
