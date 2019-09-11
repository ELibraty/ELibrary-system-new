using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount
{
    public class AllBooksViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }
    }
}
