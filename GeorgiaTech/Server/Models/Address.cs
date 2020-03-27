using System.Collections.Generic;

namespace Server.Models
{
    public partial class Address
    {
        public Address()
        {
            VolumeCurrentLocation = new HashSet<Volume>();
            VolumeHomeLocation = new HashSet<Volume>();
        }

        public int AddressId { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public int ZipCode { get; set; }

        public virtual ZipCode Zip { get; set; }

        public virtual ICollection<Volume> VolumeCurrentLocation { get; set; }
        public virtual ICollection<Volume> VolumeHomeLocation { get; set; }
    }
}
