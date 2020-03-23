using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MaterialSubject
    {
        public MaterialSubject()
        {
            MaterialSubjectAssignment = new HashSet<MaterialSubjectAssignment>();
        }

        public int SubjectId { get; set; }
        public string Subject { get; set; }

        public virtual ICollection<MaterialSubjectAssignment> MaterialSubjectAssignment { get; set; }
    }
}
