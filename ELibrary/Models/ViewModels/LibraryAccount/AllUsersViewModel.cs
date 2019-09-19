using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models.ViewModels.LibraryAccount
{
    public class AllUsersViewModel
    {
        public AllUsersViewModel()
        {
            this.SortMethods = new List<string>();
            this.SortMethods.Add("Потребителско име а-я");
            this.SortMethods.Add("Потребителско име я-а");

           /* this.SortMethods.Add("Име на автора а-я");
            this.SortMethods.Add("Име на автора я-а");

            this.SortMethods.Add("Жанр а-я");
            this.SortMethods.Add("Жанр я-а");*/
        }


        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserId { get; set; }

        public string SortMethodId { get; set; }

        public List<string> SortMethods { get; set; }          

        public int CurrentPage { get; set; }
    }
}
