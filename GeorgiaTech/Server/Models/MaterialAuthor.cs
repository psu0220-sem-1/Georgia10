namespace Server.Models
{
    public partial class MaterialAuthor
    {
        public int MaterialId { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Material Material { get; set; }
    }
}
