using System;
using System.Collections.Generic;
using System.Text;

namespace GTL_DataAccessLibrary.Models
{
    public class Member
    {
        public string MemberId { get; set; }
        public int Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Address> HomeAddressId { get; set; } = new List<Address>();
        public List<Address> CampusAddressId { get; set; } = new List<Address>();



    }
}
