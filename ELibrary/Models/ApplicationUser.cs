using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ELibrary.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.AddedBooks = new List<Book>();
            this.GettedBooks = new List<Book>();
            this.Avatar = "";
        }

        public virtual string Avatar { get; set; }

        public virtual string Type { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string LibararyName { get; set; }

        public virtual string LibraryLocation { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual ICollection<Book> AddedBooks { get; set; }

        public virtual ICollection<Book> GettedBooks { get; set; }
    }
}
