using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount
{
    public class AllBooksViewModel
    {

        public string BookName { get; set; }
        
        public string Author { get; set; }
        
        public string GenreId { get; set; }

        public List<GenreListViewModel> Genres { get; set; }

        public IEnumerable<BookViewModel> Books { get; set; }
    }
}
