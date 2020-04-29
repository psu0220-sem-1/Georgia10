using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string AdditionalInfo { get; set; }
        [Required]
        public int Zip { get; set; }
        public string City { get; set; }
    }
}
