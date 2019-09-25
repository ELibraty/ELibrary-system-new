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

            this.CountUsersOfPageList = new List<int>();

            this.CountUsersOfPageList.Add(10);
            this.CountUsersOfPageList.Add(15);
            this.CountUsersOfPageList.Add(20);

            this.CountUsersOfPage = this.CountUsersOfPageList[0];
            this.SortMethodId = this.SortMethods[0];
        }


        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SortMethodId { get; set; }

        public List<string> SortMethods { get; set; }

        public List<UserViewModel> Users { get; set; }



        public int CurrentPage { get; set; }

        public int MaxUsersPage { get; set; }

        public int CountUsersOfPage { get; set; }

        public List<int> CountUsersOfPageList { get; set; }
    }
}
