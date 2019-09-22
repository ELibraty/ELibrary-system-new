using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Models
{
    public class Message
    {
        public Message()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public virtual string Id { get; set; }

        public virtual string UserId { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual string TextOfMessage { get; set; }

        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? DeletedOn { get; set; }
    }
}
