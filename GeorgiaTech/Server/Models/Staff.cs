namespace Server.Models
{
    public partial class Staff
    {
        public int MemberId { get; set; }
        public string JobTitle { get; set; }

        public virtual Member Member { get; set; }
    }
}
