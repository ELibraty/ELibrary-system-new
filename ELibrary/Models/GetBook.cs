﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models
{
    public class GetBook
    {
        public GetBook()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.Books = new List<Book>();
        }

        public virtual string Id { get; set; }

        public virtual string UserId { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual string BookId { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? ReturnedOn { get; set; }
        public virtual DateTime? DeletedOn { get; set; }
    }
}
