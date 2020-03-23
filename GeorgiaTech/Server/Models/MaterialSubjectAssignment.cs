using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MaterialSubjectAssignment
    {
        public int MaterialId { get; set; }
        public int SubjectId { get; set; }

        public virtual Material Material { get; set; }
        public virtual MaterialSubject Subject { get; set; }
    }
}
