using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class ZipCode
    {
        public ZipCode()
        {
            Address = new HashSet<Address>();
        }

        public int Zip { get; set; }
        public string City { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }
}
