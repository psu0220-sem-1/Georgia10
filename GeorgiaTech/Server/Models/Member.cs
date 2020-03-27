using System.Collections.Generic;

namespace Server.Models
{
    public partial class Member
    {
        public Member()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();

            Card = new HashSet<Card>();
            Loan = new HashSet<Loan>();
            MemberTypeAssignment = new HashSet<MemberTypeAssignment>();
            Staff = new HashSet<Staff>();
        }

        public int MemberId { get; set; }
        public string Ssn { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int HomeAddressId { get; set; }
        public int CampusAddressId { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public virtual Address CampusAddress { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual ICollection<Card> Card { get; set; }
        public virtual ICollection<Loan> Loan { get; set; }
        public virtual ICollection<MemberTypeAssignment> MemberTypeAssignment { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
