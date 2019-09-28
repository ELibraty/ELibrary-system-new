using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount
{
    public class GiveBookViewModel
    {
        public AllBooksViewModel AllBooks;

        public AllUsersViewModel AllUsers;

        public GiveBookViewModel()
            :this(new AllBooksViewModel(), new AllUsersViewModel())
        {
        }

        public GiveBookViewModel(AllBooksViewModel allBooks, AllUsersViewModel allUsers)
        {
            AllBooks = allBooks;
            AllUsers = allUsers;
        }
    }
}
