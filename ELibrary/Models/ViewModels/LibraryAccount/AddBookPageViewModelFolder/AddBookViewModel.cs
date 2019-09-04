using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount.AddBookPageViewModelFolder
{
    public class AddBookViewModel
    {
        public string BookName { get; set; }

        public string Author { get; set; }

        public List<Genre> Genre { get; set; }
    }
}
