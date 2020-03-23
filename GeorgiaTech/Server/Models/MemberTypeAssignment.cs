using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MemberTypeAssignment
    {
        public int MemberId { get; set; }
        public int TypeId { get; set; }

        public virtual Member Member { get; set; }
        public virtual MemberType Type { get; set; }
    }
}
