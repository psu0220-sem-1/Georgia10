namespace Server.Models
{
    public partial class Membership
    {
        
        public int MemberId { get; set; }
        //What the fuck are you? The TypeID of the type of member, or the TypeID of something else? Why are you called TypeID when the same is in MemberType?
        public int TypeId { get; set; }
        //This will likely make a circular dependency as a Member has a list of Memberships...see where this is going?
        public virtual Member Member { get; set; }
        //"Enum" type from the DB.
        public virtual MemberType MemberType { get; set; }
    }
}
