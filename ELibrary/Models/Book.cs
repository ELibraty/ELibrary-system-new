using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models
{
    public class Book
    {
        public Book()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public virtual string Id { get; set; }

        public virtual string BookName { get; set; }

        public virtual string Author { get; set; }

        public virtual string GenreId { get; set; }

        public virtual string UserId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }
    }
}
