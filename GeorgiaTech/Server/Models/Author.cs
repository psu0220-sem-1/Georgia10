using System.Collections.Generic;

namespace Server.Models
{
    public partial class Author
    {
        public Author()
        {
            AuthorMaterials = new HashSet<MaterialAuthor>();
        }

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<MaterialAuthor> AuthorMaterials { get; set; }
    }
}
