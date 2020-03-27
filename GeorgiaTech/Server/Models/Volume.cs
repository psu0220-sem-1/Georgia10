using System.Collections.Generic;

namespace Server.Models
{
    public partial class Volume
    {
        public Volume()
        {
            Loan = new HashSet<Loan>();
        }

        public int VolumeId { get; set; }
        public int MaterialId { get; set; }
        public int HomeLocationId { get; set; }
        public int CurrentLocationId { get; set; }

        public virtual Material Material { get; set; }

        public virtual Address CurrentLocation { get; set; }
        public virtual Address HomeLocation { get; set; }
        public virtual ICollection<Loan> Loan { get; set; }
    }
}
