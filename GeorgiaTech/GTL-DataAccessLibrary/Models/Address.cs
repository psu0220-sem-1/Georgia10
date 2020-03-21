using System;
using System.Collections.Generic;
using System.Text;

namespace GTL_DataAccessLibrary.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public List<ZipCode> Zips { get; set; } = new List<ZipCode>();

    }
}
