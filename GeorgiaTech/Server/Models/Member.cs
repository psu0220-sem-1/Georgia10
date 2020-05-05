using System.Collections.Generic;

namespace Server.Models
{
    public partial class Member
    {
        public Member()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();
            Memberships = new HashSet<Membership>();
            Cards = new HashSet<Card>();
            Loans = new HashSet<Loan>();
        }

        public int MemberId { get; set; }
        public string SSN { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int HomeAddressId { get; set; }
        public int CampusAddressId { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual Address CampusAddress { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
