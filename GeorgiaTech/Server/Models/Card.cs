using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Card
    {
        public int MemberId { get; set; }
        public string PhotoPath { get; set; }

        public virtual Member Member { get; set; }
    }
}
