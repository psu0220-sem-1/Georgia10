namespace Server.Models
{
    public partial class MaterialSubjects
    {
        public int MaterialId { get; set; }
        public int SubjectId { get; set; }

        public virtual Material Material { get; set; }
        public virtual MaterialSubject MaterialSubject { get; set; }
    }
}
