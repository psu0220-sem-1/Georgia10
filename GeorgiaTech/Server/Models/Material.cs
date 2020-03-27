using System.Collections.Generic;

namespace Server.Models
{
    public partial class Material
    {
        public Material()
        {
            MaterialSubjects = new HashSet<MaterialSubjects>();
            MaterialAuthors = new HashSet<MaterialAuthor>();
            Volumes = new HashSet<Volume>();
        }

        public int MaterialId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public bool Lendable { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }

        public virtual MaterialType Type { get; set; }
        public virtual ICollection<MaterialSubjects> MaterialSubjects { get; set; }
        public virtual ICollection<MaterialAuthor> MaterialAuthors { get; set; }
        public virtual Acquire Acquire { get; set; }
        public virtual ICollection<Volume> Volumes { get; set; }
    }
}
