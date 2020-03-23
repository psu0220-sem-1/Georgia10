using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class AcquireReason
    {
        public AcquireReason()
        {
            Acquire = new HashSet<Acquire>();
        }

        public int ReasonId { get; set; }
        public string Reason { get; set; }

        public virtual ICollection<Acquire> Acquire { get; set; }
    }
}
