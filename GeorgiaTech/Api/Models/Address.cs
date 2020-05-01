using System;
namespace Api.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
    }
}
