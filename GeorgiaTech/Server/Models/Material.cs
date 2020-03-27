using System.Collections.Generic;

namespace Server.Models
{
    public partial class Material
    {
        public Material()
        {
            MaterialAuthor = new HashSet<MaterialAuthor>();
            MaterialSubjectAssignment = new HashSet<MaterialSubjectAssignment>();
            Volume = new HashSet<Volume>();
        }

        public int MaterialId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public bool Lendable { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }

        public virtual MaterialType Type { get; set; }

        public virtual Acquire Acquire { get; set; }
        public virtual ICollection<MaterialAuthor> MaterialAuthor { get; set; }
        public virtual ICollection<MaterialSubjectAssignment> MaterialSubjectAssignment { get; set; }
        public virtual ICollection<Volume> Volume { get; set; }
    }
}
