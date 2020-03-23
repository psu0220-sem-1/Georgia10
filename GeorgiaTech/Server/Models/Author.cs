using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Author
    {
        public Author()
        {
            MaterialAuthor = new HashSet<MaterialAuthor>();
        }

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<MaterialAuthor> MaterialAuthor { get; set; }
    }
}
