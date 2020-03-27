namespace Server.Models
{
    public partial class Membership
    {
        public int MemberId { get; set; }
        public int TypeId { get; set; }

        public virtual Member Member { get; set; }
        public virtual MemberType MemberType { get; set; }
    }
}
