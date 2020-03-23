using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Address
    {
        public Address()
        {
            MemberCampusAddress = new HashSet<Member>();
            MemberHomeAddress = new HashSet<Member>();
            VolumeCurrentLocation = new HashSet<Volume>();
            VolumeHomeLocation = new HashSet<Volume>();
        }

        public int AddressId { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public int Zip { get; set; }

        public virtual ZipCode ZipNavigation { get; set; }
        public virtual ICollection<Member> MemberCampusAddress { get; set; }
        public virtual ICollection<Member> MemberHomeAddress { get; set; }
        public virtual ICollection<Volume> VolumeCurrentLocation { get; set; }
        public virtual ICollection<Volume> VolumeHomeLocation { get; set; }
    }
}
