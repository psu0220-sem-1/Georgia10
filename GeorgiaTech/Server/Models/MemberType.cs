using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MemberType
    {
        public MemberType()
        {
            MemberTypeAssignment = new HashSet<MemberTypeAssignment>();
        }

        public int TypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<MemberTypeAssignment> MemberTypeAssignment { get; set; }
    }
}
