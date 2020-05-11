using System.Collections.Generic;

namespace Api.Models
{
    public partial class Member
    {
        public Member()
        {
        }

        public int MemberId { get; set; }
        public string SSN { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        

        public virtual ICollection<string> PhoneNumbers { get; set; }
        public virtual string CampusAddress { get; set; }
        public virtual string HomeAddress { get; set; }
        public int HomeAddressZip { get; set; }
        public string HomeAddressAdditionalInfo { get; set; }
        //This wont be set on the creation, but will get pulled from db later.
        public virtual ICollection<int> LoanIDs { get; set; }
        //no membership here, only Membertype, as Membership is a binding-table in the database.
        public virtual ICollection<MemberType> MemberTypes { get; set; }
    }
}
